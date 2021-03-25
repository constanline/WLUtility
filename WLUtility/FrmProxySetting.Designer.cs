
namespace WLUtility
{
    sealed partial class FrmProxySetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PnlInfo = new CCWin.SkinControl.SkinPanel();
            this.SuspendLayout();
            // 
            // PnlInfo
            // 
            this.PnlInfo.AutoScroll = true;
            this.PnlInfo.BackColor = System.Drawing.Color.Transparent;
            this.PnlInfo.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.PnlInfo.DownBack = null;
            this.PnlInfo.Location = new System.Drawing.Point(7, 67);
            this.PnlInfo.MouseBack = null;
            this.PnlInfo.Name = "PnlInfo";
            this.PnlInfo.NormlBack = null;
            this.PnlInfo.Size = new System.Drawing.Size(667, 376);
            this.PnlInfo.TabIndex = 0;
            // 
            // FrmProxySetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 450);
            this.Controls.Add(this.PnlInfo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProxySetting";
            this.Text = "转发配置";
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinPanel PnlInfo;
    }
}