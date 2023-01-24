using Charamaker2.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/// <summary>
/// キーボード、マウス入力を扱う名前空間
/// </summary>
namespace Charamaker2.input
{
    /// <summary>
    /// インプットのタイプ,押したとき・離されたとき・前述二個OR押されているときの三種類
    /// </summary>
    public enum itype
    {
        /// <summary>
        /// 押されたとき
        /// </summary>
        down,

        /// <summary>
        /// 押されたとき、離されたとき、押されている時すべて
        /// </summary>
        ing,

        /// <summary>
        /// 離されたとき
        /// </summary>
        up
    }
    /// <summary>
    /// キーボードとマウスの入力を扱うクラス。
    /// 毎フレームごとにtopre()とsetpointer()を行い、
    /// formのkeydown,keyupイベントにdown(),up()を接続すれば、
    /// 入力が完成する。
    /// </summary>
    [Serializable]
    public class inputin
    {

        /// <summary>
        /// ナマの入力を保存するためのやつ。キーコンフィグのときとかに使う
        /// </summary>
        [NonSerialized]
        static public inputin raw = new inputin();



        /// <summary>
        /// ナマの変換
        /// </summary>
        [NonSerialized]
        static public List<IPC> rawconv = new List<IPC>();

        List<Keys> k = new List<Keys>();
        List<MouseButtons> m = new List<MouseButtons>();
        List<Keys> pk = new List<Keys>();
        List<MouseButtons> pm = new List<MouseButtons>();
        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        public  inputin(){}
        /// <summary>
        /// マウスの座標
        /// </summary>
        public float x = 0, y = 0;
        /// <summary>
        /// 通信でインプットを受け取ったときなどにインプットをこれにコピーする
        /// </summary>
        /// <param name="moto">コピー元</param>
        public void tusininput(inputin moto)
        {
            k = moto.k;
            m = moto.m;
            pk = moto.pk;
            pm = moto.pm;

            x = moto.x;
            y = moto.y;
            tag = moto.tag;
        }
        /// <summary>
        /// ナマでキーを押す。
        /// </summary>
        /// <param name="i">押すキー</param>
        public void down(Keys i)
        {
            down(i, rawconv);
        }
        /// <summary>
        /// ナマでマウスのボタンを押す
        /// </summary>
        /// <param name="i">押すボタン</param>
        public void down(MouseButtons i)
        {
            down(i, rawconv);
        }
        /// <summary>
        /// ナマでキーを離す
        /// </summary>
        /// <param name="i">離すキー</param>
        public void up(Keys i)
        {
            up(i, rawconv);
        }
        /// <summary>
        /// ナマでマウスのボタンを離す
        /// </summary>
        /// <param name="i">離すボタン</param>
        public void up(MouseButtons i)
        {
            up(i, rawconv);
        }
        /// <summary>
        /// 変換器をもとにキーを押す。変換器にない入力はそのままになる。
        /// </summary>
        /// <param name="i">押すキー</param>
        /// <param name="converts">変換器</param>
        public void down(Keys i, List<IPC> converts)
        {

            foreach (var b in converts)
            {
                if (b.getin(i))
                {
                    if (b.KO != Keys.None)
                    {
                        if (!k.Contains(b.KO))
                        {
                            k.Add(b.KO);
                        }
                        return;

                    }
                    if (b.MO != MouseButtons.None)
                    {
                        if (!m.Contains(b.MO))
                        {
                            m.Add(b.MO);
                        }

                        return;

                    }

                }

            }
            if (!k.Contains(i))
            {
                k.Add(i);
            }

        }
        /// <summary>
        /// 変換器をもとにマウスのボタンを押す。変換器にない入力はそのままになる。
        /// </summary>
        /// <param name="i">押すボタン</param>
        /// <param name="converts">変換器</param>
        public void down(MouseButtons i, List<IPC> converts)
        {
            foreach (var b in converts)
            {
                if (b.getin(i))
                {
                    if (b.KO != Keys.None)
                    {
                        if (!k.Contains(b.KO))
                        {
                            k.Add(b.KO);
                        }
                        return;

                    }
                    if (b.MO != MouseButtons.None)
                    {
                        if (!m.Contains(b.MO))
                        {
                            m.Add(b.MO);
                        }

                        return;

                    }

                }

            }
            if (!m.Contains(i))
            {
                m.Add(i);
            }
        }
        /// <summary>
        /// 変換器をもとにキーを離す。変換器にない入力はそのままになる。
        /// </summary>
        /// <param name="i">離すキー</param>
        /// <param name="converts">変換器</param>
        public void up(Keys i, List<IPC> converts)
        {
            foreach (var b in converts)
            {
                if (b.getin(i))
                {
                    if (b.KO != Keys.None)
                    {
                        k.Remove(b.KO);
                        return;

                    }
                    if (b.MO != MouseButtons.None)
                    {
                        m.Remove(b.MO);

                        return;

                    }

                }

            }
            k.Remove(i);
        }
        /// <summary>
        /// 変換器をもとにマウスのボタンを離す。変換器にない入力はそのままになる。
        /// </summary>
        /// <param name="i">離すボタン</param>
        /// <param name="converts">変換器</param>
        public void up(MouseButtons i, List<IPC> converts)
        {
            foreach (var b in converts)
            {
                if (b.getin(i))
                {
                    if (b.KO != Keys.None)
                    {
                        k.Remove(b.KO);
                        return;

                    }
                    if (b.MO != MouseButtons.None)
                    {
                        m.Remove(b.MO);
                        return;

                    }

                }

            }
            m.Remove(i);
        }
        /// <summary>
        /// 入力を過去のものとする。ゲームのティックに伴って呼び出す。さもなくばitypeが機能しない
        /// </summary>
        public void topre()
        {
            pk = new List<Keys>(k);
            pm = new List<MouseButtons>(m);

        }
        /// <summary>
        /// 生の入力から座標をもらってくる。UI座標の時使え
        /// </summary>
        /// <param name="hyo"></param>
        /// <returns></returns>
        static public float rawX(hyojiman hyo) 
        {
           // Console.WriteLine(raw.x + "  a;sfkapo ");
            return raw.x * hyo.ww;
        }

        /// <summary>
        /// 生の入力から座標をもらってくる。UI座標の時使え
        /// </summary>
        /// <param name="hyo"></param>
        /// <returns></returns>
        static public float rawY(hyojiman hyo)
        {
            return raw.y * hyo.wh;
        }
        /// <summary>
        /// マウスの座標をカーソルからセットする。rawにもセットされる
        /// </summary>
        /// <param name="hyojiman">活動制限のためのhyojiman</param>
        /// <param name="f">座標変換のためのフォーム</param>
        /// /// <param name="gamennai">falseの時、画面外の指定を可能にする</param>
        public void setpointer(hyojiman hyojiman, Form f,bool gamennai=true)
        {
            var cu = f.PointToClient(Cursor.Position);
            raw.x = cu.X / (float)f.ClientRectangle.Width;
            raw.y = cu.Y / (float)f.ClientRectangle.Height;
            x = cu.X * (hyojiman.ww / f.ClientRectangle.Width);
            y = cu.Y * (hyojiman.wh / f.ClientRectangle.Height);
            if (gamennai)
            {
                if (x < 0) x = 0;
                if (x > hyojiman.ww) x = hyojiman.ww;
                if (y < 0) y = 0;
                if (y > hyojiman.wh) y = hyojiman.wh;
            }
            x += +hyojiman.camx;
            y += +hyojiman.camy;
        }
        /// <summary>
        /// マウスの座標を中心点との差分でポインタの座標からセットする。
        /// </summary>
        /// 
        /// <param name="prepoint">前回の戻り値。nullはやめてね</param>
        /// <param name="hyojiman">活動制限のためのhyojiman</param>
        /// <param name="f">座標変換のためのフォーム</param>
        /// <param name="gamennai">falseの時、画面外の指定を可能にする</param>
        /// <returns>保存しといて次代入するポイント</returns>>
        public System.Drawing.Point setlockpointer(System.Drawing.Point prepoint, hyojiman hyojiman, Form f, bool gamennai = true)
        {
            var cu = Cursor.Position;
            var pcu = new System.Drawing.Point(f.Location.X + f.Width / 2, f.Location.Y + f.Height / 2);


            x = prepoint.X + (cu.X - pcu.X) * (hyojiman.ww / f.ClientRectangle.Width);
            y = prepoint.Y + (cu.Y - pcu.Y) * (hyojiman.wh / f.ClientRectangle.Height);

            if (gamennai)
            {
                if (x < 0) x = 0;
                if (x > hyojiman.ww) x = hyojiman.ww;
                if (y < 0) y = 0;
                if (y > hyojiman.wh) y = hyojiman.wh;
            }
            prepoint = new System.Drawing.Point((int)x, (int)y);
            x += hyojiman.camx;
            y += hyojiman.camy;
            Cursor.Position = pcu;
            return prepoint;
        }
        /// <summary>
        /// 何かしらのボタンが押されてたりしてるか判定する
        /// </summary>
        /// <param name="t">押し方のタイプ</param>
        /// <returns>押されたりしているか</returns>
        public bool ok(itype t)
        {
            switch (t)
            {
                case itype.down:
                    foreach (var a in k)
                    {
                        if (!pk.Contains(a)) return true;
                    }
                    foreach (var a in m)
                    {
                        if (!pm.Contains(a)) return true;
                    }
                    return false;
                case itype.ing:
                    return k.Count > 0 || pk.Count > 0 || m.Count > 0 || pm.Count > 0;
                case itype.up:
                    foreach (var a in pk)
                    {
                        if (!k.Contains(a)) return true;
                    }
                    foreach (var a in pm)
                    {
                        if (!m.Contains(a)) return true;
                    }
                    return false;
                default:
                    return false;
            }

        }
        /// <summary>
        /// 特定のキーが押されたりしているか判定する
        /// </summary>
        /// <param name="i">そのキー</param>
        /// <param name="t">押されたり仕方のタイプ</param>
        /// <returns>押されたりしているか</returns>
        public bool ok(Keys i, itype t)
        {
            switch (t)
            {
                case itype.down:

                    return k.Contains(i) && !pk.Contains(i);

                case itype.ing:
                    return k.Contains(i);
                case itype.up:
                    return !k.Contains(i) && pk.Contains(i);
                default:
                    return false;
            }

        }
        /// <summary>
        /// 特定のマウスのボタンが押されたりしているか判定する
        /// </summary>
        /// <param name="i">そのボタン</param>
        /// <param name="t">押されたり仕方のタイプ</param>
        /// <returns>押されたりしているか</returns>
        public bool ok(MouseButtons i, itype t)
        {
            switch (t)
            {
                case itype.down:

                    return m.Contains(i) && !pm.Contains(i);

                case itype.ing:
                    return m.Contains(i);
                case itype.up:
                    return !m.Contains(i) && pm.Contains(i);
                default:
                    return false;

            }

        }
        /// <summary>
        /// 現在押されているキーを取得する
        /// </summary>
        /// <returns>キーの列のコピー</returns>
        public List<Keys> getdownkey()
        {
            return new List<Keys>(k);
        }
        /// <summary>
        /// 現在押されているマウスのボタンを取得する
        /// </summary>
        /// <returns>マウスのボタンの列のコピー</returns>
        public List<MouseButtons> getdownbutton()
        {
            return new List<MouseButtons>(m);
        }
        /// <summary>
        /// 過去押されていたキーを取得する
        /// </summary>
        /// <returns>キーの列のコピー</returns>
        public List<Keys> getupkey()
        {
            return new List<Keys>(k);
        }
        /// <summary>
        /// 過去押されていたマウスのボタンを取得する
        /// </summary>
        /// <returns>マウスのボタンの列のコピー</returns>
        public List<MouseButtons> getupbutton()
        {
            return new List<MouseButtons>(m);
        }
        /// <summary>
        /// キー列から変換されていないキーを削除し変換されているキーを取得しようとする。
        /// </summary>
        /// <param name="k">キー列</param>
        /// <param name="converts">変換器</param>
        /// <returns>変換されているキー列</returns>
        public List<Keys> getNCdownkey(List<Keys> k, List<IPC> converts)
        {
            var res = new List<Keys>(k);
            for (int i = res.Count - 1; i >= 0; i--)
            {

                foreach (var b in converts)
                {
                    if (b.getin(res[i]))
                    {
                        //Console.WriteLine(res[i] + "  rmv");
                        res.RemoveAt(i);
                        break;
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// マウスのボタン列から変換されていないボタンを削除し変換されているボタンを取得しようとする。
        /// </summary>
        /// <param name="m">ボタン列</param>
        /// <param name="converts">変換器</param>
        /// <returns>変換されているボタン列</returns>
        public List<MouseButtons> getNCdownbutton(List<MouseButtons> m, List<IPC> converts)
        {
            var res = new List<MouseButtons>(m);

            for (int i = res.Count - 1; i >= 0; i--)
            {

                foreach (var b in converts)
                {
                    if (b.getin(res[i]))
                    {

                        res.RemoveAt(i);
                        break;
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// インプットをバイト列へと変換する
        /// </summary>
        /// <param name="i">インプット</param>
        /// <returns>バイト列</returns>
        static public byte[] andbyte(inputin i)
        {
            try
            {
                if (i != null)
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        if (ms != null && binaryFormatter != null)
                        {
                            binaryFormatter.Serialize(ms, i);

                            return ms.ToArray();
                        }
                    }
                }
            }
            catch (Exception eee) { Console.WriteLine("input to byte error" + eee.ToString()); }
            return null;
        }
        /// <summary>
        /// バイト列をインプットに変換する
        /// </summary>
        /// <param name="i">バイト列</param>
        /// <returns>インプット</returns>
        static public inputin andbyte(byte[] i)
        {
            try
            {
                if (i != null)
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    using (MemoryStream ms = new MemoryStream(i))
                    {
                        if (ms != null && binaryFormatter != null)
                        {
                            var a = binaryFormatter.Deserialize(ms);
                            if (a != null) return (inputin)a;
                        }
                    }
                }
            }
            catch (Exception eee) { Console.WriteLine("tubusuze" + eee.ToString()); }
            return null;

        }
        /// <summary>
        /// 通信するときとかに情報を乗せる用
        /// </summary>
        public string tag = "";

        /// <summary>
        /// 変換する前の入力キー・ボタンをテキストとして取得しようとする
        /// </summary>
        /// <param name="k">そのキー</param>
        /// <param name="converts">変換IPC列</param>
        /// <returns>Key: OR Mouse: OR None</returns>
        static public string getConvertmaeinput(Keys k,List<IPC> converts) 
        {
            bool onin = false;
            foreach (var a in converts) 
            {
                if (k == a.KO) 
                {
                    if (a.KI != Keys.None)
                    {
                        return "Key:" + a.KI.ToString();
                    }
                    else 
                    {
                        return "Mus:" + a.MI.ToString();
                    }
                }
                if (k == a.KI) 
                {
                    onin = true;
                }  
            }
            if (onin)
            {
                return "Nul:None";
            }
            else 
            {
                return "Key:" + k.ToString();
            }
        }

        /// <summary>
        /// 変換する前の入力キー・ボタンをテキストとして取得しようとする
        /// </summary>
        /// <param name="m">そのボタン</param>
        /// <param name="converts">変換IPC列</param>
        /// <returns>Key: OR Mouse: OR None</returns>
        static public string getConvertmaeinput(MouseButtons m, List<IPC> converts)
        {
            bool onin = false;
            foreach (var a in converts)
            {
                if (m == a.MO)
                {
                    if (a.KI != Keys.None)
                    {
                        
                        return "Key:" + a.KI.ToString();
                    }
                    else
                    {
                        return "Mus:" + a.MI.ToString();
                    }
                }
                if (m == a.MI)
                {
                    onin = true;
                }
            }
            if (onin)
            {
                return "Nul:None";
            }
            else
            {
                return "Mus:" + m.ToString();
            }

        }

    }
    /// <summary>
    /// インプットを変換するための器具。
    /// 
    /// </summary>
    [Serializable]
    public class IPC
    {
        MouseButtons m = MouseButtons.None, mo = MouseButtons.None;
        Keys k = Keys.None, ko = Keys.None;

        /// <summary>
        /// 出力キー
        /// </summary>
        public Keys KO { get { return ko; } }
        /// <summary>
        /// 出力マウスボタン
        /// </summary>
        public MouseButtons MO { get { return mo; } }
        /// <summary>
        /// 入力キー
        /// </summary>
        public Keys KI { get { return k; } }
        /// <summary>
        /// 入力マウスボタン 
        /// </summary>
        public MouseButtons MI { get { return m; } }
        /// <summary>
        /// 変換器を作る
        /// </summary>
        /// <param name="i">入力</param>
        /// <param name="o">出力</param>
        public IPC(Keys i, Keys o)
        {
            k = i;
            ko = o;
        }
        /// <summary>
        /// 変換器をコピーする
        /// </summary>
        /// <param name="i">変換器</param>
        public IPC(IPC i)
        {
            k = i.k;
            ko = i.ko;
            m = i.m;
            mo = i.mo;
        }
        /// <summary>
        /// 変換器を作る
        /// </summary>
        /// <param name="i">入力</param>
        /// <param name="o">出力</param>
        public IPC(Keys i, MouseButtons o)
        {
            k = i;
            mo = o;
        }
        /// <summary>
        /// 変換器を作る
        /// </summary>
        /// <param name="i">入力</param>
        /// <param name="o">出力</param>
        public IPC(MouseButtons i, Keys o)
        {
            m = i;
            ko = o;
        }
        /// <summary>
        /// 変換器を作る
        /// </summary>
        /// <param name="i">入力</param>
        /// <param name="o">出力</param>
        public IPC(MouseButtons i, MouseButtons o)
        {
            m = i;
            mo = o;
        }

        /// <summary>
        /// 入力と出力を入れ替える
        /// </summary>
        public void flip()
        {
            var aas = k;
            var oos = m;
            k = ko;
            m = mo;
            mo = oos;
            ko = aas;
        }
        /// <summary>
        /// 入力が等しいかを判断する
        /// </summary>
        /// <param name="i">その入力</param>
        /// <returns>等しい</returns>
        public bool getin(Keys i)
        {
            return k == i;
        }
        /// <summary>
        /// 入力が等しいかを判断する
        /// </summary>
        /// <param name="i">その入力</param>
        /// <returns>等しい</returns>
        public bool getin(MouseButtons i)
        {
            return m == i;
        }
        /// <summary>
        /// 出力が等しいかを判断する
        /// </summary>
        /// <param name="i">その出力</param>
        /// <returns>等しい</returns>
        public bool getout(Keys i)
        {
            return ko == i;
        }
        /// <summary>
        /// 出力が等しいかを判断する
        /// </summary>
        /// <param name="i">その出力</param>
        /// <returns>等しい</returns>
        public bool getout(MouseButtons i)
        {
            return mo == i;
        }

        /// <summary>
        /// 入力を入れ替える
        /// </summary>
        /// <param name="i">入れ替え先の入力</param>
        /// <returns>入れ替えて変化したか</returns>
        public bool changein(Keys i)
        {
            if (k == i) return false;
            k = i;
            m = MouseButtons.None;
            return true;
        }
        /// <summary>
        /// 入力を入れ替える
        /// </summary>
        /// <param name="i">入れ替え先の入力</param>
        /// <returns>入れ替えて変化したか</returns>
        public bool changein(MouseButtons i)
        {
            if (m == i) return false;
            k = Keys.None;
            m = i;
            return true;
        }
        /// <summary>
        /// 何をどう変換しているのかを文字列で表す
        /// </summary>
        /// <returns>入力 => 出力</returns>
        public string getString()
        {
            string aas = "";
            aas += getinString();
            aas += " => ";


            aas += getoutString();

            return aas;
        }
        /// <summary>
        /// 出力のストリングを返す
        /// </summary>
        /// <returns>出力</returns>
        public string getoutString()
        {
            string aas = "";
           

            if (ko != Keys.None) aas += "Key:"+ko.ToString();
            if (mo != MouseButtons.None) aas += "Mus:" + mo.ToString();

            return aas;
        } 
        /// <summary>
            /// 入力のストリングを返す
            /// </summary>
            /// <returns>入力</returns>
        public string getinString()
        {
            string aas = "";
            if (k != Keys.None) aas += "Key:" + k.ToString();
            if (m != MouseButtons.None) aas += "Mus:" + m.ToString();
          

            return aas;
        }
        /// <summary>
        /// 文章の中に含まれてる入力([Key:Left],[Mus:Right])[]必要！を変換する
        /// </summary>
        /// <param name="ipss">変換器共</param>
        /// <param name="mo">変換する文字列</param>
        /// <returns>変換した文字列</returns>
        static public string convertstringinput(List<IPC>ipss,string mo) 
        {
           
            foreach (var a in ipss) 
            {
            mo=    mo.Replace("["+a.getoutString()+"]",a.getinString());

            }
            return mo;
        }
    }
}
