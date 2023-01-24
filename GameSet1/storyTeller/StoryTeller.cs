using Charamaker2.Character;
using Charamaker2.input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DevExpress.Data.Helpers.SyncHelper.ZombieContextsDetector;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using Charamaker2;
using DevExpress.Data;

namespace GameSet1.storyTeller
{
    /// <summary>
    /// ストーリーシーン
    /// </summary>
    public class StoryScene
    {
        List<character> Pcharas = new List<character>();
        List<drawings> Pdraws = new List<drawings>();
        character pcam;

        /// <summary>
        /// カメラの制御
        /// </summary>
        public character cam;
        /// <summary>
        /// 追加するキャラクターのテンプレート
        /// </summary>
        public List<character> charas = new List<character>();

        /// <summary>
        /// 追加する主に文字のテンプレート
        /// </summary>
        public List<drawings> draws = new List<drawings>();
        //public List<raintokaer> tenkos = new List<raintokaer>();
        string bgm = "";
        hyojiman hyo;
        /// <summary>
        /// 普通のコンストラクタ カメラがセットされる
        /// </summary>
        /// <param name="hyoji"></param>
        public StoryScene(hyojiman hyoji)
        {
            hyo = hyoji;

            cam = new character(hyo.camx, hyo.camy, hyo.ww, hyo.wh, 0, 0, 0, new setu("core", 0, 0
            , new picture(0, 0, 0, 1, 1, 0, 0, 0, false, 1, "def", new Dictionary<string, string> { { "def", "nothing" } })));
            /*text = new message(0, 0, 1, 1, 0, 0, 0, "", addin: false);*/
        }
        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="s"></param>
        public StoryScene(StoryScene s)
        {
            hyo = s.hyo;
            cam=s.cam.clone();
            foreach (var a in s.charas) 
            {
                charas.Add(a.clone());
            }
            foreach (var a in s.draws)
            {
                draws.Add(a.clone());
            }


            bgm = s.bgm;

        }

        /// <summary>
        /// 背景とともにシーンを作る
        /// </summary>
        /// <param name="hyoji"></param>
        /// <param name="haikei"></param>
        /// <returns></returns>
        static public StoryScene withhaikei(hyojiman hyoji, string haikei)
        {
            var res = new StoryScene(hyoji);
            res.intohaikei(haikei);
            return res;
        }

        /// <summary>
        /// シーンを始める
        /// </summary>
        public void start()
        {
            if (bgm != "") fileman.playbgm(bgm);
            Pcharas.Clear(); Pdraws.Clear();
            pcam = cam.clone();
            foreach (var a in charas)
            {
                var c = a.clone();
                c.copymotion(a, false);
                //  Console.WriteLine(c.core.p.textures[c.core.p.texname]+" :dwazoudwao: "+c.core.p.OPA);
                Pcharas.Add(c);
            }
            foreach (var a in Pcharas)
            {
                a.resethyoji(hyo);
            }
            foreach (var a in draws)
            {
                Pdraws.Add(a.clone());
            }
            foreach (var a in Pdraws)
            {
                a.add(hyo);
            }
            foreach (var a in Pcharas) a.frame(0);
        }
        /// <summary>
        /// bgmをつける
        /// </summary>
        /// <param name="bg"></param>
        public void setbgm(string bg) { bgm = bg; }
        /// <summary>
        /// フレーム処理
        /// </summary>
        /// <param name="s"></param>
        /// <param name="cl"></param>
        public void frame(Scene s, float cl = 1)
        {
            foreach (var a in Pcharas) a.frame(cl);
            pcam.frame(cl);
            hyo.camx = pcam.x;
            hyo.camy = pcam.y;
            hyo.setBairituW(pcam.w);
            //     foreach (var a in tenkos) a.frame(s, cl);
        }
        public void end()
        {
            cam.sinu(hyo);
            foreach (var a in Pcharas) a.sinu(hyo);
            foreach (var a in Pdraws) a.remove(hyo);
        }
        /// <summary>
        /// キャラクターをいい感じに召喚する
        /// </summary>
        /// <param name="name">ファイル</param>
        /// <param name="wp">ウィンドウのうちの位置</param>
        /// <param name="ground">地面の位置</param>
        /// <param name="scale">大きさ</param>
        /// <param name="mirror">ミラー</param>
        /// <returns>出したキャラクター</returns>
        public character intochara(string name, float wp = 0, float ground = 1, bool mirror = false, float scale = 0.5f)
        {
            var res = fileman.loadcharacter(name);
            float ww = res.h;
            if (ww == 0) ww = res.core.p.h;
            if (ww != 0)
            {
                res.scalechange(scale * hyo.wh / ww);
            }
            if (mirror)
            {
                res.mirror = !res.mirror;
            }
            res.setcxy(hyo.camx + hyo.ww * wp, hyo.camy + hyo.wh * ground, res.w * wp, res.h);
            charas.Add(res);
            return res;
        }
        /// <summary>
        /// セリフをいい感じに召還する
        /// </summary>
        /// <param name="who">名前ウィンドウ""だとなしになる</param>
        /// <param name="nakami">中身</param>
        /// <param name="wp">始め位置これはそのまま幅にもなりにける</param>
        /// <param name="hp">始め高さ</param>
        /// <param name="size">文字の大きさ</param>
        /// <param name="speed">表示速度</param>
        /// <returns>出したメッセージ</returns>
        public List<message> intoserif(string who, string nakami, float wp = 0.25f, float hp = 0.5f, float size = 0.03f, float speed = 3)
        {
            float si = hyo.ww * size;
            // Console.WriteLine(((1 - wp * 2) * hyo.ww / (si/2)) + " :qwsadawjofha "+si);
            var namee = message.hutidorin(si * 0.07f, hyo.camx + hyo.ww * wp, hyo.camy + hyo.wh * 0.5f
                , si, (int)((1 - wp * 2) * hyo.ww / (si / 2)), 0, 0, -1, who, 0, 0, 0, R2: 0.8f, G2: 0.8f, B2: 0.8f);
            var name = namee.First();
            name.textoraa();

            nakami += "\n   ▶";
            float widthth = (1 - wp * 2) * hyo.ww;
            
            var res = message.hutidorin(si * 0.07f, hyo.camx + hyo.ww * wp, hyo.camy + hyo.wh * hp + name.H + name.SIZE / 4
                , si, (int)(widthth / (si / 2)), 0, speed, -1, nakami, 0, 0, 0, R2: 0.8f, G2: 0.8f, B2: 0.8f);
            var text = res.First();



            if (who != "")
            {
                charas.Add(new character(hyo.camx + hyo.ww * wp - name.SIZE / 4, hyo.camy + hyo.wh * hp - name.SIZE / 4
                    , 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 10000, name.W + name.SIZE / 4, name.H + name.SIZE / 4, 0, 0, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\namebox" } }), new List<setu>())));
            }
            charas.Add(new character(hyo.camx + hyo.ww * wp - text.SIZE / 4, hyo.camy + hyo.wh * hp + name.H + name.SIZE / 4 - text.SIZE / 4
                , 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 10001
                , widthth + text.SIZE / 4, hyo.wh * (1 - hp), 0, 0, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\textbox" } }), new List<setu>())));
            foreach (var a in namee) draws.Add(a);
            foreach (var a in res) draws.Add(a);
            return res;
        }
        /// <summary>
        /// 文字が1重のセリフとかを召喚する
        /// </summary>
        /// <param name="who">名前ウィンドウ""だとなしになる</param>
        /// <param name="nakami">セリフの中身</param>
        /// <param name="wp">開始地点そのまま大きさに</param>
        /// <param name="hp">始めの高さ</param>
        /// <param name="size">文字のサイズ</param>
        /// <param name="speed">文字の表示速度</param>
        /// <returns></returns>
        public message intoserif2(string who, string nakami, float wp = 0.25f, float hp = 0.5f, float size = 0.03f, float speed = 3)
        {
            float si = hyo.ww * size;
            // Console.WriteLine(((1 - wp * 2) * hyo.ww / (si/2)) + " :qwsadawjofha "+si);
            var name = new message(hyo.camx + hyo.ww * wp, hyo.camy + hyo.wh * 0.5f, si, (int)((1 - wp * 2) * hyo.ww / (si / 2)), 0, 0, -1, who);
            name.textoraa();
            var res = new message(hyo.camx + hyo.ww * wp, hyo.camy + hyo.wh * hp + name.H + name.SIZE / 4, si, (int)((1 - wp * 2) * hyo.ww / (si / 2)), 0, speed, -1, nakami);
            var text = res;

            float widthth = (1 - wp * 2) * hyo.ww;

            if (who != "") charas.Add(new character(hyo.camx + hyo.ww * wp - name.SIZE / 4, hyo.camy + hyo.wh * hp - name.SIZE / 4, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 10000, name.W + name.SIZE / 4, name.H + name.SIZE / 4, 0, 0, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\namebox" } }), new List<setu>())));
            charas.Add(new character(hyo.camx + hyo.ww * wp - text.SIZE / 4
                , hyo.camy + hyo.wh * hp + name.H + name.SIZE / 4 - text.SIZE / 4
                , 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 10001
                , widthth + text.SIZE / 4, hyo.wh * (1 - hp), 0, 0, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\textbox" } }), new List<setu>())));
            draws.Add(name); draws.Add(text);
            return res;
        }
        /// <summary>
        /// 背景をセットする
        /// </summary>
        /// <param name="pic">背景のテクスチャ</param>
        /// <returns>背景となったキャラ</returns>
        public character intohaikei(string pic)
        {
            var ppp = new picture(-1, -1, -10000000, hyo.ww + 2, hyo.wh + 2, 1, 1, 0, false, 1, "def", new Dictionary<string, string> { { "def", pic } });

            var res = new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, ppp, new List<setu>()));
            charas.Add(res);
            return res;
        }
        /// <summary>
        /// 全てのアニメーションが終了しているか判定するとともに終わらせる
        /// </summary>
        /// <returns></returns>
        public bool owattemasuka()
        {

            bool res = true;
            foreach (var a in Pcharas)
            {
                if (!a.endmotions()) res = false;
            }
            foreach (var a in Pdraws)
            {
                var t = a.GetType();
                if (t == typeof(message) || t.IsSubclassOf(typeof(message)))
                    if (!((message)a).endmove()) res = false;
            }
            return res;

        }


        public class StoryTeller : Scene
        {
            /// <summary>
            /// キャラクターの更新を行う
            /// </summary>
            /// <param name="c"></param>
            /// <param name="refresh">表情とかをリセットするか</param>
            /// <param name="superframe">何フレームでやるか</param>
            static public void kousinchara(character c, bool refresh = false, float superframe = 99999999999999999)
            {
                c.frame(superframe);

                if (refresh)
                {
                    c.refreshtokijyun();
                }
            }
            public List<StoryScene> scenes = new List<StoryScene>();
            protected int pointer = -1;

            protected hyojiman backhyo = null;
            public StoryTeller(SceneManager s, Scene nex, hyojiman backhyojiman = null) : base(s)
            {
                backhyo = backhyojiman;
                next = nex;
            }
            protected character effectn(float x, float y, float w, float h, string tex, StoryScene ss)
            {
                var ccc = new character(hyo.ww * x, hyo.wh * y, w, h, w / 2, h / 2, 0, new setu("core", 0, 0, new picture(0, 0, 0, w, h, w / 2, h / 2, 0, false, 1, "def", new Dictionary<string, string> { { "def", tex } }), new List<setu>()));
                ss.charas.Add(ccc);

                return ccc;
            }
            protected override void onstart()
            {
                base.onstart();

                pointer = 0;
                scenes[pointer].start();
            }
            public void ssetup()
            {
                if (pointer != 0)
                {
                    pointer = 0;
                    scenes[pointer].start();
                }
            }
            public override void frame(inputin i, float cl)
            {
                base.frame(i, cl);
                if (backhyo != null)
                {
                    backhyo.hyoji(0, true, false, false);
                    hyo.hyoji(1, false);

                }
                else
                {
                    base.frame(i, cl);
                }
                scenes[pointer].frame(this, 1);

                if (i.ok(Keys.D, itype.up) || (i.ok(MouseButtons.Left, itype.down) && i.x >= hyo.ww * 0.75f))
                {
                    if (scenes[pointer].owattemasuka())
                    {
                        if (pointer >= 0 && pointer < scenes.Count())
                        {
                            scenes[pointer].end();
                        }

                        pointer += 1;
                        if (pointer >= 0 && pointer < scenes.Count())
                        {
                            scenes[pointer].start();
                            fileman.playoto("scroll1", 0.25f);
                        }
                        else if (pointer >= scenes.Count()) { end(); }
                    }
                }
                if (i.ok(Keys.A, itype.up) || (i.ok(MouseButtons.Left, itype.down) && i.x <= hyo.ww * 0.25f))
                {
                    if (pointer >= 0 && pointer < scenes.Count())
                    {
                        scenes[pointer].end();
                    }
                    if (pointer > 0) pointer -= 1;
                    if (pointer >= 0 && pointer < scenes.Count())
                    {
                        scenes[pointer].start();

                        fileman.playoto("scroll2", 0.25f);
                    }

                }
                if (i.ok(Keys.Escape, itype.down))
                {
                    end();
                    fileman.playoto("kettei");
                }
            }


          
            /// <summary>
            /// 角度だけ反映させたあれをキャラを作る
            /// </summary>
            /// <param name="mae"></param>
            /// <param name="ato"></param>
           public void kouzoken(character mae, character ato)
            {
                var tt = mae;
                //  Console.WriteLine("kozokun! "+scenes.Count());
                mae = new character(mae, true, false);
                mae.copymotion(tt, false);
                mae.removemoves(typeof(playotoman));
                mae.frame(100000);
                ato.copykakudo(mae);
                ato.mirror = mae.mirror;
            }
            /// <summary>
            /// キャラをコピーする
            /// </summary>
            /// <param name="mae"></param>
            /// <returns></returns>
             public character copyen(character mae,bool refresh=false)
            {
                var tt = mae;
                // Console.WriteLine("copeyn! " + scenes.Count());
                mae = new character(mae, true, false);
                mae.copymotion(tt, false);
                mae.removemoves(typeof(playotoman));
                mae.frame(100000);
                if (refresh) mae.refreshtokijyun();
                // Console.WriteLine(mae.motions.Count() + ":aweojfauicopymo:" + true);
                return mae;
            }
            /// <summary>
            /// キャラをコピーする
            /// </summary>
            /// <param name="mae"></param>
            /// <param name="sss"></param>
            /// <returns></returns>
             public character copyen(character mae, StoryScene sss,bool refresh=false)
            {
                mae = copyen(mae,refresh);
                sss.charas.Add(mae);
                // Console.WriteLine(mae.motions.Count() + ":aweojfauicopymo:" + true);
                return mae;
            }
            /// <summary>
            /// 背景を携えてシーンを作る 追加もする
            /// </summary>
            /// <param name="haikei"></param>
            /// <returns></returns>
            public StoryScene haikeitazusaen(string haikei = "nothing")
            {
                var sss = StoryScene.withhaikei(hyo, haikei);
                scenes.Add(sss);
                return sss;
            }
            /// <summary>
            /// シーンをコピーする。追加もする
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public StoryScene copyScene(StoryScene s) 
            {
                var res = new StoryScene(s);
                scenes.Add(res);
                return res;
            }
        }
    }
}
