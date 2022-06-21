using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Magician.Common.Logger;
using WLUtility.Core;
using WLUtility.DataManager;
using WLUtility.Helper;

namespace WLUtility.CustomControl
{
    internal partial class RoleControl : UserControl
    {
        private ProxySocket _socket;
        public ComponentResourceManager Resources;

        public RoleControl()
        {
            InitializeComponent();
            Logger = new RichTextBoxLogger(rtxtLog);
            Resources = new ComponentResourceManager(GetType());
        }

        public ILogger Logger { get; }

        public void SetProxySocket(ProxySocket proxySocket)
        {
            _socket = proxySocket;

            bagItemBox1.SetProxy(_socket);

            _socket.PlayerInfo.AutoSellItemUpdated += PlayerInfo_AutoSellItemUpdated;
            _socket.PlayerInfo.AutoDropItemUpdated += PlayerInfo_AutoDropItemUpdated;
            _socket.PlayerInfo.InfoUpdate += PlayerInfo_InfoUpdate;
        }

        private void PlayerInfo_InfoUpdate()
        {
            UpdateBaseInfo();
        }

        private void UpdateBaseInfo()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateBaseInfo));
            }
            else
            {
                lblId.Text = $@"[{_socket.PlayerInfo.Id}]";
                lblName.Text = _socket.PlayerInfo.Name;

                numWoodManPos.Text = _socket.WoodManInfo.EventNo.ToString();
            }
        }

        private void PlayerInfo_AutoSellItemUpdated()
        {
            RefreshAutoSellInfo();
        }

        private void RefreshAutoSellInfo()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RefreshAutoSellInfo));
            }
            else
            {
                chkAutoSell.Checked = _socket.PlayerInfo.IsAutoSell;
                chkSellWhenFull.Checked = _socket.PlayerInfo.IsSellWhenFull;
                lbAutoSellItem.DataSource = _socket.PlayerInfo.AutoSellItemList
                    .Select(e => DataManagers.ItemManager.GetName(e)).ToList();
                lbAutoSellItem.Refresh();
            }
        }

        private void btnExecWoodMan_Click(object sender, EventArgs e)
        {
            if (numWoodManPos.ByteValue == null)
            {
                MessageBox.Show(@"请输入木人事件序号");
                return;
            }

            _socket.WoodManInfo.StartWoodMan(numWoodManPos.ByteValue.Value);
        }

        private void btnDelSellItem_Click(object sender, EventArgs e)
        {
            if (lbAutoSellItem.SelectedIndex < 0) return;

            _socket.PlayerInfo.DelAutoSellItemIdx(lbAutoSellItem.SelectedIndex);
        }

        private void btnUpdateDropDamage_Click(object sender, EventArgs e)
        {
            _socket.PlayerInfo.UnfitWhenDamage = numDropDamage.ByteValue ?? 240;
        }

        private void chkAutoSell_CheckedChanged(object sender, EventArgs e)
        {
            _socket.PlayerInfo.SwitchAutoSell(chkAutoSell.Checked);
        }

        private void chkSellWhenFull_CheckedChanged(object sender, EventArgs e)
        {
            _socket.PlayerInfo.SwitchSellWhenFull(chkSellWhenFull.Checked);
        }

        private void tsmiClearLog_Click(object sender, EventArgs e)
        {
            rtxtLog.Clear();
        }

        private void btnDelDropItem_Click(object sender, EventArgs e)
        {
            if (lbAutoDropItem.SelectedIndex < 0) return;

            _socket.PlayerInfo.DelAutoDropItemIdx(lbAutoDropItem.SelectedIndex);
        }

        private void chkAutoDrop_CheckedChanged(object sender, EventArgs e)
        {
            _socket.PlayerInfo.SwitchAutoDrop(chkAutoDrop.Checked);
        }

        private void PlayerInfo_AutoDropItemUpdated()
        {
            RefreshAutoDropInfo();
        }

        private void RefreshAutoDropInfo()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RefreshAutoDropInfo));
            }
            else
            {
                chkAutoDrop.Checked = _socket.PlayerInfo.IsAutoDrop;
                lbAutoDropItem.DataSource = _socket.PlayerInfo.AutoDropItemList
                    .Select(e => DataManagers.ItemManager.GetName(e)).ToList();
                lbAutoDropItem.Refresh();
            }
        }
    }
}