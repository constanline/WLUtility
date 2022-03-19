using System;

namespace Magician.Common.Logger
{
    /// <summary>
    ///     ILogger ������־��¼�Ļ����ӿڣ��̰߳�ȫ��
    /// </summary>
    public interface ILogger : IDisposable
    {
        bool Enabled { get; set; }
        void Log(string msg);
        void LogWithTime(string msg);
    }

    public class EmptyLogger : ILogger
    {
        #region Enabled

        public bool Enabled
        {
            get { return true; }
            set { }
        }

        #endregion

        #region IDisposable ��Ա

        public void Dispose()
        {
        }

        #endregion

        #region ILogger ��Ա

        #region Log

        public void Log(string msg)
        {
        }

        #endregion

        #region LogWithTime

        public void LogWithTime(string msg)
        {
        }

        #endregion

        #endregion
    }
}