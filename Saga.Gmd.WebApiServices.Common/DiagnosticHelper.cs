using System;
using System.Diagnostics;
using System.Threading;

namespace Saga.Gmd.WebApiServices.Common
{
    public class DiagnosticHelper
    {
        public static void DebugWrite(string message)
        {
            Debug.WriteLine(
                DateTime.Now.ToString("hh:mm:ss.fff tt") + ", thread- " +
                Thread.CurrentThread.ManagedThreadId + " " + message
            );
        }

        public static void DebugWriteFmt(string messageTemplate, object arg1)
        {
            object[] args = new object[] { arg1 };
            DebugWriteFmt(messageTemplate, args);
        }
        public static void DebugWriteFmt(string messageTemplate, object arg1, object arg2)
        {
            object[] args = new object[] { arg1, arg2 };
            DebugWriteFmt(messageTemplate, args);
        }
        public static void DebugWriteFmt(string messageTemplate, object arg1, object arg2, object arg3)
        {
            object[] args = new object[] { arg1, arg2, arg3 };
            DebugWriteFmt(messageTemplate, args);
        }


        public static void DebugWriteFmt(string messageTemplate, object[] args)
        {

            var mTemplate =
                DateTime.Now.ToString($"hh:mm:ss.fff tt") + ", thread- " +
                Thread.CurrentThread.ManagedThreadId + " " + messageTemplate;

            Debug.WriteLine(mTemplate, args);

        }


    }
}