using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Logger
    {
        public static void Log(string user, string method,string message)
        {
            Trace.WriteLine(String.Format("Date: {0}, User: {1}, Method: {2}, Message: {3}"
                            ,DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss:ff"),user,method,message));
        }
    }
}
