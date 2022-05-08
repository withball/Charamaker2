
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Charamaker2.Character;
using System.IO;

namespace Charamaker2
{

    public partial class disp : Form
    {
        public bool autosave = false;
     
        public disp()
        {
            int tyomosu = 100;
            if (System.IO.File.Exists("難易度.txt"))
            {
                using (var reader = new StreamReader("難易度.txt"))
                {

                    string tyomo = reader.ReadLine();
                    int.TryParse(tyomo, out tyomosu);
                }
            }
           
            InitializeComponent();
          
        }
        public void reset()
        {

            Controls.Clear();

            InitializeComponent();
        }

        public void reboot()
        {
            Application.Restart();
        }
        private void disp_Load(object sender, EventArgs e)
        {
            Controls.Clear();

            this.Size = new Size((int)(970 ), (int)(670));
       

        }
        private void disp_shown(object sender, EventArgs e)
        {
            Controls.Clear();
            Controls.Add(new maker.charamaker(this));
            // gogogo();
        }
       
        


        private void resized(object sender, EventArgs e)
        {

            this.Size = new Size((int)(this.Size.Width), (int)(this.Size.Width * 670.0 / 970));

        }
        private void gasituchange(object sender, EventArgs e)
        {
          
            
        }

        private void testrizmpress(object sender, MouseEventArgs e)
        {
            //  Controls.Clear();
            //  Controls.Add(new testrizm(this));
        }

        private void charamakerpress(object sender, MouseEventArgs e)
        {
            Controls.Clear();
            Controls.Add(new maker.charamaker(this));
        }


        private void testbuturipress(object sender, MouseEventArgs e)
        {
            //  Controls.Clear();
            // Controls.Add(new testbuturi(this));
        }

        private void teststagestart(object sender, MouseEventArgs e)
        {
       
        }

        long tikoo = 0;
        int cou = 0;
        System.Diagnostics.Stopwatch ton = new System.Diagnostics.Stopwatch();
        private void ticked(object sender, EventArgs e)
        {

        }
        private void ticken()
        {
           
        }

        private void keydown(object sender, KeyEventArgs e)
        {
         
            fileman.r.Next();
        }

        private void lets_go(object sender, MouseEventArgs e)
        {
            Controls.Clear();
            gogogo();
        }
        void gogogo()
        {


        }
    }
}
