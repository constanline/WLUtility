using System.ComponentModel;
using System.Windows.Forms;
using Magician.Common.Logger;
using WLUtility.Core;
using WLUtility.Helper;

namespace WLUtility.CustomControl
{
    internal partial class RoleControl : UserControl
    {
        public ComponentResourceManager Resources;

        private ProxySocket _socket;

        public ILogger Logger { get; }

        public RoleControl()
        {
            InitializeComponent();
            Logger = new RichTextBoxLogger(rtxtLog);
            Resources = new ComponentResourceManager(GetType());
        }

        public void SetProxySocket(ProxySocket proxySocket)
        {
            _socket = proxySocket;

            bagItemBox1.SetProxy(_socket);

            _socket.PlayerInfo.AutoSellItemUpdated += PlayerInfo_AutoSellItemUpdated;
        }

        private void PlayerInfo_AutoSellItemUpdated()
        {
            lbAutoSellItem.DataSource = _socket.PlayerInfo.AutoSellItemList;
            lbAutoSellItem.Refresh();
        }

        private void btnExecWoodMan_Click(object sender, System.EventArgs e)
        {
            if (numWoodManPos.ByteValue == null)
            {
                MessageBox.Show(Resources.GetString("InputMapEventId"));
                return;
            }

            _socket.WoodManInfo.StartWoodMan(numWoodManPos.ByteValue.Value);
        }

        private void btnDelSellItem_Click(object sender, System.EventArgs e)
        {
            if (lbAutoSellItem.SelectedIndex < 0) return;

            _socket.PlayerInfo.DelAutoSellItemIdx(lbAutoSellItem.SelectedIndex);
        }
    }
}
