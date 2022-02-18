using System;
using System.Collections.Generic;
using Charamaker2;
using Charamaker2.Character;
using Charamaker2.Shapes;

namespace GameSet1
{
    /// <summary>
    /// 移動速度、加速度、とか持ってる奴。体力とかはオーバライドしてね。
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// エンテティのもとになるキャラクター
        /// そのものが物理判定なのよ
        /// </summary>
        protected character _c;

        /// <summary>
        /// キャラクターに当たり判定(図形を付与する)
        /// </summary>
        protected ataribinding _ab, _pab;
        /// <summary>
        /// 空気抵抗とかの物理のインフォメーションとか
        /// </summary>
        protected buturiinfo _bif;


        /// <summary>
        /// 今のフレームのあたり判定。
        /// </summary>
        public ataribinding ab { get { return _ab; } }
        /// <summary>
        /// 前回のフレームのあたり判定
        /// </summary>
        public ataribinding pab { get { return _pab; } }
        /// <summary>
        ///空気抵抗とかの物理のインフォメーションとか
        ///ちなみに回転は全く物理に含まれてないよ
        /// </summary>
        public buturiinfo bif { get { return _bif; } }
        /// <summary>
        /// キャラクター。物理のあたり判定はキャラクターにバインドされてる図形で判定される。
        /// </summary>
        public character c { get { return _c; } }

        /// <summary>
        /// 物理的にぶつかるかどうか。""という節を作るときに指定したか
        /// </summary>
        public bool atariable { get { return ab.core != null; } }
        /// <summary>
        /// 今の物理的なあたり判定に使うやーつ
        /// </summary>
        public Shape Acore { get { return ab.getatari(""); } }
        /// <summary>
        /// 昔の物理的なあたり判定に使うやーつ
        /// </summary>
        public Shape PAcore { get { if (pab != null) return pab.core; return ab.getatari(""); } }




        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="chara">キャラクター</param>
        /// <param name="recipie">あたり判定のレシピ</param>
        /// <param name="ai">物理とかの情報</param>
        public Entity(character chara, ABrecipie recipie, buturiinfo ai)
        {
            _c = chara;
            _ab = new ataribinding(c, recipie);
            _pab = new ataribinding(c, recipie);
            _bif = new buturiinfo(ai);
        }
        /// <summary>
        /// コピーするためのコンストラクタ
        /// </summary>
        /// <param name="E">コピー元</param>
        public Entity(Entity E)
        {
            _c = new character(E.c, false, false);

            _ab = new ataribinding(c, E.ab.RECIPIE);
            _pab = new ataribinding(c, E.ab.RECIPIE);
            _bif = (buturiinfo)Activator.CreateInstance(E.bif.GetType(), E.bif);
        }

        /// <summary>
        /// フレーム処理
        /// </summary>
        /// <param name="cl">フレームの長さ</param>
        virtual public void frame(float cl)
        {

            bif.frame(cl, c);

            c.frame(cl);
            foreach (var a in wazas) 
            {
                a.frame(cl);
            }
            ab.frame();

        }
        /// <summary>
        /// 今で、昔のあたり判定をセットする
        /// </summary>
        virtual public void setpab()
        {
            pab.frame();
        }
        /// <summary>
        /// 追加されてるエンテティマネージャー。仕様上一個しか無理なんだ。ごめんね
        /// </summary>
        protected EntityManager _EM = null;

        /// <summary>
        /// エンテティマネージャー。hyojimanとかもここからとれる
        /// </summary>
        public EntityManager EM{get{ return _EM; }}


        /// <summary>
        /// エンテティマネージャー―に追加したいときに呼ぶメソッド。
        /// </summary>
        /// <param name="e">追加する奴。同時に複数のマネージャーには追加しちゃいけない。ごめんね</param>
        /// <returns>追加されたか。trueならついでにonAddが呼び出される。</returns>
        virtual public bool add(EntityManager e) 
        {
            if ( e.add(this))
            {
               _EM = e;
                onAdd();
                return true;
            }
            return false;
        }
        /// <summary>
        /// エンテティマネージャーから削除したいときに呼び出すメソッド。
        /// </summary>
        /// <returns>削除されたかtrueならついでにonRemoveが呼び出される。</returns>
        virtual public bool remove()
        {
            if (_EM != null && _EM.remoeve(this))
            {
                onRemove();
                return true;
            }
            return false;
        }

        /// <summary>
        /// add()がtrueだったら呼び出されるメソッド
        /// </summary>
        virtual protected void onAdd()
        {

            c.resethyoji(EM.hyoji);

        }

        /// <summary>
        /// remove()がtrueだったら呼び出されるメソッド
        /// </summary>
        virtual protected void onRemove()
        {

            c.sinu(EM.hyoji);
            while (_wazas.Count > 0) 
            {
                _wazas[0].remove();
            }

        }

        /// <summary>
        /// 技のリスト
        /// </summary>
        protected List<Waza> _wazas = new List<Waza>();

        /// <summary>
        /// 技のリストのコピーをもらえる
        /// </summary>
        public List<Waza> wazas { get { return new List<Waza>(_wazas); } }

        /// <summary>
        /// タイプで絞り込んで技のリストを取得する
        /// </summary>
        /// <param name="t">技のタイプ</param>
        /// <returns>新しいリスト</returns>
        public List<Waza> getwazalis(Type t  )
        {

            if (t == typeof(Waza)) return new List<Waza>(_wazas);
            if (!t.IsSubclassOf(t)) return new List<Waza>();
            var lis = new List<Waza>();
            Type tt;
            foreach (var a in _wazas) 
            {
                tt = a.GetType();
                if (tt == t || tt.IsSubclassOf(t)) 
                {
                    lis.Add(a);
                }
            }
            return lis;
        }
        /// <summary>
        /// 技を追加する。基本Waza.add(e)で呼び出せ
        /// </summary>
        /// <param name="w">追加する技</param>
        /// <returns>追加できた</returns>
        public bool addWaza(Waza w) 
        {
            if (!_wazas.Contains(w)) 
            {
                _wazas.Add(w);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 技を削除する。基本Waza.remove(e)で呼び出せ
        /// </summary>
        /// <param name="w">削除する技</param>
        /// <returns>削除できた</returns>
        public bool removeWaza(Waza w) 
        {
            return _wazas.Remove(w);
        }

    }
   


    /// <summary>
    /// あたり判定を構築するレシピというか
    /// </summary>
    public class ABrecipie
    {
        /// <summary>
        /// あたり判定の対象の名前
        /// </summary>
        public readonly List<string> names;
        /// <summary>
        /// あたり判定の対象の図形の種類（）
        /// </summary>
        public readonly List<Shape> shapes=new List<Shape>();

        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="setunames">対象の節の名前""でキャラクターそのもの</param>
        /// <param name="shapetemples">図形のテンプレート大きさとか座標は気にしない</param>
        public ABrecipie(List<string> setunames, List<Shape> shapetemples)
        {
            names = new List<string>(setunames);
            shapes = new List<Shape>(shapetemples);

        }
        /// <summary>
        /// コピーするためのコンストラクタ。当たりバインディングに追加するときとか呼び出される
        /// </summary>
        /// <param name="rec">コピー元</param>
        public ABrecipie(ABrecipie rec)
        {
            names = new List<string>(rec.names);
            foreach (var a in rec.shapes)
            {
                shapes.Add(a.clone());
            }
        }
    }
    /// <summary>
    /// キャラクターと図形を結び付けてあたり判定と化させるためのクラス
    /// </summary>
   public class ataribinding
    {
        /// <summary>
        /// レシピ
        /// </summary>
        protected ABrecipie rec;
        /// <summary>
        /// あたり判定のレシピ
        /// </summary>
        public ABrecipie RECIPIE { get { return rec; } }
        /// <summary>
        /// 結び付けてるキャラクター
        /// </summary>
        protected character c;
        /// <summary>
        /// 結び付けられてる節共
        /// </summary>
        protected List<setu> set = new List<setu>();
        /// <summary>
        /// 物理的判定をするあたり判定
        /// </summary>
        protected Shape _core;
        /// <summary>
        /// 物理的判定をするあたり判定
        /// </summary>
        public Shape core { get { return _core; } }

        /// <summary>
        /// あたり判定を得る
        /// </summary>
        /// <param name="name">節の名前</param>
        /// <returns>無かったらnull</returns>
        public Shape getatari(string name)
        {
            var i = rec.names.IndexOf(name);
            if (i == -1) return null;

            return rec.shapes[i];
        }
       /// <summary>
       /// あたり判定のリストを得る
       /// </summary>
       /// <param name="name">名前共</param>
       /// <returns>順番は保証する。しかしnullが混じっているかも</returns>
        public List<Shape> getatari(params string[] name)
        {
            var res = new List<Shape>();
            foreach (var a in name)
            {
                int i = rec.names.IndexOf(a);
                if (i >= 0)
                {
                    res.Add(rec.shapes[i]);
                }
                else 
                {
                    res.Add(null);
                }
            }
            return res;
        }

        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="c">バインドするキャラクター</param>
        /// <param name="recipie">バインドのレシピ</param>
        public ataribinding(character c, ABrecipie recipie)
        {
            this.c = c;
            rec =(ABrecipie) Activator.CreateInstance(recipie.GetType(),recipie);
            for (int i = 0; i < rec.names.Count; i++)
            {
                var tt = c.GetSetu(rec.names[i]);
                set.Add(tt);
                if (rec.names[i] == "" && tt == null) _core = rec.shapes[i];
            }
        }
        /// <summary>
        /// キャラクターを物理用のあたり判定の方に合わせる
        /// </summary>
        virtual public void charaset() 
        {
            if (_core != null)
            {
                c.RAD = _core.rad;
                c.setcxy(_core.getcx(0, 0), _core.getcy(0, 0), 0, 0);
            }
        }
        /// <summary>
        /// あたり判定である図形共をキャラクターに合わせる。
        /// </summary>
        virtual public void frame()
        {
            for (int i = 0; i < set.Count; i++)
            {
                if (set[i] != null)
                {
                    if (!set[i].p.mir)
                    {
                        rec.shapes[i].w = set[i].p.w;
                        rec.shapes[i].h = set[i].p.h;
                        rec.shapes[i].rad = set[i].p.RAD;

                        rec.shapes[i].setcxy(set[i].p.getcx(0, 0), set[i].p.getcy(0, 0), 0, 0);
                    }
                    else
                    {
                        rec.shapes[i].w = -set[i].p.w;
                        rec.shapes[i].h = set[i].p.h;
                        rec.shapes[i].rad = set[i].p.RAD;

                        rec.shapes[i].setcxy(set[i].p.getcx(rec.shapes[i].w, 0), set[i].p.getcy(rec.shapes[i].w, 0), 0, 0);
                    }
                }
                else
                {
                    if (!c.mirror)
                    {
                        rec.shapes[i].w = c.w;
                        rec.shapes[i].h = c.h;
                        rec.shapes[i].rad = c.RAD;

                        rec.shapes[i].setcxy(c.getcx(0, 0), c.getcy(0, 0), 0, 0);
                    }
                    else
                    {
                        rec.shapes[i].w = -c.w;
                        rec.shapes[i].h = c.h;
                        rec.shapes[i].rad = c.RAD;

                        rec.shapes[i].setcxy(c.getcx(rec.shapes[i].w, 0), c.getcy(rec.shapes[i].w, 0), 0, 0);
                    }
                }
            }
        }

    }
    /// <summary>
    /// 物理をやるインフォメーション
    /// </summary>
   public class buturiinfo
    {
        /// <summary>
        /// 御存じ速度加速度
        /// </summary>
        public float vx, vy, ax, ay;

        /// <summary>
        /// 反発係数？
        /// </summary>
        protected float _hanpatu;
        /// <summary>
        /// 反発係数0~1
        /// </summary>
        public float hanpatu { get { return _hanpatu; } set { _hanpatu = value; if (_hanpatu < 0) _hanpatu = 0; if (_hanpatu > 1) _hanpatu = 1; } }
        /// <summary>
        /// 空気抵抗？
        /// </summary>
        protected float _teikou;
        /// <summary>
        /// 空気抵抗0~1
        /// </summary>
        public float teikou { get { return _teikou; } set { _teikou = value; if (_teikou < 0) _teikou = 0; if (_teikou > 1) _teikou = 1; } }
        /// <summary>
        /// 摩擦係数というか鉛直速度1あたりの摩擦パワー
        /// </summary>
        protected float _masatu = 0;
        /// <summary>
        /// 摩擦係数というか鉛直速度1あたりの摩擦パワー　0~+inf
        /// </summary>
        public float masatu { get { return _masatu; } set { _masatu = value; if (_masatu < 0) _masatu = 0; } }
        /// <summary>
        /// 重さ？
        /// </summary>
        protected float _wei;
        /// <summary>
        /// 重さの最大値
        /// </summary>
        static public readonly float MW = 1000000;
        /// <summary>
        /// 重さ 0＜＜MW
        /// -1でMW
        /// </summary>
        public float wei { get { return _wei; } set { _wei = value; if (_wei <= 0) _wei = MW ; if (_wei >= MW) _wei = MW ; } }
        /// <summary>
        /// 重さが最大かどうか
        /// </summary>
        public bool ovw { get { return _wei >= MW; } }

        /// <summary>
        /// あたり判定の分類
        /// </summary>
        List<string> atag;
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="wei">重さ</param>
        /// <param name="tik">空気抵抗</param>
        /// <param name="hanp">反射係数</param>
        /// <param name="mas">摩擦係数</param>
        /// <param name="vx">速度</param>
        /// <param name="vy">速度</param>
        /// <param name="ax">加速度</param>
        /// <param name="ay">加速度</param>
        /// <param name="atag">あたり判定の分類</param>
        public buturiinfo(float wei = 1, float tik = 0, float hanp = 0, float mas = 0, float vx=0,float vy=0,float ax=0,float ay=0,params string[] atag)
        {
            this.wei = wei;
            teikou = tik;
            hanpatu = hanp;
            masatu = mas;
            this.vx = vx;
            this.vy = vy;
            this.ax = ax;
            this.ay = ay;
            this.atag = new List<string>(atag);
        }
        /// <summary>
        /// 重さに応じて加速できたりする
        /// </summary>
        /// <param name="vx">x方向の加速度</param>
        /// <param name="vy">y方向の加速度</param>
        /// <param name="weight">加速の重さ。0以下でvxyをそのままぶち込む</param>
        public void kasoku(float vx, float vy, float weight=-1) 
        {
            if (weight <= 0)
            {
                this.vx += vx;
                this.vy += vy;
            }
            else 
            {
                this.vx += vx * this.wei / (weight + this.wei);
                this.vy += vy * this.wei / (weight + this.wei);
            }
        }


        /// <summary>
        /// コピーするためのコンストラクタ。いがいとEntityを作るときにも呼び出されてる
        /// </summary>
        /// <param name="i">コピー元</param>
        public buturiinfo(buturiinfo i)
        {

            this.wei = i.wei;
            teikou = i.teikou;
            hanpatu = i.hanpatu;
            masatu = i.masatu;
            this.vx = i.vx;
            this.vy = i.vy;
            this.ax = i.ax;
            this.ay = i.ay;

            this.atag = new List<string>(i.atag);
        }
        /// <summary>
        /// タグを持っているか
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>もってるならtrue</returns>
        public bool contains(string tag)
        {
            return atag.Contains(tag);
        }
        /// <summary>
        /// タグが全く異なっているか
        /// </summary>
        /// <param name="a">相手のインフォメーション</param>
        /// <returns>少しでも一致してたらfalse</returns>
        public bool different(buturiinfo a)
        {
            for (int i = 0; i < atag.Count; i++)
            {
                if (a.contains(atag[i])) return false;
            }
            return true;
        }
        /// <summary>
        /// フレーム処理。移動したり速度したり
        /// </summary>
        /// <param name="cl">クロックの長さ</param>
        /// <param name="c">対象のキャラクター</param>
        public void frame(float cl, character c)
        {
            vx -= vx * teikou;
            vy -= vy * teikou;

            c.settxy(c.gettx()+vx * cl + ax * cl * cl / 2, c.getty()+ vy * cl + ay * cl * cl / 2);
       
            vx += ax * cl;
            vy += ay * cl;

        }


        /// <summary>
        /// 相手と自分をなんか計算してずらす！
        /// ちなみにここでは大丈夫だけどataribinding.coreを動かしたらcharasetしないと移動が反映されないからちうい
        /// </summary>
        /// <param name="thiis">自分。重ね重ねっちゃうけどお願い</param>
        /// <param name="e">相手</param>
        /// <returns>ずらしたかどうか</returns>
        public bool zuren(Entity thiis,Entity e)
        {
            if (thiis.bif.ovw || !e.atariable || !thiis.atariable) return false;

            if (e.bif.ovw)
            {

                float nes1, nes2;
                double hos1, hos2;
                {
                    double hosent = e.PAcore.gethosen2(thiis.PAcore);
                    float ki = thiis.Acore.getsaikyo(hosent);


                    double nasukaku2 = e.Acore.nasukaku(thiis.Acore.gettx(), thiis.Acore.getty());
                    double osu2 = hosent - nasukaku2;
                    float wow = e.Acore.kyori(thiis.Acore) * (float)Math.Cos(-osu2);
                    var nownow = e.Acore.gethokyo(hosent);
                    float kyomu = nownow - wow;
                    nes1 = ki + kyomu;
                    hos1 = hosent;
                }
                {
                    double hosent = thiis.PAcore.gethosen2(e.PAcore);
                    float ki = e.Acore.getsaikyo(hosent);


                    double nasukaku2 = thiis.Acore.nasukaku(e.Acore.gettx(), e.Acore.getty());
                    double osu2 = hosent - nasukaku2;
                    float wow = thiis.Acore.kyori(e.Acore) * (float)Math.Cos(-osu2);
                    var nownow = thiis.Acore.gethokyo(hosent);
                    float kyomu = nownow - wow;
                    nes2 = ki + kyomu;
                    hos2 = hosent + Math.PI;
                }
                //    Console.WriteLine(hosent/Math.PI*180+" :zureas: "+ e.Acore.gethokyo(hosent)+" ||| "+ki+" :ki kyomu: "+kyomu);
                if (Math.Abs(nes1) > Math.Abs(nes2) /*&& nes2 > 0*/)
                {
                    nes1 = nes2;
                    hos1 = hos2;
                }
                if (nes1 > 0)
                {
                    /*
                    Console.WriteLine((ki + kyomu) + " : asdasd : " + (hosent / Math.PI * 180));
                    Console.WriteLine(ki + " : opijiojio : " + kyomu);
                    Console.WriteLine(wow + " : WOWOWOW : " + nownow);*/
                    thiis.Acore.idou((nes1) * (float)Math.Cos(hos1), (nes1) * (float)Math.Sin(hos1));
                    thiis.ab.charaset();
                    return true;
                }
            }
            else
            {
                float hi = e.bif.wei / (e.bif.wei + thiis.bif.wei);
                {
                    float nes1, nes2;
                    double hos1, hos2;
                    {
                        double hosent = e.PAcore.gethosen2(thiis.PAcore);


                        float ki = thiis.Acore.getsaikyo(hosent);


                        double nasukaku2 = e.Acore.nasukaku(thiis.Acore.gettx(), thiis.Acore.getty());
                        double osu2 = hosent - nasukaku2;
                        float wow = e.Acore.kyori(thiis.Acore) * (float)Math.Cos(-osu2);
                        var nownow = e.Acore.gethokyo(hosent);
                        float kyomu = nownow - wow;
                        nes1 = ki + kyomu;
                        hos1 = hosent;
                        /*        
                         double hosent = e.PAcore.gethosen2(PAcore);
                         float ki = thiis.Acore.getsaikyo(hosent);


                         double nasukaku2 = e.Acore.nasukaku(thiis.Acore.gettx(), thiis.Acore.getty());
                         double osu2 = hosent - nasukaku2;
                         float wow = e.Acore.kyori(thiis.Acore) * (float)Math.Cos(-osu2);
                         var nownow = e.Acore.gethokyo(hosent);
                         float kyomu = nownow - wow;
                        */
                        /*if (e.atype == bvc.getatype("tikei") || atype == bvc.getatype("tikei"))
                        {
                            Console.WriteLine("fuckkkkkkk " + (hosent) / Math.PI * 180);


                            //   Console.WriteLine(a + " : XXXXYYY : " + b + " unvo " + thiis.wei);
                            Console.WriteLine((ki + kyomu) + " : asdasd : " + (hosent / Math.PI * 180));
                            Console.WriteLine(ki + " : opijiojio : " + kyomu);
                            Console.WriteLine(wow + " : WOWOWOW : " + nownow);
                        }*/
                    }
                    {
                        double hosent = thiis.PAcore.gethosen2(e.PAcore);
                        float ki = e.Acore.getsaikyo(hosent);



                        double nasukaku2 = thiis.Acore.nasukaku(e.Acore.gettx(), e.Acore.getty());
                        double osu2 = hosent - nasukaku2;

                        float wow = thiis.Acore.kyori(e.Acore) * (float)Math.Cos(-osu2);

                        var nownow = thiis.Acore.gethokyo(hosent);
                        float kyomu = nownow - wow;
                        nes2 = ki + kyomu;
                        hos2 = hosent + Math.PI;
                        /* if (e.atype == bvc.getatype("tikei") || atype == bvc.getatype("tikei"))
                         {
                             Console.WriteLine("fwwwwkkk " + (hosent) / Math.PI * 180);


                             //    Console.WriteLine(a + " : XXXXYYY : " + b + " unvo " + thiis.wei);
                             Console.WriteLine((ki + kyomu) + " : asdasd : " + (hosent / Math.PI * 180));
                             Console.WriteLine(ki + " : opijiojio : " + kyomu);
                             Console.WriteLine(wow + " : WOWOWOW : " + nownow);
                         }*/
                    }

                    // Console.WriteLine(nes1 + " :nes1 hos1: " + hos1);
                    //    Console.WriteLine(nes2 + " :nes2 hos2: " + hos2);
                    if (Math.Abs(nes1) > Math.Abs(nes2)/*&&nes2>0*/ )
                    {

                        nes1 = nes2;
                        hos1 = hos2;
                        //      Console.WriteLine(":::2:::");
                    }
                    else
                    {
                        //   Console.WriteLine(":::1:::");
                    }


                    float a = (nes1) * (float)Math.Cos(hos1), b = (nes1) * (float)Math.Sin(hos1);
                    if (nes1 > 0)
                    {
                        //      Console.WriteLine(a * hi+" :idouone: "+ b * hi + " <asdasd> " + -(a - a * hi)+" :idouyu: "+ -(b - b * hi));
                        thiis.Acore.idou(a * hi, b * hi);
                        e.Acore.idou(-(a - a * hi), -(b - b * hi));
                        e.ab.charaset();
                        thiis.ab.charaset();
                        return true;
                    }
                }

            }


            return false;
        }

        /// <summary>
        /// 自分と対象ので反射を引き起こす。回転は考慮に入ってないつらいから
        /// </summary>
        /// <param name="thiis">自分。重ね重ねっちゃうけどお願い</param>
        /// <param name="e">あいて</param>
        /// <returns>反射が起きたかどうか</returns>
        public bool hansya(Entity thiis,Entity e)
        {
            if (thiis.bif.ovw || !e.atariable || !thiis.atariable) return false;

            float VVX = thiis.bif.vx - e.bif.vx;
            float VVY = thiis.bif.vy - e.bif.vy;
            double hosent = e.Acore.gethosen(thiis.Acore.gettx(), thiis.Acore.getty());
            var hoss = Math.PI / 2;
            //  if (!e.Acore.tokeimawari2()) hoss *= -1;



            double goway = Math.Atan2(VVY, VVX);
            double kusion = Math.Atan2(Math.Sin(Math.PI + hosent - goway), Math.Cos(Math.PI + hosent - goway));
            double kusion2 = Math.Atan2(Math.Sin(hosent - goway), Math.Cos(hosent - goway));


            double mxey = (1 + (thiis.bif.hanpatu + e.bif.hanpatu) / 2) * Math.Sqrt(Math.Pow((VVX) * Math.Cos(hosent), 2) + Math.Pow((VVY) * Math.Sin(hosent), 2));
            //   double Qmxey = 0.1f*(1-(thiis.hanpatu + e.hanpatu) / 2) * Math.Sqrt(Math.Pow((VVX) * Math.Cos(hosent), 2) + Math.Pow((VVY) * Math.Sin(hosent), 2));

            var masatun = Math.Sqrt(Math.Pow((VVX) * Math.Cos(hosent), 2) + Math.Pow((VVY) * Math.Sin(hosent), 2)) * (e.bif.masatu + thiis.bif.masatu);

            if (kusion2 < -Math.PI / 2 || kusion2 > Math.PI / 2)
            {

                if (e.bif.ovw)
                {
                    thiis.bif.vx += (float)(mxey * Math.Cos(hosent));
                    thiis.bif.vy += (float)(mxey * Math.Sin(hosent));

                    var kakkkkaku = Math.Atan2(Math.Sin(hosent + hoss), Math.Cos(hosent + hoss));
                    var bunsp = Math.Sqrt(thiis.bif.vx * thiis.bif.vx + thiis.bif.vy * thiis.bif.vy) * Math.Cos(Math.Atan2(thiis.bif.vy, thiis.bif.vx) - kakkkkaku);
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
                    thiis.bif.vx += (float)(bunsp * Math.Cos(hosent + hoss));
                    thiis.bif.vy += (float)(bunsp * Math.Sin(hosent + hoss));
                    return true;
                }
                else
                {
                    //Console.WriteLine(hosent * 180 / Math.PI + " " + hoss * 180 / Math.PI);
                    float hi = e.bif.wei / (e.bif.wei + thiis.bif.wei);
                    {

                        var a = (float)(mxey * Math.Cos(hosent));
                        var b = (float)(mxey * Math.Sin(hosent));

                        thiis.bif.vx += a * hi;
                        thiis.bif.vy += b * hi;
                        e.bif.vx -= a - a * hi;
                        e.bif.vy -= b - b * hi;
                        {

                            var kakkkkaku = Math.Atan2(Math.Sin(hosent + hoss), Math.Cos(hosent + hoss));
                            var bunsp = Math.Sqrt(thiis.bif.vx * thiis.bif.vx + thiis.bif.vy * thiis.bif.vy) * Math.Cos(Math.Atan2(thiis.bif.vy, thiis.bif.vx) - kakkkkaku);
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
                            thiis.bif.vx += (float)(bunsp * Math.Cos(hosent + hoss));
                            thiis.bif.vy += (float)(bunsp * Math.Sin(hosent + hoss));

                        }
                        {
                            var kakkkkaku = Math.Atan2(Math.Sin(hosent + hoss), Math.Cos(hosent + hoss));
                            var bunsp = Math.Sqrt(e.bif.vx * e.bif.vx + e.bif.vy * e.bif.vy) * Math.Cos(Math.Atan2(e.bif.vy, e.bif.vx) - kakkkkaku);
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
                            e.bif.vx += (float)(bunsp * Math.Cos(hosent + hoss));
                            e.bif.vy += (float)(bunsp * Math.Sin(hosent + hoss));
                            return true;
                        }
                    }

                }

            }
            return false;
        }
    }
}
