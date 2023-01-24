using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Charamaker2.input;
using Charamaker2.Shapes;

namespace GameSet1
{
    /// <summary>
    /// キャラクターの攻撃力とかセリフとかをテキストファイルから読み込んだりするクラス
    /// 名前:
    /// で行末までパラメータになる
    /// </summary>
    public static class FP
    {
        /// <summary>
        /// テキストファイルを普通に読み込む
        /// </summary>
        /// <param name="path">読み込むパス</param>
        /// <returns>読み込んだテキスト</returns>
        static public string loadtext(string path)
        {
            var res = "";

            using (var fs = new StreamReader(@".\" + path + ".txt"))
            {
                res = fs.ReadToEnd();
            }
            res=res.Replace("\r", "");
            return res;
        }
        /// <summary>
        /// エンテティの未来の位置を予測する。空気抵抗ががばがばなの許して。
        /// </summary>
        /// <typeparam name="T">このタイプへとコピーする</typeparam>
        /// <param name="e">コピー元エンテティ</param>
        /// <param name="time">何秒後か</param>
        /// <param name="cl">時間の精度になるね</param>
        /// <returns>何秒か動いたエンテティ</returns>
        static public T entitysimulate<T>(T e, float time, float cl)
            where T : Entity
        {
            var res = (T)Activator.CreateInstance(typeof(T), e);
            while (time > 0)
            {
                res.frame(cl);
                time -= cl;
            }
            return res;
        }
        /// <summary>
        /// エンテティが飛んでいくとして、その狙った地点に当てるためにはどの角度で打ち出せばいいかを教えてくれる。。
        /// </summary>



        /// <summary>
        /// エンテティが飛んでいくとして、その狙った地点に当てるためにはどの角度で打ち出せばいいかを教えてくれる。。
        /// </summary>
        /// <typeparam name="T">このタイプへとコピーする</typeparam>
        /// <param name="e">コピー元エンテティ</param>
        /// <param name="x">目標x</param>
        /// <param name="y">目標y</param>
        /// <param name="speed">与えたい速度</param>
        /// <param name="time">何秒後までか</param>
        /// <param name="cl">精度になるね</param>
        /// <param name="kuri">何回繰り返しシミュレートするか</param>
        /// <returns>いい角度</returns>
        static public double entitysimulate<T>(T e, float x, float y, float speed, float time, float cl, int kuri = 5)
            where T : Entity
        {


            double kaku = Math.Atan2(y - e.c.getty(), x - e.c.gettx());
            float tumi = 0;

            bool st = x - e.c.gettx() > 0;


            for (int i = 0; i < kuri; i++)
            {
                float pretumi;
                {
                    var res = (T)Activator.CreateInstance(typeof(T), e);
                    res.bif.kasoku(speed * (float)Math.Cos(kaku), speed * (float)Math.Sin(kaku));


                    bool now = x - e.c.gettx() > 0;
                    float nowtime = time;

                    while (now == st && nowtime > 0)
                    {
                        res.frame(cl);

                        now = x - res.c.gettx() > 0;
                        nowtime -= cl;

                    }

                    pretumi = (y - res.c.getty());

                    tumi += pretumi;
                    kaku = Math.Atan2(y + tumi - e.c.getty(), x - e.c.gettx());
                }

                {
                    var res = (T)Activator.CreateInstance(typeof(T), e);
                    res.bif.kasoku(speed * (float)Math.Cos(kaku), speed * (float)Math.Sin(kaku));


                    bool now = x - e.c.gettx() > 0;
                    float nowtime = time;

                    while (now == st && nowtime > 0)
                    {
                        res.frame(cl);

                        now = x - res.c.gettx() > 0;
                        nowtime -= cl;

                    }
                    float nowtumi = (y - res.c.getty());
                    if (Math.Abs(pretumi) < Math.Abs(nowtumi))
                    {
                        tumi -= pretumi;
                        tumi -= pretumi;
                        kaku = Math.Atan2(y + tumi - e.c.getty(), x - e.c.gettx());
                        //  Console.WriteLine("reverse!");
                    }
                    //     Console.WriteLine(res.c.gettx()+" :xy: "+res.c.getty()+" -> "+x+" :xy: "+y);
                }
                //  Console.WriteLine((x - e.c.gettx())+" :x y: " + (y + tumi - e.c.getty()) + " sal "+tumi + " kaku-> " + (kaku / Math.PI * 180+" simulating!"));
            }

            //  Console.WriteLine((x - e.c.gettx()) + " :x y: " + (y + tumi - e.c.getty()) + " sal " + tumi + " kaku-> " + (kaku / Math.PI * 180 + " SSSSSSS"));

            return kaku;
        }
        /// <summary>
        /// エンテティを一方向に動かして地形でずらすメソッド
        /// </summary>
        /// <param name="e">対象</param>
        /// <param name="EM">地形を入れたマネージャー</param>
        /// <param name="s">どの形でやるか(クローンされるから安心！)</param>
        /// <param name="zure1x">Xずらす</param>
        /// <param name="zure1y">Yずらす</param>
        /// <param name="tais">ずらす形のもと(""でキャラクターそのもの)</param>
        /// <param name="mode">0でxy両方、1でxのみ、-1でyのみ変化</param>
        static public void zuresaseEntity(Entity e,EntityManager EM,Shape s,float zure1x,float zure1y,string tais="",int mode=0) 
        {
            var tt = e.c.GetSetu(tais);
            float x = e.c.x, y = e.c.y;
            if (tais == "") tt = null;
            Shape ssss = s.clone(),psss=s.clone();
            if (tt == null)
            {
                psss.setto(e.c);
            }
            else 
            {
                psss.setto(tt.p);
            }

            e.c.settxy(e.c.gettx() +zure1x , e.c.getty() + zure1y);
            if (tt == null)
            {
                ssss.setto(e.c);
            }
            else 
            {
                ssss.setto(tt.p);
            }
            var lis = EM.overweights;
            Waza.atypefilter(lis, e.bif);
            e.zurentekiyou(lis, ssss, psss);
            if (mode == 1)
            {
                e.c.y = y;
            }
            else if(mode==-1)
            {
                e.c.x = x;
            }
        }

        /// <summary>
        /// 特殊なシーケンスを使うときとかだけ参照してね。(\nは標準で変換されるよ)
        /// </summary>
        static public Dictionary<string, string> texts = new Dictionary<string, string>();

        /// <summary>
        /// 特殊なシーケンスを使うときとかだけ参照してね。(\nは標準で変換されるよ)
        /// </summary>
        static public Dictionary<string, float> textsparam = new Dictionary<string, float>();
        /// <summary>
        /// あんま参照しないでね。
        /// </summary>
        static public Dictionary<string, float> param = new Dictionary<string, float>();
        /// <summary>
        /// パラメータを取得する
        /// </summary>
        /// <param name="name">パラメータの名前</param>
        /// <param name="i">パラメータのi番目(name+i)されるだけ</param>
        /// <returns>floatで帰ってくる無い場合は死ぬ</returns>
        static public float PR(string name,int i=0)
        {
            name += i.ToString();
            try
            {
                return param[name];
            }
            catch (Exception eee)
            {
                Console.WriteLine(name + " ne-yo");
                throw eee;
            }
        }
        /// <summary>
         /// パラメータがあるか確認する
         /// </summary>
         /// <param name="name">パラメータの名前</param>
         /// <param name="i">パラメータのi番目(name+i)されるだけ</param>
         /// <returns>/returns>
        static public bool PRExists(string name, int i = 0)
        {
            return param.ContainsKey(name + i.ToString());
        }
        /// <summary>
        /// テキストの方に記述されたパラメータを取得する
        /// </summary>
        /// <param name="name">パラメータの名前</param>
        /// <param name="i">パラメータのi番目(name+i)されるだけ</param>
        /// <returns>floatで帰ってくる無い場合は死ぬ</returns>
        static public float TPR(string name, int i = 0)
        {
            name += i.ToString();
            try
            {
                return textsparam[name];
            }
            catch (Exception eee)
            {
                Console.WriteLine(name + " ne-yo");
                throw eee;
            }
        }
        /// <summary>
        /// テキストの方に記述されたパラメータがあるか確認する
        /// </summary>
        /// <param name="name">パラメータの名前</param>
        /// <param name="i">パラメータのi番目(name+i)されるだけ</param>
        /// <returns></returns>
        static public bool TPRExists(string name, int i = 0)
        {
            return textsparam.ContainsKey(name + i.ToString());
        }
        /// <summary>
        /// テキストを取得する
        /// </summary>
        /// <param name="name">テキストの名前</param>

        /// <param name="ipcs">入力の変換([Key:Q],[Mus:Left]をへんかん)</param>
        /// /// <param name="percents">数値の変換([print:%f]をへんかん)</param>
        /// <returns>stringで帰ってくる無い場合は変な文字列</returns>
        static public string GT(string name, List<IPC> ipcs = null,params float[] percents)
        {
            if (texts.ContainsKey(name))
            {
                var res = texts[name];
                if (ipcs != null) res=IPC.convertstringinput(ipcs, res);

                foreach (var a in percents)
                {
                   var idx = res.IndexOf("[print:%f]");
                
                    if (idx >= 0)
                    {
                        res = res.Remove(idx, 10);
                        res = res.Insert(idx, a.ToString());
                    }
                    else 
                    {
                        res += "print:%fがおかしいってママが言ってた!";
                    }
                }

                return res;
            }
            else
            {
                return "テキストがないよって昨日ママに言われたんだ……";
            }
        }
        /// <summary>
        /// テキストがnullかどうかを判定する
        /// </summary>
        /// <param name="text">そのテキスト</param>
        /// <returns>text=="テキストがないよって昨日ママに言われたんだ……"</returns>
        static public bool nulltext(string text)
        {
            return text.Equals("テキストがないよって昨日ママに言われたんだ……");
        }
        /// <summary>
        /// ファイルを読み込む。[名前:パラメータとか　改行]のフォーマットで頼む。エラーで変なことになるからちうい！
        /// </summary>
        /// <param name="paramn">パラメータのファイル群(floatになる) health:10.0　ってな感じで</param>
        /// <param name="textsn">テキストのファイル群  serif1:やあ\n兄の仇！ ってな感じで</param>
        static public void seting(List<string> paramn, List<string> textsn)
        {
            try
            {

                texts.Clear();
                textsparam.Clear();
                param.Clear();
                foreach (var a in paramn)
                {
                    using (var fs = new StreamReader(@".\" + a + ".txt"))
                    {
                        string load;
                        string region = "";
                        while ((load = fs.ReadLine()) != null)
                        {

                            load = load.Replace(" ", "");
                            var lis = load.Split(':');
                            if (lis.Length == 2)
                            {
                                if (lis[0] == "#region")
                                {
                                    if (lis.Length == 1) region = "";
                                    else region = lis[1];
                                }
                                else
                                {


                                    var ps = lis[1].Split(',');
                                    Console.WriteLine(region + lis[0]+" ::desuyanparam");
                                    param.Add(region+lis[0], Convert.ToSingle(ps[0]));
                                    for (int i = 0; i < ps.Length; i++)
                                    {
                                        param.Add(region+lis[0] + i.ToString(), Convert.ToSingle(ps[i]));
                                        //  Console.WriteLine(lis[0] + "　　vcvxzxbbzx   " + lis[1]);
                                    }
                                }
                            }

                        }
                    }
                }
                foreach (var a in textsn)
                {
                    using (var fs = new StreamReader(@".\" + a + ".txt"))
                    {
                        string load;
                        string region = "";
                        while ((load = fs.ReadLine()) != null)
                        {
                            var lis = load.Split(':');
                            if (lis[0] == "#region")
                            {
                                if (lis.Length == 1) region = "";
                                else region = lis[1];
                            }
                            else
                            {

                                
                                load = load.Replace(@"\n", "\n");
                                for (int ii = 0; ii < load.Length; ii++)
                                {
                                    if (load[ii] == ':')
                                    {
                                        if (load.Substring(0, ii)[0] == '#')
                                        {
                                            var ps = lis[1].Split(',');
                                            Console.WriteLine(region + lis[0]+"::desuyanSTRINGparam");
                                            for (int i = 0; i < ps.Length; i++)
                                            {
                                                textsparam.Add(region + lis[0].Substring(1) + i.ToString(), Convert.ToSingle(ps[i]));
                                                //  Console.WriteLine(lis[0] + "　　vcvxzxbbzx   " + lis[1]);
                                            }
                                        }
                                        else
                                        {
                                            if (!texts.ContainsKey(region+load.Substring(0, ii)))
                                            {
                                                int idx;
                                                var tx = load.Substring(ii + 1);
                                                while ((idx = tx.IndexOf("[param:")) >= 0)
                                                {
                                                    var texx = tx.Substring(idx);
                                                    var end = tx.IndexOf("]");
                                                    if (end == -1) end = tx.Length - 1;

                                                    texx = tx.Substring(idx, end - idx + 1);

                                                    //Console.WriteLine(6 + " akgijaoij " + (end - idx-1) + " asf" + texx.Length);
                                                    var paramname = texx.Substring(7, end - idx - 6 - 1);
                                                    if (param.ContainsKey(paramname))
                                                    {
                                                        tx = tx.Replace(texx, param[paramname].ToString());
                                                    }
                                                    else
                                                    {
                                                        tx = tx.Replace(texx, "?" + paramname + "?");
                                                    }

                                                }
                                                Console.WriteLine(region + load.Substring(0, ii)+"::desuyoString");
                                                texts.Add(region + load.Substring(0, ii), tx);
                                            }
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                    }
                }
                foreach (var a in texts) 
                {
                   // Console.WriteLine(a.Key+":"+a.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("vcvxzxbbzx " + e.ToString());
                throw e;
            }
        }
        /// <summary>
        /// クッソ適当に作った乱数のリセット
        /// </summary>
        /// <param name="seed">シード</param>
        static public void resetrandomn(uint seed)
        {
            randomn = seed;
            randomn2 = seed + 1;


        }
        static uint randomn = 0, randomn2 = 0;
        /// <summary>
        /// クッソ適当に作った乱数の取得。全然正規分布じゃないし意味はあまりない。
        /// </summary>
        /// <returns>filemanのとは違ってシード値がリセットの奴だけ</returns>
        static public uint originalrandom()
        {
            randomn += randomn2 % 10 + 1;

            randomn2 += randomn % 10 + 1;

            var res = randomn + randomn2;

            res %= 10000;

            //   Console.WriteLine(res + " rand");

            return res;

        }
        /// <summary>
        /// 三角波的コサイン
        /// </summary>
        /// <param name="sita">角度-1~1</param>
        /// <returns>コサイン</returns>
        static public float Scos(float sita) 
        {
           var s= new sankakuha();
            s.Time = sita;
            return s.C;
        }
        /// <summary>
        /// 三角波的サイン
        /// </summary>
        /// <param name="sita">角度-1~1</param>
        /// <returns>サイン</returns>
        static public float Ssin(float sita)
        {
            var s = new sankakuha();
            s.Time = sita;
            return s.S;
        }
    }
    /// <summary>
    /// 三角波を回せるクラス。
    /// 角度は-1~1
    /// </summary>
    public class sankakuha
    {
        float time = 0;
        /// <summary>
        /// 時間、横軸
        /// </summary>
        public float Time
        {
            get { return time; }
            set
            {
                time = value;
                while (time < -1) time += 2;
                while (time > 1) time -= 2;
            }
        }
        /// <summary>
        /// コサインバージョン
        /// </summary>
        public float C
        {
            get

            {
                if (time > 0)
                {
                    return 1 - time * 2;
                }
                else
                {

                    return 1 + time * 2;
                }
            }
        }
        /// <summary>
        /// サインバージョン
        /// </summary>
        public float S
        {
            get

            {
                if (-0.5f < time || time < 0.5f)
                {
                    return 1 + time * 2;
                }
                else
                {

                    return 1 - time * 2;
                }
            }
        }
    }
}