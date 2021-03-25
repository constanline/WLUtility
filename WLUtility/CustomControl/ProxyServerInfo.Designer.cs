
namespace WLUtility.CustomControl
{
    partial class ProxyServerInfo
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
            this.IpRemote = new Magician.Common.CustomControl.IpBox();
            this.TxtRemotePort = new Magician.Common.CustomControl.NumberText();
            this.TxtMinLocalPort = new Magician.Common.CustomControl.NumberText();
            this.IpLocal = new Magician.Common.CustomControl.IpBox();
            this.TxtMaxLocalPort = new Magician.Common.CustomControl.NumberText();
            this.ChkEnabled = new CCWin.SkinControl.SkinCheckBox();
            this.SuspendLayout();
            // 
            // IpRemote
            // 
            this.IpRemote.BackColor = System.Drawing.Color.White;
            this.IpRemote.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IpRemote.Location = new System.Drawing.Point(57, 2);
            this.IpRemote.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.IpRemote.Name = "IpRemote";
            this.IpRemote.Size = new System.Drawing.Size(182, 32);
            this.IpRemote.TabIndex = 1;
            // 
            // TxtRemotePort
            // 
            this.TxtRemotePort.BackColor = System.Drawing.Color.Transparent;
            this.TxtRemotePort.DecimalPlaces = 0;
            this.TxtRemotePort.DownBack = null;
            this.TxtRemotePort.Icon = null;
            this.TxtRemotePort.IconIsButton = false;
            this.TxtRemotePort.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.TxtRemotePort.IsPasswordChat = '\0';
            this.TxtRemotePort.IsSystemPasswordChar = false;
            this.TxtRemotePort.Lines = new string[] {
        "12345"};
            this.TxtRemotePort.Location = new System.Drawing.Point(243, 4);
            this.TxtRemotePort.Margin = new System.Windows.Forms.Padding(0);
            this.TxtRemotePort.MaxLength = 5;
            this.TxtRemotePort.MaxValue = 65535;
            this.TxtRemotePort.MinimumSize = new System.Drawing.Size(28, 28);
            this.TxtRemotePort.MinValue = 1024;
            this.TxtRemotePort.MouseBack = null;
            this.TxtRemotePort.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.TxtRemotePort.Multiline = false;
            this.TxtRemotePort.Name = "TxtRemotePort";
            this.TxtRemotePort.NormlBack = null;
            this.TxtRemotePort.Padding = new System.Windows.Forms.Padding(5);
            this.TxtRemotePort.ReadOnly = false;
            this.TxtRemotePort.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TxtRemotePort.Size = new System.Drawing.Size(50, 28);
            // 
            // 
            // 
            this.TxtRemotePort.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtRemotePort.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtRemotePort.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.TxtRemotePort.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.TxtRemotePort.SkinTxt.MaxLength = 5;
            this.TxtRemotePort.SkinTxt.Name = "BaseText";
            this.TxtRemotePort.SkinTxt.Size = new System.Drawing.Size(40, 18);
            this.TxtRemotePort.SkinTxt.TabIndex = 0;
            this.TxtRemotePort.SkinTxt.Text = "12345";
            this.TxtRemotePort.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.TxtRemotePort.SkinTxt.WaterText = "端口号";
            this.TxtRemotePort.TabIndex = 2;
            this.TxtRemotePort.Text = "12345";
            this.TxtRemotePort.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtRemotePort.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.TxtRemotePort.WaterText = "端口号";
            this.TxtRemotePort.WordWrap = true;
            // 
            // TxtMinLocalPort
            // 
            this.TxtMinLocalPort.BackColor = System.Drawing.Color.Transparent;
            this.TxtMinLocalPort.DecimalPlaces = 0;
            this.TxtMinLocalPort.DownBack = null;
            this.TxtMinLocalPort.Icon = null;
            this.TxtMinLocalPort.IconIsButton = false;
            this.TxtMinLocalPort.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.TxtMinLocalPort.IsPasswordChat = '\0';
            this.TxtMinLocalPort.IsSystemPasswordChar = false;
            this.TxtMinLocalPort.Lines = new string[] {
        "12345"};
            this.TxtMinLocalPort.Location = new System.Drawing.Point(493, 4);
            this.TxtMinLocalPort.Margin = new System.Windows.Forms.Padding(0);
            this.TxtMinLocalPort.MaxLength = 5;
            this.TxtMinLocalPort.MaxValue = 65535;
            this.TxtMinLocalPort.MinimumSize = new System.Drawing.Size(28, 28);
            this.TxtMinLocalPort.MinValue = 1024;
            this.TxtMinLocalPort.MouseBack = null;
            this.TxtMinLocalPort.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.TxtMinLocalPort.Multiline = false;
            this.TxtMinLocalPort.Name = "TxtMinLocalPort";
            this.TxtMinLocalPort.NormlBack = null;
            this.TxtMinLocalPort.Padding = new System.Windows.Forms.Padding(5);
            this.TxtMinLocalPort.ReadOnly = false;
            this.TxtMinLocalPort.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TxtMinLocalPort.Size = new System.Drawing.Size(50, 28);
            // 
            // 
            // 
            this.TxtMinLocalPort.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtMinLocalPort.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtMinLocalPort.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.TxtMinLocalPort.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.TxtMinLocalPort.SkinTxt.MaxLength = 5;
            this.TxtMinLocalPort.SkinTxt.Name = "BaseText";
            this.TxtMinLocalPort.SkinTxt.Size = new System.Drawing.Size(40, 18);
            this.TxtMinLocalPort.SkinTxt.TabIndex = 0;
            this.TxtMinLocalPort.SkinTxt.Text = "12345";
            this.TxtMinLocalPort.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.TxtMinLocalPort.SkinTxt.WaterText = "端口号";
            this.TxtMinLocalPort.TabIndex = 4;
            this.TxtMinLocalPort.Text = "12345";
            this.TxtMinLocalPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtMinLocalPort.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.TxtMinLocalPort.WaterText = "端口号";
            this.TxtMinLocalPort.WordWrap = true;
            // 
            // IpLocal
            // 
            this.IpLocal.BackColor = System.Drawing.Color.White;
            this.IpLocal.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IpLocal.Location = new System.Drawing.Point(307, 2);
            this.IpLocal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.IpLocal.Name = "IpLocal";
            this.IpLocal.Size = new System.Drawing.Size(182, 32);
            this.IpLocal.TabIndex = 3;
            // 
            // TxtMaxLocalPort
            // 
            this.TxtMaxLocalPort.BackColor = System.Drawing.Color.Transparent;
            this.TxtMaxLocalPort.DecimalPlaces = 0;
            this.TxtMaxLocalPort.DownBack = null;
            this.TxtMaxLocalPort.Icon = null;
            this.TxtMaxLocalPort.IconIsButton = false;
            this.TxtMaxLocalPort.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.TxtMaxLocalPort.IsPasswordChat = '\0';
            this.TxtMaxLocalPort.IsSystemPasswordChar = false;
            this.TxtMaxLocalPort.Lines = new string[] {
        "12345"};
            this.TxtMaxLocalPort.Location = new System.Drawing.Point(553, 4);
            this.TxtMaxLocalPort.Margin = new System.Windows.Forms.Padding(0);
            this.TxtMaxLocalPort.MaxLength = 5;
            this.TxtMaxLocalPort.MaxValue = 65535;
            this.TxtMaxLocalPort.MinimumSize = new System.Drawing.Size(28, 28);
            this.TxtMaxLocalPort.MinValue = 1024;
            this.TxtMaxLocalPort.MouseBack = null;
            this.TxtMaxLocalPort.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.TxtMaxLocalPort.Multiline = false;
            this.TxtMaxLocalPort.Name = "TxtMaxLocalPort";
            this.TxtMaxLocalPort.NormlBack = null;
            this.TxtMaxLocalPort.Padding = new System.Windows.Forms.Padding(5);
            this.TxtMaxLocalPort.ReadOnly = false;
            this.TxtMaxLocalPort.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TxtMaxLocalPort.Size = new System.Drawing.Size(50, 28);
            // 
            // 
            // 
            this.TxtMaxLocalPort.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtMaxLocalPort.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtMaxLocalPort.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.TxtMaxLocalPort.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.TxtMaxLocalPort.SkinTxt.MaxLength = 5;
            this.TxtMaxLocalPort.SkinTxt.Name = "BaseText";
            this.TxtMaxLocalPort.SkinTxt.Size = new System.Drawing.Size(40, 18);
            this.TxtMaxLocalPort.SkinTxt.TabIndex = 0;
            this.TxtMaxLocalPort.SkinTxt.Text = "12345";
            this.TxtMaxLocalPort.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.TxtMaxLocalPort.SkinTxt.WaterText = "端口号";
            this.TxtMaxLocalPort.TabIndex = 5;
            this.TxtMaxLocalPort.Text = "12345";
            this.TxtMaxLocalPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtMaxLocalPort.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.TxtMaxLocalPort.WaterText = "端口号";
            this.TxtMaxLocalPort.WordWrap = true;
            // 
            // ChkEnabled
            // 
            this.ChkEnabled.AutoSize = true;
            this.ChkEnabled.BackColor = System.Drawing.Color.Transparent;
            this.ChkEnabled.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.ChkEnabled.DownBack = null;
            this.ChkEnabled.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChkEnabled.Location = new System.Drawing.Point(6, 7);
            this.ChkEnabled.MouseBack = null;
            this.ChkEnabled.Name = "ChkEnabled";
            this.ChkEnabled.NormlBack = null;
            this.ChkEnabled.SelectedDownBack = null;
            this.ChkEnabled.SelectedMouseBack = null;
            this.ChkEnabled.SelectedNormlBack = null;
            this.ChkEnabled.Size = new System.Drawing.Size(51, 21);
            this.ChkEnabled.TabIndex = 7;
            this.ChkEnabled.Text = "启用";
            this.ChkEnabled.UseVisualStyleBackColor = false;
            this.ChkEnabled.CheckedChanged += new System.EventHandler(this.ChkEnabled_CheckedChanged);
            // 
            // ProxyServerInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ChkEnabled);
            this.Controls.Add(this.TxtMaxLocalPort);
            this.Controls.Add(this.TxtMinLocalPort);
            this.Controls.Add(this.IpLocal);
            this.Controls.Add(this.TxtRemotePort);
            this.Controls.Add(this.IpRemote);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ProxyServerInfo";
            this.Size = new System.Drawing.Size(616, 36);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Magician.Common.CustomControl.IpBox IpRemote;
        private Magician.Common.CustomControl.NumberText TxtRemotePort;
        private Magician.Common.CustomControl.NumberText TxtMinLocalPort;
        private Magician.Common.CustomControl.IpBox IpLocal;
        private Magician.Common.CustomControl.NumberText TxtMaxLocalPort;
        private CCWin.SkinControl.SkinCheckBox ChkEnabled;
    }
}
