using Charamaker2;
using Charamaker2.Character;
using Charamaker2.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace omaegahajimetamonogataridaro.entity
{
    //こちらはEntityの例です。
    //EntityManagerにEntityをぶち込んで
    //EntityManagerのframe発動で物理っぽくなるはず。
    //zuren()hansya() EntityManager.frameは参考にできるかもしれない。
    public class Entity
    {
        //重なってる二つの物体をずれさせるメソッド
        public bool zuren(Entity e)
        {
            if (ovw || untouch || e.untouch) return false;

            if (e.ovw)
            {

                float nes1, nes2;
                double hos1, hos2;
                {
                    double hosent = e.pres.gethosen2(this.pres);
                    float ki = this.s.getsaikyo(hosent);


                    double nasukaku2 = e.s.nasukaku(this.s.gettx(), this.s.getty());
                    double osu2 = hosent - nasukaku2;
                    float wow = e.s.kyori(this.s) * (float)Math.Cos(-osu2);
                    var nownow = e.s.gethokyo(hosent);
                    float kyomu = nownow - wow;
                    nes1 = ki + kyomu;
                    hos1 = hosent;
                }
                {
                    double hosent = this.pres.gethosen2(e.pres);
                    float ki = e.s.getsaikyo(hosent);


                    double nasukaku2 = this.s.nasukaku(e.s.gettx(), e.s.getty());
                    double osu2 = hosent - nasukaku2;
                    float wow = this.s.kyori(e.s) * (float)Math.Cos(-osu2);
                    var nownow = this.s.gethokyo(hosent);
                    float kyomu = nownow - wow;
                    nes2 = ki + kyomu;
                    hos2 = hosent + Math.PI;
                }
                if (Math.Abs(nes1) > Math.Abs(nes2) && nes2 > 0)
                {
                    nes1 = nes2;
                    hos1 = hos2;
                }
                if (nes1 > 0)
                {
                    this.s.idou((nes1) * (float)Math.Cos(hos1), (nes1) * (float)Math.Sin(hos1));
                    return true;
                }
            }
            else
            {
                float hi = e.wei / (e.wei + this.wei);
                {
                    float nes1, nes2;
                    double hos1, hos2;
                    {
                        double hosent = e.pres.gethosen2(this.pres);
                        float ki = this.s.getsaikyo(hosent);


                        double nasukaku2 = e.s.nasukaku(this.s.gettx(), this.s.getty());
                        double osu2 = hosent - nasukaku2;
                        float wow = e.s.kyori(this.s) * (float)Math.Cos(-osu2);
                        var nownow = e.s.gethokyo(hosent);
                        float kyomu = nownow - wow;
                        nes1 = ki + kyomu;
                        hos1 = hosent;

                    }
                    {
                        double hosent = this.pres.gethosen2(e.pres);
                        float ki = e.s.getsaikyo(hosent);


                        double nasukaku2 = this.s.nasukaku(e.s.gettx(), e.s.getty());
                        double osu2 = hosent - nasukaku2;
                        float wow = this.s.kyori(e.s) * (float)Math.Cos(-osu2);
                        var nownow = this.s.gethokyo(hosent);
                        float kyomu = nownow - wow;
                        nes2 = ki + kyomu;
                        hos2 = hosent + Math.PI;
                    }
                    if (Math.Abs(nes1) > Math.Abs(nes2) && nes2 > 0)
                    {
                        nes1 = nes2;
                        hos1 = hos2;
                    }


                    float a = (nes1) * (float)Math.Cos(hos1), b = (nes1) * (float)Math.Sin(hos1);
                    if (nes1 > 0)
                    {

                        this.s.idou(a * hi, b * hi);
                        e.s.idou(-(a - a * hi), -(b - b * hi));
                        return true;
                    }
                }

            }


            return false;
        }
        //ぶつかった物体を反射させるメソッド。
        public void hansya(Entity e)
        {
            if (ovw || untouch || e.untouch) return;

            float VVX = vx - e.vx;
            float VVY = vy - e.vy;
            double hosent = e.s.gethosen(s.gettx(), s.getty());
            double goway = Math.Atan2(VVY, VVX);
            double kusion = Math.Atan2(Math.Sin(Math.PI + hosent - goway), Math.Cos(Math.PI + hosent - goway));
            double kusion2 = Math.Atan2(Math.Sin(hosent - goway), Math.Cos(hosent - goway));


            double mxey = (1 + (this.hanpatu + e.hanpatu) / 2) * Math.Sqrt(Math.Pow((VVX) * Math.Cos(hosent), 2) + Math.Pow((VVY) * Math.Sin(hosent), 2));

            var masatun = Math.Sqrt(Math.Pow((VVX) * Math.Cos(hosent), 2) + Math.Pow((VVY) * Math.Sin(hosent), 2)) * (e.masatu + masatu);

            if (kusion2 < -Math.PI / 2 || kusion2 > Math.PI / 2)
            {
                if (e.ovw)
                {
                    vx += (float)(mxey * Math.Cos(hosent));
                    vy += (float)(mxey * Math.Sin(hosent));

                    var kakkkkaku = Math.Atan2(Math.Sin(hosent + Math.PI / 2), Math.Cos(hosent + Math.PI / 2));
                    var bunsp = Math.Sqrt(vx * vx + vy * vy) * Math.Cos(Math.Atan2(vy, vx) - kakkkkaku);
                    if (Math.Abs(bunsp) < Math.Abs(masatun))
                    {
                        bunsp = -bunsp;
                    }
                    else if (bunsp > 0)
                    {
                        bunsp = -Math.Abs(masatun);
                    }
                    else
                    {

                        bunsp = Math.Abs(masatun);
                    }
                    vx += (float)(bunsp * Math.Cos(hosent + Math.PI / 2));
                    vy += (float)(bunsp * Math.Sin(hosent + Math.PI / 2));
                }
                else
                {
                    float hi = e.wei / (e.wei + this.wei);
                    {

                        var a = (float)(mxey * Math.Cos(hosent));
                        var b = (float)(mxey * Math.Sin(hosent));

                        vx += a * hi;
                        vy += b * hi;
                        e.vx -= a - a * hi;
                        e.vy -= b - b * hi;
                        {

                            var kakkkkaku = Math.Atan2(Math.Sin(hosent + Math.PI / 2), Math.Cos(hosent + Math.PI / 2));
                            var bunsp = Math.Sqrt(vx * vx + vy * vy) * Math.Cos(Math.Atan2(vy, vx) - kakkkkaku);
                            if (Math.Abs(bunsp) < Math.Abs(masatun))
                            {
                                bunsp = -bunsp;
                            }
                            else if (bunsp > 0)
                            {
                                bunsp = -Math.Abs(masatun);
                            }
                            else
                            {

                                bunsp = Math.Abs(masatun);
                            }
                            vx += (float)(bunsp * Math.Cos(hosent + Math.PI / 2));
                            vy += (float)(bunsp * Math.Sin(hosent + Math.PI / 2));

                        }
                        {
                            var kakkkkaku = Math.Atan2(Math.Sin(hosent + Math.PI / 2), Math.Cos(hosent + Math.PI / 2));
                            var bunsp = Math.Sqrt(e.vx * e.vx + e.vy * e.vy) * Math.Cos(Math.Atan2(e.vy, e.vx) - kakkkkaku);
                            if (Math.Abs(bunsp) < Math.Abs(masatun))
                            {
                                bunsp = -bunsp;
                            }
                            else if (bunsp > 0)
                            {
                                bunsp = -Math.Abs(masatun);
                            }
                            else
                            {

                                bunsp = Math.Abs(masatun);
                            }
                            e.vx += (float)(bunsp * Math.Cos(hosent + Math.PI / 2));
                            e.vy += (float)(bunsp * Math.Sin(hosent + Math.PI / 2));
                        }
                    }

                }

            }



        }

        public Shape s;//あたり判定
        public Shape pres = null;//1フレーム前のあたり判定

        public ulong atype;
        private float _hanpatu;
        public float hanpatu { get { return _hanpatu; } set { _hanpatu = value; if (_hanpatu < 0) _hanpatu = 0; if (_hanpatu > 1) _hanpatu = 1; } }

        private float _teikou;//空気抵抗
        public float teikou { get {  return _teikou; } set { _teikou = value; if (_teikou < 0) _teikou = 0; if (_teikou > 1) _teikou = 1; } }

        private float _masatu=0; //鉛直速度1あたりの摩擦パワー
        public float masatu { get { return _masatu; } set { _masatu = value; if (_masatu < 0) _masatu = 0; } }

        public character c;

        public float vx, vy;

   

        private float _wei;
        //1~99999
        static public readonly float MW = 1000000;
        public float wei  { get {  return _wei; }set { _wei = value; if (_wei <=0) _wei = 1; if (_wei >= MW) _wei = MW*2; } }
        public bool ovw { get { return _wei >= MW; } }
        protected float _ax, _ay;//加速度
        public float ax { get { return _ax; } }
        public float ay { get { return _ay; } }
        public bool untouch = false;
        public bool charasyudo = false;//キャラクターの大きさにshapeの大きさをそろえるのか

        public float speed { get { return (float)Math.Sqrt(vx * vx + vy * vy); } }

        //atypeが互いに素なEntity同士がぶつかるのよ
        public Entity(Shape ss, character cc, ulong aatype, float weight, float hanpatu=1,float teikou=0.03f,bool untouchiable=false,bool charasyu=true,float masatuu=0)
        {
            masatu = masatuu;
            s = ss;
            c = cc;
            untouch = untouchiable;
            charasyudo=charasyu;
            atype = aatype;
            if (weight < 0) wei = MW;
            else wei = weight;
            
            _hanpatu = hanpatu;
            _teikou = teikou;
            setpres();
        }
        public void setaxy(float x, float y)
        {
            _ax = x;
            _ay = y;
        }
        virtual public Entity clone() 
        {
            var res = new Entity(s.clone(),null,atype,wei,hanpatu,teikou,masatuu:masatu);
            if (c != null) res.c = new character(c);
            res.pres = pres.clone();
            res.vx = vx;
            res.vy = vy;
            res._ax = _ax;
            res._ay = _ay;
            res.untouch = untouch;
            res.charasyudo = charasyudo;
           
            return res;
        }
        float spclo = 0;
        virtual public void frame(float cl=1)    
        {
            s.x += vx * cl;
            s.y += vy * cl;

            vx += _ax * cl;
            vy += _ay * cl;

            spclo += cl;

            while (spclo >= 1)
            {
                vx = (1 - teikou ) * (vx);
                vy = (1 - teikou ) * (vy);
                spclo -= 1;
            }
            if (c != null)
            {
               
                if (charasyudo)
                {
                    charasyunosusume();
                }
                else { c.frame(); }
            }
            charasoroe();
        }
        protected void charasyunosusume() 
        {
            charasoroe();

            c.frame();
            s.rad = c.RAD;

            this.s.w = c.w;
            this.s.h = c.h;
       
            s.setcxy(c.getcx(c.w / 2, c.h / 2), c.getcy(c.w / 2, c.h / 2), s.w / 2, s.h / 2);
        }
        public float prevx() 
        {
            return (vx / (1 - teikou) - ax);
        }
        public float prevy()
        {
            return (vy / (1 - teikou) - ay);
        }
        public float prevx(float vx)
        {
            return (vx / (1 - teikou) - ax);
        }
        public float prevy(float vy)
        {
            return (vy / (1 - teikou) - ay);
        }


        public void charasoroe() 
        {
            if (c != null)
            {
                if (charasyudo)
                {
                    c.setcxy(s.getcx(s.w / 2, s.h / 2), s.getcy(s.w / 2, s.h / 2), c.w / 2, c.h / 2);
                    var m = new radtoman(1, "", s.rad / Math.PI * 180, 360);
                    m.start(c);
                    m.frame(c, 1);
                }
                else
                {
                    c.settxy(s.getcx(s.w / 2, s.h / 2), s.getcy(s.w / 2, s.h / 2));
                    var m = new radtoman(1, "", s.rad / Math.PI * 180, 360);
                    m.start(c);
                    m.frame(c, 1);
                }

            }
        }

        public bool difatype (Entity e)
        {
            return tagainiso(atype,e.atype);  
        }
        public bool difatype(ulong i)
        {
            return tagainiso(atype, i);
        }
        public bool difatype(Entity e,ulong safe)
        {
            return tagainiso(atype*safe, e.atype);
        }
        bool tagainiso(ulong a, ulong b) 
        {
         
            ulong c;
            while (b>1) 
            {
                c = a % b;
                if (c == 0) return false;
                a = b;
                b = c;
            }
            return true;
        }
               public void setpres() 
        {
            pres = s.clone();
        }

        protected EntityManager EM;
        public bool onEM { get { return EM != null&&EM.Entities.Contains(this); } }
        virtual public bool add(EntityManager em) 
        {
            if (!onEM)
            {
                setpres();
                em.Entities.Add(this);

                if (this.c != null) this.c.resethyoji(em.hyoji);
                EM = em;
                return true;
               
            }
            return false;
        }
        virtual public bool remove() 
        {
            bool res = false;
            if (onEM)
            {
                if (this.c != null) this.c.sinu(EM.hyoji);
                res = EM.Entities.Remove(this);
               // EM = null;
            }
            return res;
        }
    }

    public class EntityManager
    {
        hyojiman hyo;
        public hyojiman hyoji { get { return hyo; } }
        public List<Entity> Entities = new List<Entity>();


     
        bool wein = true, sein = true,playin=true;
        public EntityManager(hyojiman hyou)
        {
            hyo = hyou;
        }
        public List<Entity> ents{ get { return new List<Entity>(Entities); } }
   
        public List<Entity> moves 
        {
            get
            {
                if (wein)
                {
                    _moves.Clear();
                    _overweights.Clear();
                    foreach (var a in Entities)
                    {
                        if (!a.ovw)
                        {
                            _moves.Add(a);
                        }
                        else 
                        {
                            _overweights.Add(a);
                        }
                    }
                    wein = false;
                }
                return new List<Entity>(_moves);
            }
        }
        public List<Entity> overweights
        {
            get
            {
                if (wein)
                {
                    _moves.Clear();
                    _overweights.Clear();
                    foreach (var a in Entities)
                    {
                        if (!a.ovw)
                        {
                            _moves.Add(a);
                        }
                        else
                        {
                            _overweights.Add(a);
                        }
                    }
                    wein = false;
                }
                return new List<Entity>(_overweights);
            }
        }
        protected List<Entity> _moves = new List<Entity>();
        protected List<Entity> _overweights = new List<Entity>();
        public bool aru(Entity e) 
        {
            return Entities.Contains(e);
        }
   
      
      
        public void reset()
        {
            Entities.Clear();
           
            _moves.Clear();
        }
        ~EntityManager()
        {
            reset();
        }
        Dictionary<Entity,List<Entity>> atalis = new Dictionary<Entity, List<Entity>>();
        public void atattao(Entity korega,Entity koreni) 
        {
            if (!atalis.ContainsKey(korega)) atalis.Add(korega, new List<Entity>());
            atalis[korega].Add(koreni);
        }
        public bool atattano(Entity korega, Entity koreni)
        {
            if (atalis.ContainsKey(korega)) return atalis[korega].Contains(koreni);
            return false;
        }
        public int atattakazu(Entity korega)
        {
            if (atalis.ContainsKey(korega)) return atalis[korega].Count;
            return 0;
        }

        
        Dictionary<Entity, List<Entity>> hansyasu = new Dictionary<Entity, List<Entity>>();
        public void hansyao(Entity korega, Entity koreni)
        {
            if (!hansyasu.ContainsKey(korega)) hansyasu.Add(korega, new List<Entity>());
            if(!hansyasu[korega].Contains(koreni))hansyasu[korega].Add(koreni);
        }
      
        public List<Entity> hansyano(Entity korega) 
        {
            if (hansyasu.ContainsKey(korega)) return hansyasu[korega];
            return new List<Entity>();
        }
       

        public void frame(float cl=1) 
        {
            int i = 0;
            atalis.Clear();
            hansyasu.Clear();
         


            for(i=Entities.Count()-1;i>=0;i--) Entities[i].frame(cl);

         
            wein = true; sein = true;playin = true;

            bool ren;
            //movesは地形以外の奴
            foreach (var a in moves)
            {

                ren = false;

                if (!a.untouch)
                {
                    //最初は過去の奴も考慮してずらす
                    foreach (var b in Entities)
                    {
                        if (!atattano(a, b) && !b.untouch && a.difatype(b) && (a.s.atarun2(a.pres, b.s, b.pres)))
                        {
                            if (a.zuren(b))
                            {
                                atattao(a, b);//誰が誰に当たったか記録

                                ren = true;

                                hansyao(a, b);//反射するためにもこっちに記録
                            }
                        }
                    }
                    //こっちは今重なってるかだけで当たってるか判定する
                    while(ren)
                    {
                        ren = false;
                        foreach (var b in Entities)
                        {
                            if (!atattano(a, b) && !b.untouch && a.difatype(b)&&a.s.atarun(b.s) )
                            {
                                if (a.zuren(b))
                                {
                                    atattao(a, b);

                                    ren = true;

                                    hansyao(a, b);
                                }
                            }
                        }
                    }
                }
            }
            //未だに地形とぶつかってる奴をずらす
            foreach (var a in overweights) 
            {
                foreach (var b in moves)
                {
                    if (!atattano(a, b) && a.difatype(b) && a.s.atarun(b.s))
                    {
                        if (b.zuren(a))
                        {
                            
                        }
                    }
                }
            }
            //反射させる
            var cons = new Dictionary<Entity,List<Entity>>(hansyasu);
            var keys = new List<Entity>(cons.Keys);
            foreach (var a in keys)
            {
                if (cons[a] != null)
                {
                    for ( i = cons[a].Count() - 1; i >= 0; i--)
                    {
                      //  現在当たってるか、リストの最後尾(最後に当たった奴)だった場合？あれ、全部か
                      
                        if (a.s.atarun(cons[a][i].s)||hansyasu[a].Count()-1==i)
                        {

                            a.hansya(cons[a][i]);

                            //反射させた後に、もう一度反射すると変なことになるので相手cons[a][i]をキーとするListから自分aを消しておく
                            if (cons.ContainsKey(cons[a][i]))
                            {
                                cons[cons[a][i]].Remove(a);
                            }

                        }
                        else
                        {
                            hansyasu[a].RemoveAt(i);

                        }
                    }
                }
            }

         
          //全て終わった後にpresを設定し表示をそろえる。
            foreach (var a in Entities) a.setpres();
            foreach (var a in Entities) { a.charasoroe(); }
        }
    }
    /*
     * 更にこの上にEntityを動かすSousaを作ってplayerに派生させるとゲームっぽくなる。
     * SousaのフレームもEntityManagerにまかせる。
     * Sousaの繰り出す技はWazaというクラスにまとめてやるとわかりやすく？なる
     * Sousa.List<Waza>があってSousa.frame()のときに
     * foreach(var a in List<Waza>)a.frame()で代行させるかんじで。
     * 
     * 更にEntityを継承してSentitiyにし、Statusというクラスを持たせて
     * HPが0になったら自ら消えるようにすることでHPつきオブジェクトができると。
     */}
