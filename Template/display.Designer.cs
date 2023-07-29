namespace Template
{
    partial class display
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(display));
            this.clocker = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // clocker
            // 
            this.clocker.Enabled = true;
            this.clocker.Interval = 16;
            this.clocker.Tick += new System.EventHandler(this.clocked);
            // 
            // display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "display";
            this.Text = "uoootempletoooo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keydown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyup);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup);
            this.Resize += new System.EventHandler(this.resized);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer clocker;
    }
}

