using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Magician.Common.Core;
using Magician.Common.CustomControl;
using WLUtility.Core;
using WLUtility.CustomControl;
using WLUtility.DataManager;
using WLUtility.Engine;
using WLUtility.Helper;

namespace WLUtility
{
    internal partial class FrmMain : MagicianForm
    {
        private uint _processId;

        private readonly SocketEngine _socketEngine;

        private bool _isInjected;

        private readonly Dictionary<string, ProxySocket> _dicProxySocket = new Dictionary<string, ProxySocket>();


        public FrmMain()
        {
            InitializeComponent();

            _socketEngine = new SocketEngine();
            _socketEngine.CbException += HandleException;
            _socketEngine.ConnectionBuilt += _socketEngine_ConnectionBuilt;

            ProxySocket.InitRules();
            LogHelper.CbRecordPacket += LogPacket;
            LogHelper.CbLog += LogHelper_CbLog;
            DataManagers.Init();
            //ThreadPool.SetMaxThreads(MAX_SOCKET_SERVER_COUNT, MAX_SOCKET_SERVER_COUNT * 3);
        }

        private void LogHelper_CbLog(string msg)
        {
            LogInfo(msg);
        }

        private void _socketEngine_ConnectionBuilt(ProxySocket proxySocket)
        {
            proxySocket.RoleLoginFinish += ProxySocket_RoleLoginFinish;
        }

        private void ProxySocket_RoleLoginFinish(ProxySocket proxySocket)
        {
            AddRolePage(proxySocket);
        }

        private void AddRolePage(ProxySocket proxySocket)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ProxySocket>(AddRolePage), proxySocket);
            }
            else
            {
                if (_dicProxySocket.ContainsKey(proxySocket.PlayerInfo.Username))
                {
                    var rc = (RoleControl)tcAccount.TabPages[proxySocket.PlayerInfo.Username].Controls[0];
                    proxySocket.SetRoleControl(rc);
                    rc.SetProxySocket(proxySocket);
                    _dicProxySocket[proxySocket.PlayerInfo.Username] = proxySocket;
                    return;
                }
                var role = new RoleControl();
                proxySocket.SetRoleControl(role);
                role.SetProxySocket(proxySocket);
                _dicProxySocket.Add(proxySocket.PlayerInfo.Username, proxySocket);

                var tp = new TabPage(proxySocket.PlayerInfo.Username);
                tp.Name = proxySocket.PlayerInfo.Username;
                tp.Controls.Add(role);
                role.Dock = DockStyle.Fill;
                tcAccount.TabPages.Add(tp);
            }
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
                rtxtLog.AppendText($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}:{msg}\r\n");
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

        private void Inject()
        {
            var path = Application.StartupPath + "\\WLHook.dll";
            if (!_isInjected)
            {
                if (_socketEngine.IsRunning)
                {
                    LogHelper.Log(GetLanguageString("IsRunning"));
                    return;
                }


                var processes = Process.GetProcessesByName("wlmfree");
                if (processes.Length == 0)
                    processes = Process.GetProcessesByName("wlviptw");

                if (processes.Length == 0)
                {
                    MessageBox.Show(GetLanguageString("ProcessNotFound"));
                    return;
                }

                _processId = (uint)processes[0].Id;

                DllHelper.SetTargetPid(_processId);
                var result = InjectHelper.GetInstance.Inject(_processId, path);
                if (result != DllInjectionResult.Success) return;
                _isInjected = true;
                _socketEngine.StartForward();
                LogHelper.Log(GetLanguageString("InjectSuccess"));
            }
            else
            {
                _socketEngine.StopForward();
                if (InjectHelper.GetInstance.UnInject(_processId, path) != DllInjectionResult.Success) return;

                LogHelper.Log(GetLanguageString("UnInjectSuccess"));
                _isInjected = false;
                _socketEngine.StopForward();
            }
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

        private void tsmiInject_Click(object sender, EventArgs e)
        {
            Inject();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                Hide();
                e.Cancel = true;
            }
        }

        private void cmsDisplay_Click(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
