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
           
            fileman.setinguping(this,1/*SD.S.gsit*/);
            //SD.S.setvols();
         //  SD.setup<SD>();
          //  SD.savesave<SD>();
            // FP.seting(new List<string>(),new List<string> { "settingtext" });
            //通信用 C2WebRTCP2P.supertusin.setup();
            var s = loop();
            s.start();
          
        }
        protected Scene loop() 
        {
           
            var s = new SScene(sm);
            
            s.onEnds += (ob, sm) =>
            {
                var sss =loop();

                s.next = sss;

            };
            return s;
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
        Entity me=new Entity(new character(100,101,100,50,50,25,0,new setu("core",0,0,new picture(0,0,1,100,50,50,25,0,false,1,"def",new Dictionary<string, string> { { "def", "redbit" } })))
            ,new ABrecipie(new List<string> {"" },new List<Shape> {new Triangle(0,0,0,0,0) })
            ,new buturiinfo(2,0.00f,1f,0,0,0,0,1,atag:"me"));
        Entity he = new Entity(new character(400, 400, 50, 20, 25, 10, 0, new setu("core", 0, 0, new picture(0, 0, 0, 50, 50, 25, 25, 0, false, 1, "def", new Dictionary<string, string> { { "def", "bluebit" } })))
           , new ABrecipie(new List<string> { "" }, new List<Shape> { new Rectangle(0, 0, 0, 0, 0) }), new buturiinfo(atag: "he"));
       
        protected override void onstart()
        {
            em = new EntityManager(hyo);
            next = new SScene(sm);
            me.add(em);
       //    he.add(em); 
              new shapedraw().add(me);
            new shapedraw().add(he);

            {
                var e = new Entity(character.onepicturechara("redbit", 1, 0)
              , new ABrecipie(new List<string> { "" }, new List<Shape> { new Rectangle(0, 0, 0, 0, 0) }), new buturiinfo(-1, 0, 1, 0.1f,0, 0, atag: "he2"));
                e.c.addmotion(new scalechangeman(1,-1,"",hyo.ww,hyo.wh));
                e.frame(100);
                e.endframe(100);
                e.c.setcxy(hyo.ww*0, hyo.wh*0.8f, e.c.w *0, e.c.h * 0);

                e.add(em);
            }
            /*{
                var e = new Entity(character.onepicturechara("redbit", 1, 0)
              , new ABrecipie(new List<string> { "" }, new List<Shape> { new Rectangle(0, 0, 0, 0, 0) }), new buturiinfo(-1, 0, 1, 0.1f, 0, 0, atag: "he2"));
                e.c.addmotion(new scalechangeman(1, -1, "", hyo.ww, hyo.wh));
                e.frame(100);
                e.endframe(100);
                e.c.setcxy(hyo.ww * 0.01f, hyo.wh * 0.0f, e.c.w * 1, e.c.h * 0);

                e.add(em);
            }
            {
                var e = new Entity(character.onepicturechara("redbit", 1, 0)
              , new ABrecipie(new List<string> { "" }, new List<Shape> { new Rectangle(0, 0, 0, 0, 0) }), new buturiinfo(-1, 0, 1, 0.1f, 0, 0, atag: "he2"));
                e.c.addmotion(new scalechangeman(1, -1, "", hyo.ww, hyo.wh));
                e.frame(100);
                e.endframe(100);
                e.c.setcxy(hyo.ww * (1-0.01f), hyo.wh * 0.0f, e.c.w * 0, e.c.h * 0);

                e.add(em);
            }*/

            base.onstart();
        }
        int ako = 0;
        public override void frame(inputin i, float cl)
        {
        
            base.frame(i, cl);//ここにhyojiman.hyojiがある。
            if (i.ok(Keys.A, itype.ing))
            {
                me.c.idouxy(-4 * cl, 0);
            }
            if (i.ok(Keys.W, itype.down))
            {
                var m = new Waza(1);
                m.framed += (a, b) => {
                    me.c.idouxy(0, -300 * b.cl);
                };
                
                (new Waza(10)+m).add(me);
            }
            if (i.ok(Keys.D, itype.ing))
            {
                me.c.idouxy(4 * cl, 0);
            }
            if (i.ok(Keys.D2,itype.down))
            {
                me.c.scalechange(2);
            }
            if (i.ok(Keys.S, itype.ing))
            {
                var m = new Waza(100);
                m.framed += (a, b) => { 
                me.c.idouxy(0, 4 * cl); };
                m.add(me);
            }
            if (i.ok(Keys.Q, itype.ing))
            {
                new idouman(1, 0, 0, -1*cl).startAndFrame(me.c, 100);
            }
            if (i.ok(Keys.E, itype.ing))
            {
                new idouman(1, 0, 0, 1 * cl).startAndFrame(me.c, 100);
            }
            if (i.ok(Keys.Z, itype.ing))
            {
                me.c.w -= 0.5f * cl;
            }
            if (i.ok(Keys.C, itype.ing))
            {
                me.c.w += 0.5f * cl;
            }
            if (i.ok(Keys.Escape, itype.down))
            {
                end();
            }
            if (i.ok(Keys.M, itype.down))
            {
                me.c.mirror = !me.c.mirror;
            }
            if (i.ok(MouseButtons.Left, itype.down))
            {
                var mom= new Entity(new character(100, 0, 50, 50, 25, 25, 0, new setu("core", 0, 0, new picture(0, 0, 1, 50, 50, 25, 25, 0, false, 1, "def", new Dictionary<string, string> { { "def", "redbit" } })))
          , new ABrecipie(new List<string> { "" }, new List<Shape> { new Rectangle(0, 0, 0, 0, 0) })
          , new buturiinfo(3, 0.01f, 1f, 0.5f, 0, 0, 0, 1, atag: ako++.ToString()));
                mom.c.addmotion(new radtoman(1,"",fileman.whrandhani(0),360));
                mom.c.settxy(i.x, i.y);
                
                mom.add(em);
            }
            if (i.ok(MouseButtons.Right, itype.down))
            {
                var mom = new Entity(new character(100, 0, 50, 50, 25, 25, 0, new setu("core", 0, 0, new picture(0, 0, 1, 50, 50, 25, 25, 0, false, 1, "def", new Dictionary<string, string> { { "def", "redbit" } })))
          , new ABrecipie(new List<string> { "" }, new List<Shape> { new Rectangle(0, 0, 0, 0, 0) })
          , new buturiinfo(3, 0.00f, 0f, 0f, 0, 10, 0, 0, atag: fileman.r.Next().ToString()));
                mom.c.addmotion(new radtoman(1, "", fileman.whrandhani(0), 360));
                mom.c.settxy(i.x, i.y);

                mom.add(em);
            }
           // if (i.ok(Keys.Space, itype.ing))
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
                Shape.gousei(e.Acore,e.PAcore).drawshape(hyoji,R,1,1,1,3,true);
            }
        }
    }
}
