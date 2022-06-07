using System;
using System.Windows.Forms;

namespace WLUtility.CustomControl
{
    internal partial class BagItemBox : UserControl
    {

        public BagItemBox()
        {
            InitializeComponent();
            dgvItem.AutoGenerateColumns = false;
        }

        private void dgvItem_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvItem.Columns[e.ColumnIndex] == dgvItem_Pos)
            {
                e.Value = e.RowIndex;
            }
        }

        public void Refresh(object obj)
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
    }
}
