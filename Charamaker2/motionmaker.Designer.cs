namespace Charamaker2.maker
{
    public partial class motionmaker
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
            this.getsetusB = new System.Windows.Forms.Button();
            this.quickload = new System.Windows.Forms.TextBox();
            this.timelabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.loopud)).BeginInit();
            this.SuspendLayout();
            // 
            // scriptbox
            // 
            this.scriptbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.applyb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.applyb.Location = new System.Drawing.Point(1136, 628);
            this.applyb.Name = "applyb";
            this.applyb.Size = new System.Drawing.Size(88, 35);
            this.applyb.TabIndex = 1;
            this.applyb.Text = "apply";
            this.applyb.UseVisualStyleBackColor = true;
            this.applyb.Click += new System.EventHandler(this.applyb_Click);
            // 
            // exbox
            // 
            this.exbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exbox.Font = new System.Drawing.Font("MS UI Gothic", 14F);
            this.exbox.Location = new System.Drawing.Point(19, 648);
            this.exbox.Multiline = true;
            this.exbox.Name = "exbox";
            this.exbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.exbox.Size = new System.Drawing.Size(1079, 230);
            this.exbox.TabIndex = 2;
            // 
            // loadb
            // 
            this.loadb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadb.Location = new System.Drawing.Point(1136, 777);
            this.loadb.Name = "loadb";
            this.loadb.Size = new System.Drawing.Size(88, 35);
            this.loadb.TabIndex = 3;
            this.loadb.Text = "load";
            this.loadb.UseVisualStyleBackColor = true;
            this.loadb.Click += new System.EventHandler(this.motionload);
            // 
            // saveb
            // 
            this.saveb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveb.Location = new System.Drawing.Point(1136, 726);
            this.saveb.Name = "saveb";
            this.saveb.Size = new System.Drawing.Size(88, 35);
            this.saveb.TabIndex = 4;
            this.saveb.Text = "save";
            this.saveb.UseVisualStyleBackColor = true;
            this.saveb.Click += new System.EventHandler(this.motionsave);
            // 
            // loopud
            // 
            this.loopud.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loopud.DecimalPlaces = 2;
            this.loopud.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.loopud.Location = new System.Drawing.Point(1104, 686);
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
            // getsetusB
            // 
            this.getsetusB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.getsetusB.Location = new System.Drawing.Point(1136, 838);
            this.getsetusB.Name = "getsetusB";
            this.getsetusB.Size = new System.Drawing.Size(88, 40);
            this.getsetusB.TabIndex = 6;
            this.getsetusB.Text = "getsetus";
            this.getsetusB.UseVisualStyleBackColor = true;
            this.getsetusB.Click += new System.EventHandler(this.getsetusB_Click);
            // 
            // quickload
            // 
            this.quickload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.quickload.Location = new System.Drawing.Point(19, 597);
            this.quickload.Name = "quickload";
            this.quickload.Size = new System.Drawing.Size(1079, 25);
            this.quickload.TabIndex = 7;
            this.quickload.KeyDown += new System.Windows.Forms.KeyEventHandler(this.quickmotionload);
            // 
            // timelabel
            // 
            this.timelabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timelabel.AutoSize = true;
            this.timelabel.Location = new System.Drawing.Point(1121, 597);
            this.timelabel.Name = "timelabel";
            this.timelabel.Size = new System.Drawing.Size(0, 18);
            this.timelabel.TabIndex = 8;
            // 
            // motionmaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1236, 890);
            this.Controls.Add(this.timelabel);
            this.Controls.Add(this.quickload);
            this.Controls.Add(this.getsetusB);
            this.Controls.Add(this.loopud);
            this.Controls.Add(this.saveb);
            this.Controls.Add(this.loadb);
            this.Controls.Add(this.exbox);
            this.Controls.Add(this.applyb);
            this.Controls.Add(this.scriptbox);
            this.Name = "motionmaker";
            this.Text = "motionmaker";
            this.Load += new System.EventHandler(this.motionmaker_Load);
            this.Resize += new System.EventHandler(this.resized);
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
        private System.Windows.Forms.Button getsetusB;
        private System.Windows.Forms.TextBox quickload;
        private System.Windows.Forms.Label timelabel;
    }
}