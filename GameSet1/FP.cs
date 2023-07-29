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
        /// あんま参照しないでね。
        /// </summary>
        static public Dictionary<string, Dictionary<string,string>> matrix 
            = new Dictionary<string, Dictionary<string,string>>();

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

        const string nullText = "テキストがないよって昨日ママに言われたんだ……";
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
                return nullText;
            }
        }

        /// <summary>
        /// マトリックスからテキストを取得する
        /// </summary>
        /// <param name="table">テーブルの名前</param>
        /// <param name="name">テキストの名前</param>
        /// <returns>stringで帰ってくる無い場合は変な文字列</returns>
        static public string GMT(string table,string name)
        {
            try
            {
                return matrix[table][name];
            }
            catch (Exception e)
            {
                Console.WriteLine(table + "{}{}{}" + name + e.ToString());
                throw;
            }
         
        }
        /// <summary>
        /// マトリックスからパラメータを取得する
        /// </summary>
        /// <param name="table">テーブルの名前</param>
        /// <param name="name">テキストの名前</param>
        /// <returns>stringで帰ってくる無い場合は変な文字列</returns>
        static public float GMP(string table, string name)
        {
            try
            {
                return float.Parse(matrix[table][name]);
            }
            catch (global::System.Exception e)
            {
                Console.WriteLine(table+"{}{}{}"+name+e.ToString());
                throw;
            }
        }
        /// <summary>
        /// テーブルがあるか確認する
        /// </summary>
        /// <param name="name">テーブルの名前</param>
        /// <returns>/returns>
        static public bool TableExists(string table)
        {
            return matrix.ContainsKey(table);
        }
        /// <summary>
        /// テーブルに値があるか確認する
        /// </summary>
        /// <param name="name">パラメータの名前</param>
        /// <param name="i">パラメータのi番目(name+i)されるだけ</param>
        /// <returns>/returns>
        static public bool MatrixExists(string table,string name)
        {
            return TableExists(table) &&matrix[table].ContainsKey(name);
        }

        /// <summary>
        /// テキストがnullかどうかを判定する
        /// </summary>
        /// <param name="text">そのテキスト</param>
        /// <returns>text=="テキストがないよって昨日ママに言われたんだ……"</returns>
        static public bool nulltext(string text)
        {
            return text.Equals(nullText);
        }
        /// <summary>
        /// ファイルを読み込む。[名前:パラメータとか　改行]のフォーマットで頼む。エラーで変なことになるからちうい！
        /// もちろん全部リセットされるよ
        /// </summary>
        /// <param name="paramn">パラメータのファイル群(floatになる) health:10.0　ってな感じで</param>
        /// <param name="textsn">テキストのファイル群  serif1:やあ\n兄の仇！ ってな感じで</param>
        /// <param name="scvs">csvのファイル群  左上に:nameでリージョン設定</param>

        static public void seting(List<string> paramn=null, List<string> textsn=null
            ,List<string> scvs=null)
        {
            try
            {

                texts.Clear();
                textsparam.Clear();
                param.Clear();
                matrix.Clear();
                if (paramn != null)
                    foreach (var a in paramn)
                    {
                        loadparam(a);
                    }
                if (textsn != null)
                    foreach (var a in textsn)
                    {
                        loadtexts(a);
                    }

                if (scvs != null)
                    foreach (var a in scvs)
                    {
                        loadcsv(a);
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine("vcvxzxbbzx " + e.ToString());
                throw e;
            }
        }
        static private void loadparam(string file) 
        {
            using (var fs = new StreamReader(@".\" + file + ".txt"))
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
                            Console.WriteLine(region + lis[0] + " ::desuyanparam");
                            param.Add(region + lis[0], Convert.ToSingle(ps[0]));
                            for (int i = 0; i < ps.Length; i++)
                            {
                                param.Add(region + lis[0] + i.ToString(), Convert.ToSingle(ps[i]));
                                //  Console.WriteLine(lis[0] + "　　vcvxzxbbzx   " + lis[1]);
                            }
                        }
                    }

                }
            }
        }
        static private void loadtexts(string file) 
        {
            using (var fs = new StreamReader(@".\" + file + ".txt"))
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
                                    Console.WriteLine(region + lis[0] + "::desuyanSTRINGparam");
                                    for (int i = 0; i < ps.Length; i++)
                                    {
                                        textsparam.Add(region + lis[0].Substring(1) + i.ToString(), Convert.ToSingle(ps[i]));
                                        //  Console.WriteLine(lis[0] + "　　vcvxzxbbzx   " + lis[1]);
                                    }
                                }
                                else
                                {
                                    if (!texts.ContainsKey(region + load.Substring(0, ii)))
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
                                        Console.WriteLine(region + load.Substring(0, ii) + "::desuyoString");
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

        static private void loadcsv(string file)
        {
            using (var fs = new StreamReader(@".\" + file + ".csv"))
            {
                string load;
                List<string> names = new List<string>();
                while ((load = fs.ReadLine()) != null)
                {

                    load = load.Replace(" ", "");
                    var lis = load.Split(',');
                    if (lis[0].Length>0 &&lis[0][0] == ':')
                    {
                        names.Clear();
                        names.Add(lis[0].Substring(1));
                        for (int i = 1; i < lis.Length; i++)
                        {
                            names.Add(lis[i]);
                        }
                    }
                    else if (names.Count > 1 && lis[0]!="")
                    {
                        var dic = new Dictionary<string, string>();
                        var tname = names[0] + lis[0];
                        matrix.Add(tname,dic);
                        Console.Write(tname + ":");
                        for (int i=1;i<names.Count;i++) 
                        {
                            var a = names[i];
                            if (a != "")
                            {
                                Console.Write("{" + a + "," + lis[i] + "}");

                                dic.Add(a, lis[i]);
                            }
                        }
                        Console.WriteLine("");
                        
                    }
                }
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