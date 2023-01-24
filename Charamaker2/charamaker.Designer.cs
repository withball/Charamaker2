namespace Charamaker2.maker
{
    public partial class charamaker
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ticker = new System.Windows.Forms.Timer(this.components);
            this.setubox = new System.Windows.Forms.ComboBox();
            this.texturebox = new System.Windows.Forms.TextBox();
            this.texsbox = new System.Windows.Forms.ComboBox();
            this.addtexnamebox = new System.Windows.Forms.TextBox();
            this.texremb = new System.Windows.Forms.Button();
            this.texaddb = new System.Windows.Forms.Button();
            this.texpathbox = new System.Windows.Forms.TextBox();
            this.seturemvb = new System.Windows.Forms.Button();
            this.setuaddb = new System.Windows.Forms.Button();
            this.newsetubox = new System.Windows.Forms.TextBox();
            this.dxbox = new System.Windows.Forms.NumericUpDown();
            this.zbox = new System.Windows.Forms.NumericUpDown();
            this.opabox = new System.Windows.Forms.NumericUpDown();
            this.tybox = new System.Windows.Forms.NumericUpDown();
            this.txbox = new System.Windows.Forms.NumericUpDown();
            this.hbox = new System.Windows.Forms.NumericUpDown();
            this.wbox = new System.Windows.Forms.NumericUpDown();
            this.dybox = new System.Windows.Forms.NumericUpDown();
            this.radbox = new System.Windows.Forms.NumericUpDown();
            this.resetmotionb = new System.Windows.Forms.Button();
            this.motionmakerb = new System.Windows.Forms.Button();
            this.loadb = new System.Windows.Forms.Button();
            this.saveb = new System.Windows.Forms.Button();
            this.hyojibairituud = new System.Windows.Forms.NumericUpDown();
            this.pointcb = new System.Windows.Forms.CheckBox();
            this.nmchangeb = new System.Windows.Forms.Button();
            this.moviebutton = new System.Windows.Forms.Button();
            this.charareset = new System.Windows.Forms.Button();
            this.mirrorcheck = new System.Windows.Forms.CheckBox();
            this.kijyunb = new System.Windows.Forms.Button();
            this.refreshb = new System.Windows.Forms.Button();
            this.TexSizeLabel = new System.Windows.Forms.Label();
            this.quickloadB = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dxbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.opabox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tybox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dybox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hyojibairituud)).BeginInit();
            this.SuspendLayout();
            // 
            // ticker
            // 
            this.ticker.Enabled = true;
            this.ticker.Interval = 16;
            this.ticker.Tick += new System.EventHandler(this.frame);
            // 
            // setubox
            // 
            this.setubox.FormattingEnabled = true;
            this.setubox.Location = new System.Drawing.Point(859, 20);
            this.setubox.Name = "setubox";
            this.setubox.Size = new System.Drawing.Size(121, 26);
            this.setubox.TabIndex = 0;
            this.setubox.SelectedIndexChanged += new System.EventHandler(this.selectchange);
            // 
            // texturebox
            // 
            this.texturebox.Location = new System.Drawing.Point(859, 314);
            this.texturebox.Name = "texturebox";
            this.texturebox.Size = new System.Drawing.Size(150, 25);
            this.texturebox.TabIndex = 9;
            this.texturebox.TextChanged += new System.EventHandler(this.texturechange);
            // 
            // texsbox
            // 
            this.texsbox.FormattingEnabled = true;
            this.texsbox.Location = new System.Drawing.Point(859, 359);
            this.texsbox.Name = "texsbox";
            this.texsbox.Size = new System.Drawing.Size(171, 26);
            this.texsbox.TabIndex = 11;
            this.texsbox.SelectedIndexChanged += new System.EventHandler(this.changetexname);
            // 
            // addtexnamebox
            // 
            this.addtexnamebox.Location = new System.Drawing.Point(859, 402);
            this.addtexnamebox.Name = "addtexnamebox";
            this.addtexnamebox.Size = new System.Drawing.Size(150, 25);
            this.addtexnamebox.TabIndex = 12;
            // 
            // texremb
            // 
            this.texremb.Location = new System.Drawing.Point(1111, 359);
            this.texremb.Name = "texremb";
            this.texremb.Size = new System.Drawing.Size(75, 34);
            this.texremb.TabIndex = 13;
            this.texremb.Text = "remove";
            this.texremb.UseVisualStyleBackColor = true;
            this.texremb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.removetexs);
            // 
            // texaddb
            // 
            this.texaddb.Location = new System.Drawing.Point(1111, 411);
            this.texaddb.Name = "texaddb";
            this.texaddb.Size = new System.Drawing.Size(75, 34);
            this.texaddb.TabIndex = 14;
            this.texaddb.Text = "add";
            this.texaddb.UseVisualStyleBackColor = true;
            this.texaddb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.texsadd);
            // 
            // texpathbox
            // 
            this.texpathbox.Location = new System.Drawing.Point(859, 433);
            this.texpathbox.Name = "texpathbox";
            this.texpathbox.Size = new System.Drawing.Size(150, 25);
            this.texpathbox.TabIndex = 16;
            // 
            // seturemvb
            // 
            this.seturemvb.Location = new System.Drawing.Point(1086, 15);
            this.seturemvb.Name = "seturemvb";
            this.seturemvb.Size = new System.Drawing.Size(75, 34);
            this.seturemvb.TabIndex = 17;
            this.seturemvb.Text = "remove";
            this.seturemvb.UseVisualStyleBackColor = true;
            this.seturemvb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.seturemove);
            // 
            // setuaddb
            // 
            this.setuaddb.Location = new System.Drawing.Point(1046, 55);
            this.setuaddb.Name = "setuaddb";
            this.setuaddb.Size = new System.Drawing.Size(75, 34);
            this.setuaddb.TabIndex = 18;
            this.setuaddb.Text = "add";
            this.setuaddb.UseVisualStyleBackColor = true;
            this.setuaddb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.addnewsetu);
            // 
            // newsetubox
            // 
            this.newsetubox.Location = new System.Drawing.Point(859, 64);
            this.newsetubox.Name = "newsetubox";
            this.newsetubox.Size = new System.Drawing.Size(150, 25);
            this.newsetubox.TabIndex = 19;
            // 
            // dxbox
            // 
            this.dxbox.DecimalPlaces = 2;
            this.dxbox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.dxbox.Location = new System.Drawing.Point(859, 104);
            this.dxbox.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.dxbox.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.dxbox.Name = "dxbox";
            this.dxbox.Size = new System.Drawing.Size(150, 25);
            this.dxbox.TabIndex = 20;
            this.dxbox.ValueChanged += new System.EventHandler(this.dxchange);
            // 
            // zbox
            // 
            this.zbox.DecimalPlaces = 2;
            this.zbox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.zbox.Location = new System.Drawing.Point(1086, 314);
            this.zbox.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.zbox.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.zbox.Name = "zbox";
            this.zbox.Size = new System.Drawing.Size(150, 25);
            this.zbox.TabIndex = 21;
            this.zbox.ValueChanged += new System.EventHandler(this.zchange);
            // 
            // opabox
            // 
            this.opabox.DecimalPlaces = 4;
            this.opabox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.opabox.Location = new System.Drawing.Point(1086, 262);
            this.opabox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.opabox.Name = "opabox";
            this.opabox.Size = new System.Drawing.Size(150, 25);
            this.opabox.TabIndex = 22;
            this.opabox.ValueChanged += new System.EventHandler(this.opachange);
            // 
            // tybox
            // 
            this.tybox.DecimalPlaces = 2;
            this.tybox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.tybox.Location = new System.Drawing.Point(1086, 210);
            this.tybox.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.tybox.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.tybox.Name = "tybox";
            this.tybox.Size = new System.Drawing.Size(150, 25);
            this.tybox.TabIndex = 23;
            this.tybox.ValueChanged += new System.EventHandler(this.tychange);
            // 
            // txbox
            // 
            this.txbox.DecimalPlaces = 2;
            this.txbox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.txbox.Location = new System.Drawing.Point(859, 210);
            this.txbox.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.txbox.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.txbox.Name = "txbox";
            this.txbox.Size = new System.Drawing.Size(150, 25);
            this.txbox.TabIndex = 24;
            this.txbox.ValueChanged += new System.EventHandler(this.txchange);
            // 
            // hbox
            // 
            this.hbox.DecimalPlaces = 2;
            this.hbox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.hbox.Location = new System.Drawing.Point(1086, 158);
            this.hbox.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.hbox.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.hbox.Name = "hbox";
            this.hbox.Size = new System.Drawing.Size(150, 25);
            this.hbox.TabIndex = 25;
            this.hbox.ValueChanged += new System.EventHandler(this.hchange);
            // 
            // wbox
            // 
            this.wbox.DecimalPlaces = 2;
            this.wbox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.wbox.Location = new System.Drawing.Point(859, 158);
            this.wbox.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.wbox.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.wbox.Name = "wbox";
            this.wbox.Size = new System.Drawing.Size(150, 25);
            this.wbox.TabIndex = 26;
            this.wbox.ValueChanged += new System.EventHandler(this.wchange);
            // 
            // dybox
            // 
            this.dybox.DecimalPlaces = 2;
            this.dybox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.dybox.Location = new System.Drawing.Point(1086, 104);
            this.dybox.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.dybox.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.dybox.Name = "dybox";
            this.dybox.Size = new System.Drawing.Size(150, 25);
            this.dybox.TabIndex = 27;
            this.dybox.ValueChanged += new System.EventHandler(this.dychange);
            // 
            // radbox
            // 
            this.radbox.DecimalPlaces = 4;
            this.radbox.Location = new System.Drawing.Point(859, 262);
            this.radbox.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.radbox.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.radbox.Name = "radbox";
            this.radbox.Size = new System.Drawing.Size(150, 25);
            this.radbox.TabIndex = 28;
            this.radbox.ValueChanged += new System.EventHandler(this.radchange);
            // 
            // resetmotionb
            // 
            this.resetmotionb.Location = new System.Drawing.Point(832, 597);
            this.resetmotionb.Name = "resetmotionb";
            this.resetmotionb.Size = new System.Drawing.Size(121, 36);
            this.resetmotionb.TabIndex = 29;
            this.resetmotionb.Text = "resetmotion";
            this.resetmotionb.UseVisualStyleBackColor = true;
            this.resetmotionb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.resetmotionbpress);
            // 
            // motionmakerb
            // 
            this.motionmakerb.Location = new System.Drawing.Point(1086, 600);
            this.motionmakerb.Name = "motionmakerb";
            this.motionmakerb.Size = new System.Drawing.Size(121, 33);
            this.motionmakerb.TabIndex = 30;
            this.motionmakerb.Text = "motionmaker";
            this.motionmakerb.UseVisualStyleBackColor = true;
            this.motionmakerb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.motionmakerbpressed);
            // 
            // loadb
            // 
            this.loadb.Location = new System.Drawing.Point(859, 654);
            this.loadb.Name = "loadb";
            this.loadb.Size = new System.Drawing.Size(121, 36);
            this.loadb.TabIndex = 31;
            this.loadb.Text = "LOAD";
            this.loadb.UseVisualStyleBackColor = true;
            this.loadb.Click += new System.EventHandler(this.loadbpress);
            // 
            // saveb
            // 
            this.saveb.Location = new System.Drawing.Point(1086, 654);
            this.saveb.Name = "saveb";
            this.saveb.Size = new System.Drawing.Size(121, 36);
            this.saveb.TabIndex = 32;
            this.saveb.Text = "SAVE";
            this.saveb.UseVisualStyleBackColor = true;
            this.saveb.Click += new System.EventHandler(this.savebpress);
            // 
            // hyojibairituud
            // 
            this.hyojibairituud.DecimalPlaces = 1;
            this.hyojibairituud.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.hyojibairituud.Location = new System.Drawing.Point(1057, 0);
            this.hyojibairituud.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.hyojibairituud.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.hyojibairituud.Name = "hyojibairituud";
            this.hyojibairituud.Size = new System.Drawing.Size(150, 25);
            this.hyojibairituud.TabIndex = 33;
            this.hyojibairituud.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.hyojibairituud.ValueChanged += new System.EventHandler(this.hyojibairituchange);
            // 
            // pointcb
            // 
            this.pointcb.AutoSize = true;
            this.pointcb.Checked = true;
            this.pointcb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pointcb.Location = new System.Drawing.Point(859, 3);
            this.pointcb.Name = "pointcb";
            this.pointcb.Size = new System.Drawing.Size(71, 22);
            this.pointcb.TabIndex = 34;
            this.pointcb.Text = "point";
            this.pointcb.UseVisualStyleBackColor = true;
            this.pointcb.CheckStateChanged += new System.EventHandler(this.pointonoff);
            // 
            // nmchangeb
            // 
            this.nmchangeb.Location = new System.Drawing.Point(1134, 55);
            this.nmchangeb.Name = "nmchangeb";
            this.nmchangeb.Size = new System.Drawing.Size(75, 34);
            this.nmchangeb.TabIndex = 35;
            this.nmchangeb.Text = "change";
            this.nmchangeb.UseVisualStyleBackColor = true;
            this.nmchangeb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.nmchange);
            // 
            // moviebutton
            // 
            this.moviebutton.Location = new System.Drawing.Point(1111, 451);
            this.moviebutton.Name = "moviebutton";
            this.moviebutton.Size = new System.Drawing.Size(75, 34);
            this.moviebutton.TabIndex = 36;
            this.moviebutton.Text = "movie";
            this.moviebutton.UseVisualStyleBackColor = true;
            this.moviebutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.playmovie);
            // 
            // charareset
            // 
            this.charareset.Location = new System.Drawing.Point(959, 600);
            this.charareset.Name = "charareset";
            this.charareset.Size = new System.Drawing.Size(121, 33);
            this.charareset.TabIndex = 37;
            this.charareset.Text = "charareset";
            this.charareset.UseVisualStyleBackColor = true;
            this.charareset.Click += new System.EventHandler(this.chararesetcl);
            // 
            // mirrorcheck
            // 
            this.mirrorcheck.AutoSize = true;
            this.mirrorcheck.Location = new System.Drawing.Point(859, 493);
            this.mirrorcheck.Name = "mirrorcheck";
            this.mirrorcheck.Size = new System.Drawing.Size(78, 22);
            this.mirrorcheck.TabIndex = 38;
            this.mirrorcheck.Text = "mirror";
            this.mirrorcheck.UseVisualStyleBackColor = true;
            this.mirrorcheck.CheckedChanged += new System.EventHandler(this.mirorchange);
            // 
            // kijyunb
            // 
            this.kijyunb.Location = new System.Drawing.Point(920, 542);
            this.kijyunb.Name = "kijyunb";
            this.kijyunb.Size = new System.Drawing.Size(89, 31);
            this.kijyunb.TabIndex = 39;
            this.kijyunb.Text = "setkijyun";
            this.kijyunb.UseVisualStyleBackColor = true;
            this.kijyunb.Click += new System.EventHandler(this.kijyunsetclick);
            // 
            // refreshb
            // 
            this.refreshb.Location = new System.Drawing.Point(1032, 542);
            this.refreshb.Name = "refreshb";
            this.refreshb.Size = new System.Drawing.Size(89, 31);
            this.refreshb.TabIndex = 40;
            this.refreshb.Text = "refresh";
            this.refreshb.UseVisualStyleBackColor = true;
            this.refreshb.Click += new System.EventHandler(this.chararefresh);
            // 
            // TexSizeLabel
            // 
            this.TexSizeLabel.AutoSize = true;
            this.TexSizeLabel.Location = new System.Drawing.Point(1043, 493);
            this.TexSizeLabel.Name = "TexSizeLabel";
            this.TexSizeLabel.Size = new System.Drawing.Size(30, 18);
            this.TexSizeLabel.TabIndex = 41;
            this.TexSizeLabel.Text = "0,0";
            // 
            // quickloadB
            // 
            this.quickloadB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.quickloadB.Location = new System.Drawing.Point(46, 600);
            this.quickloadB.Name = "quickloadB";
            this.quickloadB.Size = new System.Drawing.Size(671, 25);
            this.quickloadB.TabIndex = 42;
            this.quickloadB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.quickload);
            // 
            // charamaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.quickloadB);
            this.Controls.Add(this.TexSizeLabel);
            this.Controls.Add(this.refreshb);
            this.Controls.Add(this.kijyunb);
            this.Controls.Add(this.mirrorcheck);
            this.Controls.Add(this.charareset);
            this.Controls.Add(this.moviebutton);
            this.Controls.Add(this.nmchangeb);
            this.Controls.Add(this.pointcb);
            this.Controls.Add(this.hyojibairituud);
            this.Controls.Add(this.saveb);
            this.Controls.Add(this.loadb);
            this.Controls.Add(this.motionmakerb);
            this.Controls.Add(this.resetmotionb);
            this.Controls.Add(this.radbox);
            this.Controls.Add(this.dybox);
            this.Controls.Add(this.wbox);
            this.Controls.Add(this.hbox);
            this.Controls.Add(this.txbox);
            this.Controls.Add(this.tybox);
            this.Controls.Add(this.opabox);
            this.Controls.Add(this.zbox);
            this.Controls.Add(this.dxbox);
            this.Controls.Add(this.newsetubox);
            this.Controls.Add(this.setuaddb);
            this.Controls.Add(this.seturemvb);
            this.Controls.Add(this.texpathbox);
            this.Controls.Add(this.texaddb);
            this.Controls.Add(this.texremb);
            this.Controls.Add(this.addtexnamebox);
            this.Controls.Add(this.texsbox);
            this.Controls.Add(this.texturebox);
            this.Controls.Add(this.setubox);
            this.MaximumSize = new System.Drawing.Size(3000, 3000);
            this.Name = "charamaker";
            this.Size = new System.Drawing.Size(1212, 693);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keydown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown);
            ((System.ComponentModel.ISupportInitialize)(this.dxbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.opabox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tybox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dybox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hyojibairituud)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer ticker;
        private System.Windows.Forms.ComboBox setubox;
        private System.Windows.Forms.TextBox texturebox;
        private System.Windows.Forms.ComboBox texsbox;
        private System.Windows.Forms.TextBox addtexnamebox;
        private System.Windows.Forms.Button texremb;
        private System.Windows.Forms.Button texaddb;
        private System.Windows.Forms.TextBox texpathbox;
        private System.Windows.Forms.Button seturemvb;
        private System.Windows.Forms.Button setuaddb;
        private System.Windows.Forms.TextBox newsetubox;
        private System.Windows.Forms.NumericUpDown dxbox;
        private System.Windows.Forms.NumericUpDown zbox;
        private System.Windows.Forms.NumericUpDown opabox;
        private System.Windows.Forms.NumericUpDown tybox;
        private System.Windows.Forms.NumericUpDown txbox;
        private System.Windows.Forms.NumericUpDown hbox;
        private System.Windows.Forms.NumericUpDown wbox;
        private System.Windows.Forms.NumericUpDown dybox;
        private System.Windows.Forms.NumericUpDown radbox;
        private System.Windows.Forms.Button resetmotionb;
        private System.Windows.Forms.Button motionmakerb;
        private System.Windows.Forms.Button loadb;
        private System.Windows.Forms.Button saveb;
        private System.Windows.Forms.NumericUpDown hyojibairituud;
        private System.Windows.Forms.CheckBox pointcb;
        private System.Windows.Forms.Button nmchangeb;
        private System.Windows.Forms.Button moviebutton;
        private System.Windows.Forms.Button charareset;
        private System.Windows.Forms.CheckBox mirrorcheck;
        private System.Windows.Forms.Button kijyunb;
        private System.Windows.Forms.Button refreshb;
        private System.Windows.Forms.Label TexSizeLabel;
        private System.Windows.Forms.TextBox quickloadB;
    }
}
