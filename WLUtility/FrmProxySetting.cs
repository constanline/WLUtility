using System.Windows.Forms;
using Magician.Common.CustomControl;
using WLUtility.CustomControl;
using WLUtility.Engine;

namespace WLUtility
{
    public sealed partial class FrmProxySetting : MagicianForm
    {
        public FrmProxySetting()
        {
            InitializeComponent();

            foreach (var pm in SocketEngine.ArrProxyMapping)
            {
                var info = new ProxyServerInfo(pm) {Dock = DockStyle.Top};
                PnlInfo.Controls.Add(info);
            }

        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014) // 禁掉清除背景消息
                return;
            base.WndProc(ref m);
        }
    }
}
