using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tgreiner.amy.common.engine
{
    public interface IComm
    {
        void OnResponse(string response);
    }
}
