using Charamaker2.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameSet1
{
    /// <summary>
    /// エンテティに追加していろいろ動作させるクラス
    /// </summary>
    public class Waza
    {
        /// <summary>
        /// 便利ショトカeのEM
        /// </summary>
        protected EntityManager EM { get { return e.EM; } }

        /// <summary>
        /// 便利ショトカeのEMのhyoji
        /// </summary>
        protected Charamaker2.hyojiman hyoji { get { return e.EM.hyoji; } }
        /// <summary>
        /// 現在時間
        /// </summary>
        protected float timer=0;
        /// <summary>
        /// 終了時間
        /// </summary>
        protected float end;
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="end">終了時間</param>
        public Waza(float end) 
        {
            this.end = end;
        }
        private Entity _e=null;
        /// <summary>
        /// この技が追加されてるエンテティ
        /// </summary>
        protected Entity e { get { return _e; } }
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
        protected float nokoritime(float cl) 
        {
            var t = end - timer;
            if (t < 0) t = 0;
            if (t< cl) return t;
            return cl;
        }
        /// <summary>
        /// フレーム処理。
        /// Onframeを呼び出した後にタイマーが進められる
        /// </summary>
        /// <param name="cl">クロック時間</param>
        virtual public void frame(float cl) 
        {
           
            onFrame(nokoritime(cl));
            timer += cl;
            if (timer >= end) remove();
        }
        /// <summary>
        /// フレーム処理の時に呼び出されるメソッド
        /// オーバーライドしてね
        /// </summary>
        /// <param name="cl">クロック時間</param>
        virtual protected void onFrame(float cl) 
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
        
        /// <summary>
        /// 技がエンテティに追加されたときに発動するメソッド
        /// オーバーライドしてね。
        /// </summary>
        virtual protected void onAdd()
        {

        }
        /// <summary>
        /// 技がエンテティから削除されたときに発動するメソッド
        /// オーバーライドしてね。
        /// ここで自分を新しく追加するとかやったら無限ループするからやめてね。
        /// </summary>
        virtual protected void onRemove()
        {

        }
        /// <summary>
        /// ListEntityに対してあたり判定でフィルターを掛ける
        /// </summary>
        /// <param name="lis">フィルター対象</param>
        /// <param name="lisataris">対象のあたり判定の節の名前群</param>
        /// <param name="e">フィルターに使うエンテティ</param>
        /// <param name="eataris">フィルターに使う奴の節の名前群</param>
        /// <param name="pre">1フレーム前のも考慮するか</param>
        /// <param name="not">当たっていないやつを残すことにする</param>
        public static void atafilter(List<Entity> lis, List<string> lisataris, Entity e, List<string> eataris, bool pre = true, bool not = false)
        {
            bool rem = false;
            List<Shape> SE = e.ab.getatari(eataris);
            List<Shape> PSE;
            List<Shape> LS, LPS;
            if (pre)
            {
                PSE = e.pab.getatari(eataris);
                for (int i = SE.Count - 1; i >= 0; i++)
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
                for (int i = SE.Count - 1; i >= 0; i++)
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
                LS = lis[i].ab.getatari(lisataris);
                if (pre)
                {
                    LPS = lis[i].pab.getatari(eataris);
                    for (int j = LS.Count - 1; j >= 0; j++)
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
                    for (int j = LS.Count - 1; j >= 0; j++)
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
                        }
                    }
                }
                if (not ^ rem)
                {
                    lis.RemoveAt(i);
                }




            }
        }
        /// <summary>
        /// ListEntityを当たりタイプでフィルターを掛ける
        /// </summary>
        /// <param name="b">フィルターを掛ける物理インフォメーション</param>
        /// <param name="lis">フィルターするリスト</param>
        /// <param name="friend">当たらないやつを残す</param>
        static protected void atypefilter(List<Entity> lis, buturiinfo b, bool friend = false)
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
        protected void atarisfilter(List<Entity> lis, int num = 0, bool exist = false)
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
        /// atarisを他の技と共有
        /// </summary>
        /// <param name="w">共有元</param>
        public void atarisconnectto(Waza w)
        {
            ataris = w.ataris;
        }
        /// <summary>
        /// 技を適用した奴を保存しておくリスト。
        /// タイマーにも使える
        /// </summary>
        protected Dictionary<int, Dictionary<Entity, float>> ataris = new Dictionary<int, Dictionary<Entity, float>>();
        /// <summary>
        /// atarisに追加する
        /// </summary>
        /// <param name="e">追加するエンテティ</param>
        /// <param name="time">なにクロック時間残るか</param>
        /// <param name="i">atarisナンバー</param>
        protected void atarisAdd(Entity e, float time, int i = 0)
        {
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
        /// atarisから消す
        /// </summary>
        /// <param name="e">消すエンテティ</param>
        /// <param name="i">atarisナンバー</param>
        protected void atarisRemove(Entity e, int i = 0)
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
        protected void atarisClear(int i = 0)
        {
            if (ataris.ContainsKey(i))
            {
                ataris[i].Clear();
            }
        }
        /// <summary>
        /// atarisを完全に消す
        /// </summary>
        protected void atarisClearAll()
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
        protected bool atarisAru(Entity e, int i = 0)
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
        protected List<Entity> atarislist(int i = 0)
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
        protected List<Entity> atarislistall()
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
        protected bool atarisDorearu(Entity e)
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
        protected bool kugirin(float kugiriko)
        {
            return timer >= kugiriko && kugiriko >= 0;
        }
      


    }
}
