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
        }

        private void btnExec_Click(object sender, System.EventArgs e)
        {
            if (rbWoodMan.Checked)
            {
                if (num1.ByteValue == null)
                {
                    MessageBox.Show(Resources.GetString("InputMapEventId"));
                    return;
                }
                
                _socket.WoodManInfo.StartWoodMan(num1.ByteValue.Value);

            }
        }
    }
}
