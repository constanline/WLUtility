using System;

namespace Magician.Common.Logger
{
    /// <summary>
    ///     IAgileLogger ���ڼ�¼��־��Ϣ��log���������׳��쳣��
    ///     ͨ������ͨ��ESFramework.Common.AdvancedFunction.SetProperty ���������������־��¼����װ�䡣
    /// </summary>
    public interface IAgileLogger
    {
        bool Enabled { set; }
        event Action<Exception> InnerExceptionOccurred;

        void LogWithTime(string msg);
        void Log(string errorType, string msg, string location, ErrorLevel level);
        void Log(Exception ee, string location, ErrorLevel level);

        /// <summary>
        ///     LogSimple ����¼�쳣�Ķ�ջλ��
        /// </summary>
        void LogSimple(Exception ee, string location, ErrorLevel level);
    }

    #region EmptyAgileLogger

    public sealed class EmptyAgileLogger : IAgileLogger
    {
        #region ILogger ��Ա

        public void Log(string errorType, string msg, string location, ErrorLevel level)
        {
        }

        public bool Enabled
        {
            set
            {

            }
        }

        #endregion

        #region IAgileLogger ��Ա

        public event Action<Exception> InnerExceptionOccurred;

        public void Log(Exception ee, string location, ErrorLevel level)
        {
        }

        public void LogSimple(Exception ee, string location, ErrorLevel level)
        {
        }

        public void LogWithTime(string msg)
        {
        }

        #endregion
    }

    #endregion
}