
namespace WLUtility
{
    partial class FrmAbout
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblGithubUrl = new CCWin.SkinControl.SkinLabel();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.textBoxDescription = new CCWin.SkinControl.SkinTextBox();
            this.labelProductName = new CCWin.SkinControl.SkinLabel();
            this.labelVersion = new CCWin.SkinControl.SkinLabel();
            this.labelCopyright = new CCWin.SkinControl.SkinLabel();
            this.btnOK = new CCWin.SkinControl.SkinButton();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67F));
            this.tableLayoutPanel.Controls.Add(this.lblGithubUrl, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelCopyright, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.btnOK, 1, 5);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(13, 36);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(409, 213);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // lblGithubUrl
            // 
            this.lblGithubUrl.AutoSize = true;
            this.lblGithubUrl.BackColor = System.Drawing.Color.Transparent;
            this.lblGithubUrl.BorderColor = System.Drawing.Color.White;
            this.lblGithubUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGithubUrl.Font = new System.Drawing.Font("微软雅黑", 10.5F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblGithubUrl.Location = new System.Drawing.Point(137, 63);
            this.lblGithubUrl.Name = "lblGithubUrl";
            this.lblGithubUrl.Size = new System.Drawing.Size(269, 21);
            this.lblGithubUrl.TabIndex = 29;
            this.lblGithubUrl.Text = "访问Github";
            this.lblGithubUrl.Click += new System.EventHandler(this.lblGithubUrl_Click);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(3, 3);
            this.logoPictureBox.Name = "logoPictureBox";
            this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 6);
            this.logoPictureBox.Size = new System.Drawing.Size(128, 207);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoPictureBox.TabIndex = 12;
            this.logoPictureBox.TabStop = false;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.BackColor = System.Drawing.Color.Transparent;
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.DownBack = null;
            this.textBoxDescription.Enabled = false;
            this.textBoxDescription.Icon = null;
            this.textBoxDescription.IconIsButton = false;
            this.textBoxDescription.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.textBoxDescription.IsPasswordChat = '\0';
            this.textBoxDescription.IsSystemPasswordChar = false;
            this.textBoxDescription.Lines = new string[] {
        "说明"};
            this.textBoxDescription.Location = new System.Drawing.Point(134, 84);
            this.textBoxDescription.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxDescription.MaxLength = 32767;
            this.textBoxDescription.MinimumSize = new System.Drawing.Size(28, 28);
            this.textBoxDescription.MouseBack = null;
            this.textBoxDescription.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.NormlBack = null;
            this.textBoxDescription.Padding = new System.Windows.Forms.Padding(5);
            this.textBoxDescription.ReadOnly = false;
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxDescription.Size = new System.Drawing.Size(275, 95);
            // 
            // 
            // 
            this.textBoxDescription.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDescription.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.textBoxDescription.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.textBoxDescription.SkinTxt.Multiline = true;
            this.textBoxDescription.SkinTxt.Name = "BaseText";
            this.textBoxDescription.SkinTxt.ReadOnly = true;
            this.textBoxDescription.SkinTxt.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxDescription.SkinTxt.Size = new System.Drawing.Size(265, 85);
            this.textBoxDescription.SkinTxt.TabIndex = 0;
            this.textBoxDescription.SkinTxt.Text = "说明";
            this.textBoxDescription.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.textBoxDescription.SkinTxt.WaterText = "";
            this.textBoxDescription.TabIndex = 25;
            this.textBoxDescription.Text = "说明";
            this.textBoxDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxDescription.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.textBoxDescription.WaterText = "";
            this.textBoxDescription.WordWrap = true;
            // 
            // labelProductName
            // 
            this.labelProductName.AutoSize = true;
            this.labelProductName.BackColor = System.Drawing.Color.Transparent;
            this.labelProductName.BorderColor = System.Drawing.Color.White;
            this.labelProductName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductName.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelProductName.Location = new System.Drawing.Point(137, 0);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(269, 21);
            this.labelProductName.TabIndex = 26;
            this.labelProductName.Text = "产品名称";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.BackColor = System.Drawing.Color.Transparent;
            this.labelVersion.BorderColor = System.Drawing.Color.White;
            this.labelVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVersion.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelVersion.Location = new System.Drawing.Point(137, 21);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(269, 21);
            this.labelVersion.TabIndex = 27;
            this.labelVersion.Text = "版本";
            // 
            // labelCopyright
            // 
            this.labelCopyright.AutoSize = true;
            this.labelCopyright.BackColor = System.Drawing.Color.Transparent;
            this.labelCopyright.BorderColor = System.Drawing.Color.White;
            this.labelCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCopyright.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCopyright.Location = new System.Drawing.Point(137, 42);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(269, 21);
            this.labelCopyright.TabIndex = 28;
            this.labelCopyright.Text = "版权";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOK.DownBack = null;
            this.btnOK.Location = new System.Drawing.Point(331, 182);
            this.btnOK.MouseBack = null;
            this.btnOK.Name = "btnOK";
            this.btnOK.NormlBack = null;
            this.btnOK.Size = new System.Drawing.Size(75, 28);
            this.btnOK.TabIndex = 30;
            this.btnOK.Text = "确定(&O)";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 261);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.Padding = new System.Windows.Forms.Padding(9, 8, 9, 8);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "关于";
            this.TitleOffset = new System.Drawing.Point(0, 2);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private CCWin.SkinControl.SkinTextBox textBoxDescription;
        private CCWin.SkinControl.SkinLabel labelProductName;
        private CCWin.SkinControl.SkinLabel labelVersion;
        private CCWin.SkinControl.SkinLabel labelCopyright;
        private CCWin.SkinControl.SkinLabel lblGithubUrl;
        private CCWin.SkinControl.SkinButton btnOK;
    }
}
