using System;
using System.Collections.Generic;
using System.Text;
using Charamaker2;
using Charamaker2.input;

namespace GameSet1
{
    /// <summary>
    /// シーンの変更のためのバッファー的な奴
    /// </summary>
    public class SceneManager
    {
        /// <summary>
        /// ここに好きなシーンが入る！そしてs.frame()
        /// </summary>
        public Scene s = null;

    }


    /// <summary>
    ///背景をランダム生成するためのピース 
    /// </summary>
    public class haikeiseiser
    {
        /// <summary>
        /// テクスチャーの名前
        /// </summary>
        public string name;
        /// <summary>
        ///基本のサイズ 
        /// </summary>
        public float size;
        /// <summary>
        /// サイズのブレ
        /// </summary>
        public float sizebure;
        /// <summary>
        /// スクロール割合x
        /// </summary>
        public float scrollwarix;
        /// <summary>
        /// スクロール割合y
        /// </summary>
        public float scrollwariy;
        /// <summary>
        /// スクロール割合xのブレ
        /// </summary>
        public float scrollwarixbure;
        /// <summary>
        /// スクロール割合yのブレ
        /// </summary>
        public float scrollwariybure;
        /// <summary>
        /// 角度
        /// </summary>
        public double rad;
        /// <summary>
        /// 角度のブレ
        /// </summary>
        public double radbure;
        /// <summary>
        /// 不透明度
        /// </summary>
        public float opa;
        /// <summary>
        /// 不透明度のブレ
        /// </summary>
        public float opabure;
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="texnm">テクスチャーの名前</param>
        /// <param name="si">基本の大きさ</param>
        /// <param name="bure">大きさのブレ(+-)</param>
        /// <param name="scx">スクロール割合x</param>
        /// <param name="scy">スクロール割合y</param>
        /// <param name="scxbure">スクロール割合xのブレ</param>
        /// <param name="scybure">スクロール割合yのブレ</param>
        /// <param name="rad">角度</param>
        /// <param name="radbure">角度のブレ</param>
        /// <param name="opa">不透明度</param>
        /// <param name="opabure">不透明度のブレ</param>
        public haikeiseiser(string texnm, float si, float bure, float scx, float scy, double rad = 0, float opa = 1, float scxbure = 0, float scybure = 0, double radbure = 0, float opabure = 0)
        {
            name = texnm;
            size = si;
            sizebure = bure;
            scrollwarix = scx;
            scrollwariy = scy;
            scrollwarixbure = scxbure;
            scrollwariybure = scybure;
            this.rad = rad;
            this.radbure = radbure;
            this.opa = opa;
            this.opabure = opabure;
        }

        /// <summary>
        /// 背景を生成するいい感じに
        /// </summary>
        /// <param name="yline">生成するyの中心地</param>
        /// <param name="ybure">生成するyのブレ(+-)</param>
        /// <param name="sx">生成するxの始点</param>
        /// <param name="ex">生成するxの終点</param>
        /// <param name="ikutu">一度にいくつ画像を生成するか</param>
        /// <param name="kaisuu">何度画像生成を行うか</param>
        /// <param name="haikeis">生成する背景のリスト</param>
        /// <param name="assyuku">背景のスクロール割合に応じて生成位置を圧縮するか(太陽とか以外はやったほうがいい)</param>
        static public List<haikeidraws> haikeisei(float yline, float ybure, float sx, float ex, float ikutu, float kaisuu, List<haikeiseiser> haikeis, bool assyuku = true)
        {
            var res = new List<haikeidraws>();
            float dx = (ex - sx) / ikutu;
            int cou = 0;
            if (haikeis.Count > 0 && dx > 0)
            {

                for (int yuyu = 0; yuyu < kaisuu; yuyu++)
                {
                    float tx = 0;
                    for (int i = 0; i < ikutu; i++)
                    {
                        tx += fileman.r.Next() % (dx + (dx * i - tx));
                        int ttt = fileman.r.Next() % haikeis.Count;
                        float si = haikeis[ttt].size;
                        if (haikeis[ttt].sizebure > 0) si += (float)fileman.r.NextDouble() * (haikeis[ttt].sizebure * 2) - haikeis[ttt].sizebure;
                        // Console.WriteLine(si + " sizedayodayotiahsk.,smala");
                        bool mir = fileman.percentin(50);

                        float ty = yline;
                        if (ybure > 0) ty += fileman.r.Next() % (ybure * 2 + 1) - ybure;
                        float pppxpxpx = (sx + tx);
                        if (assyuku)
                        {
                            pppxpxpx *= haikeis[ttt].scrollwarix;
                            ty *= haikeis[ttt].scrollwariy;
                        }
                        float opa = haikeis[ttt].opa + (float)fileman.r.NextDouble() * (haikeis[ttt].opabure * 2) - haikeis[ttt].opabure;
                        double kaku = haikeis[ttt].rad + fileman.r.NextDouble() * (haikeis[ttt].radbure * 2) - haikeis[ttt].radbure;

                        float scx = haikeis[ttt].scrollwarix + (float)fileman.r.NextDouble() * (haikeis[ttt].scrollwarixbure * 2) - haikeis[ttt].scrollwarixbure;
                        float scy = haikeis[ttt].scrollwariy + (float)fileman.r.NextDouble() * (haikeis[ttt].scrollwariybure * 2) - haikeis[ttt].scrollwariybure;

                        var p = new picture(pppxpxpx, ty - si, -100000 + cou, si, si, 0, si, kaku, mir, opa, haikeis[ttt].name, new Dictionary<string, string> { { haikeis[ttt].name, haikeis[ttt].name } });
                        res.Add(new haikeidraws(scx, scy, p));
                        //    Console.WriteLine(pppxpxpx+" :HAIKEISEISEI: "+(ty-si));
                        cou++;
                    }
                }
            }
            return res;
        }
    }
    /// <summary>
    /// scene.onframeで主に使う
    /// </summary>
    public class sceneframepack
    {
        /// <summary>
        /// インプット
        /// </summary>
        public inputin i;
        /// <summary>
        /// フレーム時間
        /// </summary>
        public float cl;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="cl"></param>
        public sceneframepack(inputin i, float cl) 
        {
            this.i = i;
            this.cl = cl;
        }
    }

    /// <summary>
    /// シーンだお
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// onstartの際に呼び出される
        /// </summary>
        public event EventHandler<SceneManager> onStarts;
        /// <summary>
        /// onendの際に呼び出される
        /// </summary>
        public event EventHandler<SceneManager> onEnds;
        /// <summary>
        /// frameの際に呼び出される
        /// </summary>
        public event EventHandler<sceneframepack> onframe;

        /// <summary>
        /// SceneManager
        /// </summary>
        public SceneManager sm;
        /// <summary>
        /// 表示マン
        /// </summary>
        public readonly hyojiman hyo;
        /// <summary>
        /// 次のシーン
        /// </summary>
        public Scene next;
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="s">シーンマネージャ</param>
        /// <param name="next">次のシーン</param>
        public Scene(SceneManager s,Scene next=null) 
        {
            this.next = next;
            sm = s;
            hyo = fileman.makehyojiman();
        }
        private bool _started=false;
        /// <summary>
        /// start(),end()が複数回発動しないようにするフラグ。startでtrueになる。
        /// 直接いじってもいいけどstart,endで変えてくれるんだけど
        /// </summary>
        protected bool started { get { return _started; } set { _started = value; } }
        /// <summary>
        /// シーンを開始したいときに発動してね。
        /// smにこれが代入されてnextがない時は何かしらを代入しておくといい
        /// </summary>
        virtual public void start() 
        {
            sm.s = this;
            if (!started)
            {
                onstart();
                //if (next == null) next = this;
                _started = true;
            }
        }
        /// <summary>
        /// シーンの正しきスタート時に呼び出される。標準では何もしない
        /// </summary>
        virtual protected void onstart() 
        {
            onStarts?.Invoke(this,sm);
        }

        /// <summary>
        /// 画面の描画。標準は画面表示とonframeだけよ
        /// </summary>
        /// <param name="i">入力情報</param>
        /// <param name="cl">クロック時間</param>
        virtual public void frame(inputin i,float cl)
        {
            hyo.hyoji(cl);
            onframe?.Invoke(this,new sceneframepack(i,cl));
        }
        /// <summary>
        /// 標準はnextをスタートしてstartedをfalseにするだけ
        /// </summary>
        virtual public void end()
        {
            if (started)
            {
                onend();
                next.start();
                _started = false;
            }
        }

        /// <summary>
        /// シーンの正しきエンド時に呼び出される。標準では何もしない
        /// </summary>
        virtual protected void onend()
        {

            onEnds?.Invoke(this, sm);
        }

 
    }
}
