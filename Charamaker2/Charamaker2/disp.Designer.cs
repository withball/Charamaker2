namespace Charamaker2
{
    partial class disp
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(disp));
            this.charamakerb = new System.Windows.Forms.Button();
            this.testrizmb = new System.Windows.Forms.Button();
            this.testbuturib = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ticker = new System.Windows.Forms.Timer(this.components);
            this.letsgobutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gasitubox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.gasitubox)).BeginInit();
            this.SuspendLayout();
            // 
            // charamakerb
            // 
            this.charamakerb.Location = new System.Drawing.Point(209, 3);
            this.charamakerb.Name = "charamakerb";
            this.charamakerb.Size = new System.Drawing.Size(191, 72);
            this.charamakerb.TabIndex = 0;
            this.charamakerb.Text = "charactermaker";
            this.charamakerb.UseVisualStyleBackColor = true;
            this.charamakerb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.charamakerpress);
            // 
            // testrizmb
            // 
            this.testrizmb.Location = new System.Drawing.Point(1385, 124);
            this.testrizmb.Name = "testrizmb";
            this.testrizmb.Size = new System.Drawing.Size(191, 72);
            this.testrizmb.TabIndex = 1;
            this.testrizmb.Text = "testrizm";
            this.testrizmb.UseVisualStyleBackColor = true;
            this.testrizmb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.testrizmpress);
            // 
            // testbuturib
            // 
            this.testbuturib.Location = new System.Drawing.Point(446, 860);
            this.testbuturib.Name = "testbuturib";
            this.testbuturib.Size = new System.Drawing.Size(191, 72);
            this.testbuturib.TabIndex = 2;
            this.testbuturib.Text = "testbuturi";
            this.testbuturib.UseVisualStyleBackColor = true;
            this.testbuturib.MouseDown += new System.Windows.Forms.MouseEventHandler(this.testbuturipress);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(191, 72);
            this.button1.TabIndex = 3;
            this.button1.Text = "test stage";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.teststagestart);
            // 
            // ticker
            // 
            this.ticker.Interval = 16;
            this.ticker.Tick += new System.EventHandler(this.ticked);
            // 
            // letsgobutton
            // 
            this.letsgobutton.Location = new System.Drawing.Point(12, 112);
            this.letsgobutton.Name = "letsgobutton";
            this.letsgobutton.Size = new System.Drawing.Size(191, 72);
            this.letsgobutton.TabIndex = 4;
            this.letsgobutton.Text = "lets go";
            this.letsgobutton.UseVisualStyleBackColor = true;
            this.letsgobutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lets_go);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(308, 203);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(604, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "let\'s go が完全に見えるまでに高さを調整してね。そんでこの辺のボタンは押さないでね";
            // 
            // gasitubox
            // 
            this.gasitubox.DecimalPlaces = 2;
            this.gasitubox.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.gasitubox.InterceptArrowKeys = false;
            this.gasitubox.Location = new System.Drawing.Point(12, 3);
            this.gasitubox.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.gasitubox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.gasitubox.Name = "gasitubox";
            this.gasitubox.Size = new System.Drawing.Size(75, 25);
            this.gasitubox.TabIndex = 6;
            this.gasitubox.TabStop = false;
            this.gasitubox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.gasitubox.ValueChanged += new System.EventHandler(this.gasituchange);
            // 
            // disp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1578, 1000);
            this.Controls.Add(this.gasitubox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.letsgobutton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.testbuturib);
            this.Controls.Add(this.testrizmb);
            this.Controls.Add(this.charamakerb);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "disp";
            this.Text = "Charamaker2";
            this.Load += new System.EventHandler(this.disp_Load);
            this.Shown += new System.EventHandler(this.disp_shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keydown);
            this.Resize += new System.EventHandler(this.resized);
            ((System.ComponentModel.ISupportInitialize)(this.gasitubox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button charamakerb;
        private System.Windows.Forms.Button testrizmb;
        private System.Windows.Forms.Button testbuturib;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer ticker;
        private System.Windows.Forms.Button letsgobutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown gasitubox;
    }
}

