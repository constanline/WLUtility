using System;
using System.Diagnostics;
using System.Windows.Forms;
using Magician.Common.Core;
using Magician.Common.CustomControl;
using WLUtility.Core;
using WLUtility.Engine;
using WLUtility.Helper;

namespace WLUtility
{
    internal partial class FrmMain : MagicianForm
    {
        private uint _processId;
        private readonly SocketEngine _socketEngine;


        public FrmMain()
        {
            InitializeComponent();

            _socketEngine = new SocketEngine();
            _socketEngine.CbException += HandleException;

            ProxySocket.InitRules();
            LogHelper.CbRecordPacket += LogPacket;
            //ThreadPool.SetMaxThreads(MAX_SOCKET_SERVER_COUNT, MAX_SOCKET_SERVER_COUNT * 3);
        }

        private void LogPacket(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LogPacket), msg);
            }
            else
            {
                rtxtPacket.AppendText(msg);
            }
        }

        private void LogInfo(string msg)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LogInfo), msg);
            }
            else
            {
                rtxtLog.AppendText(msg);
            }
        }

        private void HandleException(Exception ex)
        {
            LogInfo(ex.ToString());
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            _socketEngine.StopForward();
        }

        private void btnInject_Click(object sender, EventArgs e)
        {
            if (_socketEngine.IsRunning)
            {
                LogHelper.Log(GetLanguageString("IsRunning"));
                return;
            }


            Process[] processes = Process.GetProcessesByName("wlmfree");
            if (processes.Length == 0)
            {
                processes = Process.GetProcessesByName("wlviptw");
            }
            if (processes.Length > 0)
            {
                _processId = (uint)processes[0].Id;

                DllHelper.SetTargetPid(_processId);
                var result = InjectHelper.GetInstance.Inject(_processId, "WLHook.dll");
                if (result == DllInjectionResult.Success)
                {
                    btnInject.Enabled = false;
                    btnUnInject.Enabled = true;
                    _socketEngine.StartForward();
                    LogHelper.Log(GetLanguageString("InjectSuccess"));
                }
            }
        }

        private void btnUnInject_Click(object sender, EventArgs e)
        {
            if (InjectHelper.GetInstance.UnInject(_processId, "WLHook.dll") == DllInjectionResult.Success)
            {
                LogHelper.Log(GetLanguageString("UnInjectSuccess"));
            }
            btnInject.Enabled = true;
            btnUnInject.Enabled = false;
            _socketEngine.StopForward();
        }

        private void tsmiSimplifiedChinese_Click(object sender, EventArgs e)
        {
            MultiLanguage.SetCurrentLanguage("zh-CN");
        }

        private void tsmiTraditionalChinese_Click(object sender, EventArgs e)
        {
            MultiLanguage.SetCurrentLanguage("zh-CN");
        }

        private void tsmiEnglish_Click(object sender, EventArgs e)
        {
            MultiLanguage.SetCurrentLanguage("en-US");
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            new FrmAbout().ShowDialog();
        }

        private void tsmiProxySetting_Click(object sender, EventArgs e)
        {
            new FrmProxySetting().ShowDialog();
        }

        private void chkRecordPacket_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.RecordPacket = chkRecordPacket.Checked;
        }
    }
}
