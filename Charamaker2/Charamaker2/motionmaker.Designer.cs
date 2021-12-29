namespace Charamaker2
{
    partial class motionmaker
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
            this.scriptbox = new System.Windows.Forms.TextBox();
            this.applyb = new System.Windows.Forms.Button();
            this.exbox = new System.Windows.Forms.TextBox();
            this.loadb = new System.Windows.Forms.Button();
            this.saveb = new System.Windows.Forms.Button();
            this.loopud = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.loopud)).BeginInit();
            this.SuspendLayout();
            // 
            // scriptbox
            // 
            this.scriptbox.Font = new System.Drawing.Font("MS UI Gothic", 14F);
            this.scriptbox.Location = new System.Drawing.Point(13, 10);
            this.scriptbox.Multiline = true;
            this.scriptbox.Name = "scriptbox";
            this.scriptbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.scriptbox.Size = new System.Drawing.Size(1211, 570);
            this.scriptbox.TabIndex = 0;
            this.scriptbox.TabStop = false;
            this.scriptbox.TextChanged += new System.EventHandler(this.scriptbox_TextChanged);
            // 
            // applyb
            // 
            this.applyb.Location = new System.Drawing.Point(1136, 616);
            this.applyb.Name = "applyb";
            this.applyb.Size = new System.Drawing.Size(88, 35);
            this.applyb.TabIndex = 1;
            this.applyb.Text = "apply";
            this.applyb.UseVisualStyleBackColor = true;
            this.applyb.Click += new System.EventHandler(this.applyb_Click);
            // 
            // exbox
            // 
            this.exbox.Font = new System.Drawing.Font("MS UI Gothic", 14F);
            this.exbox.Location = new System.Drawing.Point(18, 621);
            this.exbox.Multiline = true;
            this.exbox.Name = "exbox";
            this.exbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.exbox.Size = new System.Drawing.Size(1079, 230);
            this.exbox.TabIndex = 2;
            // 
            // loadb
            // 
            this.loadb.Location = new System.Drawing.Point(1136, 802);
            this.loadb.Name = "loadb";
            this.loadb.Size = new System.Drawing.Size(88, 35);
            this.loadb.TabIndex = 3;
            this.loadb.Text = "load";
            this.loadb.UseVisualStyleBackColor = true;
            this.loadb.Click += new System.EventHandler(this.motionload);
            // 
            // saveb
            // 
            this.saveb.Location = new System.Drawing.Point(1136, 713);
            this.saveb.Name = "saveb";
            this.saveb.Size = new System.Drawing.Size(88, 35);
            this.saveb.TabIndex = 4;
            this.saveb.Text = "save";
            this.saveb.UseVisualStyleBackColor = true;
            this.saveb.Click += new System.EventHandler(this.motionsave);
            // 
            // loopud
            // 
            this.loopud.DecimalPlaces = 2;
            this.loopud.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.loopud.Location = new System.Drawing.Point(1104, 668);
            this.loopud.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.loopud.Name = "loopud";
            this.loopud.Size = new System.Drawing.Size(120, 25);
            this.loopud.TabIndex = 5;
            this.loopud.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // motionmaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1236, 890);
            this.Controls.Add(this.loopud);
            this.Controls.Add(this.saveb);
            this.Controls.Add(this.loadb);
            this.Controls.Add(this.exbox);
            this.Controls.Add(this.applyb);
            this.Controls.Add(this.scriptbox);
            this.Name = "motionmaker";
            this.Text = "motionmaker";
            this.Load += new System.EventHandler(this.motionmaker_Load);
            ((System.ComponentModel.ISupportInitialize)(this.loopud)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox scriptbox;
        private System.Windows.Forms.Button applyb;
        private System.Windows.Forms.TextBox exbox;
        private System.Windows.Forms.Button loadb;
        private System.Windows.Forms.Button saveb;
        private System.Windows.Forms.NumericUpDown loopud;
    }
}