/*-
* Copyright (c) 2003, 2004 Thorsten Greiner
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions
* are met:
* 1. Redistributions of source code must retain the above copyright
*    notice, this list of conditions and the following disclaimer.
* 2. Redistributions in binary form must reproduce the above copyright
*    notice, this list of conditions and the following disclaimer in the
*    documentation and/or other materials provided with the distribution.
*
* THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
* ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
* ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
* FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
* DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
* OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
* HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
* LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
* OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
* SUCH DAMAGE.
*
* $Id: XBoardEngine.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
/*
import tgreiner.amy.chess.book.BookDBJDBCImpl;
import tgreiner.amy.chess.book.BookMoveSelector;
import tgreiner.amy.chess.book.BookMoveSelectorImpl;
import tgreiner.amy.chess.book.DatabaseProvider;*/
using tgreiner.amy.common.engine;
using tgreiner.amy.common.timer;
//UPGRADE_TODO: The type 'org.apache.log4j.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AmySharp.chess.engine.logger;
using tgreiner.amy.bitboard;

namespace tgreiner.amy.chess.engine
{
	
	/// <summary> The main class for the XBoard/Winboard interface.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class XBoardEngine
	{
		/// <summary>The log4j Logger. </summary>
		//UPGRADE_NOTE: The initialization of  'log' was moved to static method 'tgreiner.amy.chess.engine.XBoardEngine'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static ILog log;		
		
		/// <summary>The board. </summary>
		private ChessBoard board;
		
		/// <summary>Selects book moves. </summary>
		// private BookMoveSelector bookMoveSelector;
		
		/// <summary>The transposition table. </summary>
		private ITransTable ttable = new TransTableImpl2(16);
		
		/// <summary>The search output. </summary>
		private SearchOutputXBoard searchOutput;
		
		/// <summary>The timer. </summary>
		private tgreiner.amy.common.timer.IChessTimer timer;
		
		/// <summary>The timer algorithm. </summary>
		private ExtendOnFailLowTimerAlgorithm timerAlgorithm;
		
		/// <summary>The PonderThread. </summary>
		private PonderThread ponderThread;
		
		/// <summary>Recognizes game ends. </summary>
		private GameEndRecognizer gameEndRecognizer = new GameEndRecognizer();
		
		/// <summary>The time control. </summary>
		private TimeControl timeControl;

        private int maxDepth = 50;

        private bool ponder = false;

        private IComm comm;

        string command;

        private static object syncCmd = new object();

        AutoResetEvent inputEvt = new AutoResetEvent(false);
		
		/// <summary> Create an XBoardEngine.
		/// 
		/// </summary>
		/// <param name="theIn">the input
		/// </param>
		/// <param name="theOut">the output
		/// </param>
		/// <throws>  Exception if an error occurs </throws>
		public XBoardEngine(IComm comm)
		{
            this.comm = comm;
			searchOutput = new SearchOutputXBoard(comm);

            Thread t = new Thread(this.run);
            t.Start();
            while (!t.IsAlive)
            {
                System.Threading.Thread.Sleep(100);
            }
		}
		
		/// <summary> Run the conversation with xboard.
		/// 
		/// </summary>
		/// <throws>  Exception if an error occurs </throws>
		public virtual void  run()
		{
            // expect("xboard");
            // expectRE("protover \\d+");

            sendFeatures();
			
			commandLoop();
		}

        public void Process(string cmd)
        {
            this.command = cmd;
            this.inputEvt.Set();
        }

		/// <summary> The command loop executes commands from xboard to the engine.
		/// 
		/// </summary>
		/// <throws>  Exception if an error occurs </throws>
		private void  commandLoop()
		{		
			bool respond = true;

            Regex levelPattern = new Regex("level (\\d+) (\\d+)(:(\\d+))? (\\d+)");

            for (; ; )
            {
                // wait for the command
                this.inputEvt.WaitOne();

                if ("quit".Equals(this.command))
                {
                    if (ponderThread != null)
                    {
                        ponderThread.abort();
                    }
                    LogManager.Close();
                    return;
                }
                else if (this.command.StartsWith("log "))
                {
                    System.String filename = this.command.Substring(4);
                    try
                    {
                        LogManager.SetLogFile(filename);
                    }
                    catch (IllegalEpdException)
                    {
                        this.comm.OnResponse("tellusererror Illegal position");
                    }
                }
                else if (this.command.StartsWith("new"))
                {
                    if (this.command.Length > 4)
                    {
                        board = new ChessBoard(this.command.Substring(4));
                    }
                    else
                    {
                        board = new ChessBoard();
                    }
                    timerAlgorithm = new ExtendOnFailLowTimerAlgorithm(0, 0);
                    timer = new AlgorithmBasedTimer(timerAlgorithm);
                    Console.WriteLine(board);
                    respond = true;
                }
                else if (this.command.StartsWith("usermove "))
                {
                    System.String moveStr = this.command.Substring(9);
                    try
                    {
                        int move = Move.parseSAN(board, moveStr);

                        handleMove(respond, move);
                    }
                    catch (IllegalSANException)
                    {
                        log.Error("Got bad move " + moveStr);
                    }
                }
                else if (this.command.StartsWith("showmoves "))
                {
                    System.String squareStr = this.command.Substring(10);
                    try
                    {
                        LRF lrf = new LRF(squareStr[0] - 'a',squareStr[2] - '1', squareStr[1] - 'a');

                        Console.WriteLine(board.ToString((int)lrf));
                    }
                    catch (IndexOutOfRangeException)
                    {
                        log.Error("Invalid square " + squareStr);
                    }
                }
                else if ("force".Equals(this.command))
                {
                    if (ponderThread != null)
                    {
                        ponderThread.abort();
                        ponderThread = null;
                    }
                    respond = false;
                }
                else if ("hard".Equals(this.command))
                {
                    ponder = true;
                }
                else if ("easy".Equals(this.command))
                {
                    ponder = false;
                }
                else if ("go".Equals(this.command))
                {
                    go();
                    respond = true;
                }
                else if ("undo".Equals(this.command))
                {
                    board.undoMove();
                }
                else if ("post".Equals(this.command))
                {
                    searchOutput.Post = true;
                }
                else if ("nopost".Equals(this.command))
                {
                    searchOutput.Post = false;
                }
                else if (this.command.StartsWith("setboard "))
                {
                    System.String fen = this.command.Substring(9);
                    try
                    {
                        board = new ChessBoard(fen);
                    }
                    catch (IllegalEpdException)
                    {
                        this.comm.OnResponse("tellusererror Illegal position");
                    }
                }
                else if (this.command.StartsWith("time"))
                {
                    int remaining = System.Int32.Parse(this.command.Substring(5));
                    log.Debug("Time remaining is " + remaining);

                    timeControl.RemainingTime = 10 * remaining;

                    timerAlgorithm.setDuration(timeControl.SoftLimit, timeControl.HardLimit);
                }
                else if (this.command.StartsWith("level "))
                {
                    Match levelMatcher = levelPattern.Match(this.command);
                    if (levelMatcher.Success)
                    {
                        int moves = Int32.Parse(levelMatcher.Groups[1].Value);
                        int time = Int32.Parse(levelMatcher.Groups[2].Value) * 60;

                        //if (levelMatcher.Groups[4].Value != null)
                        //{
                        //    time += Int32.Parse(levelMatcher.Groups[4].Value);
                        //}
                        //int incr = Int32.Parse(levelMatcher.Groups[5].Value);						

                        if (moves != 0)
                        {
                            log.Debug(moves + " moves in " + time + " seconds");

                            timeControl = new QuotaTimeControl(moves, time);
                        }
                        else
                        {
                            log.Debug("All moves in " + time + " seconds");

                            timeControl = new SuddenDeathTimeControl(time);
                        }
                    }
                    else
                    {
                        log.Error("Got bad move or command : " + this.command);
                    }
                }               
            }
		}

        private void handleMove(bool respond, int move)
        {
            board.doMove(move);

            checkGameEnd();

            if (respond)
            {
                if (ponder && ponderThread != null)
                {
                    if (ponderThread.PonderMove == move)
                    {
                        ponderThread.ponderHit();
                    }
                    else
                    {
                        ponderThread.abort();
                        ponderThread = null;
                    }
                }

                go();
            }
        }
		
		/// <summary> Start calculating.
		/// 
		/// </summary>
		/// <throws>  Exception if an error occurs </throws>
		private void  go()
		{
			if (ponderThread != null)
			{
				int bestMove = ponderThread.BestMove;
				int ponderMove = ponderThread.NextPonderMove;
				
				ponderThread = null;
				this.comm.OnResponse("move " + Move.toSAN(board, bestMove));
				board.doMove(bestMove);

                Console.WriteLine(board);

                checkGameEnd();
				
				if (ponder && board.isLegalMove(ponderMove))
				{
					ponderThread = new PonderThread(board, ponderMove, ttable, timer, searchOutput, maxDepth);
				}
				return ;
			}
			
			/*        if (bookMoveSelector != null) {
			int move = bookMoveSelector.selectMove(board);
			if (move != 0) {
			this.comm.OnResponse("move " + Move.toSAN(board, move));
			board.doMove(move);
			checkGameEnd();
			return;
			}           
			}*/
			
			Driver driver = new Driver(new ChessBoard(board), ttable, timer);
			driver.SearchOutput = searchOutput;
			int move = driver.search(maxDepth);
			
			if (move == 0)
			{
				return ;
			}
			
			this.comm.OnResponse("move " + Move.toSAN(board, move));
			board.doMove(move);

            Console.WriteLine(board);

            checkGameEnd();
			
			if (ponder && board.isLegalMove(driver.PonderMove))
			{
				ponderThread = new PonderThread(board, driver.PonderMove, ttable, timer, searchOutput, maxDepth);
			}
		}
		
		/// <summary> Check for end of game and generate appropriate game end output.</summary>
		private void  checkGameEnd()
		{
			if (gameEndRecognizer.isGameEnded(board))
			{
				this.comm.OnResponse(gameEndRecognizer.Result + " {" + gameEndRecognizer.Comment + "}");
			}
		}		
		
		/// <summary> Send our features to xboard.
		/// 
		/// </summary>
		/// <throws>  IOException if an I/O error occurs </throws>
		private void  sendFeatures()
		{
			this.comm.OnResponse("feature done=0");
			this.comm.OnResponse("feature myname=\"AmyJ 0.1\"");
			this.comm.OnResponse("feature usermove=1");
			this.comm.OnResponse("feature san=1");
			this.comm.OnResponse("feature sigint=0");
			this.comm.OnResponse("feature sigterm=0");
			this.comm.OnResponse("feature setboard=1");
			this.comm.OnResponse("feature done=1");
		}
		
		static XBoardEngine()
		{
			log = LogManager.GetLogger(typeof(XBoardEngine));
		}
	}
}