using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Charamaker2.input;

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
        /// エンテティの未来の位置を予測する。空気抵抗ががばがばなの許して。
        /// </summary>
        /// <typeparam name="T">このタイプへとコピーする</typeparam>
        /// <param name="e">コピー元エンテティ</param>
        /// <param name="time">何秒後か</param>
        /// <param name="cl">時間の精度になるね</param>
        /// <returns>何秒か動いたエンテティ</returns>
        static public T entitysimulate<T>(T e,float time,float cl)
            where T:Entity
        {
            var res = (T)Activator.CreateInstance(typeof(T),e);
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
        static public double entitysimulate<T>(T e,float x,float y,float speed, float time, float cl,int kuri=5)
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
        /// 特殊なシーケンスを使うときとかだけ参照してね。(\nは標準で変換されるよ)
        /// </summary>
        static public Dictionary<string, string> texts = new Dictionary<string, string>();
        /// <summary>
        /// あんま参照しないでね。
        /// </summary>
        static public Dictionary<string, float>  param= new Dictionary<string, float>();
      /// <summary>
      /// パラメータを取得する
      /// </summary>
      /// <param name="name">パラメータの名前</param>
      /// <returns>floatで帰ってくる無い場合は死ぬ</returns>
        static public float PR(string name)
        {
            try
            {
                return param[name];
            }
            catch (Exception eee)
            {
                Console.WriteLine(name+" ne-yo");
                throw eee;
            }
        }
        /// <summary>
        /// テキストを取得する
        /// </summary>
        /// <param name="name">テキストの名前</param>
        /// <param name="ipcs">入力の変換(nullでしない)</param>
        /// <returns>stringで帰ってくる無い場合は変な文字列</returns>
        static public string GT(string name,List<IPC>ipcs=null)
        {
            if (texts.ContainsKey(name))
            {
                if (ipcs != null) return IPC.convertstringinput(ipcs, texts[name]);
                return texts[name];
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
        static public void seting(List<string>paramn,List<string>textsn) 
        {
            try
            {
          
                texts.Clear();
               
                param.Clear();
                foreach (var a in paramn)
                {
                    using (var fs = new StreamReader(@".\" + a + ".txt"))
                    {
                        string load;
                        while ((load = fs.ReadLine()) != null)
                        {

                            load = load.Replace(" ", "");
                            var lis = load.Split(':');
                            if (lis.Length == 2)
                            {

                                param.Add(lis[0], Convert.ToSingle(lis[1]));
                                //  Console.WriteLine(lis[0] + "　　vcvxzxbbzx   " + lis[1]);

                            }

                        }
                    }
                }
                foreach (var a in textsn)
                {
                    using (var fs = new StreamReader(@".\"+a+".txt"))
                    {
                        string load;
                        while ((load = fs.ReadLine()) != null)
                        {
                            // Console.WriteLine(load);
                            load = load.Replace(@"\n", "\n");
                            for (int ii = 0; ii < load.Length; ii++)
                            {
                                if (load[ii] == ':')
                                {
                                    if (!texts.ContainsKey(load.Substring(0, ii)))
                                        texts.Add(load.Substring(0, ii), load.Substring(ii + 1));
                                    break;
                                }
                            }

                        }
                    }
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
            randomn2 = seed+1; 
            
        
        }
        static uint randomn = 0,randomn2=0;
        /// <summary>
        /// クッソ適当に作った乱数の取得。全然正規分布じゃないし意味はあまりない。
        /// </summary>
        /// <returns>filemanのとは違ってシード値がリセットの奴だけ</returns>
        static public uint originalrandom()
        {
            randomn += randomn2 % 10+1;

            randomn2 += randomn % 10+1;

            var res = randomn + randomn2;

            res %= 10000;

         //   Console.WriteLine(res + " rand");

            return res;

        }
    }
}
