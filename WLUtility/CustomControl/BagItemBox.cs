using System;
using System.Windows.Forms;
using WLUtility.Core;

namespace WLUtility.CustomControl
{
    internal partial class BagItemBox : UserControl
    {
        private ProxySocket _socket;

        private int _rowIdx;

        public BagItemBox()
        {
            InitializeComponent();
            dgvItem.AutoGenerateColumns = false;
        }

        public void SetProxy(ProxySocket socket)
        {
            _socket = socket;
            _socket.BagUpdated += Socket_BagUpdated;
        }

        private void Socket_BagUpdated(Model.BagItem[] obj)
        {
            Refresh(obj);
        }

        private void dgvItem_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvItem.Columns[e.ColumnIndex] == dgvItem_Pos)
            {
                e.Value = e.RowIndex;
            }
        }

        private void Refresh(object obj)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object>(Refresh), obj);
            }
            else
            {
                dgvItem.DataSource = obj;
                dgvItem.Refresh();
            }
        }

        private void dgvItem_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            _rowIdx = e.RowIndex;
            dgvItem.Rows[_rowIdx].Selected = true;
        }

        private void tsmiSellItem_Click(object sender, EventArgs e)
        {
            if (_rowIdx <= 0) return;
            _socket.SendPacket(new PacketBuilder(0x1B, 0x03).Add((byte)_rowIdx).Build());
        }
    }
}
