using System;
using System.Collections.Generic;
using Charamaker2;
using Charamaker2.Character;
using Charamaker2.Shapes;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Office.Utils;
using DevExpress.Xpo.DB;
using DevExpress.XtraEditors;

namespace GameSet1
{
    /// <summary>
    /// あれよGameSet1のイベントあーぎゅメンツよ
    /// </summary>
    public class EEventArgs 
    {
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="e">Eventerの元</param>
        /// <param name="tag">効果先</param>
        /// <param name="cl">クロック</param>
        public EEventArgs(Entity e,Entity tag=null,float cl=1) 
        {
            ent = e;
            this.cl = cl;
            this.tag = tag;
        }
        /// <summary>
        /// Eventerの仕えるエンテティ
        /// </summary>
        public Entity ent;
        /// <summary>
        /// 効果先のエンテティ
        /// </summary>
        public Entity tag;
        /// <summary>
        /// クロック
        /// </summary>
        public float cl;

    }
    /// <summary>
    /// エンテティに付属するイベント管理官
    /// </summary>
    public class EEventer
    {
        /// <summary>
        /// 元のエンテティ
        /// </summary>
        protected Entity e;
        /// <summary>
        /// 普通のコンストラクタ。
        /// </summary>
        /// <param name="E">仕えるエンテティ</param>
        public EEventer(Entity E)
        {
            e = E;
        }
        /// <summary>
        /// addされたときのイベント
        /// </summary>
       public event EventHandler<EEventArgs> added;
        /// <summary>
        /// removeされたときのイベント
        /// </summary>
        public event EventHandler<EEventArgs> removed;
        /// <summary>
        /// frameされたときのイベント
        /// </summary>
        public event EventHandler<EEventArgs> framed;

        /// <summary>
        /// 反射されたときのイベント
        /// </summary>
        public event EventHandler<EEventArgs> hansyad;


        /// <summary>
        /// フレームしたときに呼び出されるやつ。
        /// </summary>
        /// <param name="sender">呼び出し元</param>
        /// <param name="cl">クロック</param>
        public void frame(object sender,float cl) 
        {
            framed?.Invoke(sender, new EEventArgs(e,null, cl));
        }
        /// <summary>
        /// addしたときに呼び出されるやつ。
        /// </summary>
        /// <param name="sender">呼び出し元</param>
        public void add(object sender)
        {
           
            added?.Invoke(sender, new EEventArgs(e));
        }
        /// <summary>
        /// removeしたときに呼び出されるやつ。
        /// </summary>
        /// <param name="sender">呼び出し元</param>
        public void remove(object sender)
        {
            removed?.Invoke(sender, new EEventArgs(e));
        }
        /// <summary>
        /// 反射したときに呼び出される
        /// </summary>
        /// <param name="sender">呼び出し元</param>
        /// <param name="tag">あたり先</param>
        public void hansya(object sender,Entity tag)
        {
            hansyad?.Invoke(sender, new EEventArgs(e,tag));
        }
    }
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
        private ataribinding _Wpab;
        /// <summary>
        /// 空気抵抗とかの物理のインフォメーションとか
        /// </summary>
        protected buturiinfo _bif;

        /// <summary>
        /// EMに乗ってるかどうか
        /// </summary>
        public bool onEM
        {
            get{
                if(_EM==null) return false;

                return EM.ents.Contains(this);
            } 
        }

        /// <summary>
        /// 新しく辺りバインディングを設定する。pabもリセットするし
        /// </summary>
        /// <param name="recipie">レシピ。コピーされたものが使われる</param>
        public void setNewAtariBinding(ABrecipie recipie) 
        {
            _ab = new ataribinding(c, recipie);
            _pab = new ataribinding(c, recipie);
   
        }
        /// <summary>
        /// 新しく辺りバインディングを設定する。pabもリセットするし
        /// </summary>
        /// <param name="info">レシピ。コピーされたものが使われる</param>
        public void setNewAtariBinding(buturiinfo info)
        {
            _bif = new buturiinfo( info);

        }
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
        public bool atariable { get { return ab.core != null ; } }
        /// <summary>
        /// 今の物理的なあたり判定に使うやーつ
        /// </summary>
        public Shape Acore { get { return ab.core; } }
        /// <summary>
        /// 昔の物理的なあたり判定に使うやーつ
        /// </summary>
        public Shape PAcore { get { if (pab != null) return pab.core; return ab.core; } }

        #region okari
        bool okarsitaiAB = false;
        /// <summary>
        /// あたり判定をお借りする。お借り状態から抜け出すにはchangeAB
        /// </summary>
        /// <param name="e">お借り元</param>
        public void okariAB(Entity e) 
        {
            _ab = e._ab;
            _pab = e._pab;
            _Wpab = e._Wpab;
            okarsitaiAB = true;
        }
        /// <summary>
        /// あたりバインディングを変更する。お借りしてた場合リセット
        /// </summary>
        /// <param name="a"></param>
        public void changeAB(ABrecipie a) 
        {

            _ab = new ataribinding(c, a);
            _pab = new ataribinding(c, a);
            _Wpab = new ataribinding(c, a);
            okarsitaiAB = false;
        }
        #endregion

        /// <summary>
        /// イベントを司るもの。継承したやつ使うならsetEEventerをオーバーライドしろ
        /// </summary>
        protected EEventer _EEV;
        /// <summary>
        /// イベントを司るもの
        /// </summary>
        public EEventer EEV { get { return _EEV; } }
        /// <summary>
        /// EEventerをセットする。 EEventerが継承した奴ならオーバーライドしろ
        /// </summary>
        protected virtual void setEEventer()
        {
            _EEV = new EEventer(this);
        }
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="chara">キャラクター</param>
        /// <param name="recipie">あたり判定のレシピ。コピーされたものが使われる</param>
        /// <param name="ai">物理とかの情報。コピーされたものが使われる</param>
        public Entity(character chara, ABrecipie recipie, buturiinfo ai)
        {
            _c = chara;
            _ab = new ataribinding(c, recipie);
            _pab = new ataribinding(c, recipie);
            _Wpab = new ataribinding(c, recipie);
            _bif = new buturiinfo(ai);
            
            setEEventer();
            
        }
        /// <summary>
        /// コピーするためのコンストラクタ。当たりバインディングはリセットされる
        /// </summary>
        /// <param name="E">コピー元</param>
        public Entity(Entity E)
        {
            _c = new character(E.c, true, false);

            _ab = new ataribinding(c, E.ab.RECIPIE);
            _pab = new ataribinding(c, E.ab.RECIPIE);
            _Wpab = new ataribinding(c, E.ab.RECIPIE);
            _bif = (buturiinfo)Activator.CreateInstance(E.bif.GetType(), E.bif);
            setEEventer();
        }

        /// <summary>
        /// フレーム処理
        /// </summary>
        /// <param name="cl">フレームの長さ</param>
        virtual public void frame(float cl)
        {
          

            bif.frame(cl, c);

            c.frame(cl);

            setab(false);

        }
        /// <summary>
        /// エンテティが、物理的に移動した後に起こるフレーム
        /// </summary>
        /// <param name="cl">クロックの長さ</param>
       virtual public void endframe(float cl) 
        {


            setwpab();
            c.soroeru();

           // Console.WriteLine(c.gettx() + " alskf;@qw@ep " + c.getty());
            foreach (var a in wazas)
            {
                a.frame(cl);
          //      Console.WriteLine(c.gettx() + " alskf;@qw@ep " + c.getty());
            }
          //  Console.WriteLine(c.gettx() + " alskf;@qw@ep " + c.getty());
            EEV.frame(this, cl);
            c.soroeru();
            kirikaewpab();
            setab(false);
        }
        void setwpab() 
        {
            if (!okarsitaiAB)
            {
                _Wpab.frame();
            }
        }
         void kirikaewpab()
        {
            var t = _pab;
            _pab = _Wpab;
            _Wpab = t;
        }
        /// <summary>
        /// 今で、昔のあたり判定をセットする
        /// </summary>
        virtual public void setpab()
        {
            if (!okarsitaiAB)
            {

                pab.frame();
            }
        }
        /// <summary>
        /// 今のあたり判定をセットする。初期とかワープしたときとかは呼び出してね
        /// </summary>
        virtual public void setab(bool pabtoo = false)
        {
            if (!okarsitaiAB)
            {

                ab.frame();
                if (pabtoo) pab.frame();
            }
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
        /// 便利ショトカ。表示マン
        /// </summary>
        public hyojiman hyoji { get { return _EM.hyoji; } }


        /// <summary>
        /// エンテティマネージャー―に追加したいときに呼ぶメソッド。
        /// </summary>
        /// <param name="e">追加する奴。同時に複数のマネージャーには追加しちゃいけない。ごめんね</param>
        /// <param name="add">先頭に入れたいならfalse</param>
        /// <returns>追加されたか。trueならついでにonAddが呼び出される。</returns>
        virtual public bool add(EntityManager e,bool add=true) 
        {
            if ( e.add(this,add))
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
        /// 標準ではabのリセットとキャラの表示
        /// </summary>
        virtual protected void onAdd()
        {

            c.resethyoji(EM.hyoji);
            setab(true);
            EEV.add(this);
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
            EEV.remove(this);
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
            if (!t.IsSubclassOf(typeof(Waza))) return new List<Waza>();
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
        /// タイプで絞り込んで技のリストを取得する
        /// </summary>
        /// <typeparam name="T">タイプ</typeparam>
        /// <returns>技のリスト</returns>
        public List<T> getwazalis<T>()
        where T : Waza
        {
            var res = new List<T>();
            if (typeof(T) == typeof(Waza))
            {
                foreach (var a in _wazas)
                {
                    res.Add((T)a);
                }
                return res;
            }

            Type tt;
            foreach (var a in _wazas)
            {
                tt = a.GetType();
                if (tt == typeof(T) || tt.IsSubclassOf(typeof(T)))
                {
                    res.Add((T)a);
                }
            }
            return res;
        }
        /// <summary>
        /// 技を追加する。基本Waza.add(e)で呼び出せ
        /// </summary>
        /// <param name="w">追加する技</param>
        /// <returns>追加できた</returns>
        internal bool addWaza(Waza w) 
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
        internal bool removeWaza(Waza w) 
        {
            return _wazas.Remove(w);
        }


        


        /// <summary>
        /// この瞬間にずれさせる。
        /// </summary>
        /// <param name="tekiyous">ずれを適用する奴ら。</param>
        /// <param name="sugekae">あたり判定をすげかえるんだったらこれ</param>
        /// <param name="Psugekae">これも前のすでに同じエンテティの当たりバインディングにある奴を使和ナイト機能しない</param>
        /// <param name="hansya">ついでに反射もさせるか</param>
        /// <returns>ヒットしたエンテティ</returns>
        public List<Entity> zurentekiyou(List<Entity> tekiyous, Shape sugekae = null,Shape Psugekae=null,bool hansya=false)
        {
            var res = new List<Entity>();
            Shape HPAC = PAcore, HAC = Acore;
            if (sugekae!=null) 
            {
                ab.coresugekae(sugekae);
                
            }
            if (Psugekae != null)
            {
                pab.coresugekae(Psugekae);

            }
            if (this.Acore != null&&this.PAcore!=null)
            {
                foreach (var a in tekiyous)
                {
                   // Console.WriteLine((a.PAcore != null) + "paiza"+(a.Acore != null) +" iahsfu "+ a.Acore.atarun2(a.PAcore, this.Acore, this.PAcore));
                    if (a.PAcore != null && a.Acore != null&& Shape.atarun(a.Acore,a.PAcore, this.Acore, this.PAcore)) 
                    {
                        //Console.WriteLine("zanza");
                        var saa=this.bif.zuren(this, a,false);
                        if(saa!=null)Console.WriteLine(saa.ToString());
                        res.Add(a);
                        if (hansya &&saa!=null) 
                        {
                            this.bif.hansya(this, a,saa);
                        }
                    }
                }
            }
          //  Console.WriteLine(res.Count+" asdl;fmka; ");
            
            ab.coresugekae(HAC);
            pab.coresugekae(HPAC);
            return res;
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
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="setunames">対象の節の名前""でキャラクターそのもの</param>
        /// <param name="shapetemple">図形のテンプレート大きさとか座標は気にしない</param>
        public ABrecipie(List<string> setunames, Shape shapetemple)
        {
            names = new List<string>(setunames);
            shapes = new List<Shape>();
            for (int i = 0; i < names.Count; i++)
            {
                shapes.Add( shapetemple.clone());
            }
        }
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="setunames">対象の節の名前""でキャラクターそのもの</param>
        /// <param name="shapetemple">図形のテンプレート大きさとか座標は気にしない</param>
        public ABrecipie( Shape shapetemple,params string[] setunames)
        {
            names = new List<string>(setunames);
            shapes = new List<Shape>();
            for (int i = 0; i < names.Count; i++)
            {
                shapes.Add(shapetemple.clone());
            }
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
        /// あたり判定を行うコアをすげかえる。コレは一時的なものなので、必ず戻すように。
        /// frameでも揃えられるわけじゃないし、setuListに追加されるわけでもない。
        /// 一時的に物理判定を使いたいときとかに使ってね。
        /// </summary>
        /// <param name="tmp">入れ替える奴</param>
        /// <returns>入れ替え前の奴</returns>
        public Shape coresugekae(Shape tmp) 
        {
            var res=this._core;
            _core = tmp;
            return res;
        }

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
        /// あたり判定の全てのリストを得る
        /// </summary>
        /// <returns>順番は保証する。しかしnullが混じっているかも</returns>
        public List<Shape> getallatari()
        {
           
            return new List<Shape>(this.rec.shapes);
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
        /// あたり判定のリストを得る
        /// </summary>
        /// <param name="name">名前共</param>
        /// <returns>順番は保証する。しかしnullが混じっているかも</returns>
        public List<Shape> getatari(List<string>name)
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
            frame();
        }
        /// <summary>
        /// キャラクターを物理用のあたり判定の方に合わせる
        /// </summary>
        virtual public void charaset() 
        {
            if (_core != null)
            {
                c.RAD = _core.rad;
                //  if (!c.mirror)
                {
                    c.setcxy(_core.getcx(0, 0), _core.getcy(0, 0), 0, 0);
                }
              
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
                    rec.shapes[i].setMirror((int)fileman.plusminus(set[i].p.mir, false));
                    rec.shapes[i].w = set[i].p.w;
                    rec.shapes[i].h = set[i].p.h;
                    rec.shapes[i].rad = set[i].p.RAD;
                    if (!c.mirror)
                    {
                        rec.shapes[i].setcxy(set[i].p.getcx(0, 0), set[i].p.getcy(0, 0), 0, 0);
                    }
                    else 
                    {
                        rec.shapes[i].setcxy(set[i].p.getcx(set[i].p.w, 0), set[i].p.getcy(set[i].p.w, 0), 0, 0);
                    }
                    
                }
                else
                {
                    rec.shapes[i].setMirror((int)fileman.plusminus(c.mirror, false));
                  //  if (!c.mirror)
                    {
                        rec.shapes[i].w = c.w;
                        rec.shapes[i].h = c.h;
                        rec.shapes[i].rad = c.RAD;


                        rec.shapes[i].setcxy(c.getcx(0, 0), c.getcy(0, 0), 0, 0);
                    }
                   
                }
            }
        }

    }

    /// <summary>
    /// 接点を保存するクラス
    /// </summary>
    public class TouchPoint
    {
        /// <summary>
        /// 接点の座標
        /// </summary>
        public FXY xy;
        /// <summary>
        /// 接している辺の情報
        /// </summary>
        public lineX line;
        /// <summary>
        /// 誰との接点か、誰の頂点によって接触しているか
        /// </summary>
        public Entity e,from;
	/// <summary>
	/// 普通のコンストラクタ
	/// </summary>
	/// <param name="fxy">接点の位置</param>
	/// <param name="linex">接戦</param>
	/// <param name="from">誰による頂点で接触したか</param>
	/// <param name="e">接触相手</param>
	public TouchPoint(FXY fxy, lineX linex, Entity from, Entity e)
        {
            xy = fxy;
            line = linex;
            this.e = e;
            this.from = from;
        }

    };

    /// <summary>
    /// 物理をやるインフォメーション
    /// </summary>
    public class buturiinfo
    {
        /// <summary>
        /// 御存じ速度加速度
        /// </summary>
        public float vx, _vy, ax, ay;

        public float vy{get{return _vy;}set { _vy = value; } }
        /// <summary>
        /// 物体の速度
        /// </summary>
        public float speed { get { return (float)Math.Sqrt(vx * vx + vy * vy); } }
        /// <summary>
        /// スピードの方向
        /// </summary>
        public double speedvec { get { return Math.Atan2(vy, vx); } }

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
        public static float MW = 1000000;
        /// <summary>
        /// 重さ 0＜＜MW
        /// -1でMW
        /// </summary>
        public float wei { get { return _wei; } set { _wei = value; if (_wei <= 0) _wei = MW ; if (_wei >= MW) _wei = MW ; } }
        /// <summary>
        /// 重さが最大かどうか
        /// </summary>
        public bool ovw { get { return overweights(_wei);  } }
        /// <summary>
        /// その重さが無限かどうか調べる。
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        static public bool overweights(float weight) 
        {
           return weight >= MW;
        }

        

        /// <summary>
        /// あたり判定の分類
        /// </summary>
        public List<string> atag;
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
        public buturiinfo(float wei = 1, float tik = 0, float hanp = 0, float mas = 0, float vx = 0, float vy = 0, float ax = 0, float ay = 0, List<string> atag=null)
        {
            this.wei = wei;
            teikou = tik;
            hanpatu = hanp;
            masatu = mas;
            this.vx = vx;
            this.vy = vy;
            this.ax = ax;
            this.ay = ay;
            if (atag == null)
            {
                this.atag = new List<string>();
            }
            else 
            {
                this.atag = new List<string>(atag);
            }
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
                this.vx += vx * weight / (weight + this.wei);
                this.vy += vy * weight / (weight + this.wei);
            }
        }
        /// <summary>
        /// 重さに応じて加速できたりする。こちらは加速度として処理する。（つまりaxみたいに毎フレーム呼び出される奴はこっち使え）
        /// </summary>
        /// <param name="e">二度手間だけどお願い。そういう仕組みなんだ</param>
        /// <param name="vx">x方向の加速度</param>
        /// <param name="vy">y方向の加速度</param>
        /// <param name="weight">加速の重さ。0以下でvxyをそのままぶち込む</param>
        /// <param name="cl">今何フレーム経過してんのよ</param>
        public void kasoku(Entity e,float vx, float vy, float weight ,float cl)
        {
            if (weight <= 0)
            {
                e.c.x += vx * cl * cl / 2;
                e.c.y += vy * cl * cl / 2;
                this.vx += vx * cl;
                this.vy += vy * cl;
            }
            else
            {
                e.c.x += vx * weight / (weight + this.wei) * cl * cl / 2;
                e.c.y += vy * weight / (weight + this.wei) * cl * cl / 2;
                this.vx += vx * cl * weight / (weight + this.wei);
                this.vy += vy * cl * weight / (weight + this.wei);
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
            //Console.WriteLine(vy + " asfka; ");
            vx += ax * cl;
            vy += ay * cl;
            vx *= (float) Math.Pow(1-teikou, cl);
            vy *= (float) Math.Pow(1-teikou, cl);
          
            c.settxy(c.gettx()+vx * cl + ax * cl * cl / 2, c.getty()+ vy * cl + ay * cl * cl / 2);
       
            
            //Console.WriteLine(vy + " asQQQQQ; ");
        }

        /// <summary>
        /// ちゃんと移動させるメソッド。Acoreもcharacterも動かす
        /// しかし、abを基準に移動するため、waza.framedとかで軽率に呼び出すと死ぬ
        /// </summary>
        /// <param name="thiis">設計理念上こうなってしまったんや済まない</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void idou(Entity thiis,float x, float y) 
        {
            if (thiis.atariable)
            {
                //Console.WriteLine(" -> " + thiis.Acore.getCenter().ToString());
                thiis.Acore.idou(x, y);
                thiis.ab.charaset();
              //  Console.WriteLine(x + " :XY: " + y + " -> " + thiis.Acore.getCenter().ToString());

            }
            else 
            {
                thiis.c.idouxy(x, y);
            }
        }

        /// <summary>
        /// 相手と自分をなんか計算してずらす！
        /// ちなみにここでは大丈夫だけどataribinding.coreを動かしたらcharasetしないと移動が反映されないからちうい
        /// </summary>
        /// <param name="thiis">自分。重ね重ねっちゃうけどお願い</param>
        /// <param name="e">相手</param>
        /// <param name="group">グループとして当たるか</param>
        /// <returns>ずらしたかどうか</returns>
        public lineX zuren(Entity thiis,Entity e,bool group)
        {
            if (!e.atariable || !thiis.atariable || (thiis.bif.ovw && e.bif.ovw))
            {

               // Console.WriteLine("ZOUiojoijN");
                return null;
            }
            if (group) 
            {
                if (buturiinfo.overweights(e.bif.groupwei)&& buturiinfo.overweights(thiis.bif.groupwei)) 
                {
                    //Console.WriteLine("ZOUN");
                    return null;
                }
            }
            bool ok1=false, ok2 = false;
            bool ok12 = false, ok22 = false;

            FXY idou=new FXY(0, 0);
            FXY idou2 = new FXY(0, 0);
            lineX line1,line2;
            {
                line1 = e.Acore.getnearestline(thiis.PAcore.getCenter());
                var points = thiis.Acore.getzettaipoints(false);

               // Console.WriteLine(line1.ToString() + " asd ");
                //TRACE(_T("!!!!!!!!!!!!!\n"));
                for (int i = 1; i < points.Count - 1; i++)
                {

                    var tmp = Shape.getzurasi(points[i], line1);
                    //Console.WriteLine(points[i].ToString()+" :thiis");
                    //  Console.WriteLine(tmp.X + " soy " + tmp.Y);
                    if (!float.IsNaN(tmp.X) && tmp.length >= idou.length)
                    {
                        idou.X = tmp.X;
                        idou.Y = tmp.Y;
                        ok1 = true;
                        ok12 = true;
                    }
                    else if(!float.IsNaN(tmp.Y)) 
                    {
                        ok12 = true;
                    }

                }
            }
            //TRACE(_T("!!!!!!!!!!!!!\n"));
            {
                line2 = thiis.Acore.getnearestline(e.PAcore.getCenter());

                //Console.WriteLine(line2.ToString()+" asd ");
                var points = e.Acore.getzettaipoints(false);
                for (int i = 1; i < points.Count - 1; i++)
                {

                   // Console.WriteLine(points[i].ToString() + " :E");
                    var tmp = Shape.getzurasi(points[i], line2);
                     //  Console.WriteLine(tmp.X + " sey " + tmp.Y);
                    if (!float.IsNaN(tmp.X) && tmp.length >= idou2.length)
                    {

                        idou2.X = -tmp.X;
                        idou2.Y = -tmp.Y;
                        ok2 = true;
                        ok22 = true;
                    }
                    else if (!float.IsNaN(tmp.Y))
                    {
                        ok22 = true;
                    }

                }
            }
            //	idou.x += idou2.x;
            //	idou.y += idou2.y;
            // if (thiis.bif.ovw || e.bif.ovw)
            //    Console.WriteLine(idou.ToString() + "asfa" + idou2.ToString());
          /*  if (idou.length > 50||idou2.length>50)
            {
                Console.WriteLine(ok1 + " KKKKKKKKKK " + ok2);
                Console.WriteLine(idou.X + " a:fkqqql " + idou.Y );
                Console.WriteLine(line1.ToString() + "  ::linee");
                Console.WriteLine(idou2.X + " a:fqkqql " + idou2.Y);
                Console.WriteLine(line2.ToString() + "  ::linee2");
            }*/
            if (!ok12 || !ok22) return null;
            if (ok1 && ok2)
            {
                //Console.WriteLine(idou.ToString()+"double" +idou2.ToString());
                if (Math.Pow(idou2.X, 2) + Math.Pow(idou2.Y, 2) < Math.Pow(idou.X, 2) + Math.Pow(idou.Y, 2))
                {
                    idou.X = idou2.X;
                    idou.Y = idou2.Y;
                    line1 = line2;
                }
            }
            else if(!ok1) 
            {
                idou.X = idou2.X;
                idou.Y = idou2.Y;
                line1 = line2;
            }
          //  if (idou.X == 0 && idou.Y == 0) return null;
         //   if(thiis.bif.ovw||e.bif.ovw)
          //  Console.WriteLine("idou:->" +idou.ToString());

            //TRACE(_T("%d idou %f::%f\n"),notNAN,idou.x,idou.y);

            float eweight;
            float thisweight;

            if (group)
            {
                eweight = e.bif.groupwei;
                thisweight = thiis.bif.groupwei;
            }
            else
            {

                eweight = e.bif.wei;
                thisweight = thiis.bif.wei;
            }

            if (e.bif.ovw)
            {
                //	TRACE(_T("SOY\n"));
                if (group)
                {
                    if (thiis.bif.ovw)
                    {
                        thiis.bif.idou(thiis, idou.X, idou.Y);
                    }
                    else
                    {

                        thiis.bif.groupidou(thiis, idou.X, idou.Y);
                    }
                }
                else
                {
                    thiis.bif.idou(thiis, idou.X, idou.Y);
                }
                //this.idouAdd(idouinfo(idou, Entity::overweight));
            }
            else if (thiis.bif.ovw) 
            {
                if (group)
                {
                    if (e.bif.ovw)
                    {
                        e.bif.idou(e, -idou.X, -idou.Y);
                    }
                    else
                    {

                        e.bif.groupidou(e, -idou.X, -idou.Y);
                    }
                }
                else
                {
                    e.bif.idou(e, -idou.X, -idou.Y);
                }
            }
            else {
                // Console.WriteLine(thiis.PAcore.gettx() + " :XY: " + thiis.PAcore.getty() + " <PRE> "
                //  + e.PAcore.gettx() + " :XY: " + e.PAcore.getty());
                //  Console.WriteLine(thiis.Acore.gettx() + " :XY: " + thiis.Acore.getty() + " <bef> "
                //     + e.Acore.gettx() + " :XY: " + e.Acore.getty());
                //Console.WriteLine(idou.ToString()+" al;fkapo ");
                var hi = thisweight / (thisweight + eweight);
                if (group)
                {
                    thiis.bif.groupidou(thiis,(1 - hi) * idou.X, (1 - hi) * idou.Y);
                    e.bif.groupidou(e,-(hi) * idou.X, -(hi) * idou.Y);


                    if (idou.X != 0 || idou.Y != 0)
                    {
                        thiis.bif.groupAdd(thiis, e);
                    }
                }
                else
                {
                    thiis.bif.idou(thiis,(1 - hi) * idou.X, (1 - hi) * idou.Y);

                    e.bif.idou(e,(-hi) * idou.X, (-hi) * idou.Y);
                }

            //    Console.WriteLine(thiis.Acore.gettx() + " :XY: " + thiis.Acore.getty() + " <aft> "
            //        + e.Acore.gettx() + " :XY: " + e.Acore.getty());

                //	TRACE(_T("%f ZOY %f + %f\n"),idou.x, (1 - hi) * idou.x, (hi) * idou.x);
            }
           
               // Console.WriteLine(idou.X + " a:fkl " + idou.Y + " :: " + thisweight + " -> " + eweight);
               // Console.WriteLine(line1.ToString()+"  ::linee");
            
            //var line = e.s.getnearestline(this.s.getCenter());
            return line1;
        }
        /// <summary>
        /// 自分と対象ので反射を引き起こす。回転は考慮に入ってないつらいから
        /// </summary>
        /// <param name="thiis">自分。重ね重ねっちゃうけどお願い</param>
        /// <param name="e">あいて</param>
        /// <param name="line">ぶつかった辺</param>
        /// <returns>反射が起きたかどうか</returns>
        public bool hansya(Entity thiis, Entity e, lineX line) 
        {
            //Console.WriteLine(line.ToString());
            if (line == null)
            {
                return false;
            }
            return hansya(thiis, e, line.rad);
        }
        /// <summary>
        /// 自分と対象ので反射を引き起こす。回転は考慮に入ってないつらいから
        /// </summary>
        /// <param name="thiis">自分。重ね重ねっちゃうけどお願い</param>
        /// <param name="e">あいて</param>
        /// <param name="rad">ぶつかった辺の角度</param>
        /// <returns>反射が起きたかどうか</returns>
        public bool hansya(Entity thiis,Entity e,double rad)
        {
            float eweight = e.bif.wei;
            float thisweight = thiis.bif.wei;

            if (buturiinfo.overweights(eweight) && buturiinfo.overweights(thisweight)) return false;

            var hansya = (thiis.bif.hanpatu + e.bif.hanpatu) / 2;
            var masatu = (thiis.bif.masatu + e.bif.masatu) / 2;

            FXY toe=new FXY(0, 0);
            FXY tothis = new FXY(0, 0);


            var hos = rad;
            var hoh = thiis.PAcore.nasukaku(e.PAcore);
            if (Shape.radseiki(hos - hoh) >= 0)
            {
                hos += Math.PI / 2;
            }
            else
            {
                hos -= Math.PI / 2;
            }
            hos = Shape.radseiki(hos);
            //TRACE(_T(" %f :kakudo: %f\n"), rad / M_PI * 180, hos / M_PI * 180);
            //hos = line.gethosen();
            //rad = line.getrad();
            //TRACE(_T("%f :: %f   %f = %f\n"),hos/M_PI*180, rad / M_PI * 180, line2.getrad(), line.getrad())

            float evx = e.bif.vx, evy = e.bif.vy, thisvx = thiis.bif.vx, thisvy = thiis.bif.vy;


            var sp = (float)(Math.Cos(hos) * (thisvx - evx) + Math.Sin(hos) * (thisvy - evy));
            var spDMMY = (float)(Math.Cos(hos) * (thisvx) + Math.Sin(hos) * (thisvy));
            var gsp = (float)(Math.Cos(hos) * (-thisvx + evx) + Math.Sin(hos) * (-thisvy + evy));
            var sp2 = (float)(Math.Cos(rad) * (thisvx - evx) + Math.Sin(rad) * (thisvy - evy));
            if (sp < 0)
            {

            }
            else
            {
            //    Console.WriteLine(thiis.bif.vx + " " + thiis.bif.vy + "  :sokubef: " + e.bif.vx + " " + e.bif.vy);

            //    Console.WriteLine(sp+" :CANCELLED: "+hos/Math.PI*180);
                return false;
            }
           // Console.WriteLine(sp + " :KEIZOKUED: " + hos / Math.PI * 180);

            if (thiis.bif.ovw)
            {
                var msatupower = Math.Abs(masatu * sp);
                if (msatupower > Math.Abs(sp2))
                {
                    sp2 = sp2;
                }
                else
                {
                    if (sp2 < 0)
                    {
                        sp2 = -msatupower;
                    }
                    else
                    {
                        sp2 = +msatupower;
                    }

                }
                //TRACE(_T("ohhhhhh111\n"));
                toe.X += sp * (hansya + 1) * (float)Math.Cos(hos);
                toe.Y += sp * (hansya + 1) * (float)Math.Sin(hos);

                toe.X += sp2 * (float)Math.Cos(rad);
                toe.Y += sp2 * (float)Math.Sin(rad);
            }
            else if (e.bif.ovw)
            {
                var msatupower = Math.Abs(masatu * sp);
                if (msatupower > Math.Abs(sp2))
                {
                    sp2 = sp2;
                }
                else
                {
                    if (sp2 < 0)
                    {
                        sp2 = -msatupower;
                    }
                    else
                    {
                        sp2 = +msatupower;
                    }

                }
                //TRACE(_T("ohhhhhh222\n"));
                tothis.X -= sp * (hansya + 1) * (float)Math.Cos(hos);
                tothis.Y -= sp * (hansya + 1) * (float)Math.Sin(hos);

                tothis.X -= sp2 * (float)Math.Cos(rad);
                tothis.Y -= sp2 * (float)Math.Sin(rad);
            }
            else
            {
                float masatupower;
                float m1 = thisweight;
                float m2 = eweight;
                {
                    var v1z = (float)Math.Cos(hos) * (thisvx) + (float)Math.Sin(hos) * (thisvy);
                    var v2z = (float)Math.Cos(hos) * (evx) + (float)Math.Sin(hos) * (evy);
                    var vm =
                        ((float)Math.Cos(hos) * (thisvx * thisweight + evx * eweight) 
                        + (float)Math.Sin(hos) * (thisvy * thisweight + evy * eweight));
                  //  Console.WriteLine(v1z + " v1v2" + v2z + " = " + vm);

                    var vvm = (float)Math.Cos(hos) * (float)Math.Cos(hos) * (thisvx * thisvx * thisweight + evx * evx * eweight) / 2
                        + (float)Math.Sin(hos) * (float)Math.Sin(hos) * (thisvy * thisvy * thisweight + evy * evy * eweight) / 2;

                    //TRACE(_T("%f jaisfjoa \n"),cos(hos) * cos(hos) * (this.vx * this.vx * thisweight + e.vx * e.vx * eweight));
                    //TRACE(_T("%f jaisfjoa %f\n"), cos(hos) ,(this.vx * this.vx * thisweight + e.vx * e.vx * eweight));
                    //TRACE(_T("%f jaisfjoa %f :: %f\n"), hos, (this.vx * this.vx * thisweight),( e.vx * e.vx * eweight));
                    float v1 = 0, v2 = 0;
                    float v11 = 0;
                    float v12 = 0;

                   


                    float hansyapower = (1 - hansya) * (float)Math.Sqrt(2 * vvm / (m1 + m2)) * (m1 + m2);

                    //TRACE(_T("%f . %f oar %f\n"), vm, vm * hansya + hansyapower, vm * hansya - hansyapower);
                    if (Math.Abs(hansyapower - vm) < Math.Abs(-hansyapower - vm))
                    {
                        vm = vm * hansya + hansyapower;
                    }
                    else
                    {
                        vm = vm * hansya - hansyapower;
                    }
                    //TRACE(_T("%f ninatavmVM\n"), vm);
                    float naka = (4 * m1 * m1 / m2 / m2 * vm * vm) - 4 * (m1 + m1 * m1 / m2) * (vm * vm / m2 - 2 * vvm);
                    if (naka < 0)
                    {
                        //	TRACE(_T("ohhhhhh %f\n"),naka);
                        naka *= -1;
                    }
                    float sqrt = (float)Math.Sqrt(naka);

                    v11 = (2 * vm * m1 / m2 + sqrt) / 2 / (m1 + m1 * m1 / m2);
                    v12 = (2 * vm * m1 / m2 + sqrt) / 2 / (m1 + m1 * m1 / m2);

                    v1 = v11;

                    v2 = (vm - v1 * m1) / m2;
                   // Console.WriteLine(v1 + " yattaze " + v2+" a ;: "+vm);
                    if (Math.Abs(-v1 * thisweight + v2 * eweight - vm)
                    > Math.Abs(v1 * thisweight + v2 * eweight - vm))
                    {
                        v1 = -v1;
                    }
                    if (Math.Abs(-v1 * thisweight + v2 * eweight - vm)
                        > Math.Abs(-v1 * thisweight - v2 * eweight - vm))
                    {
                        v2 = -v2;
                    }
                    if (Math.Abs(-v1 * thisweight + v2 * eweight - vm)
                        > Math.Abs(v1 * thisweight - v2 * eweight - vm))
                    {
                        v1 = -v1;

                        v2 = -v2;
                    }

                    //TRACE(_T("%f or %f  tina %f\n v2.%f tina %f\n"), v11, v12, v1z,v2,v2z);
                    //TRACE(_T("%f = %f ,,\n,, %f = %f\n %f ~~~ %f\n"),v1*m1+v2*m2,vm
                    //	,v1*v1*m1/2+v2*v2*m2/2,vvm,v1,v2);

                    //TRACE(_T("%f :kekktoku: %f\n"), -(v1+v1z) * cos(hos), (v2+v2z) * cos(hos));

                    //	TRACE(_T("%f . %f :vmvvm: %f  \n"), vm,vm*vm/m1,-vvm);
                    //	TRACE(_T(" %f :v1v2: %f   tina %f - %f > 0 ?? %f\n"), v1, v2, (4 * m2 * m2 / m1 / m1*vm*vm) , 4 * (vm * vm/m1 - vvm) * (m2 * m2 / m1+m1),sqrt);
                   // Console.WriteLine(v1 + " yattaze " + v2);
                    tothis.X += -(v1 + v1z) * (float)Math.Cos(hos);
                    tothis.Y += -(v1 + v1z) * (float)Math.Sin(hos);

                    toe.X += (v2 - v2z) * (float)Math.Cos(hos);
                    toe.Y += (v2 - v2z) * (float)Math.Sin(hos);
                     masatupower = Math.Abs(v1 + v1z) / 2 * thisweight + Math.Abs(v2 - v2z)  / 2 * eweight;

                }
               
              
                {
                    float thisspeeed = thisvx * (float)Math.Cos(rad) + thisvy * (float)Math.Sin(rad);
                    float espeeed = evx * (float)Math.Cos(rad) + evy * (float)Math.Sin(rad);

                    var vm = (thisspeeed*thisweight)+(espeeed*eweight);

                    float dousoku = vm / (thisweight + eweight);
                   float gone = dousoku - thisspeeed;
                    gone *= thisweight;
                    if (Math.Abs(gone) > masatupower) 
                    {
                        if (gone > 0)
                        {
                            gone = masatupower;
                        }
                        else 
                        {
                            gone = -masatupower;
                        }
                    }
                  //  Console.WriteLine(thisspeeed+" sad "+dousoku+" asf "+espeeed );
                   // Console.WriteLine((gone) / thisweight + " -> "+(gone) / thisweight * (float)Math.Cos(rad)+" as:ew "+ (gone) / thisweight * (float)Math.Sin(rad));
                   // Console.WriteLine(-(gone) / eweight + " ->"+-(gone) / eweight * (float)Math.Cos(rad) + " as:66 " + -(gone) / eweight * (float)Math.Sin(rad));

                    tothis.X += (gone)/thisweight * (float)Math.Cos(rad);
                    tothis.Y += (gone) / thisweight * (float)Math.Sin(rad);

                    toe.X += -(gone) / eweight * (float)Math.Cos(rad);
                    toe.Y += -(gone) / eweight * (float)Math.Sin(rad);
                }

            }

         // Console.WriteLine(thiis.bif.vx + " " + thiis.bif.vy + "  :sokubef: " + e.bif.vx + " " + e.bif.vy);
            thiis.bif.vx += tothis.X;
            thiis.bif.vy += tothis.Y;
            e.bif.vx += toe.X;
            e.bif.vy += toe.Y;
         // Console.WriteLine(thiis.bif.vx + " " + thiis.bif.vy + "  :sokuaft: " + e.bif.vx + " " + e.bif.vy);

            thiis.EEV.hansya(this, e);
            e.EEV.hansya(this, thiis);

            return true;
        }
        #region group
        List<Entity> groups=new List<Entity>();

        /// <summary>
        /// 構成しているグループごと移動させる
        /// </summary>
        /// <param name="thiis">ごめん</param>
        /// <param name="x">移動距離</param>
        /// <param name="y"></param>
        protected void groupidou(Entity thiis,float x,float y)
        {
            /*{
                float sumx = 0;
                float sumy = 0;
                foreach (var a in groups)
                {
                    sumx += a.Acore.gettx() - thiis.Acore.gettx();
                    sumy += a.Acore.getty() - thiis.Acore.getty();
                }
                Console.WriteLine(sumx + " soutaibef " + sumy);
            }*/
            idou(thiis,x,y);
            
            for (int i = 0; i < groups.Count; i++)
            {
                //var tyomu = new FXY(1, );
                //var kaku = thiis.Acore.nasukaku(groups[i].Acore);
                var kaku = thiis.Acore.getnearestline(groups[i].Acore.getCenter()).hosen;
                var sa=1-(float)Math.Abs(Shape.radseiki(Math.Atan2(y, x) - kaku)
                    /(Math.PI/2));
                if (sa < 0) sa = 0;
                groups[i].bif.idou(groups[i],x*sa,y*sa);
            }
            /*{
                float sumx = 0;
                float sumy = 0;
                foreach (var a in groups)
                {
                    sumx += a.Acore.gettx() - thiis.Acore.gettx();
                    sumy += a.Acore.getty() - thiis.Acore.getty();
                }
                Console.WriteLine(sumx + " soutaiaft " + sumy);
            }*/
            // Console.WriteLine(x+":group "+groups.Count+" idoued: "+y);
        }
        /// <summary>
        /// グループ合計の重さを取得する
        /// </summary>
        /// <returns></returns>
        protected float groupwei
        {
            get
            {
                float sum = wei;
                for (int i = 0; i < groups.Count; i++)
                {
                    sum += groups[i].bif.wei;
                }
                return sum;
            }
        }
        /// <summary>
        /// グループをリセットする
        /// </summary>
        public void groupclear()
        {
            groups.Clear();
        }
        /// <summary>
        /// グループを追加する。両方にグループが追加されるから安心！
        /// </summary>
        /// <param name="thiis">ごめん</param>
        /// <param name="e">グループする相手</param>
        protected void groupAdd(Entity thiis,Entity e)
        {
            int cou = 0;
            List<int> ittied=new List<int>();
            bool go;
            for (int i = 0; i < thiis.bif.groups.Count; i++)
            {
                go = true;
                for (int t = 0; t < e.bif.groups.Count; t++)
                {
                    if (thiis.bif.groups[i] == e.bif.groups[t])
                    {
                        go = false;
                        ittied.Add(t);
                        break;
                    }
                }
                if (go)
                {
                    e.bif.groups.Add(thiis.bif.groups[i]);
                    cou += 1;
                }
            }
            for (int i = 0; i < e.bif.groups.Count - cou; i++)
            {
                go = true;
                for (int t = 0; t < ittied.Count; t++)
                {
                    if (i == ittied[t])
                    {
                        go = false;
                        break;
                    }
                }
                if (go)
                {
                    thiis.bif.groups.Add(e.bif.groups[i]);

                }
            }
            thiis.bif.groups.Add(e);
            e.bif.groups.Add(thiis);
        
        }
        /// <summary>
        /// グループになっているか
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool isgrouped(Entity e) 
        {
            return groups.Contains(e);
        }
        #endregion
        #region TouchPoint
        List<TouchPoint> sessyokus=new List<TouchPoint>() ;

        /// <summary>
        /// 接触点を追加する
        /// </summary>
        /// <param name="where">追加する接触点の座標</param>
        /// <param name="l">接触する辺</param>
        /// <param name="from">誰の接点から接触するのか</param>
        /// <param name="e">接触相手</param>
        protected void sessyokuAdd(FXY where, lineX l, Entity from, Entity e)
        {
            //TRACE(_T("%f :sessyoku: %f\n"),where.x,where.y);
            sessyokus.Add(new TouchPoint(where, l, from, e));
        }

        /// <summary>
        /// 接触点をセットする。当然両者のAcoreは存在してるよね？
        /// </summary>
        /// <param name="thiis">当たるエンテティ</param>
        /// <param name="E">接点をセットする相手</param>
        /// <returns></returns>
        public bool setsessyoku(Entity thiis,Entity E)
        {
            if(thiis.Acore.atattenai(E.Acore)) return false;

            var thispoints = thiis.Acore.getzettaipoints();
            var Epoints = E.Acore.getzettaipoints();
            bool added = false;
            //Console.WriteLine("settyoku");
       
            for (int i = 0; i < thispoints.Count; i++)
            {
                if (E.Acore.onhani(thispoints[i].X,thispoints[i].Y, 1.00001f))
                {
           //         Console.WriteLine("FOOO");
                    added = true;
                    var line = E.Acore.getnearestline(thispoints[i]);
                    thiis.bif.sessyokuAdd(thispoints[i], line, thiis, E);
                    E.bif.sessyokuAdd(thispoints[i], line, thiis, thiis);
                }
            }
            for (int i = 0; i < Epoints.Count; i++)
            {
                if (thiis.Acore.onhani(Epoints[i].X, Epoints[i].Y,1.00001f))
                {
            //        Console.WriteLine("FOOwwwwwO");
                    added = true;
                    var line = thiis.Acore.getnearestline(Epoints[i]);
                    thiis.bif.sessyokuAdd(Epoints[i], line, E, E);
                    E.bif.sessyokuAdd(Epoints[i], line, E, thiis);
                }
            }
          // Console.WriteLine(added+" settyokuEND ");
            return added;
        }
        /// <summary>
        /// 接触点によって反射を行う
        /// </summary>
        /// <param name="thiis">このbifの所属してるやつだよ！</param>
        /// <param name="e">反射相手</param>
        public void SessyokuHansya(Entity thiis,Entity e)
        {
          //  Console.WriteLine("SESSSYOKUHANSYA");
            List<TouchPoint> vex=new List<TouchPoint>();
            for (int i = 0; i < sessyokus.Count; i++)
            {
                var eee = sessyokus[i];
                if (e == eee.e)
                {
                    vex.Add(sessyokus[i]);

                }
            }
            if (vex.Count == 0 || (e.bif.ovw && thiis.bif.ovw)) { return; }
          // Console.WriteLine("VEX: "+vex.Count);
            if (vex.Count == 1)
            {
                this.hansya(thiis,e, vex[0].line);
                //e->hansya(this, vex[0].line.getrad());
            }
            else if (vex.Count > 1)
            {
                double rad =0;
                for (int i = 0; i < vex.Count; i++) 
                {
                    double temp = 0;
                    float tempkyo = -1;
                    for (int t = 0; t < vex.Count; t++)
                    { 
                       
                        FXY soy = (vex[i].xy - vex[t].xy);
                        if (soy.length > tempkyo)
                        {
                            temp = Shape.radkatamuki(soy.rad) ;
                            tempkyo = soy.length;
                        }
                    }
                  //  Console.WriteLine(temp * 180 / Math.PI + " tou");
                    rad += temp / vex.Count;
                }
                this.hansya(thiis, e, rad);
            }

        }
        /// <summary>
        /// 接触点をクリアする
        /// </summary>
        public void resetsessyokus()
        {
            sessyokus.Clear();
        }
        /*
         /// <summary>
         /// 接触点から回転軸を取得する
         /// </summary>
         /// <returns>必ず2つの点</returns>
         lineX getkaitenjiku();

         /// <summary>
         /// 接触点をもとに回転を行う
         /// </summary>
         /// <param name="cl">時間の速さ</param>
         void sessyokukaiten(float cl);*/
        /// <summary>
        /// 接触点をリセットする
        /// </summary>

        #endregion

        #region energy
        FXY energyPoint = new FXY(0, 0);
        /// <summary>
        /// atarableに付けてね
        /// 位置エネルギーの基準点をセットする
        /// </summary>
        public void setEnergyPoint(Entity thiis)
        {
            energyPoint = thiis.Acore.getCenter();
        }
        /// <summary>
        /// 位置エネルギーを保存する.
        /// atarableに付けてね
        /// </summary>
        public void energyConserv(Entity thiis)
        {
            return;
            var c = thiis.Acore.getCenter();
            float dx = -c.X + energyPoint.X;
            float dy = -c.Y + energyPoint.Y;
            float xxx = (float)Math.Sqrt(Math.Abs(thiis.bif.ax * dx * 2));
            float yyy = (float)Math.Sqrt(Math.Abs(thiis.bif.ay * dy * 2));
            if (thiis.bif.ax * dx < 0)
            {
                thiis.bif.kasoku(-xxx, 0);
            }
            else
            {
                thiis.bif.kasoku(xxx, 0);
            }
            if (thiis.bif.ay * dy < 0)
            {
                thiis.bif.kasoku(0, -yyy);
            }
            else
            {
                thiis.bif.kasoku(0, yyy);
            }
            setEnergyPoint(thiis);
        }
        #endregion
    }
}
