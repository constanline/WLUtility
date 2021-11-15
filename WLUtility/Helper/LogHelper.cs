using System;
using System.Windows.Forms;

namespace WLUtility.Helper
{
    internal class LogHelper
    {
        public static Action<string> CbLog;
        public static Action<string> CbRecordPacket;
        

        public static void Log(string msg)
        {
            MessageBox.Show(msg);
            CbLog?.Invoke(msg);
        }

        public static void LogPacket(byte[] buffer, bool isSend)
        {
            CbRecordPacket?.Invoke((isSend ? "Send:" : "Recv:") + StringHelper.Buffer2Hex(buffer) + Environment.NewLine);
        }
    }
}