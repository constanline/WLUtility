using System;
using System.Windows.Forms;
using WLUtility.Helper;
using static WLUtility.Helper.DllHelper;

namespace WLUtility.CustomControl
{
    internal partial class ProxyServerInfo : UserControl
    {
        private ProxyMappingInfo _pm;

        public ProxyServerInfo(ProxyMappingInfo pm)
        {
            InitializeComponent();
            _pm = pm;

            TxtRemotePort.Text = pm.RemotePort.ToString();
            IpRemote.SetIp(pm.RemoteIp);

            TxtMinLocalPort.Text = pm.LocalMinPort.ToString();
            TxtMaxLocalPort.Text = pm.LocalMaxPort.ToString();
            IpLocal.SetIp(pm.LocalIp);

            IpRemote.TextChanged += IpRemote_TextChanged;
            IpLocal.TextChanged += IpLocal_TextChanged;

            TxtRemotePort.TextChanged += TxtRemotePort_TextChanged;
            TxtMinLocalPort.TextChanged += TxtLocalPort_TextChanged;
            TxtMaxLocalPort.TextChanged += TxtLocalPort_TextChanged;

            ChkEnabled.Checked = pm.IsEnabled;
        }

        public ProxyMappingInfo GetProxyMappingInfo()
        {
            return _pm;
        }

        private void TxtRemotePort_TextChanged(object sender, EventArgs e)
        {
            _pm.RemotePort = Convert.ToUInt16(TxtRemotePort.Value);
        }

        private void IpLocal_TextChanged(object sender, EventArgs e)
        {
            _pm.LocalIp = IpLocal.GetIp();
        }

        private void IpRemote_TextChanged(object sender, EventArgs e)
        {
            _pm.RemoteIp = IpRemote.GetIp();
        }

        private void TxtLocalPort_TextChanged(object sender, EventArgs e)
        {
            var localMinPort = Convert.ToUInt16(TxtMinLocalPort.Value);
            var localMaxPort = Convert.ToUInt16(TxtMaxLocalPort.Value);
            for (var i = localMinPort; i <= localMaxPort; i++)
            {
                if (SocketHelper.PortInUse(i)) continue;

                _pm.IsEnabled = true;
                _pm.LocalPort = i;
                break;
            }
            // _pm.LocalMinPort = Convert.ToUInt16(TxtMinLocalPort.Value);
            // _pm.LocalMaxPort = Convert.ToUInt16(TxtMaxLocalPort.Value);
            //
            // if (_pm.LocalMinPort <= _pm.LocalMaxPort) return;
            // var tmp = _pm.LocalMinPort;
            // _pm.LocalMinPort = _pm.LocalMaxPort;
            // _pm.LocalMaxPort = tmp;
            //
            // TxtMinLocalPort.Text = _pm.LocalMinPort.ToString();
            // TxtMaxLocalPort.Text = _pm.LocalMaxPort.ToString();
        }

        private void ChangeEnabled(bool isEnabled)
        {
            _pm.IsEnabled = isEnabled;

            IpRemote.Enabled = isEnabled;
            IpLocal.Enabled = isEnabled;

            TxtRemotePort.Enabled = isEnabled;
            TxtMinLocalPort.Enabled = isEnabled;
            TxtMaxLocalPort.Enabled = isEnabled;
        }

        private void ChkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            ChangeEnabled(ChkEnabled.Checked);
        }
    }
}
