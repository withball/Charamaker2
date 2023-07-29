using Charamaker2.Character;
using Charamaker2.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameSet1
{
    /// <summary>
    /// エンテティに追加していろいろ動作させるクラス
    /// </summary>
    public class Waza
    {
        #region operator
        /// <summary>
        /// 元の技が時間で終わったときにaddとframeを発動させる。
        /// </summary>
        /// <param name="moto"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        static public Waza operator +(Waza moto, Waza add) 
        {
            moto.ended += (s,t)=> { add.add(moto.e);add.frame(t.cl); };
            return moto;
        }
        /// <summary>
        /// 元の技が時間で終わったときにEntityをAddする。
        /// </summary>
        /// <param name="moto"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        static public Waza operator +(Waza moto, Entity add)
        {
            moto.ended += (s, t) => { add.add(moto.EM); };
            return moto;
        }
        /// <summary>
        /// 元の技が時間で終わったときに発動する。
        /// </summary>
        /// <param name="moto"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        static public Waza operator +(Waza moto,Func<object> add)
        {
            moto.ended += (s, t) => { add.Invoke(); };
            return moto;
        }
        #endregion

        /// <summary>
        /// Wazaがaddされたときのイベント
        /// </summary>
        public event EventHandler<EEventArgs> added;
        /// <summary>
        /// Wazaがremoveされたときのイベント
        /// </summary>
        public event EventHandler<EEventArgs> removed;
        /// <summary>
        /// Wazaが時間で死んだときのイベント
        /// </summary>
        public event EventHandler<EEventArgs> ended;

        /// <summary>
        /// Wazaがframeされたときのイベント
        /// </summary>
        public event EventHandler<EEventArgs> framed;

        /// <summary>
        /// 便利ショトカeのEM
        /// </summary>
        public EntityManager EM { get { return e.EM; } }

        /// <summary>
        /// 便利ショトカeのEMのhyoji
        /// </summary>
        public Charamaker2.hyojiman hyoji { get { return e.EM.hyoji; } }
        /// <summary>
        /// 現在時間
        /// </summary>
        public float timer=0;
        /// <summary>
        /// 技の残り時間
        /// </summary>
        public float nokori { get { return end - timer; } }

        /// <summary>
        /// 終了時間
        /// </summary>
        public float end;
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="end">終了時間</param>
        public Waza(float end) 
        {
            if (end < 0)
            {
                this.end = 9999999999999;
            }
            else
            {
                this.end = end;
            }
        }
        private Entity _e=null;
        /// <summary>
        /// この技が追加されてるエンテティ
        /// </summary>
        public Entity e { get { return _e; } }
        /// <summary>
        /// 技をエンテティに追加する。複数のに追加しないでね
        /// 追加できたらOnaddを呼び出す。
        /// </summary>
        /// <param name="e">追加するエンテティ</param>
        /// <returns>追加されたかどうか</returns>
        virtual public bool add(Entity e)
        {
            if(e.addWaza(this))
            {
                this._e = e;
                
                onAdd();
                return true;
            }
            return false;

        }
        /// <summary>
        /// addしてframeする
        /// </summary>
        /// <param name="e"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool addAndFrame(Entity e, float time) 
        {
            if (add(e)) 
            {
                frame(time);
                return true;
            }
            return false; 
        }

        /// <summary>
        /// 技をエンテティから削除する。
        /// 削除できたらonRemoveを呼び出す
        /// </summary>
        /// <returns></returns>
        virtual public bool remove()
        {
            if (e!=null&&e.removeWaza(this))
            {

                onRemove();
                return true;
            }
            return false;

        }
        /// <summary>
        /// 残りの消費できる時間を算出する
        /// </summary>
        /// <param name="cl">このぐらい消費したいです！</param>
        /// <returns>消費できる時間</returns>
       public float nokoritime(float cl) 
        {
            return nokoritime(cl, end);
        }
        /// <summary>
        /// 残りの消費できる時間を算出する
        /// </summary>
        /// <param name="cl">このぐらい消費したいです！</param>
        /// <param name="endin">区切りの時間</param>
        /// <returns>消費できる時間</returns>
        public float nokoritime(float cl,float endin)
        {
            var t = endin - timer;
            if (t < 0) t = 0;
            if (t < cl) return t;
            return cl;
        }
        /// <summary>
        /// フレーム処理。
        /// Onframeを呼び出した後にタイマーが進められる
        /// </summary>
        /// <param name="cl">クロック時間</param>
        virtual public void frame(float cl) 
        {
            float cll = nokoritime(cl);
            onFrame(cll);

            timer += cl;
            if (timer >= end)
            {
                ended?.Invoke(this, new EEventArgs(e,null,cl));
                remove();
            }
        }
        /// <summary>
        /// フレーム処理の時に呼び出されるメソッド
        /// オーバーライドしてね
        /// </summary>
        /// <param name="cl">クロック時間</param>
        virtual protected void onFrame(float cl) 
        {
            framed?.Invoke(this,new EEventArgs(e, null, cl));
           // Console.WriteLine(e.c.getty() + "@poppqqqopo@ ");
            if (!atarisconnected)
            {
                List<Entity> a;
                foreach (var b in ataris)
                {
                    a = new List<Entity>(b.Value.Keys);

                    for (int i = a.Count - 1; i >= 0; i--)
                    {


                        b.Value[a[i]] -= cl;
                        if (b.Value[a[i]] <= 0) b.Value.Remove(a[i]);

                    }
                }
            }
        }
        
        /// <summary>
        /// 技がエンテティに追加されたときに発動するメソッド
        /// オーバーライドしてね。
        /// </summary>
        virtual protected void onAdd()
        {
            added?.Invoke(this,new EEventArgs(e));
        }
        /// <summary>
        /// 技がエンテティから削除されたときに発動するメソッド
        /// オーバーライドしてね。
        /// ここで自分を新しく追加するとかやったら無限ループするからやめてね。
        /// </summary>
        virtual protected void onRemove()
        {
            removed?.Invoke(this, new EEventArgs(e));
        }
        /// <summary>
        /// ListEntityに対してあたり判定でフィルターを掛ける
        /// </summary>
        /// <param name="lis">フィルター対象</param>
        /// <param name="lisataris">対象のあたり判定の節の名前群,nullでそれぞれ全部</param>
        /// <param name="e">フィルターに使うエンテティ</param>
        /// <param name="eataris">フィルターに使う奴の節の名前群,nullで全部</param>
        /// <param name="pre">1フレーム前のも考慮するか</param>
        /// <param name="not">当たっていないやつを残すことにする</param>
        public static void atafilter<T>(List<T> lis, List<string> lisataris, Entity e, List<string> eataris, bool pre = true, bool not = false)
        where T:Entity
        {

            var chunks = bunkatu(lis, 3);
            lis.Clear();
            var tasks = new List<Task>();
            foreach (var a in chunks)
            {
                tasks.Add(new Task(() =>
                {
                    var ss = atafilter_0<T>(a, lisataris, e, eataris, pre, not);
                    foreach (var b in ss) lis.Add(b);
                }));


            }
            foreach (var a in tasks)
            {
                a.Start();
            }

            foreach (var a in tasks)
            {
                a.Wait();
            }
            

        }

        static List<T> atafilter_0<T>(List<T> lis, List<string> lisataris, Entity e, List<string> eataris, bool pre = true, bool not = false)
         where T : Entity
        {
            bool rem = false;
            List<Shape> SE;
            if (eataris == null)
            {
                SE = e.ab.getallatari();
            }
            else
            {
                SE = e.ab.getatari(eataris);
            }
            List<Shape> PSE;
            List<Shape> LS, LPS;
            if (pre)
            {
                if (eataris == null)
                {
                    PSE = e.pab.getallatari();
                }
                else
                {
                    PSE = e.pab.getatari(eataris);
                }
                for (int i = SE.Count - 1; i >= 0; i--)
                {
                    if (SE[i] == null || PSE[i] == null)
                    {
                        SE.RemoveAt(i);
                        PSE.RemoveAt(i);
                    }
                }
            }
            else
            {
                for (int i = SE.Count - 1; i >= 0; i--)
                {
                    if (SE[i] == null)
                    {
                        SE.RemoveAt(i);
                    }
                }
                PSE = SE;
            }

            for (int i = lis.Count - 1; i >= 0; i--)
            {
                rem = true;
                if (lisataris == null)
                {
                    LS = lis[i].ab.getallatari();
                }
                else
                {
                    LS = lis[i].ab.getatari(lisataris);
                }
                if (pre)
                {
                    if (lisataris == null)
                    {
                        LPS = lis[i].pab.getallatari();
                    }
                    else
                    {
                        LPS = lis[i].pab.getatari(lisataris);
                    }

                    for (int j = LS.Count - 1; j >= 0; j--)
                    {
                        if (LS[j] == null || LPS[j] == null)
                        {
                            LS.RemoveAt(j);
                            LPS.RemoveAt(j);
                        }
                    }
                }
                else
                {
                    for (int j = LS.Count - 1; j >= 0; j--)
                    {
                        if (LS[j] == null)
                        {
                            LS.RemoveAt(j);
                        }
                    }
                    LPS = LS;
                }

                for (int j = 0; j < LS.Count && rem; j++)
                {
                    for (int t = 0; t < SE.Count && rem; t++)
                    {
                        if ((LS[j].atarun2(LPS[j], SE[t], PSE[t])))
                        {
                            rem = false;
                            break;
                        }
                    }
                }
                if (not ^ rem)
                {
                    lis.RemoveAt(i);
                }




            }      //お守り。無くしたらnullになるっておかしくなる
            foreach (var a in lis) ;
            return lis;
        }
        /// <summary>
        /// listを任意の数に分割する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lis"></param>
        /// <param name="cou"></param>
        /// <returns></returns>
        public static List<List<T>> bunkatu<T>(List<T> lis, int cou) 
        {
            List<List<T>> llis = new List<List<T>>();
            for (int i = 0; i < cou; i++) 
            {
                llis.Add(new List<T>());
            }
            for (int i=0;i<lis.Count;i++) 
            {
                llis[i % cou].Add(lis[i]);
            }
            return llis;
        }
        /// <summary>
        /// ListEntityに対してあたり判定でフィルターを掛ける
        /// </summary>
        /// <param name="lis">フィルター対象</param>
        /// <param name="lisataris">対象のあたり判定の節の名前群,nullでそれぞれ全部</param>
        /// <param name="s">フィルターに使う図形</param>
        /// <param name="pres">フィルターに使う図形の一フレーム前</param>
        /// <param name="pre">1フレーム前のも考慮するか</param>
        /// <param name="not">当たっていないやつを残すことにする</param>
        public static void atafilter<T>(List<T> lis, List<string> lisataris, Shape s, Shape pres, bool pre = true, bool not = false)
        where T : Entity
        {
            var chunks = bunkatu(lis, 3);
            lis.Clear();
            var tasks = new List<Task>();
            foreach (var a in chunks)
            {
                tasks.Add(new Task(() =>
                {
                    var ss = atafilter_0<T>(a, lisataris, s, pres, pre, not);
                    foreach (var b in ss) lis.Add(b);
                }));


            }
            foreach (var a in tasks)
            {
                a.Start();
            }

            foreach (var a in tasks)
            {
                a.Wait();
            }
        }
        static List<T> atafilter_0<T>(List<T> lis, List<string> lisataris, Shape s, Shape pres, bool pre = true, bool not = false)
        where T : Entity
        {
            bool rem = false;
            List<Shape> LS, LPS;


            for (int i = lis.Count - 1; i >= 0; i--)
            {
                rem = true;
                if (lisataris == null)
                {
                    LS = lis[i].ab.getallatari();
                }
                else
                {
                    LS = lis[i].ab.getatari(lisataris);
                }
                if (pre)
                {
                    if (lisataris == null)
                    {
                        LPS = lis[i].pab.getallatari();
                    }
                    else
                    {
                        LPS = lis[i].pab.getatari(lisataris);
                    }

                    for (int j = LS.Count - 1; j >= 0; j--)
                    {
                        if (LS[j] == null || LPS[j] == null)
                        {
                            LS.RemoveAt(j);
                            LPS.RemoveAt(j);
                        }
                    }
                }
                else
                {
                    for (int j = LS.Count - 1; j >= 0; j--)
                    {
                        if (LS[j] == null)
                        {
                            LS.RemoveAt(j);
                        }
                    }
                    LPS = LS;
                }

                for (int j = 0; j < LS.Count && rem; j++)
                {

                    if ((LS[j].atarun2(LPS[j], s, pres)))
                    {
                        rem = false;
                        break;
                    }

                }
                if (not ^ rem)
                {
                    lis.RemoveAt(i);
                }




            }
            return lis;
        }
        /// <summary>
        /// ListEntityを当たりタイプでフィルターを掛ける
        /// </summary>
        /// <param name="b">フィルターを掛ける物理インフォメーション(atagのみ参照)</param>
        /// <param name="lis">フィルターするリスト</param>
        /// <param name="friend">当たらないやつを残す</param>
        static public void atypefilter<T>(List<T> lis, buturiinfo b, bool friend = false)
            where T:Entity
        {

            for (int i = lis.Count - 1; i >= 0; i--)
            {
                if (lis[i].bif.different(b) == friend)
                {
                    lis.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// atarisをもとにフィルターを掛ける
        /// </summary>
        /// <param name="lis">フィルターをかける奴</param>
        /// <param name="num">フィルターを掛けるatarisのナンバー</param>
        /// <param name="exist">atarisにある場合リストに残すモード</param>
        public void atarisfilter<T>(List<T> lis, int num = 0, bool exist = false)
                  where T : Entity
        {
            for (int i = lis.Count - 1; i >= 0; i--)
            {
                if (atarisAru(lis[i], num) != exist)
                {
                    lis.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// リスト二つでフィルターを掛ける。両方被りがないことを想定。
        /// </summary>
        /// <param name="lis">フィルター対象</param>
        /// <param name="filter">フィルター</param>
        /// <param name="aru">フィルターにある奴を残す</param>
        static public void listfilter(List<Entity> lis, List<Entity> filter, bool aru = true)
        {
            if (aru)
            {
                for (int i = lis.Count - 1; i >= 0; i--)
                {
                    if (filter.Contains(lis[i]))
                    {
                        filter.Remove(lis[i]);
                    }
                    else
                    {
                        lis.RemoveAt(i);
                    }

                }
            }
            else
            {
                for (int i = lis.Count - 1; i >= 0; i--)
                {
                    if (filter.Contains(lis[i]))
                    {
                        filter.Remove(lis[i]);
                        lis.RemoveAt(i);
                    }
                    else
                    {

                    }

                }
            }
        }

        /// <summary>
        /// このatarisを他の技のatarisとする。この技でatarisのframeが二度とおこなわれなくなるのでちうい
        /// </summary>
        /// <param name="w">共有元</param>
        public void atarisconnectto(Waza w)
        {
            ataris = w.ataris;
            atarisconnected = true;
        }
        bool atarisconnected = false;
        /// <summary>
        /// 技を適用した奴を保存しておくリスト。
        /// タイマーにも使える
        /// </summary>
        protected Dictionary<int, Dictionary<Entity, float>> ataris = new Dictionary<int, Dictionary<Entity, float>>();
        /// <summary>
        /// atarisに追加する
        /// </summary>
        /// <param name="e">追加するエンテティ</param>
        /// <param name="time">なにクロック時間残るか,0より小さければ999999999999999</param>
        /// <param name="i">atarisナンバー</param>
        public void atarisAdd(Entity e, float time, int i = 0)
        {
            if (time < 0) 
            {
                time = 999999999999999;
            }
            if (!ataris.ContainsKey(i))
            {
                ataris.Add(i, new Dictionary<Entity, float>());
                ataris[i].Add(e, time);
            }
            else
            {
                if (!ataris[i].ContainsKey(e))
                {
                    ataris[i].Add(e, time);
                }
                else if (ataris[i][e] < time)
                {
                    ataris[i][e] = time;
                }
            }
        }
        /// <summary>
        /// atarisに追加する
        /// </summary>
        /// <param name="e">追加するエンテティ</param>
        /// <param name="time">なにクロック時間残るか</param>
        /// <param name="i">atarisナンバー</param>
        public void atarisAddRange(List<Entity> e, float time, int i = 0)
        {
            if (!ataris.ContainsKey(i))
            {
                ataris.Add(i, new Dictionary<Entity, float>());
                
                e.ForEach((a)=>ataris[i].Add(a, time));
            }
            else
            {
                foreach (var a in e)
                {
                    if (!ataris[i].ContainsKey(a))
                    {
                        ataris[i].Add(a, time);
                    }
                    else if (ataris[i][a] < time)
                    {
                        ataris[i][a] = time;
                    }
                }
            }
        }
        /// <summary>
        /// atarisから消す
        /// </summary>
        /// <param name="e">消すエンテティ</param>
        /// <param name="i">atarisナンバー</param>
        public void atarisRemove(Entity e, int i = 0)
        {
            if (ataris.ContainsKey(i))
            {
                ataris[i].Remove(e);
            }
        }
        /// <summary>
        /// atarisをクリアする
        /// </summary>
        /// <param name="i">atarisナンバー</param>
        public void atarisClear(int i = 0)
        {
            if (ataris.ContainsKey(i))
            {
                ataris[i].Clear();
            }
        }
        /// <summary>
        /// atarisを完全に消す
        /// </summary>
        public void atarisClearAll()
        {
            foreach (var a in ataris)
            {
                a.Value.Clear();
            }
        }
        /// <summary>
        /// atarisに存在するか調べる
        /// </summary>
        /// <param name="e">調べるエンテティ</param>
        /// <param name="i">atarisナンバー</param>
        /// <returns>あったのか？</returns>
        public bool atarisAru(Entity e, int i = 0)
        {
            if (ataris.ContainsKey(i))
            {
                return ataris[i].ContainsKey(e);
            }
            return false;
        }
        /// <summary>
        /// atarisを持ってくる
        /// </summary>
        /// <param name="i">atarisナンバー</param>
        /// <returns>りすと！</returns>
        public List<Entity> atarislist(int i = 0)
        {
            if (ataris.ContainsKey(i))
            {
                return new List<Entity>(ataris[i].Keys);
            }
            return new List<Entity>();
        }
        /// <summary>
        /// atarisの全てのリストを結合して持ってくる
        /// </summary>
        /// <returns>Entityが重複してるかもしれないリスト</returns>
        public List<Entity> atarislistall()
        {
            var res = new List<Entity>();
            foreach (var a in ataris)
            {
                res.AddRange(new List<Entity>(a.Value.Keys));
            }
            return res;
        }
        /// <summary>
        /// Entityがどこかしらのatarisにあるか調べる
        /// </summary>
        /// <param name="e">Entity</param>
        /// <returns>ナンバー関係なくあるか</returns>
        public bool atarisDorearu(Entity e)
        {
            foreach (var a in ataris)
            {
                if (a.Value.ContainsKey(e)) return true;
            }
            return false;
        }
        /// <summary>
        /// timerが区切り時間以上かを調べる
        /// </summary>
        /// <param name="kugiriko">区切り時間</param>
        /// <returns>区切り時間が-1かtimerが区切り時間未満だとfalse</returns>
        public bool kugirin(float kugiriko)
        {
            return timer >= kugiriko && kugiriko >= 0;
        }
      


    }
    /// <summary>
    ///  addしたEntityを数秒後死に至らしめる劇毒
    /// </summary>
    public class jisatukun : Waza 
    {
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="end"></param>
        public jisatukun(float end) : base(end) 
        {
        
        }
        protected override void onRemove()
        {
            e.remove();
            base.onRemove();
        }

    }
    /// <summary>
    ///  addしたEntityの速度と角度を合わせる奴
    /// </summary>
    public class speedkakusoroe : Waza
    {
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="end"></param>
        /// <param name="basekaku">ベースの角度(°)</param>
        public speedkakusoroe(float basekaku,float end) : base(end)
        {
            this.basekaku = basekaku;
        }
        float basekaku;

        protected override void onFrame(float cl)
        {
            base.onFrame(cl);
            e.c.addmotion(new radtoman(cl,"", basekaku + (Math.Atan2(e.bif.vy, e.bif.vx))/Math.PI*180,360));
        }

    }
}
