using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tgreiner.amy
{
    //
    // In order to convert some functionality to Visual C#, the Java Language Conversion Assistant
    // creates "support classes" that duplicate the original functionality.  
    //
    // Support classes replicate the functionality of the original code, but in some cases they are 
    // substantially different architecturally. Although every effort is made to preserve the 
    // original architecture of the application in the converted project, the user should be aware that 
    // the primary goal of these support classes is to replicate functionality, and that at times 
    // the architecture of the resulting solution may differ somewhat.
    //

    using System;

    /// <summary>
    /// This interface should be implemented by any class whose instances are intended 
    /// to be executed by a thread.
    /// </summary>
    public interface IThreadRunnable
    {
        /// <summary>
        /// This method has to be implemented in order that starting of the thread causes the object's 
        /// run method to be called in that separately executing thread.
        /// </summary>
        void Run();
    }

    /// <summary>
    /// Contains conversion support elements such as classes, interfaces and static methods.
    /// </summary>
    public class SupportClass
    {
        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>
        public static int URShift(int number, int bits)
        {
            if (number >= 0)
                return number >> bits;
            else
                return (number >> bits) + (2 << ~bits);
        }

        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>
        public static int URShift(int number, long bits)
        {
            return URShift(number, (int)bits);
        }

        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>
        public static long URShift(long number, int bits)
        {
            if (number >= 0)
                return number >> bits;
            else
                return (number >> bits) + (2L << ~bits);
        }

        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number
        /// </summary>
        /// <param name="number">Number to operate on</param>
        /// <param name="bits">Ammount of bits to shift</param>
        /// <returns>The resulting number from the shift operation</returns>
        public static long URShift(long number, long bits)
        {
            return URShift(number, (int)bits);
        }



        /*******************************/
        /// <summary>
        /// Represents a collection ob objects that contains no duplicate elements.
        /// </summary>	
        public interface SetSupport : System.Collections.ICollection, System.Collections.IList
        {
            /// <summary>
            /// Adds a new element to the Collection if it is not already present.
            /// </summary>
            /// <param name="obj">The object to add to the collection.</param>
            /// <returns>Returns true if the object was added to the collection, otherwise false.</returns>
            new bool Add(System.Object obj);

            /// <summary>
            /// Adds all the elements of the specified collection to the Set.
            /// </summary>
            /// <param name="c">Collection of objects to add.</param>
            /// <returns>true</returns>
            bool AddAll(System.Collections.ICollection c);
        }


        /*******************************/
        /// <summary>
        /// The class performs token processing in strings
        /// </summary>
        public class Tokenizer : System.Collections.IEnumerator
        {
            /// Position over the string
            private long currentPos = 0;

            /// Include demiliters in the results.
            private bool includeDelims = false;

            /// Char representation of the String to tokenize.
            private char[] chars = null;

            //The tokenizer uses the default delimiter set: the space character, the tab character, the newline character, and the carriage-return character and the form-feed character
            private string delimiters = " \t\n\r\f";

            /// <summary>
            /// Initializes a new class instance with a specified string to process
            /// </summary>
            /// <param name="source">String to tokenize</param>
            public Tokenizer(System.String source)
            {
                this.chars = source.ToCharArray();
            }

            /// <summary>
            /// Initializes a new class instance with a specified string to process
            /// and the specified token delimiters to use
            /// </summary>
            /// <param name="source">String to tokenize</param>
            /// <param name="delimiters">String containing the delimiters</param>
            public Tokenizer(System.String source, System.String delimiters)
                : this(source)
            {
                this.delimiters = delimiters;
            }


            /// <summary>
            /// Initializes a new class instance with a specified string to process, the specified token 
            /// delimiters to use, and whether the delimiters must be included in the results.
            /// </summary>
            /// <param name="source">String to tokenize</param>
            /// <param name="delimiters">String containing the delimiters</param>
            /// <param name="includeDelims">Determines if delimiters are included in the results.</param>
            public Tokenizer(System.String source, System.String delimiters, bool includeDelims)
                : this(source, delimiters)
            {
                this.includeDelims = includeDelims;
            }


            /// <summary>
            /// Returns the next token from the token list
            /// </summary>
            /// <returns>The string value of the token</returns>
            public System.String NextToken()
            {
                return NextToken(this.delimiters);
            }

            /// <summary>
            /// Returns the next token from the source string, using the provided
            /// token delimiters
            /// </summary>
            /// <param name="delimiters">String containing the delimiters to use</param>
            /// <returns>The string value of the token</returns>
            public System.String NextToken(System.String delimiters)
            {
                //According to documentation, the usage of the received delimiters should be temporary (only for this call).
                //However, it seems it is not true, so the following line is necessary.
                this.delimiters = delimiters;

                //at the end 
                if (this.currentPos == this.chars.Length)
                    throw new System.ArgumentOutOfRangeException();
                //if over a delimiter and delimiters must be returned
                else if ((System.Array.IndexOf(delimiters.ToCharArray(), chars[this.currentPos]) != -1)
                         && this.includeDelims)
                    return "" + this.chars[this.currentPos++];
                //need to get the token wo delimiters.
                else
                    return nextToken(delimiters.ToCharArray());
            }

            //Returns the nextToken wo delimiters
            private System.String nextToken(char[] delimiters)
            {
                string token = "";
                long pos = this.currentPos;

                //skip possible delimiters
                while (System.Array.IndexOf(delimiters, this.chars[currentPos]) != -1)
                    //The last one is a delimiter (i.e there is no more tokens)
                    if (++this.currentPos == this.chars.Length)
                    {
                        this.currentPos = pos;
                        throw new System.ArgumentOutOfRangeException();
                    }

                //getting the token
                while (System.Array.IndexOf(delimiters, this.chars[this.currentPos]) == -1)
                {
                    token += this.chars[this.currentPos];
                    //the last one is not a delimiter
                    if (++this.currentPos == this.chars.Length)
                        break;
                }
                return token;
            }


            /// <summary>
            /// Determines if there are more tokens to return from the source string
            /// </summary>
            /// <returns>True or false, depending if there are more tokens</returns>
            public bool HasMoreTokens()
            {
                //keeping the current pos
                long pos = this.currentPos;

                try
                {
                    this.NextToken();
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    return false;
                }
                finally
                {
                    this.currentPos = pos;
                }
                return true;
            }

            /// <summary>
            /// Remaining tokens count
            /// </summary>
            public int Count
            {
                get
                {
                    //keeping the current pos
                    long pos = this.currentPos;
                    int i = 0;

                    try
                    {
                        while (true)
                        {
                            this.NextToken();
                            i++;
                        }
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        this.currentPos = pos;
                        return i;
                    }
                }
            }

            /// <summary>
            ///  Performs the same action as NextToken.
            /// </summary>
            public System.Object Current
            {
                get
                {
                    return (Object)this.NextToken();
                }
            }

            /// <summary>
            //  Performs the same action as HasMoreTokens.
            /// </summary>
            /// <returns>True or false, depending if there are more tokens</returns>
            public bool MoveNext()
            {
                return this.HasMoreTokens();
            }

            /// <summary>
            /// Does nothing.
            /// </summary>
            public void Reset()
            {
                ;
            }
        }
        /*******************************/
        /// <summary>
        /// Provides functionality to perform character operations within a string
        /// </summary>
        public class StringIterator : System.Object
        {
            //Beginning position
            private int begin;
            //Ending position
            private int end;
            //Current position
            private int position;
            //String to operate on
            private System.String source;
            //Final character constant
            private const char DONE = '\uFFFF';

            /// <summary>
            /// Initializes a new object instance with the default values
            /// </summary>
            public StringIterator() : this("") { }

            /// <summary>
            /// Initializes a new object instance with the specified string
            /// </summary>
            /// <param name="text">String to process</param>
            public StringIterator(System.String text) : this(text, 0) { }

            /// <summary>
            /// Initializes a new object instance with the specified string
            /// starting processing in the given position
            /// </summary>
            /// <param name="text">String to process</param>
            /// <param name="pos">Starting position to work in</param>
            public StringIterator(System.String text, int pos) : this(text, 0, text.Length, pos) { }

            /// <summary>
            /// Initializes a new object instance with the specified string,
            /// starting processing in the given position and within the
            /// specified limits
            /// </summary>
            /// <param name="text">String to process</param>
            /// <param name="begin">Lower limit to work in</param>
            /// <param name="end">Upper limit to work in</param>
            /// <param name="pos">Starting position to work in</param>
            public StringIterator(System.String text, int begin, int end, int pos)
            {
                if (text == null) throw new System.NullReferenceException();
                if ((begin < 0) || (begin > end) || (end > text.Length) || (begin > text.Length)) throw new System.ArgumentException("Invalid substring range");
                if ((pos < begin) || (pos > end)) throw new System.ArgumentException("Invalid position");

                this.source = text;
                this.begin = begin;
                this.end = end;
                this.position = pos;
            }

            /// <summary>
            /// The character value in the current position
            /// </summary>
            public virtual char Current
            {
                get
                {
                    if (this.position < this.end) return this.source[this.position];
                    else return DONE;
                }
            }

            /// <summary>
            /// Moves the position to the first element and returns it's value
            /// </summary>
            public virtual char First
            {
                get
                {
                    this.position = this.begin;
                    return this.source[this.position];
                }
            }

            /// <summary>
            /// Returns the lower limit of the iteration
            /// </summary>
            public virtual int BeginIndex
            {
                get
                {
                    return this.begin;
                }
            }

            /// <summary>
            /// Returns the upper limit of the iteration
            /// </summary>
            public virtual int EndIndex
            {
                get
                {
                    return this.end;
                }
            }

            /// <summary>
            /// Returns the current position
            /// </summary>
            /// <returns></returns>
            public virtual int GetIndex()
            {
                return this.position;
            }

            /// <summary>
            /// Sets the current position index. It must be within the specified
            /// limits, or an argument exception will be thrown
            /// </summary>
            /// <param name="index">Position value to assign</param>
            /// <returns>The character value at the specified position</returns>
            public virtual char SetIndex(int index)
            {
                if (index == this.EndIndex)
                    return DONE;
                else if (index < this.begin || index > this.end)
                    throw new System.ArgumentException("Invalid index");

                this.position = index;

                return this.source[this.position];
            }

            /// <summary>
            /// Moves the position to the last element and returns it's value
            /// </summary>
            public virtual char Last
            {
                get
                {
                    this.position = this.end - 1;
                    return this.source[this.position];
                }
            }

            /// <summary>
            /// Moves the position to the next element and returns it's value
            /// </summary>
            /// <returns>The new character</returns>
            public virtual char Next()
            {
                if ((++this.position) < this.end)
                {
                    return this.source[this.position];
                }
                else return DONE;
            }

            /// <summary>
            /// Moves the position to the previous element and returns it's value
            /// </summary>
            /// <returns>The new character</returns>
            public virtual char Previous()
            {
                if ((this.position) > this.begin)
                {
                    return this.source[--this.position];
                }
                else return DONE;
            }
        }

        /*******************************/
        /// <summary>
        /// Creates a new positive random number 
        /// </summary>
        /// <param name="random">The last random obtained</param>
        /// <returns>Returns a new positive random number</returns>
        public static long NextLong(System.Random random)
        {
            long temporaryLong = random.Next();
            temporaryLong = (temporaryLong << 32) + random.Next();
            if (random.Next(-1, 1) < 0)
                return -temporaryLong;
            else
                return temporaryLong;
        }
        /*******************************/


        /*******************************/
        /// <summary>
        /// Converts an array of sbytes to an array of bytes
        /// </summary>
        /// <param name="sbyteArray">The array of sbytes to be converted</param>
        /// <returns>The new array of bytes</returns>
        public static byte[] ToByteArray(sbyte[] sbyteArray)
        {
            byte[] byteArray = null;

            if (sbyteArray != null)
            {
                byteArray = new byte[sbyteArray.Length];
                for (int index = 0; index < sbyteArray.Length; index++)
                    byteArray[index] = (byte)sbyteArray[index];
            }
            return byteArray;
        }

        /// <summary>
        /// Converts a string to an array of bytes
        /// </summary>
        /// <param name="sourceString">The string to be converted</param>
        /// <returns>The new array of bytes</returns>
        public static byte[] ToByteArray(System.String sourceString)
        {
            return System.Text.UTF8Encoding.UTF8.GetBytes(sourceString);
        }

        /// <summary>
        /// Converts a array of object-type instances to a byte-type array.
        /// </summary>
        /// <param name="tempObjectArray">Array to convert.</param>
        /// <returns>An array of byte type elements.</returns>
        public static byte[] ToByteArray(System.Object[] tempObjectArray)
        {
            byte[] byteArray = null;
            if (tempObjectArray != null)
            {
                byteArray = new byte[tempObjectArray.Length];
                for (int index = 0; index < tempObjectArray.Length; index++)
                    byteArray[index] = (byte)tempObjectArray[index];
            }
            return byteArray;
        }

        /*******************************/
        /// <summary>
        /// Receives a byte array and returns it transformed in an sbyte array
        /// </summary>
        /// <param name="byteArray">Byte array to process</param>
        /// <returns>The transformed array</returns>
        public static sbyte[] ToSByteArray(byte[] byteArray)
        {
            sbyte[] sbyteArray = null;
            if (byteArray != null)
            {
                sbyteArray = new sbyte[byteArray.Length];
                for (int index = 0; index < byteArray.Length; index++)
                    sbyteArray[index] = (sbyte)byteArray[index];
            }
            return sbyteArray;
        }

        /*******************************/
        /// <summary>
        /// This class provides functionality to reads and unread characters into a buffer.
        /// </summary>
        public class BackReader : System.IO.StreamReader
        {
            private char[] buffer;
            private int position = 1;
            //private int markedPosition;

            /// <summary>
            /// Constructor. Calls the base constructor.
            /// </summary>
            /// <param name="streamReader">The buffer from which chars will be read.</param>
            /// <param name="size">The size of the Back buffer.</param>
            public BackReader(System.IO.Stream streamReader, int size, System.Text.Encoding encoding)
                : base(streamReader, encoding)
            {
                this.buffer = new char[size];
                this.position = size;
            }

            /// <summary>
            /// Constructor. Calls the base constructor.
            /// </summary>
            /// <param name="streamReader">The buffer from which chars will be read.</param>
            public BackReader(System.IO.Stream streamReader, System.Text.Encoding encoding)
                : base(streamReader, encoding)
            {
                this.buffer = new char[this.position];
            }

            /// <summary>
            /// Checks if this stream support mark and reset methods.
            /// </summary>
            /// <remarks>
            /// This method isn't supported.
            /// </remarks>
            /// <returns>Always false.</returns>
            public bool MarkSupported()
            {
                return false;
            }

            /// <summary>
            /// Marks the element at the corresponding position.
            /// </summary>
            /// <remarks>
            /// This method isn't supported.
            /// </remarks>
            public void Mark(int position)
            {
                throw new System.IO.IOException("Mark operations are not allowed");
            }

            /// <summary>
            /// Resets the current stream.
            /// </summary>
            /// <remarks>
            /// This method isn't supported.
            /// </remarks>
            public void Reset()
            {
                throw new System.IO.IOException("Mark operations are not allowed");
            }

            /// <summary>
            /// Reads a character.
            /// </summary>
            /// <returns>The character read.</returns>
            public override int Read()
            {
                if (this.position >= 0 && this.position < this.buffer.Length)
                    return (int)this.buffer[this.position++];
                return base.Read();
            }

            /// <summary>
            /// Reads an amount of characters from the buffer and copies the values to the array passed.
            /// </summary>
            /// <param name="array">Array where the characters will be stored.</param>
            /// <param name="index">The beginning index to read.</param>
            /// <param name="count">The number of characters to read.</param>
            /// <returns>The number of characters read.</returns>
            public override int Read(char[] array, int index, int count)
            {
                int readLimit = this.buffer.Length - this.position;

                if (count <= 0)
                    return 0;

                if (readLimit > 0)
                {
                    if (count < readLimit)
                        readLimit = count;
                    System.Array.Copy(this.buffer, this.position, array, index, readLimit);
                    count -= readLimit;
                    index += readLimit;
                    this.position += readLimit;
                }

                if (count > 0)
                {
                    count = base.Read(array, index, count);
                    if (count == -1)
                    {
                        if (readLimit == 0)
                            return -1;
                        return readLimit;
                    }
                    return readLimit + count;
                }
                return readLimit;
            }

            /// <summary>
            /// Checks if this buffer is ready to be read.
            /// </summary>
            /// <returns>True if the position is less than the length, otherwise false.</returns>
            public bool IsReady()
            {
                return (this.position >= this.buffer.Length || this.BaseStream.Position >= this.BaseStream.Length);
            }

            /// <summary>
            /// Unreads a character.
            /// </summary>
            /// <param name="unReadChar">The character to be unread.</param>
            public void UnRead(int unReadChar)
            {
                this.position--;
                this.buffer[this.position] = (char)unReadChar;
            }

            /// <summary>
            /// Unreads an amount of characters by moving these to the buffer.
            /// </summary>
            /// <param name="array">The character array to be unread.</param>
            /// <param name="index">The beginning index to unread.</param>
            /// <param name="count">The number of characters to unread.</param>
            public void UnRead(char[] array, int index, int count)
            {
                this.Move(array, index, count);
            }

            /// <summary>
            /// Unreads an amount of characters by moving these to the buffer.
            /// </summary>
            /// <param name="array">The character array to be unread.</param>
            public void UnRead(char[] array)
            {
                this.Move(array, 0, array.Length - 1);
            }

            /// <summary>
            /// Moves the array of characters to the buffer.
            /// </summary>
            /// <param name="array">Array of characters to move.</param>
            /// <param name="index">Offset of the beginning.</param>
            /// <param name="count">Amount of characters to move.</param>
            private void Move(char[] array, int index, int count)
            {
                for (int arrayPosition = index + count; arrayPosition >= index; arrayPosition--)
                    this.UnRead(array[arrayPosition]);
            }
        }


        /*******************************/
        /// <summary>
        /// Checks if the giving File instance is a directory or file, and returns his Length
        /// </summary>
        /// <param name="file">The File instance to check</param>
        /// <returns>The length of the file</returns>
        public static long FileLength(System.IO.FileInfo file)
        {
            if (file.Exists)
                return file.Length;
            else
                return 0;
        }

        /*******************************/
        /// <summary>
        /// Provides support functions to create read-write random acces files and write functions
        /// </summary>
        public class RandomAccessFileSupport
        {
            /// <summary>
            /// Creates a new random acces stream with read-write or read rights
            /// </summary>
            /// <param name="fileName">A relative or absolute path for the file to open</param>
            /// <param name="mode">Mode to open the file in</param>
            /// <returns>The new System.IO.FileStream</returns>
            public static System.IO.FileStream CreateRandomAccessFile(System.String fileName, System.String mode)
            {
                System.IO.FileStream newFile = null;

                if (mode.CompareTo("rw") == 0)
                    newFile = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
                else if (mode.CompareTo("r") == 0)
                    newFile = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                else
                    throw new System.ArgumentException();

                return newFile;
            }

            /// <summary>
            /// Creates a new random acces stream with read-write or read rights
            /// </summary>
            /// <param name="fileName">File infomation for the file to open</param>
            /// <param name="mode">Mode to open the file in</param>
            /// <returns>The new System.IO.FileStream</returns>
            public static System.IO.FileStream CreateRandomAccessFile(System.IO.FileInfo fileName, System.String mode)
            {
                return CreateRandomAccessFile(fileName.FullName, mode);
            }

            /// <summary>
            /// Writes the data to the specified file stream
            /// </summary>
            /// <param name="data">Data to write</param>
            /// <param name="fileStream">File to write to</param>
            public static void WriteBytes(System.String data, System.IO.FileStream fileStream)
            {
                int index = 0;
                int length = data.Length;

                while (index < length)
                    fileStream.WriteByte((byte)data[index++]);
            }

            /// <summary>
            /// Writes the received string to the file stream
            /// </summary>
            /// <param name="data">String of information to write</param>
            /// <param name="fileStream">File to write to</param>
            public static void WriteChars(System.String data, System.IO.FileStream fileStream)
            {
                WriteBytes(data, fileStream);
            }

            /// <summary>
            /// Writes the received data to the file stream
            /// </summary>
            /// <param name="sByteArray">Data to write</param>
            /// <param name="fileStream">File to write to</param>
            public static void WriteRandomFile(sbyte[] sByteArray, System.IO.FileStream fileStream)
            {
                byte[] byteArray = ToByteArray(sByteArray);
                fileStream.Write(byteArray, 0, byteArray.Length);
            }
        }

    }
}
