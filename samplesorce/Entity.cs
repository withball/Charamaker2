using Charamaker2;
using Charamaker2.Character;
using Charamaker2.input;
using Charamaker2.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAVE_THE_SANTACOS_
{
    //エンティティはピクチャーとshapeを持ってると定義
    public class Entity
    {
        //ホントはクラスで管理したいけどめんどいからこのリストで今いるえんててぃを把握
        static public List<Entity> ents = new List<Entity>();
        public picture p;
        public Shape s,ps;
        //所属する表示マンを記録
       protected hyojiman hyo;
        public float vx, vy;
        public Entity(hyojiman hyou, Shape ss, float x, float y, float z, float w, float h, string tex,string texname)
        {
            hyo = hyou;
            p = new picture(x - w / 2, y - h / 2, z, w, h, w / 2, h / 2, 0, false, 1, texname, new Dictionary<string, string> { { texname, tex } });
            s = ss;
            setshape();
            setshape();
        }
        virtual public bool add()
        {
            if (hyo.addpicture(p))
            {
                ents.Add(this);
                return true;
            }
            return false;
        }
        virtual public bool remove()
        {
            ents.Remove(this);
            return hyo.removepicture(p);
        }
        protected void setshape()
        {
            ps = s.clone();//1フレーム前の形を置いておく
            s.w = p.w;
            s.h = p.h;
            s.rad = p.RAD;
            s.settxy(p.gettx(), p.getty());
            
        }
        virtual public void frame()
        {
            p.x += vx;p.y += vy;
            setshape();
        }
        //あたり判定！
        public List<Entity> ataris()
        {
            var res=new List<Entity>();
            foreach (var a in ents)
            {
                if (this.p.texname!=a.p.texname&&a.s.atarun2(a.ps,this.s,this.ps))//これで割と正確にあたり判定できる 
                {
                    res.Add(a);
                }
            }

            return res;

        }

    }
    public class blade : Entity 
    {
        float timer = 30;
        ironman iro;
        bool mir,gya;
        public blade(ironman iron,hyojiman hyo, float sc,bool mirror,bool gyaku) : base(hyo, new Triangle(0, 0, 0, 0,0.5f, 0), 0, 0, iron.p.z+1, 160 * 1.6f * sc, 32 * 1.6f * sc, "blade", "Blade")
        {
            gya = gyaku;
            mir = mirror;
            p.RAD = Math.Atan2(iron.vy,iron.vx);
            if (mir) p.RAD += Math.PI;
            iro = iron;
            p.tx = 0;
            p.OPA = 0.7f;
            p.settxy(iro.p.gettx(), iro.p.getty());
            base.frame();
            
        }
        public override void frame()
        {
            p.settxy(iro.p.gettx(), iro.p.getty());
            if (gya)
            {
                p.RAD += 0.5f;
            }
            else 
            {
                p.RAD -= 0.5f;
            }
            base.frame();

            //当たった奴らをpictureのtexnameで識別し色々効果を発動！
            foreach (var a in ataris())
            {
              
                if (a.p.texname == "viran")
                {
                    iro.energy -= 10;
                    a.remove();
                    hyo.playoto("biin");
                }
                if (a.p.texname == "softviran")
                {
                    iro.energy -= 10;
                    a.remove();
                    hyo.playoto("biin");
                }
                if (a.p.texname == "human")
                {
                    iro.energy -= 10;
                    iro.dark++;
                    a.p.texname = "yami";
                    a.p.textures = new Dictionary<string, string> { { "yami", "yami" } };
                    hyo.playoto("biin");
                }
            }

            timer -= 1;
            if (timer <= 0) remove();
        }
    }
    public class ironman : Entity 
    {
        public float energy = 3000;
        float sca = 0;

        public int dark = 0;
        public ironman(hyojiman hyo,float sc):base(hyo,new Circle(0, 0, 0, 0, 0), 0, 0, 10, 50*1.3f*sc, 70*1.3f*sc, "iron","ironman") 
        {
            sca = sc;
        }
        public override void frame()
        {
        }
        float sp = 3;
        float teikou = 0.95f;
       public int cou = 0;
        public virtual void frame2(inputin i)
        {
            vx *= teikou;vy *= teikou;
            double kaku=Math.Atan2(i.y - p.getty(),i.x-p.gettx());
            vx +=sp*(float)Math.Cos(kaku);vy += sp *(float)Math.Sin(kaku);

            if (i.ok(MouseButtons.Left,itype.ing)) //入力に応じた操作！
            {
            
                vx *= teikou; vy *= teikou;
                vx *= teikou; vy *= teikou;
            }

            var eff = new effectchara(hyo, 10, p.gettx(), p.getty(), 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(p), new List<setu> { }));
            eff.core.p.z -= 0.01f;
            eff.addmotion(new motion(new texpropman(10, "", 471, 0)));

            float px = p.gettx(), py = p.getty();
            base.frame();
            energy -= (Math.Abs(p.gettx() - px) + Math.Abs(p.getty() - py)) * 0.10f;
            foreach (var a in ataris()) 
            {
                if (a.p.texname == "ramune") 
                {
                    fileman.r.Next();
                    energy += 320;
                    energy = Math.Min(energy, 3000);
                    hyo.playoto("jett");
                    a.remove();
                }
                if (a.p.texname == "viran")
                {
                    energy -= 200;
                    a.remove();
                    var efff = new effectchara(hyo, 60, a.p.gettx(), a.p.getty(), 0, 0, 0, 0, 0, new setu("core", 0, 0,new picture(a.p),new List<setu>()));
                    efff.addmotion(new motion(new texpropman(60, "", 471, 0)));
                    hyo.playoto("nyuin");
                }
                if (a.p.texname == "softviran")
                {
                    energy -= 150;
                    a.remove();
                    var efff = new effectchara(hyo, 60, a.p.gettx(), a.p.getty(), 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(a.p), new List<setu>()));
                    efff.addmotion(new motion(new texpropman(60, "", 471, 0)));
                    hyo.playoto("nyuin");
                }
                if (a.p.texname == "human")
                {
                    fileman.r.Next();
                    cou++;
                    energy -= 90;
                    var ppp = new picture(a.p);
                    if (fileman.percentin(30))
                    {
                        ppp.textures = new Dictionary<string, string> { { "human", "tabaco" } };
                    }
                    else if (fileman.percentin(50))
                    {
                        ppp.textures = new Dictionary<string, string> { { "human", "happy" } };
                     }
                    else 
                    {
                        ppp.textures = new Dictionary<string, string> { { "human", "nikkori" } };


                    }
                    var efff=new effectchara(hyo,120,a.p.gettx(),a.p.getty(),0,0,0,0,0,new setu("core",0,0,ppp,new List<setu> { new setu("bou", ppp.tx, 0, new picture(0, 0, ppp.z + 0.1f, ppp.w, ppp.h, ppp.tx, ppp.h * 0.8f, 0, false, 1, "def", new Dictionary<string, string> { { "def", "bousi" } }), new List<setu>()) }));
                    efff.addmotion(new motion(new texpropman(120, "", 471, 0)));
                    a.remove();
                    hyo.playoto("bomber");
                }
            }
            if (i.ok(MouseButtons.Right, itype.down))
            {
                energy -= 30;
                hyo.playoto("biin");
            
                var bas=fileman.percentin(50);
                new blade(this, hyo, sca,true,bas).add();
                new blade(this, hyo, sca,false,bas).add();
            }
        }
    }
    public class viran : Entity
    {
        float scc;
        public viran(hyojiman hyo, float x,float y,float sc,bool soft=false) : base(hyo, new Circle(0, 0, 0, 0, 0),x,y, 11, 80 *sc, 80 * sc, "viran", "viran")
        {
            scc = sc;
            if (soft) 
            {
                p.texname = "softviran";
                p.textures = new Dictionary<string, string> { {"softviran","viran" } };
                p.w = 60 * sc;
                p.h = 60 * sc;
                p.tx = 60/2 * sc;
                p.ty = 60/2 * sc;
            }
        }
        public override void frame()
        {
            base.frame();
            foreach (var a in ataris())
            {
                if (a.p.texname == "human")
                {
                    
                    hyo.playoto("hoaa");//音を鳴らしたり
                    a.remove();

                    var e = new viran(hyo,a.p.gettx(),a.p.getty(),scc,true);
                    e.vx = a.vx;
                    e.vy = a.vy;
                    e.add();
                }
            }
        }
       
      
    }
    
}
