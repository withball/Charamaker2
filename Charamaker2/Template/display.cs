using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Charamaker2;
using Charamaker2.input;
using Charamaker2.Character;
using GameSet1;
using Charamaker2.Shapes;
//通信用　using C2WebRTCP2P;死体場合はC2webRTCCP2Pを参照に追加してね！
// なんかわからんけど、mrwebrtc.dllがないって言われたらpackages->MixedRealityWebrtc->runtimes->win10-x86->nativにある
// やつをexeのとこまで持ってけ！

namespace Template
{

    public partial class display : Form
    {
        /// <summary>
        /// テクスチャーとか音とか使いたいなら.exeの横にtex,otoってフォルダ作ってbmpとwavぶちこみな！
        ///モーションはmotion、キャラクターはcharacterにCharamaker2で作ってな
        ///hyojiman.playoto("jett");
        ///fileman.loadcharacter("yoshino");
        ///fileman.ldmotion("yoshino\\kougeki");
        ///とかいう具合や！
        ///セッティングを使用するためにはセッティング用のとこを解放しろ！
        /// </summary>
        inputin i = new inputin();
        SceneManager sm = new SceneManager();
        System.Drawing.Size size=new System.Drawing.Size(800,800);
        public display()
        {
            InitializeComponent();
            this.ClientSize=new System.Drawing.Size(size.Width,size.Height);
            //SD.setup();
           
            fileman.setinguping(this,1/*SD.S.gsit*/);
            //SD.S.setvols();

            // FP.seting(new List<string>(),new List<string> { "settingtext" });
            //通信用 C2WebRTCP2P.supertusin.setup();
            new SScene(sm).start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void clocked(object sender, EventArgs e)
        {
            i.setpointer(sm.s.hyo, this);
            sm.s.frame(i, 1);
            i.topre();
            //セッティング用inputin.raw.topre();
        }

        private void keydown(object sender, KeyEventArgs e)
        {
            
            i.down(e.KeyCode,inputin.rawconv/*セッティング用SD.S.converts*/);
            //  セッティング用 inputin.raw.down(e.KeyCode, inputin.rawconv);
        }

        private void keyup(object sender, KeyEventArgs e)
        {
            i.up(e.KeyCode, inputin.rawconv/*セッティング用SD.S.converts*/);
            //  セッティング用 inputin.raw.up(e.KeyCode, inputin.rawconv);
        }

        private void mousedown(object sender, MouseEventArgs e)
        {
            i.down(e.Button, inputin.rawconv/*セッティング用SD.S.converts*/);
            // セッティング用  inputin.raw.down(e.Button, inputin.rawconv);
        }

        private void mouseup(object sender, MouseEventArgs e)
        {
            i.up(e.Button, inputin.rawconv/*セッティング用SD.S.converts*/);
            //セッティング用   inputin.raw.up(e.Button, inputin.rawconv);
        }

        private void closing(object sender, FormClosingEventArgs e)
        {
           //通信用 supertusin.shutdown();
        }

        private void resized(object sender, EventArgs e)
        {
            int sum = this.ClientSize.Width + this.ClientSize.Height;
            this.ClientSize = new System.Drawing.Size(sum * size.Width / (size.Width+size.Height), sum * size.Height / (size.Width + size.Height));
        }

        private void shown(object sender, EventArgs e)
        {
            /*出来上がりの時は開放してね
            sm.s.hyo.hyoji();
            fileman.loadfiletoka();
        */
        }
    }
    class SScene : Scene 
    {
        EntityManager em;
        public SScene(SceneManager sm):base(sm ) 
        {
        }
        Entity me=new Entity(new character(0,0,50,50,25,25,0,new setu("core",0,0,new picture(0,0,1,50,50,25,25,0,false,1,"def",new Dictionary<string, string> { { "def", "redbit" } })))
            ,new ABrecipie(new List<string> {"" },new List<Shape> {new Rectangle(0,0,0,0,0) }),new buturiinfo(atag:"me"));
        Entity he = new Entity(new character(100, 100, 50, 20, 25, 10, 0, new setu("core", 0, 0, new picture(0, 0, 0, 50, 50, 25, 25, 0, false, 1, "def", new Dictionary<string, string> { { "def", "bluebit" } })))
           , new ABrecipie(new List<string> { "" }, new List<Shape> { new Rectangle(0, 0, 0, 0, 0) }), new buturiinfo(atag: "he"));
        Entity he2 = new Entity(new character(100, 100, 50, 20, 25, 10, 0, new setu("core", 0, 0, new picture(0, 0, 99999, 50, 50, 25, 25, 0, false, 1, "def", new Dictionary<string, string> { { "def", "yellowbit" } })))
            , new ABrecipie(new List<string> { "" }, new List<Shape> { new Triangle(0, 0, 0, 0, 0,0,-1) }), new buturiinfo(-1,atag: "he2"));

        protected override void onstart()
        {
            em = new EntityManager(hyo);
            next = new SScene(sm);
            me.add(em);
            he.add(em); 
            he2.add(em);
            new shapedraw().add(me);
            new shapedraw().add(he); 
            new shapedraw().add(he2);
            base.onstart();
        }
        public override void frame(inputin i, float cl)
        {
            base.frame(i, cl);//ここにhyojiman.hyojiがある。
            if (i.ok(Keys.A, itype.ing))
            {
                me.c.idouxy(-4 * cl, 0);
            }
            if (i.ok(Keys.W, itype.ing))
            {
                me.c.idouxy(0, -4 * cl);
            }
            if (i.ok(Keys.D, itype.ing))
            {
                me.c.idouxy(4 * cl, 0);
            }
            if (i.ok(Keys.S, itype.ing))
            {
                me.c.idouxy(0, 4 * cl);
            }
            if (i.ok(Keys.Q, itype.ing))
            {
                me.c.RAD -= 0.1 * cl;
            }
            if (i.ok(Keys.E, itype.ing))
            {
                me.c.RAD += 0.1 * cl;
            }
            if (i.ok(Keys.Escape, itype.down))
            {
                end();
            }
            em.frame(1);
        }
    }
    class shapedraw : Waza 
    {

        public shapedraw() : base(10000) 
        {
        
        }
        protected override void onAdd()
        {
            base.onAdd();
            e.EEV.hansyad += hansyama;
        }
        protected override void onRemove()
        {
            base.onRemove();
            e.EEV.hansyad -= hansyama;
        }
        float R = 0;
        protected void hansyama(object sender,EEventArgs e) 
        {
            R = 1;
        }
        public override void frame(float cl)
        {
            base.frame(cl);
            R = 0;
            timer = 0;
        }
        protected override void onFrame(float cl)
        {
            base.onFrame(cl);
            if (e.atariable) 
            {
                //これクソ重いよ。ただあたり判定を把握するのにはヨシ
                e.Acore.drawshape(hyoji,R,1,1,1,true);
            }
        }
    }
}
