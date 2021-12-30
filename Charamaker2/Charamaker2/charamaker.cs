using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Charamaker2.Character;

namespace Charamaker2
{

    public partial class charamaker : UserControl
    {

    
        public character sel;
        setu rootselll;
        public setu selll;
        disp display;
        picture selpoi = new picture(0, 0, 10000, 4, 4, 2, 2, 0, true, 0.8f, "def", new Dictionary<string, string>{{"def","yellowbit"}, { "def2", "whitebit" } });
        picture rootpoi = new picture(0, 0, 10000, 4, 4, 2, 2, 0, false, 0.5f, "def", new Dictionary<string, string> { { "def", "redbit" },{ "def2", "blackbit" } });
        movie.Movie movie = null;
        hyojiman hyojiman;
        character loaded;
        List<motionmaker> makers = new List<motionmaker>();
        public charamaker(disp f)
        {
            InitializeComponent();
            display = f;
            int w = f.Size.Width;
            int h = f.Size.Height;
            
            this.Size = new Size(w, h);

            fileman.setinguping(this);

            hyojiman = fileman.makehyojiman();

            {

                var temp = new character();
                temp.x = 20;
                temp.y = 20;
                temp.w = 20;
                temp.h = 66;
                temp.tx = 10;
                temp.ty = 19;


                {
                    temp.core = new setu("body", 0, 0, new picture(0, 0, 0, 20, 17, 20 / 2f, 20 / 17f, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/body" } }), new List<setu>());
                    temp.core.GetSetu("body").sts.Add(new setu("neck", 11, 2, new picture(0, 0, -2, 5, 4, 2.5f, 3, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/neck" } }), new List<setu>()));
                    temp.core.GetSetu("neck").sts.Add(new setu("head", 2.5f, 0, new picture(0, 0, -1, 22, 22, 13, 19, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/head" },{"sleepy","yoshino/headsleepy" } }), new List<setu>()));
                    temp.core.GetSetu("head").sts.Add(new setu("hair", 11, 11, new picture(0, 0, -3, 22, 22, 11, 11, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/hair" } }), new List<setu>()));
                    temp.core.GetSetu("body").sts.Add(new setu("waist", 12, 16, new picture(0, 0, -1, 19, 22, 11, 1, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/waist" } }), new List<setu>()));
                    temp.core.GetSetu("waist").sts.Add(new setu("hip", 11, 1, new picture(0, 0, -2, 19, 22, 11, 1, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/hip" } }), new List<setu>()));
                    temp.core.GetSetu("body").sts.Add(new setu("rarm", 17, 5, new picture(0, 0, -4, 4, 14, 2, 2, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/rarm" } }), new List<setu>()));
                    temp.core.GetSetu("body").sts.Add(new setu("larm", 3, 5, new picture(0, 0, 4, 4, 14, 3, 2, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/larm" } }), new List<setu>()));
                    temp.core.GetSetu("rarm").sts.Add(new setu("rhand", 3, 14, new picture(0, 0, -3, 7, 14, 4, 1, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/rhand" } }), new List<setu>()));
                    temp.core.GetSetu("larm").sts.Add(new setu("lhand", 2, 14, new picture(0, 0, 6, 7, 14, 4, 1, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/lhand" } }), new List<setu>()));
                    temp.core.GetSetu("lhand").sts.Add(new setu("lwep", 3, 12, new picture(0, 0, 5, 64, 14, 16, 7, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/onsa" } }), new List<setu>()));
                    temp.core.GetSetu("waist").sts.Add(new setu("rleg", 15.5f, 7.5f, new picture(0, 0, 2, 6, 16, 2, 1, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/rleg" } }), new List<setu>()));
                    temp.core.GetSetu("waist").sts.Add(new setu("lleg", 5.5f, 7.5f, new picture(0, 0, 4, 7, 16, 5, 1, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/lleg" } }), new List<setu>()));
                    temp.core.GetSetu("rleg").sts.Add(new setu("rfoot", 1, 12, new picture(0, 0, 1, 5, 16, 1, 1, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/rfoot" } }), new List<setu>()));
                    temp.core.GetSetu("lleg").sts.Add(new setu("lfoot", 5, 12, new picture(0, 0, 3, 6, 16, 3, 1, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yoshino/lfoot" } }), new List<setu>()));

                }




                temp.resethyoji(hyojiman);
                sentaku(temp);
                loaded = new character(temp);
            }
            hyojiman.addpicture(selpoi);
            hyojiman.addpicture(rootpoi);
            hyojiman.resetpics();

         
            haiti();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }


        private void haiti()
        {
            pointcb.Location = new Point(this.Size.Width - 300, 0);
            hyojibairituud.Location = new Point(this.Size.Width-150, 0);
            setubox.Location = new Point(this.Size.Width - 300, 40);
            seturemvb.Location = new Point(this.Size.Width - 150, 40);
            newsetubox.Location = new Point(this.Size.Width - 300, 80);
            setuaddb.Location = new Point(this.Size.Width - 150, 80);
            nmchangeb.Location = new Point(this.Size.Width - 100, 80);
            dxbox.Location = new Point(this.Size.Width - 300, 120);
            dybox.Location = new Point(this.Size.Width - 150, 120);
            wbox.Location = new Point(this.Size.Width - 300, 160);
            hbox.Location = new Point(this.Size.Width - 150, 160);
            txbox.Location = new Point(this.Size.Width - 300, 200);
            tybox.Location = new Point(this.Size.Width - 150, 200);
            radbox.Location = new Point(this.Size.Width - 300, 240);
            opabox.Location = new Point(this.Size.Width - 150, 240);
            texturebox.Location = new Point(this.Size.Width - 300, 280);
            zbox.Location = new Point(this.Size.Width - 150, 280);
            texsbox.Location = new Point(this.Size.Width - 300, 320);

            texremb.Location = new Point(this.Size.Width - 150, 320);
            addtexnamebox.Location = new Point(this.Size.Width - 300, 400);
            texaddb.Location = new Point(this.Size.Width - 150, 400);
            moviebutton.Location = new Point(this.Size.Width - 150, 400);
            texpathbox.Location = new Point(this.Size.Width - 300, 360);
            moviebutton.Location = new Point(this.Size.Width - 150, 360);

            mirrorcheck.Location = new Point(this.Size.Width - 300, 440);


            kijyunb.Location = new Point(this.Size.Width - 300, 520);
            refreshb.Location = new Point(this.Size.Width - 150, 520);

            resetmotionb.Location = new Point(this.Size.Width - 330, 560);
            charareset.Location = new Point(this.Size.Width - 230, 560);
            motionmakerb.Location = new Point(this.Size.Width - 150, 560);
            
            loadb.Location = new Point(this.Size.Width - 300, 600);
            saveb.Location = new Point(this.Size.Width - 150, 600);
            

        }
        private void sentaku(character c)
        {

            sel = c;
            setubox.Items.Clear();
            foreach (var a in c.core.getallsetu())
            {
                setubox.Items.Add(a.nm);
            }
            setubox.Items.Add("");
           
        }
        private void setuselect(string n) 
        {

            selll = sel.core.GetSetu(n);
            rootselll = sel.core.Getrootsetu(n);
            if (selll != null)
            {

                dxbox.Text = selll.dx.ToString();
                dybox.Text = selll.dy.ToString();
                wbox.Text = selll.p.w.ToString();
                hbox.Text = selll.p.h.ToString();
                txbox.Text = selll.p.tx.ToString();
                tybox.Text = selll.p.ty.ToString();
                radbox.Value = (decimal)(selll.p.RAD / Math.PI * 180);
                opabox.Text = selll.p.OPA.ToString();
                texturebox.Text = selll.p.texname;
                zbox.Text = selll.p.z.ToString();

                texsbox.Items.Clear();
                foreach (var a in selll.p.textures)
                {
                    texsbox.Items.Add(a.Key);
                }
                addtexnamebox.Text = "";
                texpathbox.Text = "";
                mirrorcheck.Checked = selll.p.mir;


            }
            else 
            {
                dxbox.Text = sel.x.ToString();
                dybox.Text = sel.y.ToString();
                wbox.Text = sel.w.ToString();
                hbox.Text = sel.h.ToString();
                txbox.Text = sel.tx.ToString();
                tybox.Text = sel.ty.ToString();
                radbox.Value = (decimal)(sel.RAD/Math.PI*180);
                opabox.Text = "ナシゴレン";
                texturebox.Text = "ナシゴレン";
                zbox.Text = "ナシゴレン";

                texsbox.Items.Clear();
               
                addtexnamebox.Text = "ナシゴレン";
                texpathbox.Text = "ナシゴレン";

                mirrorcheck.Checked = sel.mirror;

            }
            if (rootselll != null) newsetubox.Text = rootselll.nm;
        }

        private void frame(object sender, EventArgs e)
        {
          
            //pic.rad += 0.01;
            if (movie == null || movie.ended)
            {
                
                sel.frame();
                if (selll != null)
                {
                    selpoi.RAD = selll.p.RAD;
                    selpoi.settxy(selll.p.gettx(), selll.p.getty());

                }
                else
                {
                    selpoi.RAD = sel.RAD;
                    selpoi.settxy(sel.x + sel.tx * (float)Math.Cos(sel.RAD) - sel.ty * (float)Math.Sin(sel.RAD), sel.y + sel.tx * (float)Math.Sin(sel.RAD) + sel.ty * (float)Math.Cos(sel.RAD));
                }
                if (rootselll != null)
                {
                    rootpoi.w = 4;
                    rootpoi.h = 4;
                    rootpoi.tx = 2;
                    rootpoi.ty = 2;
                    rootpoi.RAD = rootselll.p.RAD;
                    rootpoi.settxy(rootselll.p.gettx(), rootselll.p.getty());

                }
                else
                {
                    rootpoi.w = sel.w;
                    rootpoi.h = sel.h;
                    rootpoi.tx = sel.tx;
                    rootpoi.ty = sel.ty;
                    rootpoi.RAD = sel.RAD;
                    rootpoi.settxy(sel.x + sel.tx * (float)Math.Cos(sel.RAD) - sel.ty * (float)Math.Sin(sel.RAD), sel.y + sel.tx * (float)Math.Sin(sel.RAD) + sel.ty * (float)Math.Cos(sel.RAD));
                }

                hyojiman.hyoji();
                hyojiman.resetpics();
            }
            else 
            {
                movie.frame();
            }
        }

        private void selectchange(object sender, EventArgs e)
        {
            setuselect(((ComboBox)sender).Text);
        }

        private void dxchange(object sender, EventArgs e)
        {
            if (selll != null)
            {
                selll.dx = (float)((NumericUpDown)sender).Value;
            }
            else
            {

                
                sel.x = (float)((NumericUpDown)sender).Value;


            }
        }
        private void dychange(object sender, EventArgs e)
        {
            if (selll != null)
            {
               

                    selll.dy = (float)((NumericUpDown)sender).Value;
            }
            else
            {

              
                    sel.y = (float)((NumericUpDown)sender).Value; ;

            }
        }

        private void wchange(object sender, EventArgs e)
        {
            if (selll != null)
            {
        
                    selll.p.w = (float)((NumericUpDown)sender).Value; ;
            }
            else 
            {

                    sel.w = (float)((NumericUpDown)sender).Value; ;

            }
        }

        private void hchange(object sender, EventArgs e)
        {
            if (selll != null)
            {
               
                    selll.p.h = (float)((NumericUpDown)sender).Value; ;
            }
            else
            {

             
                    sel.h = (float)((NumericUpDown)sender).Value; ;

            }
        }

        private void txchange(object sender, EventArgs e)
        {
            if (selll != null)
            {
          
                    selll.p.tx = (float)((NumericUpDown)sender).Value; ;
            }
            else
            {

                    sel.tx = (float)((NumericUpDown)sender).Value; ;

            }
        }

        private void tychange(object sender, EventArgs e)
        {
            if (selll != null)
            {
              
                    selll.p.ty = (float)((NumericUpDown)sender).Value;
            }
            else
            {

              
                    sel.ty = (float)((NumericUpDown)sender).Value;

            }
        }

        private void radchange(object sender, EventArgs e)
        {
            if (selll != null)
            {
             
                    selll.p.RAD = (double)((NumericUpDown)sender).Value*Math.PI/180; 
            }
            else
            {
              
                    sel.RAD = (float)((NumericUpDown)sender).Value * Math.PI / 180;

            }
        }

        private void opachange(object sender, EventArgs e)
        {
            if (selll != null)
            {
               
                    selll.p.OPA = (float)((NumericUpDown)sender).Value;
            }
        }

        private void texturechange(object sender, EventArgs e)
        {
            if (selll != null)
            {
                selll.p.texname= ((TextBox)sender).Text;
            }
        }

        private void zchange(object sender, EventArgs e)
        {
            if (selll != null)
            {
        
                    selll.p.z = (float)((NumericUpDown)sender).Value;
                    sel.resethyoji(hyojiman);
                
            }
        }

        private void removetexs(object sender, MouseEventArgs e)
        {
            if (selll != null) 
            {

                selll.p.textures.Remove(texsbox.Text);
                texsbox.Items.Clear();
                foreach (var a in selll.p.textures)
                {
                    texsbox.Items.Add(a.Key);
                }

            }
        }

        private void texsadd(object sender, MouseEventArgs e)
        {
            if (selll != null) 
            {


                if (selll.p.textures.ContainsKey(addtexnamebox.Text) == false)
                {
                
                    selll.p.textures.Add(addtexnamebox.Text, texpathbox.Text);
                    fileman.ldtex(texpathbox.Text, true);
                    texsbox.Items.Clear();
                    foreach (var a in selll.p.textures)
                    {
                        texsbox.Items.Add(a.Key);
                    }

                }
                else 
                {
                    selll.p.textures[addtexnamebox.Text]= texpathbox.Text;

                }

            }
        }

        private void changetexname(object sender, EventArgs e)
        {
            if (selll != null) 
            {
                
                if (selll.p.textures.ContainsKey(texsbox.Text))
                {
                    texpathbox.Text = selll.p.textures[texsbox.Text];
                    addtexnamebox.Text = texsbox.Text;
                }
            }
        }

        private void seturemove(object sender, MouseEventArgs e)
        {
            sel.core.Removesetu(setubox.Text, hyojiman);
            hyojiman.resetpics();
            sentaku(sel);
        }

        private void addnewsetu(object sender, MouseEventArgs e)
        {
            if (selll != null && newsetubox.Text != "")
            {
                if (sel.GetSetu(newsetubox.Text) == null)
                {
                    selll.sts.Add(new setu(newsetubox.Text, 0, 0, new picture(0, 0, 0, 0, 0, 0, 0, 0, false, 1, "def", new Dictionary<string, string>()), new List<setu>()));
                    sel.resethyoji(hyojiman);
                    sentaku(sel);
                }
            }
            else 
            {
                sel.core.nm = newsetubox.Text;
                sel.resethyoji(hyojiman);
                sentaku(sel);
            }
        }

        private void resetmotionbpress(object sender, MouseEventArgs e)
        {
            sel.resetmotion();
        }

        private void motionmakerbpressed(object sender, MouseEventArgs e)
        {
         var f=   new motionmaker(this);
            f.Show();
            makers.Add(f);
        }

        private void savebpress(object sender, EventArgs e)
        {
            try
            {

                fileman.savecharacter(sel);
            }
            catch { }
        }

        private void loadbpress(object sender, EventArgs e)
        {

            var l = fileman.loadcharacter(true);
            if (l != null)
            {
                var a = l.core.dx;
                sel.sinu(hyojiman);
                sel = l;

                sel.resethyoji(hyojiman);


                loaded = new character(sel);
                sentaku(sel);
                hyojiman.resetpics();
                motionmakerwow();
            }


        }

        private void hyojibairituchange(object sender, EventArgs e)
        {
            hyojiman.bairitu = (float)hyojibairituud.Value;
        }
        bool colchan = false;
        private void pointonoff(object sender, EventArgs e)
        {
            if (pointcb.Checked)
            {
                if (colchan == false)
                {
                    selpoi.texname = "def2";
                    rootpoi.texname = "def2";
                }
                else
                {
                    selpoi.texname = "def";
                    rootpoi.texname = "def";
                }
                colchan = !colchan;
                selpoi.OPA = 0.6f;
                rootpoi.OPA = 0.5f;
            }
            else 
            {
                selpoi.OPA = 0.0f;
                rootpoi.OPA = 0.0f;
            }
        }

        private void nmchange(object sender, MouseEventArgs e)
        {
            if (selll != null) 
            {
                selll.nm = newsetubox.Text;
                sentaku(sel);
            
            }
        }

        private void keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) 
            {
                this.sinu();
            }
            if (e.KeyCode == Keys.Space) 
            {
                if (movie != null) movie.waittimer = 0;
            }
        }
        public void sinu() 
        {
            display.reset();




            this.Dispose();
        }

        private void mousedown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) 
            {
              //  this.sinu();
            }
        }

        private void playmovie(object sender, MouseEventArgs e)
        {
            movie = fileman.loadmovie(texpathbox.Text);
            movie.start(hyojiman);
        }

        private void chararesetcl(object sender, EventArgs e)
        {
            if (loaded != null) 
            {
                sel.resettokijyun();
                /*
                sel.sinu();
                sel = loaded;

                sel.resethyoji();


                sentaku(sel);
                loaded = new character(sel);
                hyojiman.resetpics();
                motionmakerwow();*/
            }
        }
        public void motionmakerwow() 
        {
            for (int i = makers.Count() - 1; i >= 0; i--) 
            {
                if (makers[i] != null && makers[i].Visible)
                {
                    makers[i].changesel(sel);
                }
                else 
                {
                    makers.RemoveAt(i);
                }
            }
        }

        private void kijyunsetclick(object sender, EventArgs e)
        {
            sel.setkijyuns();
        }

        private void mirorchange(object sender, EventArgs e)
        {
            if (selll != null)
            {
                selll.p.mir = mirrorcheck.Checked;
            }
            else 
            {

                sel.mirror = mirrorcheck.Checked;
            }
        }

        private void chararefresh(object sender, EventArgs e)
        {
            sel.refreshtokijyun();
        }
    }
}
