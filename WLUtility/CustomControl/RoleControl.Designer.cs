namespace WLUtility.CustomControl
{
    partial class RoleControl
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
            this.components = new System.ComponentModel.Container();
            this.rtxtLog = new System.Windows.Forms.RichTextBox();
            this.cmsLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiClearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblId = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tcInfo = new System.Windows.Forms.TabControl();
            this.tpBag = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpCommon = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.numDropDamage = new Magician.Common.CustomControl.NumberText();
            this.btnUpdateDropDamage = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numWoodManPos = new Magician.Common.CustomControl.NumberText();
            this.btnExecWoodMan = new System.Windows.Forms.Button();
            this.tpSellItem = new System.Windows.Forms.TabPage();
            this.chkSellWhenFull = new System.Windows.Forms.CheckBox();
            this.chkAutoSell = new System.Windows.Forms.CheckBox();
            this.btnDelSellItem = new System.Windows.Forms.Button();
            this.lbAutoSellItem = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.bagItemBox1 = new WLUtility.CustomControl.BagItemBox();
            this.cmsLog.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tcInfo.SuspendLayout();
            this.tpBag.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpCommon.SuspendLayout();
            this.tpSellItem.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtxtLog
            // 
            this.rtxtLog.BackColor = System.Drawing.SystemColors.Window;
            this.rtxtLog.ContextMenuStrip = this.cmsLog;
            this.rtxtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtLog.Location = new System.Drawing.Point(4, 24);
            this.rtxtLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtxtLog.Name = "rtxtLog";
            this.rtxtLog.ReadOnly = true;
            this.rtxtLog.Size = new System.Drawing.Size(565, 283);
            this.rtxtLog.TabIndex = 3;
            this.rtxtLog.Text = "";
            // 
            // cmsLog
            // 
            this.cmsLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiClearLog});
            this.cmsLog.Name = "cmsLog";
            this.cmsLog.Size = new System.Drawing.Size(141, 26);
            // 
            // tsmiClearLog
            // 
            this.tsmiClearLog.Name = "tsmiClearLog";
            this.tsmiClearLog.Size = new System.Drawing.Size(140, 22);
            this.tsmiClearLog.Text = "清空日志(&C)";
            this.tsmiClearLog.Click += new System.EventHandler(this.tsmiClearLog_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.lblId);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(832, 155);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "状态区";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(100, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(37, 20);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "名称";
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(7, 24);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(22, 20);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "Id";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tcInfo);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(0, 155);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(259, 497);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "信息区";
            // 
            // tcInfo
            // 
            this.tcInfo.Controls.Add(this.tpBag);
            this.tcInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcInfo.Location = new System.Drawing.Point(4, 24);
            this.tcInfo.Name = "tcInfo";
            this.tcInfo.SelectedIndex = 0;
            this.tcInfo.Size = new System.Drawing.Size(251, 468);
            this.tcInfo.TabIndex = 0;
            // 
            // tpBag
            // 
            this.tpBag.Controls.Add(this.bagItemBox1);
            this.tpBag.Location = new System.Drawing.Point(4, 29);
            this.tpBag.Name = "tpBag";
            this.tpBag.Padding = new System.Windows.Forms.Padding(3);
            this.tpBag.Size = new System.Drawing.Size(243, 435);
            this.tpBag.TabIndex = 0;
            this.tpBag.Text = "背包";
            this.tpBag.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tabControl1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox3.Location = new System.Drawing.Point(259, 467);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(573, 185);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "功能区";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpCommon);
            this.tabControl1.Controls.Add(this.tpSellItem);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(565, 156);
            this.tabControl1.TabIndex = 3;
            // 
            // tpCommon
            // 
            this.tpCommon.Controls.Add(this.label2);
            this.tpCommon.Controls.Add(this.numDropDamage);
            this.tpCommon.Controls.Add(this.btnUpdateDropDamage);
            this.tpCommon.Controls.Add(this.label1);
            this.tpCommon.Controls.Add(this.numWoodManPos);
            this.tpCommon.Controls.Add(this.btnExecWoodMan);
            this.tpCommon.Location = new System.Drawing.Point(4, 29);
            this.tpCommon.Name = "tpCommon";
            this.tpCommon.Padding = new System.Windows.Forms.Padding(3);
            this.tpCommon.Size = new System.Drawing.Size(557, 123);
            this.tpCommon.TabIndex = 0;
            this.tpCommon.Text = "综合";
            this.tpCommon.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "装备卸下耐久";
            // 
            // numDropDamage
            // 
            this.numDropDamage.BackColor = System.Drawing.Color.Transparent;
            this.numDropDamage.DecimalPlaces = 0;
            this.numDropDamage.DownBack = null;
            this.numDropDamage.Icon = null;
            this.numDropDamage.IconIsButton = false;
            this.numDropDamage.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.numDropDamage.IsPasswordChat = '\0';
            this.numDropDamage.IsSystemPasswordChar = false;
            this.numDropDamage.Lines = new string[] {
        "240"};
            this.numDropDamage.Location = new System.Drawing.Point(101, 46);
            this.numDropDamage.Margin = new System.Windows.Forms.Padding(0);
            this.numDropDamage.MaxLength = 0;
            this.numDropDamage.MaxValue = 100;
            this.numDropDamage.MinimumSize = new System.Drawing.Size(37, 27);
            this.numDropDamage.MinValue = 1;
            this.numDropDamage.MouseBack = null;
            this.numDropDamage.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.numDropDamage.Multiline = true;
            this.numDropDamage.Name = "numDropDamage";
            this.numDropDamage.NormlBack = null;
            this.numDropDamage.Padding = new System.Windows.Forms.Padding(7, 4, 7, 4);
            this.numDropDamage.ReadOnly = false;
            this.numDropDamage.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numDropDamage.ShowErrorProvider = false;
            this.numDropDamage.Size = new System.Drawing.Size(44, 30);
            // 
            // 
            // 
            this.numDropDamage.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numDropDamage.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numDropDamage.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.numDropDamage.SkinTxt.Location = new System.Drawing.Point(7, 4);
            this.numDropDamage.SkinTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numDropDamage.SkinTxt.MaxLength = 255;
            this.numDropDamage.SkinTxt.Multiline = true;
            this.numDropDamage.SkinTxt.Name = "BaseText";
            this.numDropDamage.SkinTxt.Size = new System.Drawing.Size(30, 22);
            this.numDropDamage.SkinTxt.TabIndex = 0;
            this.numDropDamage.SkinTxt.Text = "240";
            this.numDropDamage.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.numDropDamage.SkinTxt.WaterText = "";
            this.numDropDamage.TabIndex = 3;
            this.numDropDamage.Text = "240";
            this.numDropDamage.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numDropDamage.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.numDropDamage.WaterText = "";
            this.numDropDamage.WordWrap = true;
            // 
            // btnUpdateDropDamage
            // 
            this.btnUpdateDropDamage.Location = new System.Drawing.Point(149, 47);
            this.btnUpdateDropDamage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUpdateDropDamage.Name = "btnUpdateDropDamage";
            this.btnUpdateDropDamage.Size = new System.Drawing.Size(59, 27);
            this.btnUpdateDropDamage.TabIndex = 4;
            this.btnUpdateDropDamage.Text = "修改";
            this.btnUpdateDropDamage.UseVisualStyleBackColor = true;
            this.btnUpdateDropDamage.Click += new System.EventHandler(this.btnUpdateDropDamage_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "木人事件序号";
            // 
            // numWoodManPos
            // 
            this.numWoodManPos.BackColor = System.Drawing.Color.Transparent;
            this.numWoodManPos.DecimalPlaces = 0;
            this.numWoodManPos.DownBack = null;
            this.numWoodManPos.Icon = null;
            this.numWoodManPos.IconIsButton = false;
            this.numWoodManPos.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.numWoodManPos.IsPasswordChat = '\0';
            this.numWoodManPos.IsSystemPasswordChar = false;
            this.numWoodManPos.Lines = new string[0];
            this.numWoodManPos.Location = new System.Drawing.Point(101, 7);
            this.numWoodManPos.Margin = new System.Windows.Forms.Padding(0);
            this.numWoodManPos.MaxLength = 0;
            this.numWoodManPos.MaxValue = 100;
            this.numWoodManPos.MinimumSize = new System.Drawing.Size(37, 27);
            this.numWoodManPos.MinValue = 1;
            this.numWoodManPos.MouseBack = null;
            this.numWoodManPos.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.numWoodManPos.Multiline = true;
            this.numWoodManPos.Name = "numWoodManPos";
            this.numWoodManPos.NormlBack = null;
            this.numWoodManPos.Padding = new System.Windows.Forms.Padding(7, 4, 7, 4);
            this.numWoodManPos.ReadOnly = false;
            this.numWoodManPos.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numWoodManPos.ShowErrorProvider = false;
            this.numWoodManPos.Size = new System.Drawing.Size(44, 30);
            // 
            // 
            // 
            this.numWoodManPos.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numWoodManPos.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numWoodManPos.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.numWoodManPos.SkinTxt.Location = new System.Drawing.Point(7, 4);
            this.numWoodManPos.SkinTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numWoodManPos.SkinTxt.MaxLength = 255;
            this.numWoodManPos.SkinTxt.Multiline = true;
            this.numWoodManPos.SkinTxt.Name = "BaseText";
            this.numWoodManPos.SkinTxt.Size = new System.Drawing.Size(30, 22);
            this.numWoodManPos.SkinTxt.TabIndex = 0;
            this.numWoodManPos.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.numWoodManPos.SkinTxt.WaterText = "";
            this.numWoodManPos.TabIndex = 0;
            this.numWoodManPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numWoodManPos.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.numWoodManPos.WaterText = "";
            this.numWoodManPos.WordWrap = true;
            // 
            // btnExecWoodMan
            // 
            this.btnExecWoodMan.Location = new System.Drawing.Point(149, 8);
            this.btnExecWoodMan.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExecWoodMan.Name = "btnExecWoodMan";
            this.btnExecWoodMan.Size = new System.Drawing.Size(59, 27);
            this.btnExecWoodMan.TabIndex = 1;
            this.btnExecWoodMan.Text = "执行";
            this.btnExecWoodMan.UseVisualStyleBackColor = true;
            this.btnExecWoodMan.Click += new System.EventHandler(this.btnExecWoodMan_Click);
            // 
            // tpSellItem
            // 
            this.tpSellItem.Controls.Add(this.chkSellWhenFull);
            this.tpSellItem.Controls.Add(this.chkAutoSell);
            this.tpSellItem.Controls.Add(this.btnDelSellItem);
            this.tpSellItem.Controls.Add(this.lbAutoSellItem);
            this.tpSellItem.Location = new System.Drawing.Point(4, 29);
            this.tpSellItem.Name = "tpSellItem";
            this.tpSellItem.Padding = new System.Windows.Forms.Padding(3);
            this.tpSellItem.Size = new System.Drawing.Size(557, 123);
            this.tpSellItem.TabIndex = 1;
            this.tpSellItem.Text = "贩卖";
            this.tpSellItem.UseVisualStyleBackColor = true;
            // 
            // chkSellWhenFull
            // 
            this.chkSellWhenFull.AutoSize = true;
            this.chkSellWhenFull.Location = new System.Drawing.Point(145, 72);
            this.chkSellWhenFull.Name = "chkSellWhenFull";
            this.chkSellWhenFull.Size = new System.Drawing.Size(84, 24);
            this.chkSellWhenFull.TabIndex = 3;
            this.chkSellWhenFull.Text = "整组出售";
            this.chkSellWhenFull.UseVisualStyleBackColor = true;
            this.chkSellWhenFull.CheckedChanged += new System.EventHandler(this.chkSellWhenFull_CheckedChanged);
            // 
            // chkAutoSell
            // 
            this.chkAutoSell.AutoSize = true;
            this.chkAutoSell.Location = new System.Drawing.Point(145, 42);
            this.chkAutoSell.Name = "chkAutoSell";
            this.chkAutoSell.Size = new System.Drawing.Size(84, 24);
            this.chkAutoSell.TabIndex = 2;
            this.chkAutoSell.Text = "自动出售";
            this.chkAutoSell.UseVisualStyleBackColor = true;
            this.chkAutoSell.CheckedChanged += new System.EventHandler(this.chkAutoSell_CheckedChanged);
            // 
            // btnDelSellItem
            // 
            this.btnDelSellItem.Location = new System.Drawing.Point(145, 6);
            this.btnDelSellItem.Name = "btnDelSellItem";
            this.btnDelSellItem.Size = new System.Drawing.Size(75, 30);
            this.btnDelSellItem.TabIndex = 1;
            this.btnDelSellItem.Text = "删除";
            this.btnDelSellItem.UseVisualStyleBackColor = true;
            this.btnDelSellItem.Click += new System.EventHandler(this.btnDelSellItem_Click);
            // 
            // lbAutoSellItem
            // 
            this.lbAutoSellItem.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbAutoSellItem.FormattingEnabled = true;
            this.lbAutoSellItem.ItemHeight = 20;
            this.lbAutoSellItem.Location = new System.Drawing.Point(3, 3);
            this.lbAutoSellItem.Name = "lbAutoSellItem";
            this.lbAutoSellItem.Size = new System.Drawing.Size(136, 117);
            this.lbAutoSellItem.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rtxtLog);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(259, 155);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(573, 312);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "日志区";
            // 
            // bagItemBox1
            // 
            this.bagItemBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bagItemBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bagItemBox1.Location = new System.Drawing.Point(3, 3);
            this.bagItemBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bagItemBox1.Name = "bagItemBox1";
            this.bagItemBox1.Size = new System.Drawing.Size(237, 429);
            this.bagItemBox1.TabIndex = 0;
            // 
            // RoleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "RoleControl";
            this.Size = new System.Drawing.Size(832, 652);
            this.cmsLog.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tcInfo.ResumeLayout(false);
            this.tpBag.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpCommon.ResumeLayout(false);
            this.tpCommon.PerformLayout();
            this.tpSellItem.ResumeLayout(false);
            this.tpSellItem.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox rtxtLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnExecWoodMan;
        private Magician.Common.CustomControl.NumberText numWoodManPos;
        private System.Windows.Forms.TabControl tcInfo;
        private System.Windows.Forms.TabPage tpBag;
        private BagItemBox bagItemBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpCommon;
        private System.Windows.Forms.TabPage tpSellItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbAutoSellItem;
        private System.Windows.Forms.Button btnDelSellItem;
        private System.Windows.Forms.Label label2;
        private Magician.Common.CustomControl.NumberText numDropDamage;
        private System.Windows.Forms.Button btnUpdateDropDamage;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chkSellWhenFull;
        private System.Windows.Forms.CheckBox chkAutoSell;
        private System.Windows.Forms.ContextMenuStrip cmsLog;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearLog;
    }
}
