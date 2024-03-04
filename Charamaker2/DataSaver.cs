using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charamaker2
{
    /// <summary>
    /// [name]{naiyou}をロードするクラス。
    /// [name]{naiyou2}
    /// って感じで二つ並んでたら基本上しか読み取れない。
    /// 元々セーブデータ用だしね。
    /// [name]の方は別に保存されてない
    /// []がディレクトリで{}が中に書いてあるファイルorディレクトリのイメージ。
    /// </summary>
    public class DataSaver
    {
        string Data = "";

        /// <summary>
        /// 最後の所に改行を追加する
        /// </summary>
        public void linechange(int cou=1) 
        {
            Data += new string('\n',cou);
        }

        /// <summary>
        /// 全体をインデントする
        /// </summary>
        public void indent(int num=1)
        {
            var s = Data.Split('\n');
            string res = "";
            for (int i = 0; i < s.Length; i++)
            {
                res += new string(' ', num) + s[i]+"\n";
            }
            Data=res;
        }

        /// <summary>
        /// 改行->消す。
        /// \n->改行。
        /// に変換する
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string escapen(string s)
        {
            s = s.Replace("\n", "");
            s = s.Replace("\r", "");
            s = s.Replace(@"\n", "\n");

            return s;
        }


        /// <summary>
        /// データをエスケープする。データを読み取る前に一回行ってね
        /// 改行->消す。
        /// \n->改行。
        /// に変換する
        /// </summary>
        /// <param name="s"></param>
        /// <returns>そのまま自分を返す。便利なだけ</returns>
        public DataSaver escaped()
        {
            Data = escapen(Data);
            return this;
        }

        /// <summary>
        /// 書いてある内容をもらう。
        /// </summary>
        /// <returns></returns>
        public string getData()
        {
            return Data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="escape">読み込んだ奴をエスケープするか</param>
        /// <param name="ext">拡張子</param>
        /// <returns></returns>
        static public DataSaver loadFromPath(string path ,bool escape=true,string ext=".txt")
        {
            path = path + ext;
            DataSaver res;
            try
            {
                using (var reader = new StreamReader(path))
                {
                    res=new DataSaver(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                res=new DataSaver("ふぁいるないよないよ");
                Console.WriteLine(path+" sippai"+e.ToString());
            }
            if (escape) 
            {
                res.escaped();
            }
            return res;

        } /// <summary>
          /// 
          /// </summary>
          /// <param name="path"></param>
          /// <param name="ext">拡張子</param>
          /// <returns></returns>
        public void saveToPath(string path,string ext=".txt")
        {
            path = path + ext;
            try
            {

                using (var writer = new StreamWriter(path))
                {
                    writer.Write(Data);
                }
                Console.WriteLine(path + " Savekanryou!");

            }
            catch (Exception e)
            {
                Console.WriteLine(path + " savesippai " + e.ToString());
            }
        }
        public DataSaver(string data = "")
        {
            Data = data;
        }
        /// <summary>
        /// 今のデータに新しいパックを追加する
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="kaigyo"></param>
        public void packAdd(string name, string data, bool kaigyo = false)
        {
            string k = "";
            if (kaigyo) k = "\n";
            Data += "[" + name + "]" + "{"+k + data + "}"+k;
        }

        /// <summary>
        /// こっちのバージョンもある
        /// </summary>
        /// <param name="name"></param>
        /// <param name="d"></param>
        /// <param name="kaigyo"></param>
        public void packAdd(string name, DataSaver d, bool kaigyo = true)
        {
            packAdd(name, d.Data,kaigyo);
        }
        /// <summary>
        /// こっちのバージョンもある
        /// </summary>
        /// <param name="name"></param>
        /// <param name="d"></param>
        /// <param name="kaigyo"></param>
        public void packAdd(string name, bool d, bool kaigyo = false)
        {
            packAdd(name, d.ToString(), kaigyo);
        }
        /// <summary>
        /// こっちのバージョンもある
        /// </summary>
        /// <param name="name"></param>
        /// <param name="d"></param>
        /// <param name="kaigyo"></param>
        public void packAdd<T>(string name, T d, bool kaigyo = false)
            where T :IFormattable
        {
            packAdd(name, d.ToString(), kaigyo);
        }
       
        /// <summary>
        /// データの中にパックがある前提ね。文字にアンパックする
        /// </summary>
        /// <param name="name">対象</param>
        /// <param name="nothing">もし中身がなかった時に返す値</param>
        /// <returns></returns>
        public string unPackDataS(string name, string nothing = "")
        {
            name = "[" + name + "]";
            int hiraki = 0;
            bool findname = false;
            int start = -1;
            for (int idx = 0; idx < Data.Length; idx++)
            {
                if (hiraki == 0)
                {
                    if (start == -1 && !findname)
                    {
                        if (idx + name.Length < Data.Length)
                        {
                            if (Data.Substring(idx, name.Length) == name)
                            {
                                findname = true;
                            }
                        }
                    }
                    else
                    {

                    }
                }
                if (Data[idx] == '{')
                {
                    hiraki += 1;
                    if (findname)
                    {
                        findname = false;
                        start = idx + 1;
                    }
                }
                if (Data[idx] == '}')
                {
                    hiraki -= 1;
                    hiraki = Math.Max(0, hiraki);
                    if (start != -1 && hiraki == 0)
                    {
                        var dd = idx - start;
                        if (dd == 0)
                        {
                            return nothing;
                        }
                        /*if (escape)エスケープをやってた時の名残
                        {
                            return escapen(Data.Substring(start, dd));
                        }
                        else*/
                        {
                            return Data.Substring(start, dd);
                        }
                        start = -1;
                    }
                }
            }
            //   Console.WriteLine("nakattayo " + name);
            return nothing;
        }
        /// <summary>
        /// Datasaverにアンパックする
        /// 最初にロードするときだけescape=trueにする。
        /// </summary>
        /// <param name="name">対象</param>
        /// <param name="escape">\nとかを変換するか</param>
        /// <returns></returns>
        public DataSaver unPackDataD(string name)
        {
            var sou = unPackDataS(name);
            //    if (sou == "") return null;

            return new DataSaver(sou);
        }
        /// <summary>
        /// floatでアンパックする
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Nan">もし中身がなかった時の返す値</param>
        /// <param name="escape">\nとかを変換するかどうか</param>
        /// <returns></returns>
        public float unPackDataF(string name, float Nan = float.NaN)
        {
            var sou = unPackDataS(name);
            float res = 0;
            if (sou != null && float.TryParse(sou, out res)) return res;
            return Nan;
        }
        /// <summary>
        /// boolでアンパックする
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Nan">もし中身がなかった時の返す値</param>
        /// <param name="escape">\nとかを変換するかどうか</param>
        /// <returns></returns>
        public bool unPackDataB(string name, bool Nan = false)
        {
            var sou = unPackDataS(name);
            bool res;
            if (sou != null && bool.TryParse(sou, out res)) return res;
            return Nan;
        }
        /// <summary>
        /// 全てのデータをアンパックする。
        /// 初めてアンパックするときだけescape=trueに
        /// </summary>
        /// <param name="escape">\nとかを変換</param>
        /// <returns></returns>
        public List<string> unpackAlldataS()
        {
            var res = new List<string>();
            foreach (var a in getAllPacks())
            {
                res.Add(unPackDataS(a));
            }
            return res;
        }
        /// <summary>
        /// 全てのデータをアンパックする。
        /// 初めてアンパックするときだけescape=trueに
        /// </summary>
        /// <param name="escape">\nとかを変換</param>
        /// <returns></returns>
        public List<DataSaver> unpackAlldataD()
        {
            var res = new List<DataSaver>();
            foreach (var a in getAllPacks())
            {
                res.Add(unPackDataD(a));
            }
            return res;
        }
        /// <summary>
        /// 全てのパックを取得する
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> getAllPacks()
        {
            var res = new List<string>();
            int hiraki = 0;
            int start = -1;
            for (int idx = 0; idx < Data.Length; idx++)
            {
                if (hiraki == 0)
                {
                    if (start == -1)
                    {
                        if (Data[idx] == '[')
                        {
                            start = idx + 1;
                        }
                    }
                    else
                    {
                        if (Data[idx] == ']')
                        {
                            var dd = idx - start;
                            if (dd == 0)
                            {
                                res.Add("");
                            }
                            else
                            {
                                res.Add(Data.Substring(start, dd));

                            }
                        }
                    }
                }
                if (Data[idx] == '{')
                {
                    hiraki += 1;
                    start = -1;
                }
                if (Data[idx] == '}')
                {
                    hiraki -= 1;
                    start = -1;
                }
            }
            return res;
        }
        public void reset()
        {
            Data = "";
        }
        /// <summary>
        /// データを特定の文字で区切って変換する
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<string> splitData(char c = ':')
        {
            var sp = Data.Split(c);
            var res = new List<string>();
            foreach (var a in sp)
            {
                res.Add(a);
            }
            return res;
        }

        /// <summary>
        /// データを特定の文字で区切って変換する
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<float> splitData2(char c = ':')
        {
            var sp = Data.Split(c);
            var res = new List<float>();
            foreach (var a in sp)
            {
                res.Add(float.Parse(a));
            }
            return res;
        }
        /// <summary>
        /// データを特定の文字で区切ったのち、idx番目を変換する
        /// </summary>
        /// <param name="c"></param>
        /// <param name="def">idx番目が存在しなかった時の戻り値</param>
        /// <returns></returns>
        public string splitOneData(int idx, string def = "", char c = ':')
        {
            var sp = Data.Split(c);
            if (idx >= sp.Length)
            {
                return def;
            }
            return sp[idx];
        }
        /// <summary>
        /// データを特定の文字で区切ったのち、idx番目を変換する
        /// </summary>
        /// <param name="c"></param>
        /// <param name="def">idx番目が存在しなかった時の戻り値</param>
        /// <returns></returns>
        public float splitOneData2(int idx, float def = float.NaN, char c = ':')
        {
            var sp = Data.Split(c);
            float res = def;
            if (idx >= sp.Length)
            {
                return res;
            }
            if (!float.TryParse(sp[idx], out res)) { res = def; }

            return res;
        }

        /// <summary>
        /// データのすべての構造を取得する。多分呼び出すのは一番上でなのでescape=trueがいいな。
        /// </summary>
        /// <param name="escape">\nとかを変換するか</param>
        /// <param name="indent">0でいいよ</param>
        /// <returns></returns>
        public string getAllkouzou(int indent=0) 
        {
            var st = "";
            var lis = getAllPacks();
            foreach (var a in lis) 
            {;
                var b=unPackDataD(a);
                st += new string(' ', indent) + a+"\n";
                st +=new string(' ',indent)+b.getAllkouzou(indent+1) + "\n";

            }

            return st;
        }

    }
}
