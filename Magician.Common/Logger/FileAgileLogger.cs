using System;
using System.Globalization;
using System.IO;
using Magician.Common.Core;

namespace Magician.Common.Logger
{
    /// <summary>
    ///     FileAgileLogger ����־��¼���ı��ļ����̰߳�ȫ��
    /// </summary>
    public sealed class FileAgileLogger : IAgileLogger, IDisposable
    {
        private FileLogger _fileLogger;

        #region FileLogger

        private FileLogger FileLogger => _fileLogger ?? (_fileLogger = new FileLogger(_filePath) {MaxLength4ChangeFile = _maxLength});

        #endregion

        public event Action<Exception> InnerExceptionOccurred;

        #region IDisposable ��Ա

        public void Dispose()
        {
            _fileLogger?.Dispose();
        }

        #endregion

        private readonly string _filePath;

        #region MaxLength

        private long _maxLength = long.MaxValue;

        /// <summary>
        ///     ����־�ļ����ӵ�һ���Ĵ�Сʱ��������һ���µ��ļ���¼��־��
        /// </summary>
        public long MaxLength4ChangeFile
        {
            get { return _maxLength; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException($"MaxLength4ChangeFile must greater than 0.");
                _maxLength = value;
                if (_fileLogger != null) _fileLogger.MaxLength4ChangeFile = value;
            }
        }

        #endregion

        #region Ctor

        public FileAgileLogger(string fileName)
        {
            var dirPath = AppDomain.CurrentDomain.BaseDirectory + "log\\";
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            _filePath = dirPath + fileName;
        }

        #endregion

        #region FileAgileLogger ��Ա

        public void LogWithTime(string msg)
        {
            try
            {
                if (!_enabled) return;

                FileLogger.LogWithTime(msg);
            }
            catch (Exception ee)
            {
                InnerExceptionOccurred?.Invoke(ee);
            }
        }

        public void Log(string errorType, string msg, string location, ErrorLevel level)
        {
            try
            {
                if (!_enabled) return;

                var ss = string.Format("\n{0} : {1} ���� {2} ����������:{3}��λ�ã�{4}",
                    DateTime.Now.ToString(CultureInfo.CurrentCulture), level.GetDescription(), msg, errorType,
                    location);

                FileLogger.Log(ss);
            }
            catch (Exception ee)
            {
                InnerExceptionOccurred?.Invoke(ee);
            }
        }

        public void Log(Exception ee, string location, ErrorLevel level)
        {
            var msg = ee.Message + " [:] " + ee.StackTrace;
            Log(ee.GetType().ToString(), msg, location, level);
        }

        public void LogSimple(Exception ee, string location, ErrorLevel level)
        {
            Log(ee.GetType().ToString(), ee.Message, location, level);
        }

        #region Enabled

        private bool _enabled = true;

        public bool Enabled
        {
            set { _enabled = value; }
        }

        #endregion

        #endregion
    }
}