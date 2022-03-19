using System;
using System.Threading;

namespace Magician.Common.Threading.Engine
{
    /// <summary>
    /// BaseCycleEngine ICycleEngine�ĳ���ʵ�֣�ѭ������ֱ�Ӽ̳�����ʵ��DoDetect�������ɡ�
    /// zhuweisky 2006.12.21
    /// </summary>
    public abstract class BaseCycleEngine : ICycleEngine
    {
        private const int SleepTime = 10;//ms 
        private volatile bool isStop = true;
        private ManualResetEvent manualResetEvent4Stop = new ManualResetEvent(false);
        private int totalSleepCount = 0;
        public event Action<Exception> EngineStopped; //�����������б�Ϊֹͣ״̬ʱ�����������¼���������쳣ֹͣ�����¼�����Ϊ�쳣���󣬷����¼�����Ϊnull��

        #region abstract
        /// <summary>
        /// DoDetect ÿ��ѭ��ʱ��������Ҫִ�еĺ��Ķ�����
        /// (1)�÷����������׳��쳣��
        /// (2)�÷����в��������BaseCycleEngine.Stop()����������ᵼ��������
        /// </summary>
        /// <returns>����ֵ���Ϊfalse����ʾ�˳�����ѭ���߳�</returns>
        protected abstract bool DoDetect();
        #endregion

        #region Property
        #region DetectSpanInSecs       
        /// <summary>
        /// ���������ѯ�ļ������λ���롣ֻ������DetectSpan��DetectSpanInSecs���е�һ�����ɡ�
        /// </summary>
        public virtual int DetectSpanInSecs
        {
            get { return detectSpan/1000; }
            set { detectSpan = value * 1000; }
        }
        #endregion    

        private int detectSpan = 0;
        /// <summary>
        /// ���������ѯ�ļ������λ�����롣DetectSpan=0����ʾ�޼�϶�������棻DetectSpanС��0���ʾ��ʹ�����档Ĭ��ֵΪ0��
        /// </summary>
        public int DetectSpan
        {
            get { return detectSpan; }
            set { detectSpan = value; }
        }

        #region IsRunning
        public bool IsRunning
        {
            get
            {
                return !this.isStop;
            }
        }
        #endregion
        #endregion

        public BaseCycleEngine()
        {
            this.EngineStopped += delegate { };
        }

        #region ICycleEngine ��Ա

        #region Start
        public virtual void Start()
        {
            this.totalSleepCount = this.detectSpan / BaseCycleEngine.SleepTime;

            if (this.detectSpan < 0)
            {
                return;
            }

            if (!this.isStop)
            {
                return;
            }

            this.manualResetEvent4Stop.Reset();
            this.isStop = false;
            Action cb = this.Worker;
            cb.BeginInvoke(null, null);
        }
        #endregion

        #region Stop
        /// <summary>
        /// ֹͣ���档ǧ��Ҫ��DoDetect�����е��ø÷������ᵼ�����������Ը���StopAsyn������
        /// </summary>
        public virtual void Stop()
        {
            if (this.isStop)
            {
                return;
            }

            this.isStop = true;
            this.manualResetEvent4Stop.WaitOne();
            this.manualResetEvent4Stop.Reset();
        }

        /// <summary>
        /// �첽ֹͣ���档
        /// </summary>
        public void StopAsyn()
        {
            Action cb = this.Stop;
            cb.BeginInvoke(null, null);
        }
        #endregion

        #region Worker
        protected virtual void Worker()
        {
            Exception exception = null;
            try
            {
                while (!this.isStop)
                {
                    #region Sleep
                    for (int i = 0; i < this.totalSleepCount; i++)
                    {
                        if (this.isStop)
                        {
                            break;
                        }
                        Thread.Sleep(BaseCycleEngine.SleepTime);
                    }
                    #endregion

                    if (!this.DoDetect())
                    {                       
                        break;
                    }
                }                
            }
            catch(Exception ee)
            {
                exception  = ee ;                
                throw;
            }
            finally
            {
                this.isStop = true;
                this.manualResetEvent4Stop.Set();
                this.EngineStopped(exception);
            }
        }
        #endregion
        #endregion
    }
}
