using System;
using System.Diagnostics;
using System.Windows.Forms;
using Magician.Common.Core;
using Magician.Common.CustomControl;
using WLUtility.Data;
using WLUtility.Engine;
using WLUtility.Helper;

namespace WLUtility
{
    public partial class FrmMain : MagicianForm
    {
        uint _processId;
        readonly SocketEngine _socketHelper;


        public FrmMain()
        {
            InitializeComponent();

            _socketHelper = new SocketEngine();
            _socketHelper.CbException += HandleException;

            PacketAnalyzer.InitRules();
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

        private void HandleException(Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            _socketHelper.StopForward();
        }

        private void btnInject_Click(object sender, EventArgs e)
        {
            if (_socketHelper.IsRunning)
            {
                LogHelper.Log(GetLanguageString("IsRunning"));
                return;
            }


            Process[] processes = Process.GetProcessesByName("nwlvipcn");
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
                    _socketHelper.StartForward();
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
            _socketHelper.StopForward();
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
            SocketEngine.RecordPacket = chkRecordPacket.Checked;
        }
    }
}
