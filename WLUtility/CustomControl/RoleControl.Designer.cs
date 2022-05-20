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
            this.rtxtLog = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbWoodMan = new System.Windows.Forms.RadioButton();
            this.btnExec = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.num1 = new Magician.Common.CustomControl.NumberText();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtxtLog
            // 
            this.rtxtLog.BackColor = System.Drawing.SystemColors.Window;
            this.rtxtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtLog.Location = new System.Drawing.Point(4, 24);
            this.rtxtLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtxtLog.Name = "rtxtLog";
            this.rtxtLog.ReadOnly = true;
            this.rtxtLog.Size = new System.Drawing.Size(565, 283);
            this.rtxtLog.TabIndex = 3;
            this.rtxtLog.Text = "";
            // 
            // groupBox1
            // 
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
            // groupBox2
            // 
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbWoodMan);
            this.groupBox3.Controls.Add(this.btnExec);
            this.groupBox3.Controls.Add(this.num1);
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
            // rbWoodMan
            // 
            this.rbWoodMan.AutoSize = true;
            this.rbWoodMan.Location = new System.Drawing.Point(140, 31);
            this.rbWoodMan.Name = "rbWoodMan";
            this.rbWoodMan.Size = new System.Drawing.Size(69, 24);
            this.rbWoodMan.TabIndex = 2;
            this.rbWoodMan.TabStop = true;
            this.rbWoodMan.Text = "木人桩";
            this.rbWoodMan.UseVisualStyleBackColor = true;
            // 
            // btnExec
            // 
            this.btnExec.Location = new System.Drawing.Point(64, 29);
            this.btnExec.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExec.Name = "btnExec";
            this.btnExec.Size = new System.Drawing.Size(59, 27);
            this.btnExec.TabIndex = 1;
            this.btnExec.Text = "执行";
            this.btnExec.UseVisualStyleBackColor = true;
            this.btnExec.Click += new System.EventHandler(this.btnExec_Click);
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
            // num1
            // 
            this.num1.BackColor = System.Drawing.Color.Transparent;
            this.num1.DecimalPlaces = 0;
            this.num1.DownBack = null;
            this.num1.Icon = null;
            this.num1.IconIsButton = false;
            this.num1.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.num1.IsPasswordChat = '\0';
            this.num1.IsSystemPasswordChar = false;
            this.num1.Lines = new string[0];
            this.num1.Location = new System.Drawing.Point(16, 28);
            this.num1.Margin = new System.Windows.Forms.Padding(0);
            this.num1.MaxLength = 0;
            this.num1.MaxValue = 100;
            this.num1.MinimumSize = new System.Drawing.Size(37, 27);
            this.num1.MinValue = 1;
            this.num1.MouseBack = null;
            this.num1.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.num1.Multiline = true;
            this.num1.Name = "num1";
            this.num1.NormlBack = null;
            this.num1.Padding = new System.Windows.Forms.Padding(7, 4, 7, 4);
            this.num1.ReadOnly = false;
            this.num1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.num1.ShowErrorProvider = false;
            this.num1.Size = new System.Drawing.Size(44, 30);
            // 
            // 
            // 
            this.num1.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.num1.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.num1.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.num1.SkinTxt.Location = new System.Drawing.Point(7, 4);
            this.num1.SkinTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.num1.SkinTxt.MaxLength = 255;
            this.num1.SkinTxt.Multiline = true;
            this.num1.SkinTxt.Name = "BaseText";
            this.num1.SkinTxt.Size = new System.Drawing.Size(30, 22);
            this.num1.SkinTxt.TabIndex = 0;
            this.num1.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.num1.SkinTxt.WaterText = "";
            this.num1.TabIndex = 0;
            this.num1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.num1.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.num1.WaterText = "";
            this.num1.WordWrap = true;
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
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox rtxtLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnExec;
        private Magician.Common.CustomControl.NumberText num1;
        private System.Windows.Forms.RadioButton rbWoodMan;
    }
}
