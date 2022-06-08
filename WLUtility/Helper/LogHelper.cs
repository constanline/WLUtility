using System;
using System.Windows.Forms;
using Magician.Common.Logger;

namespace WLUtility.Helper
{
    internal class LogHelper
    {
        public static event Action<string> CbLog;
        public static event Action<string> CbRecordPacket;


        public static void Log(string msg)
        {
            MessageBox.Show(msg);
            CbLog?.Invoke(msg);
        }

        public static void SilentLog(string msg)
        {
            CbLog?.Invoke(msg);
        }

        public static void Log(Exception e)
        {
            Log(e.StackTrace);
        }

        public static void SilentLog(Exception e)
        {
            SilentLog(e.StackTrace);
        }

        public static void LogPacket(byte[] buffer, bool isSend)
        {
            CbRecordPacket?.Invoke((isSend ? "Send:" : "Recv:") + StringHelper.Buffer2Hex(buffer) + Environment.NewLine);
        }
    }

    internal class RichTextBoxLogger : ILogger
    {
        private readonly RichTextBox _txtBox;
        public void Dispose()
        {
        }

        public RichTextBoxLogger(RichTextBox txtBox)
        {
            _txtBox = txtBox;
        }

        public bool Enabled { get; set; } = true;

        public void Log(string msg)
        {
            AppendText(msg + "\r\n");
        }

        public void LogWithTime(string msg)
        {
            AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + msg + "\r\n");
        }

        private void AppendText(string msg)
        {
            if (_txtBox.InvokeRequired)
            {
                _txtBox.Invoke(new Action<string>(AppendText), msg);
            }
            else
            {
                _txtBox.AppendText(msg);
            }
        }
    }
}