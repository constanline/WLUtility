
namespace WLUtility.CustomControl
{
    partial class BagItemBox
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvItem = new System.Windows.Forms.DataGridView();
            this.dgvItem_Pos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvItem_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvItem_Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItem)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvItem
            // 
            this.dgvItem.AllowUserToAddRows = false;
            this.dgvItem.AllowUserToDeleteRows = false;
            this.dgvItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvItem_Pos,
            this.dgvItem_Name,
            this.dgvItem_Qty});
            this.dgvItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItem.Location = new System.Drawing.Point(0, 0);
            this.dgvItem.Name = "dgvItem";
            this.dgvItem.ReadOnly = true;
            this.dgvItem.RowHeadersWidth = 4;
            this.dgvItem.RowTemplate.Height = 23;
            this.dgvItem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItem.Size = new System.Drawing.Size(269, 558);
            this.dgvItem.TabIndex = 0;
            this.dgvItem.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvItem_CellFormatting);
            // 
            // dgvItem_Pos
            // 
            this.dgvItem_Pos.HeaderText = "位置";
            this.dgvItem_Pos.Name = "dgvItem_Pos";
            this.dgvItem_Pos.ReadOnly = true;
            this.dgvItem_Pos.Width = 60;
            // 
            // dgvItem_Name
            // 
            this.dgvItem_Name.DataPropertyName = "Name";
            this.dgvItem_Name.HeaderText = "名称";
            this.dgvItem_Name.Name = "dgvItem_Name";
            this.dgvItem_Name.ReadOnly = true;
            this.dgvItem_Name.Width = 130;
            // 
            // dgvItem_Qty
            // 
            this.dgvItem_Qty.DataPropertyName = "Quantity";
            this.dgvItem_Qty.HeaderText = "数量";
            this.dgvItem_Qty.Name = "dgvItem_Qty";
            this.dgvItem_Qty.ReadOnly = true;
            this.dgvItem_Qty.Width = 80;
            // 
            // BagItemBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvItem);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "BagItemBox";
            this.Size = new System.Drawing.Size(269, 558);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvItem_Pos;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvItem_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvItem_Qty;
    }
}
