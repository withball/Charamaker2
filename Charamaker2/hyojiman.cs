using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vortice.Direct2D1;
using Vortice.DirectWrite;
using Vortice.DXGI;
using Vortice;
using Vortice.Multimedia;
using System.Numerics;
using Vortice.Mathematics;
using Charamaker2.Character;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing;
using System.Web;
using Charamaker2.Shapes;

namespace Charamaker2
{
    /// <summary>
    /// hyojimanが受け入れられる描画器の基底クラス
    /// </summary>
    [Serializable]
    abstract public class drawings
    {
        /// <summary>
        /// x,y,z。zは描画順
        /// </summary>
        public float x, y, z;
        /// <summary>
        /// 基底のコンストラクタ
        /// </summary>
        /// <param name="xx">始点のx</param>
        /// <param name="yy">始点のy</param>
        /// <param name="zz">描画順z</param>
        public drawings(float xx, float yy, float zz) { x = xx; y = yy; z = zz; }
        /// <summary>
        /// コピーするときのコンストラクタ
        /// </summary>
        /// <param name="d">コピー元</param>
        public drawings(drawings d) { x = d.x; y = d.y; z = d.z; }
        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        public drawings() { }
        /// <summary>
        /// 書くときに呼び出されるメソッド
        /// </summary>
        /// <param name="hyo">書くhyojiman</param>
        /// <param name="cl">描画で変化があるならその時の時間の速さ。</param>
        ///<param name="draw"> 描画するか</param>
        /// <returns>描画したか</returns>
        /// 
        abstract public bool draw(hyojiman hyo, float cl, bool draw);
        /// <summary>
        /// 表示マンに追加する
        /// </summary>
        /// <param name="hyo">追加するやーつ</param>
        /// <returns>追加できたか</returns>
        virtual public bool add(hyojiman hyo)
        {
            return hyo.addpicture(this);
        }
        /// <summary>
        /// 表示マンから削除する
        /// </summary>
        /// <param name="hyo">削除するやつ</param>
        /// <returns>削除できたか</returns>
        virtual public bool remove(hyojiman hyo)
        {
            return hyo.removepicture(this);
        }
        /// <summary>
        /// コピーするためのメソッド
        /// </summary>
        /// <param name="d">コピー元</param>
        public void copy(drawings d)
        {
            x = d.x;
            y = d.y;
            z = d.z;
        }
        /// <summary>
        /// クローンするためのメソッド
        /// </summary>
        /// <returns></returns>
        public abstract drawings clone();
      
        /// <summary>
        /// 表示マンにて表示されている範囲に入っているか調べる
        /// </summary>
        /// <param name="hyo"></param>
        /// <returns></returns>

        public virtual bool inHyoji(hyojiman hyo) 
        {
            return true;
        }
    }

    /// <summary>
    /// PBHに仕えるクラス
    /// </summary>
    public class PBHman
    {
        public ID2D1Bitmap bmp;
        public int time;
        public Matrix3x2 trans;
        public PBHman(ID2D1Bitmap bmp, int time, Matrix3x2 trans)
        {
            this.trans = trans;
            this.time = time;
            this.bmp = bmp;
        }
    }

    /// <summary>
    /// めっさーじを持つピクチャー
    /// </summary>
    public class messagepicture:picture
    {
        /// <summary>
        /// メッセージ
        /// </summary>
        public message m;
      
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="p"></param>
        /// <param name="m"></param>
        public messagepicture(picture p, message m) :base(p)
        {
            this.m = m;
            
        }   /// <summary>
            /// ピクチャーをコピーするときのコンストラクタ
            /// </summary>
            /// <param name="p">コピー元</param>
        public messagepicture(messagepicture p) : base(p)
        {
           if(p.m!=null) m = (message)Activator.CreateInstance(p.m.GetType(), p.m);
        }
        public override bool add(hyojiman hyo)
        {
            var res= base.add(hyo); 
            m?.add(hyo);
            return res;
        }
        public override bool remove(hyojiman hyo)
        {
            m?.remove(hyo);
            return base.remove(hyo);
        }
        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        public messagepicture():base() { }
        /// <summary>
        /// メッセージを整える
        /// </summary>
        protected void setmessage() 
        {
            m.x = this.gettx();
            m.y = this.getty();
            m.rad = rad;
            m.opa = opa;
            m.SIZE = w*2 / m.nmoji;
           
        }
        public override bool draw(hyojiman hyo, float cl, bool draw)
        {
            var res= base.draw(hyo, cl, draw); 
            if (m != null)
            {
                setmessage();
                m.draw(hyo, cl, draw);
                //   Console.WriteLine(z + " :QEWR: " + m.z+"  "+dz);
            }
            return res;
        }
        public override drawings clone()
        {
            return new messagepicture(this);
        }
    }
    /// <summary>
    /// 画像一枚のクラス
    /// </summary>
    [Serializable]
    public class picture:drawings
    {

      /// <summary>
      /// 幅と高さ
      /// </summary>
        public float w, h;
        /// <summary>
        /// 画像の中の中心点
        /// </summary>
        public float tx, ty;
        /// <summary>
        /// 画像の中心点xを返す。
        /// </summary>
        public float TX { get { if (mir) return w - tx; else return tx; } }
        /// <summary>
        /// 画像の中心点yを返す
        /// </summary>
        public float TY { get { return ty; } }

        /// <summary>
        /// ラヂアン
        /// </summary>
        protected double rad;
        /// <summary>
        /// 画像を反転させる
        /// </summary>
        public bool mir;
        
        private string pretex;
        /// <summary>
        /// 現在のテクスチャー無い場合は"";
        /// </summary>
        public string nowtex { get {if(textures.ContainsKey(texname))return textures[texname]; return ""; } }
        /// <summary>
        /// 不透明度。0＜＝＜＝1
        /// </summary>
        public float OPA { get { return opa; } set { if (value > 1) opa = 1; else if (value < 0) opa = 0; else opa = value; } }
        protected float opa;

       // [NonSerialized] ID2D1BitmapRenderTarget bmprt=null;

        /// <summary>
        /// テクスチャーの名前。texturesになければ無になる。"nothing"と指定しても無になる
        /// </summary>
        public string texname;
        /// <summary>
        /// 角度。-Pi＜＝＜＝Pi
        /// </summary>
        public double RAD { get { return rad; } set { rad = Math.Atan2(Math.Sin(rad), Math.Cos(rad)); float x = gettx(), y = getty(); rad = value; settxy(x, y); rad = Math.Atan2(Math.Sin(rad), Math.Cos(rad)); } }
        /// <summary>
        /// テクスチャーの塊
        /// </summary>
        public Dictionary<string, string> textures = new Dictionary<string, string>();

        /// <summary>
        /// 一枚のテクスチャでピクチャーを生成する
        /// </summary>
        /// <param name="tex">テクスチャー</param>
        /// <param name="z">Z</param>
        /// <param name="widthscale">横幅をここにそろえる</param>
        /// <param name="hin">やっぱ縦幅にする</param>
        /// <param name="txp">中心点の位置x</param>
        /// <param name="typ">中心点の位置y</param>
        /// <param name="opa">透明度</param>
        /// <param name="rad">角度</param>
        /// <param name="tx">中心の位置x</param>
        /// <param name="ty">中心の位置y</param>
        /// <param name="mirror">反転</param>
        /// <returns></returns>
        static public picture onetexpic(string tex, float widthscale, float z = 0, bool hin = false, float txp = 0.5f, float typ = 0.5f, float opa = 1, double rad = 0,float tx=0,float ty=0,bool mirror=false) 
        {
            var si = fileman.gettexsize(tex);
            float scale;
            if (hin)
            {
                scale = widthscale / (float)si.Height;
            }
            else
            {
                scale = widthscale / (float)si.Width;
            }
            var p= new picture(0, 0, z, si.Width * scale, si.Height * scale, si.Width * scale * txp, si.Height * scale * typ
                    , rad, mirror, opa, "def", new Dictionary<string, string> { { "def", tex } });
            p.settxy(tx, ty);
            return p;
        }
        
        /// <summary>
        /// ピクチャーを作るためのコンストラクタ
        /// </summary>
        /// <param name="xx">左上のx座標</param>
        /// <param name="yy">左上のy座標</param>
        /// <param name="zz">z</param>
        /// <param name="ww">幅</param>
        /// <param name="hh">高さ</param>
        /// <param name="ttx">中心x点</param>
        /// <param name="tty">中心y点</param>
        /// <param name="sita">ラディアン角度</param>
        /// <param name="mirror">反転しているか</param>
        /// <param name="opacity">不透明度</param>
        /// <param name="tex">テクスチャーの名前</param>
        /// <param name="texture">テクスチャーの辞書。new Dictionarystringstring{{"def","redbit"}}って感じで</param>
        public picture(float xx, float yy, float zz, float ww, float hh, float ttx, float tty, double sita, bool mirror, float opacity, string tex, Dictionary<string, string> texture):base(xx,yy,zz)
        {
          
            w = ww;
            h = hh;
            tx = ttx;
            ty = tty;
            rad = sita;
            mir = mirror;
            OPA = opacity;
            texname = tex;
            textures = new Dictionary<string, string>(texture);
        }
        /// <summary>
        /// ピクチャーをコピーするときのコンストラクタ
        /// </summary>
        /// <param name="p">コピー元</param>
        public picture(picture p):base(p)
        {
            w = p.w;
            h = p.h;
            tx = p.tx;
            ty = p.ty;
            rad = p.rad;
            mir = p.mir;
            opa = p.OPA;
            texname = p.texname;
           
            textures = new Dictionary<string, string>(p.textures);
        }
        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        public picture() { }
        /// <summary>
        /// 図形上の一点のx座標を取得する(回転の影響を考慮してるってこと)
        /// </summary>
        /// <param name="ww">左上を0としたときの図形の点の位置</param>
        /// <param name="hh">左上を0としたときの図形の点の位置</param>
        /// <returns>返されるのはx座標の値</returns>
        public float getcx(float ww, float hh)
        {
            float W; if (mir) W = w - ww; else W = ww;
            float H; if (mir && 1 == 0) H = h - hh; else H = hh;
            float rx = x + W * (float)Math.Cos(rad) - H * (float)Math.Sin(rad);
            return rx;
        }
        /// <summary>
        /// 図形上の一点のy座標を取得する(回転の影響を考慮してるってこと)
        /// </summary>
        /// <param name="ww">左上を0としたときの図形の点の位置</param>
        /// <param name="hh">左上を0としたときの図形の点の位置</param>
        /// <returns>返されるのはy座標の値</returns>
        public float getcy(float ww, float hh)
        {
            float W; if (mir) W = w - ww; else W = ww;
            float H; if (mir && 1 == 0) H = h - hh; else H = hh;
            float ry = y + W * (float)Math.Sin(rad) + H * (float)Math.Cos(rad);
            return ry;
        }
        /// <summary>
        /// 中心点のx座標を返す。
        /// </summary>
        /// <returns>x座標</returns>
        public float gettx()
        {

            return x + TX * (float)Math.Cos(rad) - TY * (float)Math.Sin(rad);

        }
        /// <summary>
        /// 中心点のy座標を返す。
        /// </summary>
        /// <returns>y座標</returns>
        public float getty()
        {
            return y + TX * (float)Math.Sin(rad) + TY * (float)Math.Cos(rad);

        }
        /// <summary>
        /// 中心をxy座標にセットする。
        /// </summary>
        /// <param name="xx">セットするx座標</param>
        /// <param name="yy">セットするy座標</param>
        public void settxy(float xx, float yy)
        {
            x = xx - TX * (float)Math.Cos(rad) + TY * (float)Math.Sin(rad);
            y = yy - TX * (float)Math.Sin(rad) - TY * (float)Math.Cos(rad);
        }
        /// <summary>
        /// スケールする
        /// </summary>
        /// <param name="sc">倍率</param>
        public void scale(float sc) 
        {
            float tx = gettx();
            float ty = getty();

            w *= sc;
            h *= sc;
            this.tx *= sc;
            this.ty *= sc;
            settxy(tx, ty);
        }

        /// <summary>
        /// 画像上の任意の一点をxy座標にセットする
        /// </summary>
        /// <param name="xx">セットするx座標</param>
        /// <param name="yy">セットするy座標</param>
        /// <param name="cx">画像上のwの点</param>
        /// <param name="cy">画像上のhの点</param>
        public void setcxy(float xx, float yy, float cx, float cy)
        {
            x += xx - getcx(cx, cy);
            y += yy - getcy(cx, cy);
        }
        /// <summary>
        /// 回転している方向に移動する奴
        /// </summary>
        /// <param name="dx">x方向の移動量</param>
        /// <param name="dy">y方向の移動量</param>
        public void wowidouxy(float dx, float dy)
        {
            settxy(gettx() + dx * (float)Math.Cos(rad) - dy * (float)Math.Sin(rad),
         getty() + dx * (float)Math.Sin(rad) + dy * (float)Math.Cos(rad));
        }
        /// <summary>
        /// 書くときに呼び出されるメソッド。画面外にある場合は描画されない
        /// </summary>
        /// <param name="hyo">書くhyojiman</param>
        /// <param name="cl">描画で変化があるならその時の時間の速さ。</param>
        ///<param name="draw"> 描画するか</param>
        /// <returns>描画したか</returns>
        public override bool draw(hyojiman hyo, float cl, bool draw)
        {
          
            if (hyo == null || !draw) return false;



            var p = this;
            if (p.textures.ContainsKey(p.texname) && p.textures[p.texname] != fileman.nothing)
            {
                var bitmap = fileman.ldtex(p.textures[p.texname]);
                if (bitmap != null)
                {
                    drawbitmap(bitmap,hyo);
                }


            }
            else
            {

            }
            return false;
        }


        /// <summary>
        /// 表示の範囲に入っているかチェックする
        /// </summary>
        /// <param name="hyo">どの表示マンか</param>
        /// <returns></returns>
        override public bool inHyoji(hyojiman hyo) 
        {
            return Math.Abs((getcx(w / 2, h / 2)) - (hyo.ww / 2 + hyo.camx)) * 2 <= hyo.ww + Math.Abs(w * Math.Cos(RAD)) + Math.Abs(h * Math.Sin(RAD))
                 && Math.Abs((getcy(w / 2, h / 2)) - (hyo.wh / 2 + hyo.camy)) * 2 <= hyo.wh + Math.Abs(w * Math.Sin(RAD)) + Math.Abs(h * Math.Cos(RAD));


        }

        /// <summary>
        /// ビットマップを描画するぞ
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="hyo"></param>
        /// <returns>完全な画面外だと描画しない</returns>
        protected bool drawbitmap(ID2D1Bitmap bitmap,hyojiman hyo)
        {
            float bairitu = hyo.Tbai;
            var p = this;
            if (this.OPA > 0 && inHyoji(hyo))
            {
                //  bmprt = byoga(hyo, this, bmprt);


                /*
                var x = p.getcx(p.w / 2, p.h / 2);
                var y = p.getcy(p.w / 2, p.h / 2);
                float si = bmprt.Bitmap.Size.Width;
                var btmpos = new RawRectF((x - hyo.camx)*bairitu-si/2, (y - hyo.camy)*bairitu-si/2, (x - hyo.camx) * bairitu + si / 2, (y - hyo.camy) * bairitu + si / 2);
                var bitmon = new RawRectF(0, 0, bmprt.Size.Width, bmprt.Size.Height);
                hyo.render.DrawBitmap(bmprt.Bitmap, btmpos, this.OPA, BitmapInterpolationMode.Linear, bitmon);
                */

                var a = Matrix3x2.CreateRotation((float)p.RAD, new Vector2((p.x * bairitu - hyo.camx * bairitu) + (p.tx * 0) * bairitu, (p.y * bairitu - hyo.camy * bairitu) + (p.ty * 0) * bairitu));
                if (p.mir)
                {
                    a = Matrix3x2.Multiply(new Matrix3x2(-1, 0, 0, 1, 0, 0), a);
                    a = Matrix3x2.Multiply(Matrix3x2.CreateTranslation(-(p.x + p.w / 2 - hyo.camx) * 2 * bairitu, 0), a);
                }
                if (p.w < 0)
                {
                    a = Matrix3x2.Multiply(new Matrix3x2(-1, 0, 0, 1, 0, 0), a);

                    a = Matrix3x2.Multiply(Matrix3x2.CreateTranslation(-(p.x + p.w / 2 - hyo.camx) * 2 * bairitu, 0), a);
                }

                if (p.h < 0)
                {
                    a = Matrix3x2.Multiply(Matrix3x2.CreateScale(1, -1), a);

                    a = Matrix3x2.Multiply(Matrix3x2.CreateTranslation(0, -(p.y + p.h / 2 - hyo.camy) * 2 * bairitu), a);
                }
                hyo.render.Transform = a;


                hyo.render.DrawBitmap(bitmap
                    , new RawRectF((p.x * bairitu - hyo.camx * bairitu), (p.y * bairitu - hyo.camy * bairitu)
                    , (p.x * bairitu + p.w * bairitu - hyo.camx * bairitu), (p.y * bairitu + p.h * bairitu - hyo.camy * bairitu))
                    , this.OPA, BitmapInterpolationMode.Linear
                    , new RawRectF(0, 0, bitmap.Size.Width, bitmap.Size.Height));

                // Console.WriteLine(p.textures[p.texname]+" asffffffffffffff   " +btmpos.ToString());
            }
            return true;
        }


        /// <summary>
        /// コピーするためのメソッド
        /// </summary>
        /// <param name="p">コピー元</param>
        public void copy(picture p)
        {
            base.copy(p);
            w = p.w;
            h = p.h;
            tx = p.tx;
            ty = p.ty;
            rad = p.rad;
            mir = p.mir;
            opa = p.OPA;
            texname = p.texname;

            textures = new Dictionary<string, string>(p.textures);
        }
        public override drawings clone()
        {
            var p = new picture(this);
            return p;
        }
        /*
        static ID2D1BitmapRenderTarget byoga(hyojiman hyo, picture p, ID2D1BitmapRenderTarget tag)
        {

            ID2D1Bitmap bitmap = null;
            bool byo = false;

            if (p.textures.ContainsKey(p.texname))
            {
                if (p.textures[p.texname] == "nothing")
                {
                    releasebts(tag);
                    //   tag.Dispose();

                    return null;
                }
                bitmap = fileman.ldtex(p.textures[p.texname]);
                byo = p.pretex != p.texname;
                p.pretex = p.texname;
            }
            if (bitmap == null)
            {
                releasebts(tag);
                // tag.Dispose();

                return null;
            }
            int won = 10;
            var siz = ((int)(Math.Sqrt(p.w * p.w + p.h * p.h) * hyo.Tbai / won)) * won + won;


            int size = siz;

            // Console.WriteLine(btmsize + " sizeeee " + height+" "+width+" "+SITAR+" "+SITA);
            if (tag == null || (int)tag.Bitmap.Size.Width != size)
            {
                releasebts(tag);
                //tag.Dispose();
                //   Console.WriteLine(size+" iyon "+ Math.Sqrt(p.w * p.w + p.h * p.h) * bairitu);
                tag = getbts(size);
                byo = true;
            }

            size = (int)tag.Size.Width;
            
            var a = Matrix3x2.CreateRotation((float)p.RAD, new Vector2(size / 2f, size / 2f));
            if (p.mir)
            {
                a = Matrix3x2.Multiply(new Matrix3x2(-1, 0, 0, 1, 0, 0), a);
                a = Matrix3x2.Multiply(Matrix3x2.CreateTranslation(-size, 0), a);


            }
            if (p.w < 0)
            {
                a = Matrix3x2.Multiply(new Matrix3x2(-1, 0, 0, 1, 0, 0), a);

                a = Matrix3x2.Multiply(Matrix3x2.CreateTranslation(-size, 0), a);


            }

            if (p.h < 0)
            {
                a = Matrix3x2.Multiply(Matrix3x2.CreateScale(1, -1), a);

                a = Matrix3x2.Multiply(Matrix3x2.CreateTranslation(0, -size), a);


            }
            var bb = Matrix3x2.CreateTranslation((size - p.w * hyo.Tbai) / 2f, (size - p.h * hyo.Tbai) / 2f);
            a = Matrix3x2.Multiply(bb, a);


            var c = Matrix3x2.CreateScale(p.w * hyo.Tbai / bitmap.Size.Width, p.h * hyo.Tbai / bitmap.Size.Height);
            a = Matrix3x2.Multiply(c, a);

            var transe = new Matrix3x2(a.M11, a.M12, a.M21, a.M22, a.M31, a.M32);

            if (!tag.Transform.Equals(transe) || byo)
            {

                tag.BeginDraw();

                tag.Clear(new Color4(0, 0, 0, 0));


                tag.Transform = transe;

                tag.DrawBitmap(bitmap, 1, BitmapInterpolationMode.Linear);

                //bitmap.Factory.Dispose();

                tag.EndDraw();

            }

            bitmap = null;
            
            return tag;



        }
        
        static Dictionary<int, List<ID2D1BitmapRenderTarget>> bts = new Dictionary<int, List<ID2D1BitmapRenderTarget>>();

        /// <summary>
        /// レンダーをもらう
        /// </summary>
        /// <param name="size">レンダーのサイズ</param>
        /// <returns>レンダー</returns>
        static public ID2D1BitmapRenderTarget getbts(int size)
        {
            if (size > 5000) size = 5000;
            // Console.WriteLine("aaaa gets bits");

            if (!bts.ContainsKey(size))
            {
                bts.Add(size, new List<ID2D1BitmapRenderTarget>());
            }
            if (bts[size].Count > 0)
            {
                var res = bts[size][0];
                bts[size].RemoveAt(0);
                return res;
            }
            else
            {
                var tag = fileman.render.CreateCompatibleRenderTarget(new System.Drawing.SizeF(size, size), CompatibleRenderTargetOptions.None);
                return tag;
            }
        }
        /// <summary>
        /// レンダーを返す
        /// </summary>
        /// <param name="b"></param>
        static public void releasebts(ID2D1BitmapRenderTarget b)
        {
            
           
            if (b != null)
            {
                  bts[(int)(b.Size.Width)].Add(b);
            }
        }*/
    }
    /// <summary>
    /// 内部にdrawingを格納し、背景画像として描画するクラス
    /// </summary>
    [Serializable]
    public class haikeidraws
    {
        /// <summary>
        /// x,y方向のスクロール割合
        /// </summary>
        public float x,y;
        /// <summary>
        /// 描く奴
        /// </summary>
        public drawings d;
        /// <summary>
        /// スクロール割合をかけ合わせた奴
        /// </summary>
        public float kakesc { get {return (x * y); } }
        /// <summary>
        /// スクロール割合の小さい方
        /// </summary>
        public float minsc { get { return Math.Min(x , y); } }
        /// <summary>
        /// コンストラクタだよ
        /// </summary>
        /// <param name="scrollwariaix">x方向のスクロール割合</param>
        /// <param name="scrollwariaiy">y方向のスクロール割合</param>
        /// <param name="dd">なにかdrawings</param>

        public haikeidraws(float scrollwariaix, float scrollwariaiy, drawings dd) 
        {
            d = dd;
            x = scrollwariaix;
            y = scrollwariaiy;
       
            //z = scroll;
        }
        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        public haikeidraws() { }

        /// <summary>
        ///コピーするためのコンストラクタ
        /// </summary>
        public haikeidraws(haikeidraws h) 
        {
            x = h.x;
            y = h.y;
            d =(drawings)Activator.CreateInstance(h.d.GetType(),h.d);

        }
        /// <summary>
        /// 表示マンにぶち込む
        /// </summary>
        /// <param name="h"></param>
        /// <returns>ぶち込めたか</returns>
       virtual public bool add(hyojiman h) {return h.addhaikeipicture(this); }

        /// <summary>
        /// 表示マンから消す
        /// </summary>
        /// <param name="h"></param>
        /// <returns>ぶち消せたか</returns>
        virtual public bool remove(hyojiman h) {return h.removehaikeipicture(this); }
        /// <summary>
        /// 描くためのメソッド
        /// </summary>
        /// <param name="hyo">描画する表示マン</param>
        /// <param name="cl">クロック</param>
        ///<param name="draw"> 描画するか</param>
        /// <returns></returns>
        virtual public bool draw(hyojiman hyo,float cl,bool draw) 
        {

            if (hyo == null)
            {
                d.draw(hyo,cl,draw);
                return false;
            }
            float tx = hyo.camx,ty=hyo.camy;
            float xx = d.x, yy = d.y;

            hyo.camx = (hyo.camx + hyo.ww/2) * x ;
            hyo.camy = (hyo.camy + hyo.wh/2) * y;
            d.x += hyo.ww / 2;
            d.y += hyo.wh / 2;
            var res = d.draw(hyo,cl,draw);
            hyo.camx = tx;
            hyo.camy = ty;
            d.x = xx;
            d.y = yy;

            return res;
        }

        /// <summary>
        /// 表示マンの表示できる範囲に入っているか調べる
        /// </summary>
        /// <param name="hyo"></param>
        /// <returns></returns>
         public bool inHyoji(hyojiman hyo)
        {

            float tx = hyo.camx, ty = hyo.camy;
            float xx = d.x, yy = d.y;

            hyo.camx = (hyo.camx + hyo.ww / 2) * x;
            hyo.camy = (hyo.camy + hyo.wh / 2) * y;
            d.x += hyo.ww / 2;
            d.y += hyo.wh / 2;
            var res = d.inHyoji(hyo);
            hyo.camx = tx;
            hyo.camy = ty;
            d.x = xx;
            d.y = yy;

            return res;
        }

        /// <summary>
        /// 頑張ってクローン
        /// </summary>
        /// <returns></returns>
        virtual public haikeidraws clone() 
        {
            return new haikeidraws(this);
        }
    }
    /// <summary>
    /// 時間経過で消える背景
    /// </summary>
    [Serializable]
    public class timehaikeidraws:haikeidraws
    {
        float timer = 0;
        /// <summary>
        /// コンストラクタだよ
        /// </summary>
        /// <param name="time">持続時間</param>
        /// <param name="scrollwariaix">x方向のスクロール割合</param>
        /// <param name="scrollwariaiy">y方向のスクロール割合</param>
        /// <param name="dd">なにかdrawings</param>
        public timehaikeidraws(float time,float scrollwariaix, float scrollwariaiy, drawings dd):base(scrollwariaix,scrollwariaiy,dd)
        {
            timer = time;
        }
        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        public timehaikeidraws() { }
        override public bool draw(hyojiman hyo, float cl,bool draw)
        {
            timer -= cl;
        
            if(timer<=0)hyo.removehaikeipicture(this);
            return base.draw(hyo, cl,draw);
        }

        /// <summary>
        ///コピーするためのコンストラクタ
        /// </summary>
        public timehaikeidraws(timehaikeidraws h):base(h)
        {
            timer = h.timer;

        }
        public override haikeidraws clone()
        {
            return new timehaikeidraws(this);
        }
    }
    /// <summary>
    /// 表示マンが書ける
    /// </summary>
    public class Phyojiman :picture
    {

        /// <summary>
        /// 表示マン
        /// </summary>
        readonly public hyojiman hj;
        /// <summary>
        /// 自動的に表示マンを進める
        /// </summary>
        public bool auto;
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="p"></param>
        /// <param name="h"></param>
        /// <param name="auto">自動的にフレームするか</param>
        public Phyojiman(picture p, hyojiman h,bool auto=true) : base(p)
        {
            this.hj = h;
            this.auto = auto;
        }   /// <summary>
            /// ピクチャーをコピーするときのコンストラクタ
            /// </summary>
            /// <param name="p">コピー元</param>
        public Phyojiman(Phyojiman p) : base(p)
        {
            if (p.hj != null) hj = (hyojiman)Activator.CreateInstance(p.hj.GetType(), p.hj);
            auto = p.auto;
        }
        public override bool add(hyojiman hyo)
        {
            return base.add(hyo);
        }
        public override bool remove(hyojiman hyo)
        {
            return base.remove(hyo);
        }
        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        public Phyojiman() : base() { }
        public override bool draw(hyojiman hyo, float cl, bool draw)
        {
            var res= base.draw(hyo, cl, draw); 
            if (hj != null)
            {
                if (auto) {
                    hj.hyoji(cl, draw:draw) ;
                    
                }
                var bmpp = hj.getbitmap();
                if (bmpp == null)
                {
                    var rd = (ID2D1BitmapRenderTarget)fileman.getBitmapRender(hyo.render,"Hyojitemp");

                    drawbitmap(rd.Bitmap, hyo);
                }
                else 
                {
                    drawbitmap(bmpp, hyo);
                }
              

            //    Console.WriteLine(hj.ww+"aksgdja"+hj.render.PixelSize.Width);
            //    Console.WriteLine(x+"::"+y + "aksgdja" + w);
            }
            return res;
        }
   
        public override drawings clone()
        {
            return new Phyojiman(this);
        }
    }


    /// <summary>
    /// 画面を描画したり、音を出したり、そういうのを行う
    /// </summary>
    [Serializable]
    public class hyojiman
    {
        /// <summary>
        /// レンダーの描画を開始する。enddrawの間に描画を記述する
        /// </summary>
        /// <param name="resethaikei">resethaikeiも呼び出すか</param>
        public void begindraw(bool resethaikei=false)
        {
            render.BeginDraw();
            if (resethaikei) 
            {
                this.resethaikei();
            }
        }
        /// <summary>
        /// 背景をリセットする。begindrawは呼び出しといてね
        /// </summary>
        public void resethaikei() 
        {
            render.Clear(new Color4(HR, HG, HB, HOpa));
        }

        /// <summary>
        /// レンダーの描画を終了するbegindrawの間に描画を記述する。
        /// </summary>
        public void enddraw()
        {
            render.EndDraw();
        }
        readonly bool destroyRender=false;

        /// <summary>
        /// この表示マンの表示を別レンダーに生き写す
        /// </summary>
        /// <returns>このビットマップはDisposeしてね</returns>
        virtual public void ikiutusi(ID2D1RenderTarget render) 
        {
           

            hyoji2(render, 0, true, true, false, otoOptin.ordinal,true);
            
        }
        /// <summary>
        /// ビットマップレンダーならビットマップをもらう。
        /// </summary>
        /// <returns>ないならnull</returns>
        virtual public ID2D1Bitmap getbitmap()
        {
            if (render.GetType() == typeof(ID2D1BitmapRenderTarget)) 
            {
                return ((ID2D1BitmapRenderTarget)render).Bitmap;
            }
            return null;
        }


        /// <summary>
        /// 再生する音
        /// </summary>
        protected List<string> otos = new List<string>();
        /// <summary>
        /// 再生す音の音量
        /// </summary>
        protected List<float> otovols = new List<float>();
        /// <summary>
        /// この表示マンの音量
        /// </summary>
        public float volume = 1;
        
        /// <summary>
        /// 再生するBGM
        /// </summary>
        protected string bgm = "";
        /// <summary>
        /// 同じBGMを流すときに最初から再生するか
        /// </summary>
        protected bool butu = false;
        /// <summary>
        /// 音を出す。通信や、リプレイの保存をするならばこれを使わなくてはならない。
        /// filemanのものとほぼ同じである。
        /// </summary>
        /// <param name="otoe">.\oto\*.wav</param>
        /// <param name="vol">この音のボリューム</param>
        public void playoto(string otoe, float vol = 1)
        {
            otos.Add(otoe);
            otovols.Add(vol);
        }
        /// <summary>
        /// 他のhyojimanから音をもってくる
        /// </summary>
        /// <param name="hyo">持ってくる元</param>
        public void playoto(hyojiman hyo)
        {
            for(int i=0;i<hyo.otos.Count;i++)
            {
                otos.Add(hyo.otos[i]);
                otovols.Add(hyo.otovols[i]);
            }

        }
        /// <summary>
        /// 他のhyojimanのBGMをコピーする
        /// </summary>
        /// <param name="hyo">コピー元</param>
        public void playbgm(hyojiman hyo)
        {
            bgm = hyo.bgm;
            butu = hyo.butu;
        }
        /// <summary>
        /// bgmを開始する。リプレイの保存をするならばこれを使わなくてはならない。
        /// filemanのものとほぼ同じである。
        /// </summary>
        /// <param name="bgme">.\oto\bgm\*.wav</param>
        /// <param name="butugiri"></param>
        public void playbgm(string bgme ="", bool butugiri = false)
        {
            bgm = bgme;
            butu = butugiri;
        }
        /// <summary>
        /// 背景をどれだけぼかすか。スクロール割合0.1につき一回このぼかしが入る。
        /// </summary>
        public float haikeibokasi = 0.04f;
        /// <summary>
        /// 描画物体のリスト
        /// </summary>
        public List<drawings> pics = new List<drawings>();
        /// <summary>
        /// 背景描画物体のリスト
        /// </summary>
        public List<haikeidraws> haikeipics = new List<haikeidraws>();
        /// <summary>
        /// エフェクトのリスト
        /// </summary>
        [NonSerialized]
        public List<effectchara> effects = new List<effectchara>();

        /// <summary>
        /// 画面左上のカメラ座標
        /// </summary>
        public float camx = 0,camy = 0;
        /// <summary>
        /// 背景色
        /// </summary>
        public float HR = 0, HG = 0.8f, HB = 0.9f,HOpa=1;
        bool resetpicsman;
        bool resethaikeipicsman;
        /// <summary>
        /// 拡大率。オブジェクトの大きさを一定にしたいときはこっちを参照
        /// </summary>
        public float bairitu = 1;
        /// <summary>
        /// 画面の画質も含めた本当の拡大率。ガチ描画の時には使う
        /// </summary>
        public float Tbai { get { return bairitu * gasitu; } }

        /// <summary>
        /// 画質
        /// </summary>
        public readonly float gasitu;

        /// <summary>
        /// いろいろ載せられるタグ。
        /// </summary>
        public string tag = "";
        /// <summary>
        /// 謎の変数
        /// </summary>
        int skipn = 0;
        /// <summary>
        /// レンダー
        /// </summary>
        [NonSerialized]
        public ID2D1RenderTarget render=null;
        /// <summary>
        /// ウィンドウの座標での幅
        /// </summary>
        public float ww { get { return render.PixelSize.Width / Tbai; } }
        /// <summary>
        /// ウィンドウの座標での高さ
        /// </summary>
        public float wh { get { return render.PixelSize.Height / Tbai; } }

        /// <summary>
        /// 画質を考慮したピクセルサイズ
        /// </summary>
        public Size gasituSize { get { return render.PixelSize ; } }

        /// <summary>
        /// 本来のピクセルサイズ
        /// </summary>
        public Size TrueSize { get { return new Size((int)Math.Round(render.PixelSize.Width/gasitu)
            , (int)Math.Round(render.PixelSize.Height / gasitu)); } }

        /// <summary>
        /// 同時に表示できる描画オブジェクトの限界量
        /// </summary>
        public int maxdraws = 1111;

        /// <summary>
        /// bairituを幅を指定して決める
        /// </summary>
        /// <param name="w">その幅</param>
        /// <param name="h">高さにするか</param>
        public void setBairituW(float w,bool h=false) 
        {
            w = Math.Abs(w);
            if (w == 0) return;
            if (h)
            {
                bairitu = render.PixelSize.Height/gasitu  /w;
            }
            else
            {
                bairitu = render.PixelSize.Width / gasitu / w;
            }
        }

        /// <summary>
        /// 表示マンのコンストラクタ
        /// </summary>
        /// <param name="ren"></param>
        /// <param name="gasitu"></param>
        /// <param name="destroyRender">レンダーを死ぬとき破壊するか</param>
        public hyojiman(ID2D1RenderTarget ren,float gasitu,bool destroyRender=false)
        {
            reset();
            render = ren;
            this.destroyRender = destroyRender;
            this.gasitu = gasitu;
        }
        
        
        /// <summary>
        /// 通信を受け取ったときにコピーするためのメソッド。
        /// </summary>
        /// <param name="moto">コピー元</param>
        public void tusinhyoji(hyojiman moto)
        {

            pics = moto.pics;
            haikeipics = moto.haikeipics;
            otos = moto.otos;
            volume = moto.volume;

            otovols = moto.otovols;
            bgm = moto.bgm;
            butu = moto.butu;
            tag = moto.tag;
            haikeibokasi = moto.haikeibokasi;
            camx = moto.camx;
            camy = moto.camy;
            HR = moto.HR; HG = moto.HG; HB = moto.HB; HOpa = moto.HOpa;
            resethaikeipicsman = moto.resethaikeipicsman;
            resetpicsman = moto.resetpicsman;
            bairitu = moto.bairitu;
         
        }
        /// <summary>
        /// 別の表示マンをちゃんとコピーする
        /// </summary>
        /// <param name="h">コピー元</param>
        public void truecopy(hyojiman h) 
        {
            pics = new List<drawings>(h.pics);
            haikeipics = new List<haikeidraws>(h.haikeipics);
            effects = new List<effectchara>(h.effects);
            otos = new List<string>(h.otos);
            otovols = new List<float>(h.otovols);
            volume = h.volume;

            bgm = h.bgm;
            butu = h.butu;
            tag = h.tag;

            haikeibokasi = h.haikeibokasi;
            camx = h.camx;
            camy = h.camy;
            HR = h.HR; HG = h.HG; HB = h.HB; HOpa = h.HOpa;
            resethaikeipicsman = h.resethaikeipicsman;
            resetpicsman = h.resetpicsman;
            bairitu = h.bairitu;
            skipn = h.skipn;
            maxdraws = h.maxdraws;
        }

        /// <summary>
        /// ちょっと変なコピー。hyojiを行う前に分裂させ、1フレーム限りのエフェクト(UIとか)を乗せてhyojiしたのちnisehyojiし通信するとよき
        /// </summary>
        /// <param name="moto">コピー元</param>
        public void copy(hyojiman moto)
        {
            var mmm = new List<effectchara>(moto.effects);
            int i;
            for (i=0; i<mmm.Count;i++) mmm[i].sinu(moto);

            pics = new List<drawings>(moto.pics);
            haikeipics = new List<haikeidraws>(moto.haikeipics);

            for (i = 0; i < mmm.Count; i++) mmm[i].add();

            effects = new List<effectchara>();
            for (i = 0; i < moto.effects.Count(); i++) 
            {
                
                Activator.CreateInstance(moto.effects[i].GetType(),Convert.ChangeType(moto.effects[i], moto.effects[i].GetType()), true, this, true, false);
            }
            otos = new List<string>(moto.otos);
            otovols = new List<float>(moto.otovols);
            volume = moto.volume;
            bgm = moto.bgm;
            butu = moto.butu;
            tag = moto.tag;

            haikeibokasi = moto.haikeibokasi;
            camx = moto.camx;
            camy = moto.camy;
            HR = moto.HR; HG = moto.HG; HB = moto.HB; HOpa = moto.HOpa;
            resethaikeipicsman = moto.resethaikeipicsman;
            resetpicsman = moto.resetpicsman;
            bairitu = moto.bairitu;
            skipn = moto.skipn;
            maxdraws = moto.maxdraws;
        }
        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        public hyojiman() { }
        /// <summary>
        /// エフェクトのフレームだけ行う
        /// </summary>
        /// <param name="cl">クロック</param>
        public void nisehyoji(float cl = 1)
        {
            for (int i = effects.Count() - 1; i >= 0; i--)
            {
                effects[i].frame();
            }
        }
       public  enum otoOptin 
        {
            /// <summary>
            /// 普通
            /// </summary>
             ordinal,
             /// <summary>
             /// 再生するけどそのまんま。録画を再生するときに。
             /// </summary>
            PlayandDontRemove,
            /// <summary>
            /// 何もしない。保存される
            /// </summary>
            Dont,
            /// <summary>
            /// BGMだけ再生し、残りは破棄する。シーンのスキップに。
            /// </summary>
            OnlyBgm,
            /// <summary>
            /// 全てを破棄する
            /// </summary>
            BreakAll
        }


        /// <summary>
        /// 別のレンダーに描画する
        /// </summary>
        /// <param name="cl">クロック</param>
        /// <param name="begin">描画開始をするか</param>
        /// <param name="end">描画終了をするか</param>
        /// <param name="doEffectframe">エフェクトのフレームを行うか</param>
        /// <param name="oO">音を再生した後その情報を消すか(録画を再生するときのみdontremove,撮影はDontで?)</param>
        /// <param name="draw">描画とか音再生するか</param>
        /// <param name="rend">別のレンダーに書きたい場合はこれ</param>
        public void hyoji2(ID2D1RenderTarget rend, float cl = 1, bool begin = true, bool end = true, bool doEffectframe = true, otoOptin oO = otoOptin.ordinal, bool draw = true)
        {
            var swap = this.render;
            this.render = rend;
            hyoji(cl, begin, end, doEffectframe, oO, draw);
            this.render = swap;

        }

        /// <summary>
        /// 表示を行う
        /// </summary>
        /// <param name="cl">クロック</param>
        /// <param name="begin">描画開始をするか</param>
        /// <param name="end">描画終了をするか</param>
        /// <param name="doEffectframe">エフェクトのフレームを行うか</param>
        /// <param name="oO">音を再生した後その情報を消すか(録画を再生するときのみdontremove,撮影はDontで?)</param>
        /// <param name="draw">描画とか音再生するか</param>
        public void hyoji(float cl = 1, bool begin = true, bool end = true,bool doEffectframe=true, otoOptin oO = otoOptin.ordinal, bool draw=true)
        {
            if (oO != otoOptin.Dont)
            {
                for (int i = 0; i < otos.Count; i++)
                {
                    if (oO==otoOptin.ordinal||oO==otoOptin.PlayandDontRemove) fileman.playoto(otos[i], otovols[i] * volume);
                }

                if (bgm != "") 
                    if (oO==otoOptin.ordinal|| oO == otoOptin.PlayandDontRemove||oO==otoOptin.OnlyBgm) 
                        fileman.playbgm(bgm, butu);
                if (oO == otoOptin.ordinal||oO==otoOptin.OnlyBgm||oO==otoOptin.BreakAll)
                {
                    bgm = "";
                    otos.Clear();
                    otovols.Clear();
                }
            }
           
            if(doEffectframe)
            for (int i = effects.Count() - 1; i >= 0; i--)
            {
                effects[i].frame(cl);
            }

            if (resetpicsman || 1 == 1)
            {
                resetpicsman = false;
                //       pics = new List<picture>(pictures.Keys);
                //     pics.Sort((a, b) => (int)(-a.z + b.z));
               picssort();
            }
            if (resethaikeipicsman || 1 == 1)
            {
                resethaikeipicsman = false;
                //       pics = new List<picture>(pictures.Keys);
                //     pics.Sort((a, b) => (int)(-a.z + b.z));
                haikeisort();
            }
            //   render.Factory.ReloadSystemMetrics();
            skipn += 1;
            if (skipn >= 2)
            {
                /*   
                   for (int i = messages.Count() - 1; i >= 0; i--)
                   {
                       messages[i].susumukun();
                   }
                   skipn = 0;
                   return;*/
                skipn = 0;
            }
            if (begin)
            {
                begindraw(true);
            }

            float kugiriman = 0f;
            //   Console.WriteLine(pictures.Count + " pics count" + haikeipictures.Count());
            var haikeislb = render.CreateSolidColorBrush(new Color4(HR, HG, HB, haikeibokasi));
            for (int i = haikeipics.Count() - 1; i >= 0; i--)
            {


                while (haikeipics[i].minsc >= kugiriman && kugiriman < 1)
                {
                    render.Transform = Matrix3x2.CreateTranslation(0, 0);
                    kugiriman += 0.1f;

                    render.FillRectangle(new System.Drawing.RectangleF(0, 0, ww * Tbai, wh * Tbai), haikeislb);

                }
                haikeipics[i].draw(this, cl,draw);
            }
            //  Console.WriteLine("asfn"+kugiriman);
            render.Transform = Matrix3x2.CreateTranslation(0, 0);
            render.FillRectangle(new RawRectF(0, 0, ww * Tbai, wh * Tbai), haikeislb);
            haikeislb.Dispose();



            int cou = 0;
            for (int i = pics.Count() - 1; i >= 0; i--)
            {
                if (cou > maxdraws) break;

                if (pics[i].draw(this, cl,draw)) cou++;




            }
            render.Transform = Matrix3x2.CreateTranslation(0, 0);


            if (end)
            {
                enddraw();
            }

        }



        /// <summary>
        /// picsの並び順を整えたいときにどうぞ
        /// </summary>
        public void resetpics()
        {
            resetpicsman = true;


        }

        /// <summary>
        /// 背景の並び順を整えたいときにどうぞ
        /// </summary>
        public void resethaikeipics()
        {
            resethaikeipicsman = true;


        }



        /// <summary>
        /// リセットするメソッド。
        /// これよりfileman.gethyojimanのほうがいいよ
        /// </summary>
        public void reset()
        {
            clearpics();
            clearhaikeipics();
            effects.Clear();
            resetoto();
            haikeipics.Clear();
            pics.Clear();
            camx = 0;
            camy = 0;
        }
        /// <summary>
        /// 音、BGMをリセットする
        /// </summary>
        public void resetoto() 
        {
            bgm = "";
            butu = false;
            otos.Clear();
            otovols.Clear();

        }

        /// <summary>
        /// 追加されてるdrawingsを全部消す
        /// </summary>
        public void clearpics() 
        {
            for (int i = pics.Count() - 1; i >= 0; i--)
            {
                removepicture(pics[i]);
            }
        }

        /// <summary>
        /// 追加されてるhaikeidrawingsを全部消す
        /// </summary>
        public void clearhaikeipics() 
        {

            for (int i = haikeipics.Count() - 1; i >= 0; i--)
            {
                removehaikeipicture(haikeipics[i]);
            }
        }

        /// <summary>
        /// drawingsを追加する
        /// </summary>
        /// <param name="p">なんでも</param>
        /// <returns>追加ができたかどうか</returns>
        internal bool addpicture(drawings p)
        {
            resetpics();
            if (!pics.Contains(p))
            {

                pics.Add(p);
                return true;
            }
            return false;
        }
        /// <summary>
        /// drawingsを削除する
        /// </summary>
        /// <param name="p">なんでも</param>
        /// <returns>削除できたかどうか</returns>
       internal bool removepicture(drawings p)
        {

            return pics.Remove(p);


        }
        /// <summary>
        /// 背景を追加する
        /// </summary>
        /// <param name="p">なんでも</param>
        /// <returns>追加できたかどうか</returns>
        internal bool addhaikeipicture(haikeidraws p)
        {


            resethaikeipics();
            if (!haikeipics.Contains(p))
            {
                haikeipics.Add(p);
                return true;
            }
            return false;

        }
        /// <summary>
        /// 背景を削除する
        /// </summary>
        /// <param name="p">なんでも</param>
        /// <returns>削除できたかどうか</returns>
        internal bool removehaikeipicture(haikeidraws p)
        {

           return haikeipics.Remove(p);

        }
        /// <summary>
        /// timソートでpicsを整理する
        /// </summary>
        private void picssort()
        {

            if (pics.Count <= 0) return;
            List<List<drawings>> sorts = new List<List<drawings>>();
            int si = pics.Count();
            int i, p, q;
            drawings temp;
            int tsi = 0;
            while (true)
            {
                sorts.Add(new List<drawings>());
                for (i = 0; i < 64; i++)
                {
                    if (tsi * 64 + i >= si)
                    {
                        break;
                    }
                    sorts[tsi].Add(pics[tsi * 64 + i]);

                }

                if (tsi * 64 + i >= si)
                {

                    break;
                }
                tsi++;


            }

            for (i = 0; i <= tsi; i++)
            {

                for (p = 1; p < sorts[i].Count; p++)
                {

                    temp = sorts[i][p];
                    if (sorts[i][p - 1].z < temp.z)
                    {
                        q = p;
                        do
                        {
                            sorts[i][q] = sorts[i][q - 1];
                            q--;
                        } while (q > 0 && sorts[i][q - 1].z < temp.z);
                        sorts[i][q] = temp;
                    }
                }

            }

            while (sorts.Count > 1)
            {
                p = 1;
                while (p < sorts.Count)
                {
                    i = 0; q = 0;
                    //マージ


                    while (i < sorts[p - 1].Count)
                    {
                        if (sorts[p - 1][i].z < sorts[p][q].z)
                        {
                            sorts[p - 1].Insert(i, sorts[p][q]);
                            i++;
                            q++;
                        }
                        else
                        {
                            i++;
                        }
                        if (q >= sorts[p].Count) break;
                    }
                    while (q < sorts[p].Count)
                    {
                        sorts[p - 1].Add(sorts[p][q]);
                        q++;
                    }
                    //戦後処理
                    sorts.RemoveAt(p);
                    if (sorts.Count() - 1 == p)
                    {
                    }
                    else p++;
                }


            }
            pics = sorts[0];
        }
        /// <summary>
        /// timソートで背景を整理する
        /// </summary>
        private void haikeisort()
        {
            if (haikeipics.Count <= 0) return;
            List<List<haikeidraws>> sorts = new List<List<haikeidraws>>();
            int si = haikeipics.Count();
            int i, p, q;
            int tsi = 0;
            haikeidraws temp;
            while (true)
            {
                sorts.Add(new List<haikeidraws>());
                for (i = 0; i < 64; i++)
                {
                    if (tsi * 64 + i >= si)
                    {
                        break;
                    }
                    sorts[tsi].Add(haikeipics[tsi * 64 + i]);

                }

                if (tsi * 64 + i >= si)
                {

                    break;
                }
                tsi++;


            }

            for (i = 0; i <= tsi; i++)
            {

                for (p = 1; p < sorts[i].Count; p++)
                {

                    temp = sorts[i][p];
                    if (sorts[i][p - 1].x * sorts[i][p - 1].y < temp.x * temp.y)
                    {
                        q = p;
                        do
                        {
                            sorts[i][q] = sorts[i][q - 1];
                            q--;
                        } while (q > 0 && sorts[i][q - 1].x * sorts[i][q - 1].y < temp.x * temp.y);
                        sorts[i][q] = temp;
                    }
                }

            }

            while (sorts.Count > 1)
            {
                p = 1;
                while (p < sorts.Count)
                {
                    i = 0; q = 0;
                    //マージ


                    while (i < sorts[p - 1].Count)
                    {
                        if (sorts[p - 1][i].x * sorts[p - 1][i].y < sorts[p][q].x * sorts[p][q].y)
                        {
                            sorts[p - 1].Insert(i, sorts[p][q]);
                            i++;
                            q++;
                        }
                        else
                        {
                            i++;
                        }
                        if (q >= sorts[p].Count) break;
                    }
                    while (q < sorts[p].Count)
                    {
                        sorts[p - 1].Add(sorts[p][q]);
                        q++;
                    }
                    //戦後処理
                    sorts.RemoveAt(p);
                    if (sorts.Count() - 1 == p)
                    {
                    }
                    else p++;
                }


            }
            haikeipics = sorts[0];
        }
        /// <summary>
        /// 指定ポイントに触れているピクチャーのリストを返す
        /// </summary>
        /// <param name="x">ポイントのx座標</param>
        /// <param name="y">ポイントのy座標</param>
        /// <param name="s">ピクチャーをどの図形で見るか</param>
        /// <returns>ピクチャーのリスト</returns>
        public List<picture> picturegets(float x,float y,Shapes.Shape s)
        {
            var res = new List<picture>();
            foreach (var a in pics) 
            {
                if (a.GetType() == typeof(picture) || a.GetType().IsSubclassOf(typeof(picture))) 
                {
                    var b = (picture)a;
                    s.w = b.w;
                    s.h = b.h;
                    s.rad += b.RAD;
                    s.setcxy(b.getcx(b.w / 2, b.h / 2), b.getcy(b.w / 2, b.h / 2), b.w / 2, b.h / 2);
                    if (s.onhani(x, y)) { res.Add(b); }
                    s.rad -= b.RAD;
                }
            }
            return res;
        }
        /// <summary>
        /// 表示マンをバイト列に変換する
        /// </summary>
        /// <param name="hyo">変換する表示マン</param>
        /// <returns>バイト列</returns>
        static public byte[] andbyte(hyojiman hyo)
        {
            if (hyo != null)
            {
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        if (ms != null && binaryFormatter != null)
                        {
                            binaryFormatter.Serialize(ms, hyo);
                          
                            return ms.ToArray();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("hyo to byte error" + e.ToString());
                }
            }
            return null;
        }
        /// <summary>
        /// バイト列を表示マンに変換する
        /// </summary>
        /// <param name="hyo">変換するバイト列</param>
        /// <returns>表示マン</returns>
        static public hyojiman andbyte(byte[] hyo)
        {
            if (hyo != null)
            {
              //  ulong sum = 0;
                //        foreach (var a in hyo) Console.Write(" + " + a + " + ");
                //    Console.WriteLine(hyo.Length+" kitaze: " +hyo[0] + " " + hyo[hyo.Length - 1]);
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    using (MemoryStream ms = new MemoryStream(hyo))
                    {
                        if (ms != null && binaryFormatter != null)
                        {
                            //  Console.WriteLine(ms.ToString() + " asf " + binaryFormatter.ToString());
                            var a = binaryFormatter.Deserialize(ms);
                            //   Console.WriteLine("uuh"+a.ToString());
                            if (a != null)
                                return (hyojiman)a;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("sajudfhasuipop" + e.ToString());
                }
            }
            return null;
        }

        /// <summary>
        /// 線を描く
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="X2"></param>
        /// <param name="Y2"></param>
        /// <param name="col"></param>
        /// <param name="hutosa"></param>
        public void drawLine(float X,float Y,float X2,float Y2,Color4 col, float hutosa ) 
        {
            var bruh = render.CreateSolidColorBrush(col);
            render.DrawLine(new PointF((X - camx) *Tbai, (Y - camy) * Tbai)
                    , new PointF((X2- camx) * Tbai, (Y2 - camy) * Tbai), bruh, hutosa * Tbai);
            bruh.Dispose();
        }
        /// <summary>
        /// 座標を引数表示マンの座標に変える
        /// </summary>
        /// <param name="hyo"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public float ZahyoconvertToX(hyojiman hyo, float x) 
        {
            x -= this.camx;
            return hyo.camx + (x * hyo.ww / this.ww);
        }
        /// <summary>
        /// 座標を引数表示マンの座標に変える
        /// </summary>
        /// <param name="hyo"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public float ZahyoconvertToY(hyojiman hyo, float y)
        {
            y -= this.camy;
            return hyo.camy + (y * hyo.wh / this.wh);
        }
        /// <summary>
        /// 表示マンを図形に変換する。
        /// 表示領域にキャラクターが含まれるか判定したいときに使ってね
        /// </summary>
        /// <returns></returns>
        public Shape ToShape() 
        {
            var rad = 0;
            return new Shapes.Rectangle(camx,camy,ww,wh,rad);
        }

    }
    /// <summary>
    /// 文字を書くためのクラス
    /// </summary>
    [Serializable]
    public class message : drawings
    {

        /// <summary>
        /// メッセージに記述できる最大の文字数
        /// </summary>
        static public int maxmoji = 5000;

        /// <summary>
        /// 何文字の幅か
        /// </summary>
        private int _nmoji;
        /// <summary>
        /// 文字数で表す文字列の中心
        /// </summary>
        private int _tyusin;
        /// <summary>
        /// 何文字の幅か
        /// </summary>
        public int nmoji { get { return _nmoji; }set { _nmoji = value;_nmoji = Math.Min(_nmoji, maxmoji); } }

        /// <summary>
        /// 文字数で表す文字列の中心
        /// </summary>
        public int tyusin { get { return _tyusin; } set { _tyusin = value; _tyusin = Math.Min(_tyusin, maxmoji); } }

        /// <summary>
        /// 文字のサイズ
        /// </summary>
        public float size;
        /// <summary>
        /// RGBでしかない
        /// </summary>
        public float R, G, B;
        /// <summary>
        /// 表示するテキスト
        /// </summary>
        public string text;
        /// <summary>
        /// 1文字表示するのに必要なクロック
        /// </summary>
        protected float speed;
        /// <summary>
        /// 持続時間
        /// </summary>
        protected float time;
        /// <summary>
        /// タイマーでしかない
        /// </summary>
        public float timer;
        /// <summary>
        /// 強調するか
        /// </summary>
        protected bool kyotyou;


      /// <summary>
      /// 表示マンに乗っているか
      /// </summary>
      /// <param name="hyojiman">その表示マン</param>
      /// <returns>乗っているか</returns>
        public bool onhyoji(hyojiman hyojiman) {return hyojiman.pics.Contains(this);  }
        /// <summary>
        /// 動きを追わらせる
        /// </summary>
        /// <param name="power">終わらせる時間のパワー</param>
        /// <returns>終わっていたらtrue</returns>
        public bool endmove(float power = 10000)
        {
            bool res = true;

            if (inlength > 0)
            {
                if (inlength + instart > timer) res = false;
            }
            if (outlength > 0)
            {
                if (outlength + outstart > timer) res = false;
            }
            if (text.Length * speed > timer) res = false;

            if (res == false) timer += power;

            return res;


        }
        /// <summary>
        /// 死ぬべきか
        /// </summary>
        public bool sinderu { get { if (time >= 0) return timer > time + text.Length * speed; return false; } }
        /// <summary>
        /// 合計の表示時間
        /// </summary>
        public float TIMEA { get { if (time >= 0) return time + text.Length * speed; return text.Length * speed; } }
        /// <summary>
        /// 文字のサイズ
        /// </summary>
        public float SIZE { get { return size; }set { size = value; } }
        /// <summary>
        /// バイト的なカウント
        /// </summary>
        protected float w = 1;
        /// <summary>
        /// 最大の横幅のバイト的な文字数のカウント
        /// </summary>
        public float maxbytecount { get { return w; } }
        /// <summary>
        /// 横幅の長さ
        /// </summary>
        public float W { get { return size / 2 * w; } }
        /// <summary>
        /// 最大であろう横幅の長さ
        /// </summary>
        public float WWW { get { return size / 2 * nmoji*2f; } }

        /// <summary>
        /// \nや文字幅オーバーとかで分割された列
        /// </summary>
        protected List<string> sts = new List<string>();
        /// <summary>
        /// その行のバイト的なカウントを取る
        /// </summary>
        /// <param name="gyo">n行目</param>
        /// <returns>カウント</returns>
        public float stsw(int gyo)
        {
            if (gyo >= 0 && gyo < sts.Count)
            {
                Encoding e = Encoding.GetEncoding("shift_jis");
                char[] chars = new char[sts[gyo].Count()];

                for (int t = 0; t < sts[gyo].Count(); t++)
                {
                    chars[t] = sts[gyo][t];
                }

                return e.GetByteCount(chars, 0, sts[gyo].Count()) * size / 2;
            }
            return 0;
        }
        
        /// <summary>
        /// 文字列の高さ
        /// </summary>
        public float H { get { return size * sts.Count; } }
        
        /// <summary>
        /// 下ぞろえをするときに移動する距離
        /// </summary>
        protected float sitazoroeman {get{ return size * (sts.Count - 1); } }
        /// <summary>
        /// 文字を書き始めるx
        /// </summary>
        public float mx { get { return x - size * 0.32f * tyusin; } }
        /// <summary>
        /// 文字を書き始めるy
        /// </summary>
        public float my { get { return y; } }
        /// <summary>
        /// 不透明度
        /// </summary>
        public float opa { get { return _opa;  }set { _opa = value;if (_opa < 0) _opa = 0; if (_opa > 1) _opa = 1; } }
        /// <summary>
        /// 角度
        /// </summary>
        public double rad { get { return _rad; } set { _rad = value; _rad = Math.Atan2(Math.Sin(_rad), Math.Cos(_rad)); } }
        float _opa = 1;
        double _rad = 0;
        /// <summary>
        /// 反転するか
        /// </summary>
        public bool mirror = false;
        /// <summary>
        /// なぜだか倍率
        /// </summary>
        public float baix=1, baiy = 1;
        /// <summary>
        /// 下でそろえる
        /// </summary>
        public bool sita = false;
        /// <summary>
        /// メッセージをつくる
        /// </summary>
        /// <param name="x">開始x座標</param>
        /// <param name="y">開始y座標</param>
        /// <param name="ookisa">フォントの大きさ</param>
        /// <param name="mojisuu">文字数の幅(日本語はちょっとデカい)</param>
        /// <param name="tyusindoko">中心の文字数</param>
        /// <param name="hyojispeed">文字が表示されるに当たって必要なクロック数</param>
        /// <param name="hyojitime">文字が完全に表示されたのちに表示される時間(-1)で無制限</param>
        /// <param name="textt">文字の内容</param>
        /// <param name="RR">R</param>
        /// <param name="GG">G</param>
        /// <param name="BB">B</param>
        /// <param name="kyotyousuru">強調するか</param>
        /// <param name="z">z</param>
        /// <param name="sitazoroe">下でそろえるか</param>
        public message(float x, float y, float ookisa, int mojisuu, int tyusindoko, float hyojispeed, float hyojitime, string textt, float RR = 0, float GG = 0, float BB = 0, bool kyotyousuru = true,float z=1000000000,bool sitazoroe=false):base(x,y,z)
        {
            sita = sitazoroe;
            
            size = ookisa;
            
            nmoji = mojisuu;
            tyusin = tyusindoko;

            speed = hyojispeed;
            time = hyojitime;
            R = RR;
            G = GG;
            B = BB;
            text = textt;
            timer = 0;
            kyotyou = kyotyousuru;
            textoraa();


        }
       /// <summary>
       /// コピーのコンストラクタ
       /// </summary>
       /// <param name="m"></param>
        public message(message m):base(m)
        {
          
            size = m.size;
            nmoji = m.nmoji;
            tyusin = m.tyusin;
            speed = m.speed;
            time = m.time;
            R = m.R;
            G = m.G;
            B = m.B;
            text = m.text;
            timer = 0;
            sita = m.sita;
            textoraa();
        }
        /// <summary>
        /// フェードインアウトの最大の不透明度を設定する
        /// </summary>
        /// <param name="opa"></param>
        public void setmaxopa(float opa) 
        {
            maxopa = opa;
            if (maxopa < 0) maxopa = 0;

            if (maxopa > 1) maxopa = 1;
        }
        /// <summary>
        /// フェードアウトを設定する。ちなみに自動でhyojimanから消える。
        /// </summary>
        /// <param name="start">開始時間</param>
        /// <param name="length">長さ</param>
        public void setfadeout(float start, float length )
        {
            outstart = start;
            outlength = length;
        }
        float outstart = -1, outlength = -1, inlength = -1,instart=-1;
        private float maxopa = 1;
        /// <summary>
        /// フェードインを設定する
        /// </summary>
        /// <param name="length">長さ</param>
        /// <param name="start">開始時間</param>
        public void setfadein(float length,float start=0)
        {
            inlength = length;
            instart = start;
        }
        /// <summary>
        /// 一時的に死なないようにする
        /// </summary>
        public void sinanu()
        {
            if (timer > text.Length * speed) timer = text.Length * speed;
        }
        /// <summary>
        /// テキストを表示しきるのにかかる時間。
        /// </summary>
        public float texthyojitime { get {
                return
                    text.Length * speed;
                
            } }
        /// <summary>
        /// テキストを作り出す
        /// </summary>
        /// <returns>生成された奴</returns>
        public string textoraa()
        {
            if (nmoji < 0) return "";
            int kazu = text.Length;
            if (speed > 0)
            {
                kazu = (int)(timer / speed);
            }
            if (kazu > text.Length) kazu = text.Length;
            sts.Clear();
            float cou = 0;
            string temp = "";
            bool auted = false;
            for (int i = 0; i < kazu; i++)
            {
                bool gooon = false;

                if (text[i] == '\n'&&!auted)
                {
                    sts.Add(temp);
                    cou = 0;
                    temp = "";
                }
                else
                {
                    auted = false;
                    Encoding e = Encoding.GetEncoding("shift_jis");
                  
                    char[] chars = new char[1];
                    chars[0] = text[i];
                    var temmp = e.GetByteCount(chars, 0, 1);
                    if (temmp > 1)
                    {
                        if (cou + temmp * 0.9f <= nmoji)
                        {
                            cou += temmp * 0.9f;
                            temp += text[i];
                        }
                        else 
                        {
                            gooon = true;
                            i--;
                        }
                    }
                    else
                    {
                        if (cou + temmp  <= nmoji)
                        {
                            cou += temmp ;
                            temp += text[i];
                        }
                        else
                        {
                            gooon = true;
                            i--;
                        }
                    }

                   
                    //cou += 1;
                }
                if (cou > nmoji||gooon)
                {
                    auted = true;
                    sts.Add(temp);
                    cou = 0;
                    temp = "";
                }
            }
            sts.Add(temp);
            float hi = (float)tyusin / (float)nmoji;
            w = 1;
            for (int i = 0; i < sts.Count(); i++)
            {
                if (sts[i] != "")
                {
                    cou = 0;
                    Encoding e = Encoding.GetEncoding("shift_jis");
                    char[] chars = new char[sts[i].Count()];

                    for (int t = 0; t < sts[i].Count(); t++)
                    {
                        chars[t] = sts[i][t];
                    }

                    cou += e.GetByteCount(chars, 0, sts[i].Count());
                    if (w < cou) w = cou;
                    float tya = Math.Max(((nmoji - cou) * (hi)),0);
                    // Console.WriteLine(tya + " tyaaa "+ nmoji +" tamukenkayo "+cou+" "+ hi+" "+tyusin);
                    if (sts[i].Count() > 0)
                    {
                        var tempn = new string(' ',(int)Math.Round(tya));

                        sts[i] = tempn + sts[i];
                    }

                }
            }
            string res = "";


            for (int i = 0; i < sts.Count(); i++)
            {
                // Console.WriteLine(sts[i]);
                res += sts[i] + Environment.NewLine;
                if (!(i == sts.Count() - 1 && sts[i] == "")) { }

            }
            //h -= size / 2;

            return res;
        }
        public override bool draw(hyojiman hyo,float cl,bool draw)
        {

            byoga(hyo,cl,draw);
            return true;
        }
        /// <summary>
        /// タイマーを進めるだけ
        /// </summary>
        /// <param name="hyo"></param>
        /// <param name="cl"></param>
        public void susumukun(hyojiman hyo,float cl = 1)
        {
            timer += cl;
            if (sinderu) remove(hyo);
        }
        /// <summary>
        /// 文字を描画する
        /// </summary>
        /// <param name="hyo"></param>
        /// <param name="cl"></param>
        /// <param name="draw"></param>
        public void byoga(hyojiman hyo, float cl, bool draw)
        {

            var render = hyo.render;
            float bairitu = hyo.Tbai;
            timer += cl;


            Vortice.DirectWrite.FontStyle style;

            if (kyotyou) style = Vortice.DirectWrite.FontStyle.Oblique;
            else style = Vortice.DirectWrite.FontStyle.Normal;

            float si = size * bairitu;
            if (si <= 1) si = 1;
         

            if (inlength > 0)
            {

                if (instart < timer)
                {
                    _opa = maxopa * (timer - instart) / inlength;
                }
                else
                {
                    _opa = 0;
                }
                //  Console.WriteLine(opa + " :dwad: " + timer + " :dawf: " + instart);
            }
            if (outstart >= 0 && outlength > 0)
            {
                if (timer > outstart + outlength)
                {
                    _opa = 0;
                    remove(hyo);
                    //改善点だけどまあいいケルジャクソン
                }
                else if (outstart <= timer)
                {
                    _opa = maxopa - maxopa * (timer - outstart) / outlength;
                }
            }
            if (draw)
            {
                //   Console.WriteLine(opa + " :dwad: " + timer + " :dawf: " + instart);
                {
                    var a = Matrix3x2.CreateRotation((float)rad, new Vector2((x * bairitu - hyo.camx * bairitu), (y * bairitu - hyo.camy * bairitu)));
                    if (mirror)
                    {
                        a = Matrix3x2.Multiply(new Matrix3x2(-1, 0, 0, 1, 0, 0), a);
                        a = Matrix3x2.Multiply(Matrix3x2.CreateTranslation(-(x - hyo.camx) * 2 * bairitu, 0), a);


                    }
                    if (baix != 0 && baiy != 0)
                    {
                        a = Matrix3x2.Multiply(Matrix3x2.CreateTranslation(new Vector2((mx - hyo.camx) * bairitu * (1 - baix), (my - hyo.camy) * bairitu * (1 - baiy)
                         )), a);
                        a = Matrix3x2.Multiply(Matrix3x2.CreateScale(baix, baiy), a);

                    }

                    hyo.render.Transform = a;


                }
            
                string text = textoraa();
                float yyy = 0;
                if (sita) yyy = sitazoroeman;
                if (this.opa > 0 &&
               Math.Abs((x) - (hyo.ww / 2 + hyo.camx)) * 2 <= hyo.ww + Math.Abs(WWW * Math.Cos(rad)) + Math.Abs(H * Math.Sin(rad)) &&
               Math.Abs((y) - (hyo.wh / 2 + hyo.camy)) * 2 <= hyo.wh + Math.Abs(WWW * Math.Sin(rad)) + Math.Abs(H * Math.Cos(rad))
               )
                {
                    var slb = render.CreateSolidColorBrush(new Color4(R, G, B, opa));
                    var fa = Vortice.DirectWrite.DWrite.DWriteCreateFactory<IDWriteFactory>();
                    var fom = fa.CreateTextFormat("MS UI Gothic", FontWeight.Light, style, si);
                    render.DrawText(text, fom,
                             new RawRectF((mx - hyo.camx) * bairitu, (my - hyo.camy - yyy) * bairitu,
                             (mx + WWW  - hyo.camx) * bairitu, (my + H - hyo.camy - yyy) * bairitu), slb);

                    fa.Dispose();
                    fom.Dispose();
                    slb.Dispose();
                }
               
            }
            if (sinderu) remove(hyo);
        }

        /// <summary>
        /// 縁取りを適用したメッセージ群を作り出す
        /// </summary>
        /// <param name="sabun">どのぐらいずらすか</param>
        /// <param name="hyo">追加先の表示マン</param>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="ookisam">文字の大きさ</param>
        /// <param name="mojisuu">文字数</param>
        /// <param name="tyusindoko">中心</param>
        /// <param name="hyojispeed">1文字表示に必要な時間</param>
        /// <param name="hyojitime">表示する時間</param>
        /// <param name="textt">テキスト</param>
        /// <param name="RR">R</param>
        /// <param name="GG">G</param>
        /// <param name="BB">B</param>
        /// <param name="kyoutyousuru">強調するか</param>
        /// <param name="z">z</param>
        /// <param name="sitazoroe">下でそろえるか</param>
        /// <param name="R2">後ろのR,-1で自動</param>
        /// <param name="G2">後ろのG,-1で自動</param>
        /// <param name="B2">後ろのB,-1で自動</param>
        /// <param name="dz">後ろの前とのzの差</param>
        /// <param name="kazu">何個で縁取りするか。0~4</param>
        /// <returns></returns>
        public static List<message> hutidorin(float sabun , float x, float y, float ookisam, int mojisuu, int tyusindoko,float  hyojispeed, float hyojitime, string textt,float RR=0
            ,float  GG=0,float BB=0,bool kyoutyousuru=true,float z=(float)1E+09, hyojiman hyo=null, bool sitazoroe=false,float R2=-1, float G2 = -1, float B2 = -1,float dz=-1,int kazu=1) 
        {
            var res = new List<message>();
            float R = 1, G = 1, B = 1;
            if (RR > 0.5) R = 0;
            if (GG > 0.5) G = 0;
            if (BB > 0.5) B = 0;

            if (R2 != -1) R = R2;
            if (G2 != -1) G = G2;
            if (B2 != -1) B = B2;

             res.Add(new message(x, y, ookisam, mojisuu, tyusindoko, hyojispeed, hyojitime, textt, RR, GG, BB, kyoutyousuru, z, sitazoroe));
            switch (kazu) 
            {
                case 1:
                    res.Add(new message(x + sabun, y+sabun, ookisam, mojisuu, tyusindoko, hyojispeed, hyojitime, textt, R, G, B, kyoutyousuru, z + dz, sitazoroe));

                    break;
                case 2:
                    res.Add(new message(x + sabun, y + sabun, ookisam, mojisuu, tyusindoko, hyojispeed, hyojitime, textt, R, G, B, kyoutyousuru, z + dz, sitazoroe));
                    res.Add(new message(x - sabun, y - sabun, ookisam, mojisuu, tyusindoko, hyojispeed, hyojitime, textt, R, G, B, kyoutyousuru, z + dz, sitazoroe));
                    break;
                case 3:
                    res.Add(new message(x + sabun, y + sabun, ookisam, mojisuu, tyusindoko, hyojispeed, hyojitime, textt, R, G, B, kyoutyousuru, z + dz, sitazoroe));
                    res.Add(new message(x - sabun, y , ookisam, mojisuu, tyusindoko, hyojispeed, hyojitime, textt, R, G, B, kyoutyousuru, z + dz, sitazoroe));
                    res.Add(new message(x + sabun, y - sabun, ookisam, mojisuu, tyusindoko, hyojispeed, hyojitime, textt, R, G, B, kyoutyousuru, z + dz, sitazoroe));

                    break;
                case 4:
                    res.Add(new message(x + sabun, y , ookisam, mojisuu, tyusindoko, hyojispeed, hyojitime, textt, R, G, B, kyoutyousuru, z + dz, sitazoroe));
                    res.Add(new message(x - sabun, y, ookisam, mojisuu, tyusindoko, hyojispeed, hyojitime, textt, R, G, B, kyoutyousuru, z + dz, sitazoroe));
                    res.Add(new message(x  , y - sabun, ookisam, mojisuu, tyusindoko, hyojispeed, hyojitime, textt, R, G, B, kyoutyousuru, z + dz, sitazoroe));
                    res.Add(new message(x  , y + sabun, ookisam, mojisuu, tyusindoko, hyojispeed, hyojitime, textt, R, G, B, kyoutyousuru, z + dz, sitazoroe));
                    break;
            }
            if (hyo!=null) 
            {
                foreach(var a in res)
                hyo.addpicture(a);
            }
            return res;
        }
        /// <summary>
        /// コピーするためのメソッド
        /// </summary>
        /// <param name="m">コピー元</param>
        public void copy(message m)
        {
            base.copy(m);
            size = m.size;
            nmoji = m.nmoji;
            tyusin = m.tyusin;
            speed = m.speed;
            time = m.time;
            R = m.R;
            G = m.G;
            B = m.B;
            text = m.text;
            timer = 0;
            sita = m.sita;
        }
        public override drawings clone()
        {
            var m = new message(this);
            return m;
        }
    }


}
