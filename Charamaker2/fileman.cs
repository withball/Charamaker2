using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct2D1;
using Vortice.DXGI;
using Vortice.Mathematics;
using Vortice.XAudio2;
using Vortice;
using Vortice.Multimedia;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Charamaker2.Character;
using Vortice.DCommon;
using Microsoft.CodeAnalysis.Scripting;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Xml.Linq;
using System.Drawing;
using Vortice.WIC;
using System.Security.Policy;
using System.Drawing.Imaging;
using SharpGen.Runtime;

namespace Charamaker2
{
    /// <summary>
    /// レンダーターゲットを管理するためのクラス
    /// </summary>
    class rendercontainer
    {
        public string name;
        public ID2D1RenderTarget moto;
        public ID2D1RenderTarget render;
        /// <summary>
        /// ウィンドウに根差したやつか
        /// </summary>
        public bool isWindow { get { return render.GetType() == typeof(ID2D1HwndRenderTarget); } }
        /// <summary>
        /// ビットマップに根差したやつか
        /// </summary>
        public bool isBitmap { get { return render.GetType() == typeof(ID2D1BitmapRenderTarget); } }
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="moto"></param>
        /// <param name="render"></param>
        /// <param name="name"></param>
        public rendercontainer(ID2D1RenderTarget moto,ID2D1RenderTarget render,string name) 
        {
            this.moto = moto;
            this.name = name;
            this.render = render;
        }
        public bool ok(ID2D1RenderTarget moto,string name, int w, int h, bool window)
        {
          
            return this.moto == moto && name == this.name
                && (w == (int)this.render.PixelSize.Width || w <= 0)
                && (h == (int)this.render.PixelSize.Height||h<=0)
                && isWindow == window;
        }

    }
    class renderList
    {
        List<rendercontainer> lis = new List<rendercontainer>();
        /// <summary>
        /// ビットマップをもらう
        /// </summary>
        /// <param name="moto">製造元となったレンダー</param>
        /// <param name="name"></param>
        /// <param name="w">-1ならフリー</param>
        /// <param name="h"></param>
        /// <param name="window">trueならウィンドウfalseならbitmap</param>
        /// <returns>ないならnull</returns>
        public ID2D1RenderTarget getRender(ID2D1RenderTarget moto,string name, int w=-1, int h=-1, bool window=true) 
        {
            foreach (var a in lis) 
            {
                if (a.ok(moto, name, w, h, window))
                {
                    return a.render;
                }
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moto">製造元となったレンダー</param>
        /// <param name="render"></param>
        /// <param name="name"></param>
        public void addRender(ID2D1RenderTarget moto, ID2D1RenderTarget render, string name)
        {
            lis.Add(new rendercontainer(moto,render, name));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="render">製造元から根こそぎ持ってくよ</param>
        public void removeRender(ID2D1RenderTarget render)
        {
            for (int i = lis.Count - 1; i >= 0; i--) 
            {
                if (lis[i].render == render) 
                {
                    lis.RemoveAt(i);
                }
            }
        }

    }


    /// <summary>
    /// ファイルの入出力・このシステムのセットアップを扱うクラス。
    /// setingupingでセットアップは完了する。
    /// character,motion,tex,oto,oto\bgmというフォルダがexeファイルと同じディレクトリに必要。
    /// movie機能を使うならmovieというフォルダがexeと同じところに必要。
    /// </summary>
    static public class fileman
    {
        /// <summary>
        /// なんとなく保存してるファクトリ
        /// </summary>
        static ID2D1Factory fac;
        /// <summary>
        /// もちろん保存しないといけないレンダーターゲット
        /// </summary>
        static List<ID2D1RenderTarget> rendertargets=new List<ID2D1RenderTarget>();
        /// <summary>
        /// ガシーツ
        /// </summary>
        public static List<float> gasyu =new List<float>();
        /// <summary>
        /// セットアップされたフォーム
        /// </summary>
        static List<ContainerControl> _CC=new List<ContainerControl>();

        static renderList renderlist = new renderList();

        /// <summary>
        /// 何もないことを示す魔法の言葉
        /// </summary>
        public const string nothing = "nothing";
        /// <summary>
        /// 画面のサイズを設定し、hyojimanを生成可能にする。
        /// </summary>
        /// <param name="bairitu">画質の倍率</param>
        /// <param name="Hand">ハンドラー</param>
        /// <param name="wi">画面の幅</param>
        /// <param name="hei">画面の高さ</param>
        static private void resizen(float bairitu, IntPtr Hand, float wi, float hei)
        {
            D2D1.D2D1CreateFactory<ID2D1Factory>(FactoryType.SingleThreaded, out fac);
            var renpro = new RenderTargetProperties();
            var hrenpro = new HwndRenderTargetProperties();
            hrenpro.Hwnd = Hand;
            System.Drawing.Size si = new System.Drawing.Size((int)(wi * bairitu), (int)(hei * bairitu));
            hrenpro.PixelSize = si;

            int i = rendertargets.Count;
            rendertargets.Add(fac.CreateHwndRenderTarget(renpro, hrenpro));
            gasyu.Add(bairitu);
            renderlist.addRender(rendertargets[i], rendertargets[i], i.ToString());

        }
        /// <summary>
        /// hyojimanを取得する
        /// </summary>
        /// <param name="numberoftarget">何番目の画面か</param>
        /// <returns>新しいhyojiman</returns>
        static public hyojiman makehyojiman(int numberoftarget=0)
        {
            var aa = new hyojiman(rendertargets[numberoftarget], gasyu[numberoftarget]);
            return aa;
        }
        /// <summary>
        ///     bmprenderをがっちゃする
        /// </summary>
        /// <param name="name">""でウィンドウの。あとはbitmapに</param>
        /// <param name="w">幅(-1で自動)</param>
        /// <param name="h">高さ(-1で自動)</param>
        /// <param name="numberoftarget">何番目の画面か</param>
        /// <returns></returns>
        static public ID2D1RenderTarget getBitmapRender(string name = "", int w = -1, int h = -1, int numberoftarget = 0) 
        {
            var render = rendertargets[numberoftarget];
            var gasitu = gasyu[numberoftarget];
            var size = render.PixelSize;
            if (w > 0) size.Width = (int)(w * gasitu);
            if (h > 0) size.Height = (int)(h * gasitu);

            ID2D1RenderTarget rend;
            if (name == "")
            {
                rend = render;
            }
            else
            {
                // Console.WriteLine(name+" Seeking...");
                rend = renderlist.getRender(render,name, w, h, false);
                if (rend == null)
                {
                    var fom = new Vortice.DCommon.PixelFormat();
                    Console.WriteLine(name + "render maked!");
                    rend = render.CreateCompatibleRenderTarget(size,size
                        ,fom
                        , CompatibleRenderTargetOptions.None);
                    renderlist.addRender(render,rend, name);
                    
                }
            }
           
            return rend;
        }
        /// <summary>
        ///     bmprenderをがっちゃする
        /// </summary>
        /// <param name="name">""でウィンドウのあとはbitmapに</param>
        /// <param name="w">幅(-1で自動)</param>
        /// <param name="h">高さ(-1で自動)</param>
        /// <param name="number">番号の代わりに直接レンダー</param>
        /// <returns></returns>
        static public ID2D1RenderTarget getBitmapRender(ID2D1RenderTarget number,string name = "", int w = -1, int h = -1)
        {
            var numberoftarget = rendertargets.IndexOf(number);
            numberoftarget = Math.Max(0, numberoftarget);
            return getBitmapRender(name, w, h, numberoftarget);
        }


        /// <summary>
        /// ピクチャーとして扱えるPhyojimanを取得する
        /// </summary>
        /// <param name="tex">ピクチャーのテクスチャ</param>
        /// <param name="name">""でウィンドウのあとはbitmapに</param>
        /// <param name="w">幅(-1で自動)</param>
        /// <param name="h">高さ(-1で自動)</param>
        /// <param name="numberoftarget">何番目の画面か</param>
        /// <param name="auto">Phuojimanを自動的にhyojiをするか</param>
        /// <returns>新しいhyojiman</returns>
        static public Phyojiman makePhyojiman(string tex=fileman.nothing,bool auto=true, string name = "",int w=-1,int h=-1,int numberoftarget = 0)
        {

            //  Console.WriteLine("maked P!");
            var rend = getBitmapRender(name, w, h, numberoftarget);

            var hyo = new hyojiman(rend, gasyu[numberoftarget],true);
            
            var res= new Phyojiman(picture.onetexpic(tex, hyo.ww), hyo, auto);
            
            return res;
        }
        /// <summary>
        /// セットアップをする。
        /// 画像の表示、音の再生が使用可能になる。
        /// </summary>
        /// <param name="f">素となるフォームとかユーザーコントロール</param>
        /// <param name="risouw">理想のクライアントサイズ</param>
        /// <param name="risouh">理想のクライアントサイズ</param>
        /// <param name="bai">画質の倍率</param>
        static public void setinguping(ContainerControl f,float risouw,float risouh, float bai = 1)
        {
            Console.WriteLine("fileman setup go");
            _CC.Add(f);
            resizen(bai, f.Handle, risouw, risouh);
            fileman.resetfileman(f.Handle);
            Console.WriteLine("fileman setup ok");
            
        }
        /// <summary>
        /// 新しいウィンドウに対して画面を操れるようにする。
        /// </summary>
        /// <param name="f">素となるフォームとかユーザーコントロール</param>
        /// <param name="bai">画質の倍率</param>
        static public void setNewWindow(ContainerControl f, float bai) 
        {
            _CC.Add(f);
            resizen(bai, f.Handle, f.ClientSize.Width, f.ClientSize.Height);
        }
        /// <summary>
        /// ウィンドウを消したいな
        /// </summary>
        /// <param name="f">消したいウィンドウ</param>
        static public void deleteOldWindow(ContainerControl f)
        {
            var i = _CC.IndexOf(f);
            if (i != -1) 
            {
                renderlist.removeRender(rendertargets[i]);
                _CC.RemoveAt(i);
                gasyu.RemoveAt(i);
                rendertargets.RemoveAt(i);
                
            }
        }
        /// <summary>
        /// 読み込んだテクスチャーを保存しとく
        /// </summary>
        public static Dictionary<string, ID2D1Bitmap> texs = new Dictionary<string, ID2D1Bitmap>();
        /// <summary>
        /// 読み込んだモーションを保存しとく
        /// </summary>
        public static Dictionary<string, motionsaveman> motions = new Dictionary<string, motionsaveman>();
        /// <summary>
        /// 読み込んだキャラクターを保存しとく
        /// </summary>
        public static Dictionary<string, character> characters = new Dictionary<string, character>();
        /// <summary>
        /// 読み込まれたキャラクターをアンロード。表情を追加したときとかに！
        /// </summary>
        static public void resetcharacters()
        {
            characters.Clear();
        }
        /// <summary>
        /// 読み込まれたモーションをリセット
        /// </summary>
        static public void resetmotions()
        {
            motions.Clear();
        }
        /// <summary>
        /// 読み込まれたテクスチャーをリセット
        /// </summary>
        static public void resettextures()
        {
            texs.Clear();
            foreach (var a in basebitnames)
            {
                settextobit(a.Key, a.Value);
            }
        }

        /// <summary>
        /// 読み込んだ音を保存しとく
        /// </summary>
        public static Dictionary<string, otoman> otos = new Dictionary<string, otoman>();
        /// <summary>
        /// 今ならしている音を保存しとく。ちゃんとメモリ開放できるように。
        /// </summary>
       static List<otoman> oton = new List<otoman>();
        /// <summary>
        /// 乱数ロット
        /// </summary>
        static public Random r = new Random();




        /// <summary>
        /// 画面のスクリーンショットを取る。effectcharaがぶれるバグあり！
        /// </summary>
        /// <param name="h">保存する表示マン</param>
        /// <param name="format">保存フォーマット</param>
        /// <param name="addname">追加で付ける名前</param>
        static public void screenShot(hyojiman h, string format = "bmp",string addname="")
        {
            /*
            var bt = h.render.CreateCompatibleRenderTarget(size, CompatibleRenderTargetOptions.None);
            //  var bt = new ID2D1BitmapRenderTarget(bm.NativePointer);


            h.hyoji2(bt, 0, true, true, false, false, true);
            */

            string dir = @".\shots\";

            if (Directory.Exists(dir))
            {
            }
            else
            {
                Directory.CreateDirectory(dir);
            }

            var Size = h.TrueSize;
            //サイズの違いでバグる可能性あり！！！丸めてどうにかしたが、画質の値によっては今後もやばいぞ！
            string name = addname+DateTime.Now.ToString() + "." + format;
            var bt = (ID2D1BitmapRenderTarget)getBitmapRender("ScreenShot",Size.Width
                , Size.Height);
            h.ikiutusi(bt);
            var pxs = GetPixels(bt.Bitmap, bt);

           var size = new Size(pxs[0].Count,pxs.Count);

            var save = new Bitmap(size.Width, size.Height);

            //Console.WriteLine(size.Width + " a:ga:la " + size.Height);
          //  Console.WriteLine(bt.Size.Width + " a:ga:la " + bt.Size.Height);
            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    //  Console.WriteLine(pxs[y][x].A+" al:skfa :");
                    save.SetPixel(x, y, System.Drawing.Color.FromArgb((int)(pxs[y][x].A)
                        , (int)(pxs[y][x].R), (int)(pxs[y][x].G), (int)(pxs[y][x].B))
                        );
                }
            }
            name = name.Replace("/", "_");
            name = name.Replace(" ", "_");
            name = name.Replace(":", "_");
            Console.WriteLine(dir + name);
            save.Save(dir + name);
          

        }
        /// <summary>
        /// texsに色のついたビットを追加する。
        /// </summary>
        /// <param name="bitname">bitname+"bit"</param>
        /// <param name="color">色</param>
        static private void settextobit(string bitname, System.Drawing.Color color)
        {
            using (var tempStream = new DataStream(1, true, true))
            {
                int rgba = color.R | (color.G << 8) | (color.B << 16) | (color.A << 24);
                tempStream.Write(rgba);
                var bitmapProperties = new BitmapProperties(new Vortice.DCommon.PixelFormat(Vortice.DXGI.Format.R8G8B8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied));
                regestBmp(bitname + "bit.bmp", rendertargets[0].CreateBitmap(new System.Drawing.Size(1, 1), tempStream.BasePointer, sizeof(int), bitmapProperties));
               
            }

        }

        /// <summary>
        /// ビットマップを読み込むときに使うクロマキーの色
        /// </summary>
        static public Color3 clmcol = new Color3(0, 254, 254);
        /// <summary>
        /// テクスチャーのサイズを取得する
        /// </summary>
        /// <param name="file">そのテクスチャー</param>
        /// <returns></returns>
        static public System.Drawing.Size gettexsize(string file)
        {

            var a = ldtex(file);
            if (a != null)
            {
                return a.PixelSize;
            }
            return new System.Drawing.Size(1,1);
        }
        /// <summary>
        /// テクスチャの色を取得する。透過色はA=0;
        ///  毎回ロードするし、多分重い。redBitとかは無理というかする必要なくね
        /// </summary>
        /// <param name="file">ファイル</param>
        /// <returns></returns>
        static public List<List<System.Drawing.Color>> GetPixels(string file) 
        {
            var res = new List<List<System.Drawing.Color>>();
            file = dotset(file);
            file = slashformat(file);

            {
                if (file != nothing + ".bmp")
                {
                    Console.WriteLine(file + "PixelLoad!");
                }
                string fi = @".\tex\" + file;
                // System.Drawing.Imageを使ってファイルから画像を読み込む
                if (System.IO.File.Exists(fi))
                {
                    using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(fi))
                    {
                        // BGRA から RGBA 形式へ変換する
                        // 1行のデータサイズを算出
                        int stride = bitmap.Width * sizeof(int);
                        using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                        {
                            // 読み込み元のBitmapをロックする
                            var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                            var bitmapData = bitmap.LockBits(sourceArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                            // 変換処理
                            for (int y = 0; y < bitmap.Height; y++)
                            {
                                res.Add(new List<System.Drawing.Color>());
                                int offset = bitmapData.Stride * y;
                                for (int x = 0; x < bitmap.Width; x++)
                                {

                                    // 1byteずつデータを読み込む
                                    byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                                    byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                                    byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                                    byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                                    //Console.WriteLine(B + " " + G + " " + R + " " + A);

                                    if (R == clmcol.R && G == clmcol.G && B == clmcol.B) res[y].Add(System.Drawing.Color.FromArgb(0,R, G, B));
                                    else res[y].Add(System.Drawing.Color.FromArgb(R, G, B));


                                }
                            }
                            // 読み込み元のBitmapのロックを解除する
                            bitmap.UnlockBits(bitmapData);
                            tempStream.Position = 0;

                            // 変換したデータからBitmapを生成して返す

                        }
                    }
                }
               
            }
            // Console.WriteLine(texs.Count() + "texcount");
            return res;

        }



       
        /// <summary>
        /// bitmapテクスチャーを読み込む。既に読み込んでいた場合は読み込まずに返す。
        /// .bmpはつけてもつけなくてもいい
        /// </summary>
        /// <param name="file">.\tex\に続くファイルパス</param>
        /// <param name="reset">強制的に再読み込みする</param>
        /// <returns>clmcolを透明にしたビットマップ</returns>
        static public ID2D1Bitmap ldtex(string file, bool reset = false)
        {
            file = slashformat(file);
            file = dotset(file);
            if (!texs.ContainsKey(file) || reset)
            {
                string fi = @".\tex\" + file;
                if (!File.Exists(fi))
                {
                    Console.WriteLine("texture " + fi + " not exists");
                    regestBmp(file, null); 
                    return null;
                }

                // System.Drawing.Imageを使ってファイルから画像を読み込む
                if (System.IO.File.Exists(fi))
                {
                    using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(fi))
                    {
                        // BGRA から RGBA 形式へ変換する
                        // 1行のデータサイズを算出
                        int stride = bitmap.Width * sizeof(int);
                        using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                        {
                            // 読み込み元のBitmapをロックする
                            var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                            var bitmapData = bitmap.LockBits(sourceArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                            // 変換処理
                            for (int y = 0; y < bitmap.Height; y++)
                            {
                                int offset = bitmapData.Stride * y;
                                for (int x = 0; x < bitmap.Width; x++)
                                {

                                    // 1byteずつデータを読み込む
                                    byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                                    byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                                    byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                                    byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                                    //Console.WriteLine(B + " " + G + " " + R + " " + A);
                                    byte a = 0;
                                    int gaba = a | (a << 8) | (a << 16) | (a << 24);
                                    //tempStream.Write(gaba);


                                    int rgba = R | (G << 8) | (B << 16) | (A << 24);
                                    if (R == clmcol.R && G == clmcol.G && B == clmcol.B) tempStream.Write(gaba);
                                    else
                                        tempStream.Write(rgba);


                                }
                            }
                            // 読み込み元のBitmapのロックを解除する
                            bitmap.UnlockBits(bitmapData);
                            tempStream.Position = 0;

                            // 変換したデータからBitmapを生成して返す

                            var size = new System.Drawing.Size(bitmap.Width, bitmap.Height);
                            var bitmapProperties = new BitmapProperties(new Vortice.DCommon.PixelFormat(Vortice.DXGI.Format.R8G8B8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied));

                            var result = rendertargets[0].CreateBitmap(size, tempStream.BasePointer, stride, bitmapProperties);
                            regestBmp(file,result);
                           
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            // Console.WriteLine(texs.Count() + "texcount");
            return texs[file];
        }
        /// <summary>
        /// スラッシュをバックスラッシュに変換する。
        /// だってモーションとかの登録を入力パッスにしてるからさ、
        /// \/が混じってると二回ロードしちゃうんだよな
        /// </summary>
        /// <returns></returns>
        static string slashformat(string path) 
        {
            return path.Replace(@"/",@"\");
        }
        /// <summary>
        /// バックスラッシュをスラッシュに変換する。
        /// これでfolder\nantokaが勘違いされなくなる
        /// </summary>
        /// <returns></returns>
        public static string nonbackslash(string path)
        {
            return path.Replace(@"\", @"/");
        }
        /// <summary>
        /// .bmpをつける。.pngならそのまま
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string dotset(string name) 
        {
            var aa = Path.GetExtension(name);

            if (aa != ".bmp"&&aa!=".png" && aa != ".jpg") name += ".bmp";
            return name;
        }

        /// <summary>
        /// bmpをtexsに登録する
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bmp"></param>
        /// <param name="dispouwagaki">同じ名前のbmpを登録するときdisposeするか。Renderからのbmpを登録してる場所にはfalseじゃないとだめ</param>
        static public void regestBmp(string name,ID2D1Bitmap bmp,bool dispouwagaki=true) 
        {

            name = slashformat(name);
            name = dotset(name);

            if (texs.ContainsKey(name))
            {

                if(dispouwagaki)texs[name]?.Dispose();
                texs[name] = bmp;
            }
            else
            {
                //if (bmp != null) Console.WriteLine(bmp.PixelSize.ToString());
                Console.WriteLine(name + " load sitao!");
                texs.Add(name, bmp);
            }
        }
       
        static List<List<Color4>> GetPixels(ID2D1Bitmap image, ID2D1RenderTarget renderTarget)
        {
            var deviceContext2d = renderTarget.QueryInterface<ID2D1DeviceContext>();
            var bitmapProperties = new BitmapProperties1();
            bitmapProperties.BitmapOptions = BitmapOptions.CannotDraw | BitmapOptions.CpuRead;
            bitmapProperties.PixelFormat = image.PixelFormat;
            
            var bitmap1 = deviceContext2d.CreateBitmap( image.PixelSize,image.NativePointer
                ,sizeof(int),ref bitmapProperties);

            
            bitmap1.CopyFromBitmap(renderTarget.CreateSharedBitmap(image, new BitmapProperties(image.PixelFormat))
            );
            var map = bitmap1.Map(MapOptions.Read);
            var size = image.PixelSize.Width * image.PixelSize.Height * 4;
            byte[] bytes = new byte[size];
            Marshal.Copy(map.Bits, bytes, 0, size);
            bitmap1.Unmap();
            bitmap1.Dispose();
            deviceContext2d.Dispose();
            var res = new List<List<Color4>>();
            for(int y = 0; y < image.PixelSize.Height; y++) 
            {
                res.Add(new List<Color4>());
                for (int x = 0; x < image.PixelSize.Width; x++) 
                {
                    var position = (y * image.PixelSize.Width + x) * 4;
                    res[y].Add(
                        new Color4(bytes[position + 2], bytes[position + 1], bytes[position + 0], bytes[position + 3]));
                
                }
            }
            return res;
        }

        /// <summary>
        /// 作成したキャラクターをダイアログから保存する。拡張子は.c2cにしなよ
        /// </summary>
        /// <param name="c">保存するキャラクター</param>
        [Obsolete("c3cファイルのみ読み込めるloadcharacter3を使用してください。c2cファイルを使用しないでください")]
        static public void savecharacter(character c)
        {
            var saveData = c;

            System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @".\character";
            sfd.FileName = "character" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".c3c";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(sfd.FileName) == ".c3c")
                {
                    savecharacter3(sfd.FileName,c,"");
                }
                else
                {
                    //指定したパスにファイルを保存する
                    Stream fileStream = sfd.OpenFile();
                    BinaryFormatter bF = new BinaryFormatter();
                    if (saveData != null)
                    {
                        bF.Serialize(fileStream, saveData);
                        Console.WriteLine("save : OKay. Chipping arouuund kick my brains around the floor");
                    }
                    fileStream.Close();
                }
            }
        }
        /// <summary>
        /// 作成したキャラクターをダイアログから保存する。拡張子は.c2cにしなよ
        /// </summary>
        /// <param name="path">保存するパス</param>
        /// <param name="c">保存するキャラクター</param>
        [Obsolete("c3cファイルのみ読み込めるloadcharacter3を使用してください。c2cファイルを使用しないでください")]
        static public void savecharacter(string path, character c)
        {
            var saveData = c;

            Stream fileStream = new FileStream(path, FileMode.OpenOrCreate);

            //指定したパスにファイルを保存する
            BinaryFormatter bF = new BinaryFormatter();
            if (saveData != null)
            {
                bF.Serialize(fileStream, saveData);
                Console.WriteLine("save : OKay. Chipping arouuund kick my brains around the floor");
            }
            fileStream.Close();
            float tt = 0;
            new tyusinchangeman(tt, 0, "hair", -0.1f, true, true, false);
        }
        /// <summary>
        /// キャラクターをダイアログからロードする。.c2cとか関係なくロードできるのかな？
        /// </summary>
        /// <param name="reset">既にロードしていた場合もロードし直す</param>
        /// <returns>ロードしたキャラクター</returns>
        [Obsolete("c3cファイルのみ読み込めるloadcharacter3を使用してください。c2cファイルを使用しないでください")]
        static public character loadcharacter(bool reset = false)
        {
            character res = null;
            System.Windows.Forms.OpenFileDialog sfd = new OpenFileDialog();
            sfd.InitialDirectory = @".\character";
            sfd.FileName = "character" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".c3c";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(sfd.SafeFileName) == ".c3c")
                {
                    res = loadcharacter3(sfd.FileName,path:"");
                }
                else
                {
                    string file = sfd.SafeFileName;
                    if (!characters.ContainsKey(file) || reset)
                    {
                        Object loadedData = null;

                        string dir = sfd.FileName;



                        //ファイルを読込
                        try
                        {
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            using (var fs = File.OpenRead(dir))
                            {

                                loadedData = binaryFormatter.Deserialize(fs);
                                fs.Close();
                            }
                            if (!characters.ContainsKey(file))
                            {
                                characters.Add(file, (character)loadedData);
                            }
                            else
                            {
                                characters[file] = (character)loadedData;
                            }
                        }
                        catch (Exception e) { Console.WriteLine(e.ToString()); }

                    }
                    if (characters.ContainsKey(file))
                    {
                        res = characters[file];
                    }
                }
            }
            if (res == null) return null;
            return new character(res);

        }
        /// <summary>
        /// 作成したキャラクターをロードする。
        /// </summary>
        /// <param name="file">.\character\*.c2cの*部分.c2cは書いてもいいし</param>
        /// <param name="scale">キャラクターのスケール</param>
        /// <param name="reset">再ロードする</param>
        /// <returns>ロードしたキャラクター</returns>
        /// 
        [Obsolete("c3cファイルのみ読み込めるloadcharacter3を使用してください。c2cファイルを使用しないでください")]
        static public character loadcharacter(string file, float scale = 1, bool reset = false)
        {
            {
                var names = file.Split('.');
                var name = names[0];
                for (int i = 1; i < names.Length - 1; i++) 
                {
                    if (i != 0) 
                    {
                        name += ".";
                    }
                    name += names[i];
                }
                var c3 = loadcharacter3(name, scale, reset);
                if (c3 != null)
                {
                    return c3;
                }
            }
            var a = Path.GetExtension(file);
            if (a != ".c2c") file += ".c2c";
            file = slashformat(file);

            character res = null;
            if (!characters.ContainsKey(file) || reset)
            {
                Object loadedData = null;

                string dir = @".\character\" + file;

                Console.WriteLine(file + " character load");
                if (!File.Exists(dir))
                {
                    Console.WriteLine("character " + dir + " not exists");
                    return null;
                }

                if (System.IO.File.Exists(dir))
                {



                    //ファイルを読込
                    try
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        using (var fs = File.OpenRead(dir))
                        {

                            loadedData = binaryFormatter.Deserialize(fs);
                            fs.Close();
                        }
                        if (!characters.ContainsKey(file))
                        {
                            characters.Add(file, new character((Character.character)loadedData));
                        }
                        else
                        {
                            characters[file] = new character((Character.character)loadedData);
                        }
                    }
                    catch (Exception e) { Console.WriteLine(e.ToString()); }
                }
            }
            if (characters.ContainsKey(file))
            {
                res = characters[file];
            }

            if (res != null)
            {
                var ret = new character(res);
                ret.scalechange(scale);
                //         res.x = -1000;
                //         res.y = -1000;
                return ret;
            }
            return res;
        }
        /// <summary>
        /// 作成したモーションをスクリプトと合わせてセーブする。拡張子は.c2mにしなよ
        /// </summary>
        /// <param name="s">モーションを作ったスクリプト</param>
        /// <param name="m">モーション本体</param>
        [Obsolete("c3mファイルのみ読み込めるloadmotionを使用してください。c2mファイルを使用しないでください")]

        static public void savemotion(string s, motion m)
        {
            var saveData = new motionsaveman();
            saveData.m = m;
            saveData.text = s;
            System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @".\motion";
            sfd.FileName = "motion" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".c3m";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(sfd.FileName) == ".c3m")
                {
                    savemotion3(sfd.FileName,s,m,"");
                }
                else
                {//指定したパスにファイルを保存する
                    Stream fileStream = sfd.OpenFile();
                    BinaryFormatter bF = new BinaryFormatter();
                    if (saveData.m != null)
                    {
                        bF.Serialize(fileStream, saveData);
                        Console.WriteLine("save : OK ");
                    }
                    fileStream.Close();
                }
            }
        }
        /// <summary>
        /// 作成したモーションをスクリプトと合わせてセーブする。拡張子は.c2mにしなよ
        /// </summary>
        /// <param name="path">保存するパス</param>
        /// <param name="s">モーションを作ったスクリプト</param>
        /// <param name="m">モーション本体</param>
        [Obsolete("c3mファイルのみ読み込めるloadmotionを使用してください。c2mファイルを使用しないでください")]
        static public void savemotion(string path, string s, motion m)
        {
            var saveData = new motionsaveman();
            saveData.m = m;
            saveData.text = s;

            //指定したパスにファイルを保存する
            Stream fileStream = new FileStream(path,FileMode.OpenOrCreate);
            BinaryFormatter bF = new BinaryFormatter();
            if (saveData.m != null)
            {
                bF.Serialize(fileStream, saveData);
                Console.WriteLine("save : OK ");
            }
            fileStream.Close();

        }

        /// <summary>
        /// 作成したモーションをスクリプトと合わせてセーブする。拡張子は.c3mにしなよ
        /// </summary>
        /// <param name="s">モーションを作ったスクリプト</param>
        /// <param name="m">モーション本体</param>
        static public void savemotion3(string s, motion m)
        {
           
            System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @".\motion";
            sfd.FileName = "motion" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".c3m";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                savemotion3(sfd.FileName,s,m,"");
            }
        }
        /// <summary>
        /// 作成したモーションをスクリプトと合わせてセーブする。拡張子は.c3mにしなよ
        /// </summary>
        /// <param name="file">ファイルの名前</param>
        /// <param name="path">保存するパス</param>
        /// <param name="s">モーションを作ったスクリプト</param>
        /// <param name="m">モーション本体</param>
        static public void savemotion3(string file, string s, motion m,string path=@".\motion\")
        {
            var saveData = new motionsaveman();
            saveData.m = m;
            saveData.text = s;

            saveData.ToSave().saveToPath(path+file, "");

        }

        /// <summary>
        /// モーションをダイアログからロードする。
        /// </summary>
        /// <param name="reset">ロードされている場合も再度ロードする</param>
        /// <returns>ロードしたモーション</returns>
        [Obsolete("c3mファイルのみ読み込めるloadmotionを使用してください。c2mファイルを使用しないでください")]
        static public motionsaveman loadmotion(bool reset = false)
        {

            motionsaveman res = null;
            System.Windows.Forms.OpenFileDialog sfd = new OpenFileDialog();
            sfd.InitialDirectory = @".\motion";
            sfd.FileName = "motion" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".c3m";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string file = sfd.SafeFileName;
                if (!motions.ContainsKey(file) || reset)
                {
                    Object loadedData = null;
                    string dir = sfd.FileName;

                    if (Path.GetExtension(dir) == ".c3m")
                    {
                        res = loadmotion3(sfd.FileName, path: "");
                    }
                    else
                    {

                        //ファイルを読込
                        try
                        {
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            using (var fs = File.OpenRead(dir))
                            {

                                loadedData = binaryFormatter.Deserialize(fs);
                                fs.Close();
                            }
                            if (!motions.ContainsKey(file))
                            {
                                motions.Add(file, ((motionsaveman)loadedData));
                            }
                            else
                            {
                                motions[file] = ((motionsaveman)loadedData);
                            }
                        }
                    catch (Exception e) { Console.WriteLine(e.ToString()); }
                    res = (motionsaveman)loadedData;
                } }


            }
            return new motionsaveman(res);
        }
        /// <summary>
        /// モーションファイル(.c2m)をロードし、モーション部分を返す。.c2mは書いても書かなくてもよい
        /// </summary>
        /// <param name="file">.\motion\に続くパス</param>
        /// <param name="sp">モーションのスピード</param>
        /// <param name="reset">再ロードする</param>
        /// <returns>ロードしたモーション</returns>
        [Obsolete("c3mファイルのみ読み込めるloadmotionを使用してください。c2mファイルを使用しないでください")]
        static public motion ldmotion(string file, float sp = 1, bool reset = false)
        {
            var m = loadmotion(file, sp, reset);
            if (m == null) return null;
            return m.m;
        }
        /// <summary>
        /// モーションファイル(.c2m)をロードしそのまま返す。モーションの編集するならこっち。
        /// </summary>
        /// <param name="file">.\motion\に続くパス.c2mは書かなくてよい</param>
        /// <param name="sp">モーションのスピード</param>
        /// <param name="reset">再ロードする</param>
        /// <returns>ロードしたモーションファイル</returns>
        [Obsolete("c3mファイルのみ読み込めるloadmotionを使用してください。c2mファイルを使用しないでください")]
        static public motionsaveman loadmotion(string file, float sp = 1, bool reset = false)
        {
            if(1==1)
            {
                var names = file.Split('.');
                var name = names[0];
                for (int i = 1; i < names.Length - 1; i++)
                {
                    if (i != 0)
                    {
                        name += ".";
                    }
                    name += names[i];
                }
                var c3 = loadmotion3(name, sp, reset);
                if (c3 != null)
                {
                    return c3;
                }
            }
            var a = Path.GetExtension(file);
            if (a != ".c2m") file += ".c2m";
            file = slashformat(file);

            //   Console.WriteLine(file + " motion load");
            motionsaveman res = null;

            if (!motions.ContainsKey(file) || reset)
            {
                Object loadedData = null;
                string dir = @".\motion\" + file;
                Console.WriteLine(file + " motion load");
                if (!File.Exists(dir)) 
                {
                    Console.WriteLine("motion " + dir + " not exists" );
                    return null;
                }


                //ファイルを読込
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    using (var fs = File.OpenRead(dir))
                    {

                        loadedData = binaryFormatter.Deserialize(fs);
                        fs.Close();
                    }
                    if (!motions.ContainsKey(file))
                    {
                        motions.Add(file, (motionsaveman)loadedData);
                    }
                    else
                    {
                        motions[file] = (motionsaveman)loadedData;
                    }
                }
                catch (Exception e) { Console.WriteLine(e.ToString()); }

            }
            if (motions.ContainsKey(file))
            {
                res = motions[file];
            }
            if (res != null)
            {
                var ress = new motionsaveman(res);
                ress.m.sp = sp;
                return ress;
            }
            else return null;
        }


        /// <summary>
        /// モーションをダイアログからロードする。
        /// </summary>
        /// <param name="reset">ロードされている場合も再度ロードする</param>
        /// <returns>ロードしたモーション</returns>
         static public motionsaveman loadmotion3(bool reset = false)
        {

            motionsaveman res = null;
            System.Windows.Forms.OpenFileDialog sfd = new OpenFileDialog();
            sfd.InitialDirectory = @".\motion";
            sfd.FileName = "motion" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".c3m";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                res = loadmotion3(sfd.FileName,reset:reset,path:"");

            }
            return new motionsaveman(res);
        }
        /// <summary>
        /// モーションファイル(.c2m)をロードし、モーション部分を返す。.c2mは書いても書かなくてもよい
        /// </summary>
        /// <param name="file">.\motion\に続くパス</param>
        /// <param name="sp">モーションのスピード</param>
        /// <param name="reset">再ロードする</param>
        /// <returns>ロードしたモーション</returns>
        static public motion ldmotion3(string file, float sp = 1, bool reset = false)
        {
            var m = loadmotion3(file, sp, reset);
            if (m == null) return null;
            return m.m;
        }
        /// <summary>
        /// モーションファイル(.c2m)をロードしそのまま返す。モーションの編集するならこっち。
        /// </summary>
        /// <param name="file">.\motion\に続くパス.c2mは書かなくてよい</param>
        /// <param name="sp">モーションのスピード</param>
        /// <param name="reset">再ロードする</param>
        /// <returns>ロードしたモーションファイル</returns>
        static public motionsaveman loadmotion3(string file, float sp = 1, bool reset = false,string path=@".\motion\")
        {

            var a = Path.GetExtension(file);
            if (a != ".c3m") file += ".c3m";
            file = slashformat(file);

            //   Console.WriteLine(file + " motion load");
            motionsaveman res = null;

            if (!motions.ContainsKey(file) || reset)
            {
                Object loadedData = null;
                string dir = path + file;
                Console.WriteLine(file + " motion load");
                if (!File.Exists(dir))
                {
                    Console.WriteLine("motion " + dir + " not exists");
                    return null;
                }


                //ファイルを読込
                try
                {
                    loadedData = motionsaveman.ToLoad(DataSaver.loadFromPath(dir,false,""));

                    if (!motions.ContainsKey(file))
                    {
                        motions.Add(file, (motionsaveman)loadedData);
                    }
                    else
                    {
                        motions[file] = (motionsaveman)loadedData;
                    }
                }
                catch (Exception e) { Console.WriteLine(e.ToString()); }

            }
            if (motions.ContainsKey(file))
            {
                res = motions[file];
            }
            if (res != null)
            {
                var ress = new motionsaveman(res);
                ress.m.sp = sp;
                return ress;
            }
            else return null;
        }

        // バージョン違いとかから強制的に移植するときにこんな風なのを書く例としてのこしとーく


        /*static public void kyusai()
        {
         //   string[] filesM = System.IO.Directory.GetFiles(@".\motion", "*", System.IO.SearchOption.AllDirectories);
            string[] filesC = System.IO.Directory.GetFiles(@".\character", "*", System.IO.SearchOption.AllDirectories);
         foreach(var a in filesM)
            {
                if (!a.Contains("zfile"))
                {
                    Console.WriteLine(a + " kyusaisimasu");
                    if (!a.Contains("zfile"))
                    {
                        string file = (a.Replace(@".\motion\", @""));
                        string dir = @".\motion\" + file;
                        object loadedData;
                      
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            using (var fs = File.OpenRead(dir))
                            {

                                loadedData = binaryFormatter.Deserialize(fs);
                                fs.Close();
                            }
                            var loaded = (Band_Brave_Journey.motionsaveman)loadedData;

                            motionsaveman saveData = new motionsaveman();
                            var mm = new motionmaker();
                            saveData.m = mm.motionmake(loaded.text);
                            saveData.text = loaded.text;

                            using (var sfd = File.OpenWrite(@".\motion\zfile\" + file))
                            {
                                //指定したパスにファイルを保存する
                                Stream fileStream = sfd;
                                BinaryFormatter bF = new BinaryFormatter();
                                if (saveData.m != null)
                                {
                                    bF.Serialize(fileStream, saveData);
                                }
                                fileStream.Close();
                            }

                            Console.WriteLine(a + " kyusaiend");
                        

                    }
                }
            }
            foreach (var a in filesC)
            {
                if (!a.Contains("zfile"))
                {
                    Console.WriteLine(a + " kyusaisimasu");
                    if (!a.Contains("zfile"))
                    {
                        string file = (a.Replace(@".\character\", @""));
                        string dir = @".\character\" + file;
                        object loadedData;
                        try
                        {
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            using (var fs = File.OpenRead(dir))
                            {

                                loadedData = binaryFormatter.Deserialize(fs);
                                fs.Close();
                            }
                            var loaded = (Band_Brave_Journey.Character.character)loadedData;

                            character saveData = new character(loaded);

                            using (var sfd = File.OpenWrite(@".\character\zfile\" + file))
                            {
                                //指定したパスにファイルを保存する
                                Stream fileStream = sfd;
                                BinaryFormatter bF = new BinaryFormatter();
                                if (saveData != null)
                                {
                                    bF.Serialize(fileStream, saveData);
                                }
                                fileStream.Close();
                            }

                            Console.WriteLine(a + " kyusaiend");
                        }
                        catch { }

                    }
                }
            }
        }*/


        /// <summary>
        /// キャラクターをダイアログからロードする。.c2cとか関係なくロードできるのかな？
        /// </summary>
        /// <param name="reset">既にロードしていた場合もロードし直す</param>
        /// <returns>ロードしたキャラクター</returns>
        static public character loadcharacter3(bool reset = false)
        {
            character res = null;
            System.Windows.Forms.OpenFileDialog sfd = new OpenFileDialog();
            sfd.InitialDirectory = @".\character";
            sfd.FileName = "character" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".c3c";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string file = sfd.FileName;
                res = loadcharacter3(file,1,reset,path:"");
            }
            if (res == null) return null;
            return new character(res);

        }
        /// <summary>
        /// ダイアログからセーブ
        /// </summary>
        /// <param name="c">基準がセーブされる</param>
        static public void savecharacter3(character c)
        {
            System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @".\character";
            sfd.FileName = "character" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".c3c";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string file = sfd.FileName;
                savecharacter3(file, c,"");
            }
        }
        /// <summary>
        /// キャラクターをセーブ。拡張子はc3cになる。
        /// </summary>
        /// <param name="file"></param>
        /// <param name="c">基準がセーブされる</param>
        static public void savecharacter3(string file, character c,string path=@".\character\")
        {
            string ext = ".c3c";
            if (Path.GetExtension(file) == ext) 
            {
                ext = "";
            }
            c.getkijyun().ToSave().saveToPath(path+file,ext);
        }
        /// <summary>
        /// 作成したキャラクターをロードする。ロードするのは基準だけ
        /// </summary>
        /// <param name="file">.\character\*.c2cの*部分.c2cは書いてもいいし</param>
        /// <param name="scale">キャラクターのスケール</param>
        /// <param name="reset">再ロードする</param>
        /// <param name="path">既定のパス</param>
        /// <returns>ロードしたキャラクター</returns>
        static public character loadcharacter3(string file, float scale = 1, bool reset = false,string path=@".\character\")
        {
            var a = Path.GetExtension(file);
            if (a != ".c3c") file += ".c3c";
            file = path+slashformat(file);


            character res = null;


            if (!characters.ContainsKey(file) || reset)
            {
                Console.WriteLine(file + " character load");
                if (!File.Exists(file))
                {
                    Console.WriteLine("character " + file + " not exists");
                    return null;
                }
                var d = DataSaver.loadFromPath(file, ext: "");
                var loadedData = character.ToLoad(d);
                
               
                if (!characters.ContainsKey(file))
                {
                    characters.Add(file, new character(loadedData));
                }
                else
                {
                    characters[file] = new character(loadedData);
                }
            }

            if (characters.ContainsKey(file))
            {
                res = characters[file];
            }

            if (res != null)
            {
                var ret = new character(res);
                ret.scalechange(scale);
                ret.setkijyuns();
                //         res.x = -1000;
                //         res.y = -1000;
                return ret;
            }
            return res;
        }


        /// <summary>
        /// グローバルボリューム。0＜＝＜＝1
        /// </summary>
        static public float glovol { get { return _glovol; } set { _glovol = value; if (_glovol < 0) _glovol = 0; if (_glovol > 1) _glovol = 1; MV.SetVolume(glovol); } }
        static float _glovol = 1f;
        /// <summary>
        /// 効果音のボリューム。0＜＝＜＝1
        /// </summary>
        static public float glovolkou { get { return _glovolkou; } set { _glovolkou = value; if (_glovolkou < 0) _glovolkou = 0; if (_glovolkou > 1) _glovolkou = 1; } }
        static float _glovolkou = 1f;
        /// <summary>
        /// bgmのボリューム。0＜＝＜＝1
        /// </summary>
        static public float glovolbgm { get { return _glovolbgm; } set { _glovolbgm = value; if (_glovolbgm < 0) _glovolbgm = 0; if (_glovolbgm > 1) _glovolbgm = 1; } }
        static float _glovolbgm = 0.3f;
        /// <summary>
        /// 音をロードする。ロードするだけ
        /// </summary>
        /// <param name="file">.\oto\*.wavの*部分</param>
        public static void loadoto(string file)
        {
            var a = Path.GetExtension(file);
            if (a != ".wav") file += ".wav";
            file = slashformat(file);


            Console.WriteLine(file + "  otoload");
            if (File.Exists(@".\oto\" + file) == false) return;
            var reader = new BinaryReader(File.OpenRead(@".\oto\" + file));

            // Read in the wave file header.
            var chunkId = new string(reader.ReadChars(4));
            var chunkSize = reader.ReadInt32();
            var format = new string(reader.ReadChars(4));
            var subChunkId = new string(reader.ReadChars(4));
            var subChunkSize = reader.ReadInt32();
            var audioFormat = (WaveFormatEncoding)reader.ReadInt16();
            var numChannels = reader.ReadInt16();
            var sampleRate = reader.ReadInt32();
            var bytesPerSecond = reader.ReadInt32();
            var blockAlign = reader.ReadInt16();
            var bitsPerSample = reader.ReadInt16();
            var dataChunkId = new string(reader.ReadChars(4));
            var dataSize = reader.ReadInt32();

            // Check that the chunk ID is the RIFF format
            // and the file format is the WAVE format
            // and sub chunk ID is the fmt format
            // and the audio format is PCM
            // and the wave file was recorded in stereo format
            // and at a sample rate of 44.1 KHz
            // and at 16 bit format
            // and there is the data chunk header.
            // Otherwise return false.
            //Console.WriteLine(chunkId + " " + format + " " + subChunkId.Trim() + " " + audioFormat + " = " + WaveFormatEncoding.Pcm + " " + numChannels + " " + sampleRate + " " + bitsPerSample + " " + dataChunkId);
            if (chunkId != "RIFF" || format != "WAVE" || subChunkId.Trim() != "fmt" || audioFormat != WaveFormatEncoding.Pcm || bitsPerSample != 16 || dataChunkId != "data")
            {
                Console.WriteLine(chunkId + format + subChunkId.Trim() + (audioFormat != WaveFormatEncoding.Pcm) + bitsPerSample + dataChunkId + " otoloadsippai");
                return;
            }


            // Set the buffer description of the secondary sound buffer that the wave file will be loaded onto and the wave format.


            // Create a temporary sound buffer with the specific buffer settings.
            var formattt = new WaveFormat(sampleRate, 16, numChannels);

            var SecondaryBuffer = audio.CreateSourceVoice(formattt);

            var waveData = reader.ReadBytes(dataSize);

            int size = Marshal.SizeOf(waveData[0]) * waveData.Length;
            IntPtr WDintPtr = Marshal.AllocHGlobal(size);

            Marshal.Copy(waveData, 0, WDintPtr, size);
            var buffer = new AudioBuffer();
            buffer.Flags = BufferFlags.EndOfStream;

            buffer.AudioBytes = dataSize;
            buffer.AudioDataPointer = WDintPtr;



            SecondaryBuffer.SubmitSourceBuffer(buffer);


            // Read in the wave file data into the temporary buffer.


            // Close the reader
            reader.Close();
            reader.Dispose();

            if (otos.ContainsKey(file))
            {
                otos[file].dispo();
                otos[file] = new otoman(SecondaryBuffer, formattt, buffer);
            }
            else
            {
                //  Console.WriteLine(file + " otoloadok");

                otos.Add(file, new otoman(SecondaryBuffer, formattt, buffer));
                //    Console.WriteLine("asfajsfjsal");

                //    Console.WriteLine(file + " otoloadok");
            }
        }
        /// <summary>
        /// 効果音を鳴らす
        /// </summary>
        /// <param name="file">.\oto\*.wavの*部分</param>
        /// <param name="vol">この音のボリューム</param>
        static public void playoto(string file, float vol = 1)
        {
            if (file == nothing) return;
            var a = Path.GetExtension(file);
            if (a != ".wav") file += ".wav";
            file = slashformat(file);

            vol = vol * glovolkou;

            if (!otos.ContainsKey(file))
            {

                loadoto(file);


                // Console.WriteLine(otos.Count() + "otocount");

            }
            if (otos.ContainsKey(file))
            {
                var otoman = otos[file];

                int kannin = maxsameoto;
                /*
                for (int i = oton.Count-1;i>=0 ; i--) 
                {
                    if (kannin-- == 0) 
                    {
                        oton[i].dispo();
                        oton.RemoveAt(i);

                        kannin++;
                    }
                }*/

                var nbuf = audio.CreateSourceVoice(otoman.wvf);

                
                nbuf.SubmitSourceBuffer(otoman.buf);

                

                // Lock the secondary buffer to write wave data into it.
                

                nbuf.SetVolume(vol);
                oton.Add(new otoman(nbuf, otoman.wvf, otoman.buf));
                nbuf.Start();


                if (oton.Count > maxoto)
                {
                    oton[0].dispo();
                    oton.RemoveAt(0);


                }
            }
        }
        /// <summary>
        /// 同時にならすことのできる音の数
        /// </summary>
        public static int maxoto = 50;
        /// <summary>
        /// 同時にならすことのできる同じ音の数.今はマジで意味ない
        /// </summary>
        public static int maxsameoto = 3;

        static otoman bgmman;
        static string nowbgm = "";
        /// <summary>
        /// bgmを止めるためのbgm
        /// </summary>
        public const string stopbgm = "stop";
        /// <summary>
        /// bgmを流す。bgmは一つしか流せない
        /// </summary>
        /// <param name="file">.\oto\bgm\*.wavの*部分。stopbgmとすることで無音にできる</param>
        /// <param name="butu">おなじbgmを流したときに最初から再生するか</param>
        static public void playbgm(string file, bool butu = false)
        {
            if (file == nothing) return;
            var a = Path.GetExtension(file);
            if (a != ".wav") file += ".wav";
            file = slashformat(file);


            if (file == stopbgm+".wav")
            {
                nowbgm = "";
                if (bgmman != null)
                {
                    bgmman.sorce.Stop();
                    bgmman.dispo();
                }
                bgmman = null;
            }
            file = @"bgm\" + file;
            var vol = glovolbgm;

            if (!otos.ContainsKey(file))
            {

                loadoto(file);


                // Console.WriteLine(file + " BGMload"+ otos.ContainsKey(file)+ "  "+(nowbgm != file || butu));
                /*foreach (var a in otos) 
                {
                    Console.WriteLine(a.Key + " :asfsaiex: ");
                }*/
            }

            if (otos.ContainsKey(file) && (nowbgm != file || butu))
            {

                // Console.WriteLine(file + " BGMpaly");
                nowbgm = file;
                var otoman = otos[file];


                var nbuf = audio.CreateSourceVoice(otoman.wvf);
                otoman.buf.LoopCount = 255;

                nbuf.SubmitSourceBuffer(otoman.buf);



                // Lock the secondary buffer to write wave data into it.


                nbuf.SetVolume(vol);

                if (bgmman != null)
                {
                    bgmman.sorce.Stop();
                    bgmman.dispo();
                }
                otoman.buf.LoopCount = 0;
                bgmman = new otoman(nbuf, otoman.wvf, otoman.buf);


                nbuf.Start();


            }
        }
        /// <summary>
        /// audioのあれ
        /// </summary>
        static IXAudio2 audio;
        /// <summary>
        /// 音を合成したときの奴を表す奴
        /// </summary>
        static IXAudio2MasteringVoice MV;

        /// <summary>
        /// 基本的なビットのあれよ。名前とか
        /// </summary>
        static public Dictionary<string, System.Drawing.Color> basebitnames = new Dictionary<string, System.Drawing.Color>
                {
                    {"red",System.Drawing.Color.Red },{"blue",System.Drawing.Color.Blue },{"green",System.Drawing.Color.Green },
                    {"white",System.Drawing.Color.White },{"gray",System.Drawing.Color.Gray },{"black",System.Drawing.Color.Black },
                    {"yellow",System.Drawing.Color.Yellow },{"cyan",System.Drawing.Color.Cyan },{"purple",System.Drawing.Color.Purple },
                    {"aqua",System.Drawing.Color.Aqua },{"brown",System.Drawing.Color.Brown },{"crimson",System.Drawing.Color.Crimson },
                    {"pink",System.Drawing.Color.Pink },{"orange",System.Drawing.Color.Orange },{"indigo",System.Drawing.Color.Indigo }

                };

        /// <summary>
        /// ランダム変数に現在時刻をシードであれする
        /// </summary>
        public static void setrandomseed()
        {
            r = new Random(DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
        }

        /// <summary>
        /// ファイルマンを初期化する
        /// </summary>
        /// <param name="hand"></param>
        static private void resetfileman(IntPtr hand)
        {

            if (audio != null)
            {
                audio.Dispose();
                MV.Dispose();
            }

            audio = Vortice.XAudio2.XAudio2.XAudio2Create();
            MV = audio.CreateMasteringVoice();

            setrandomseed();
            foreach (var a in texs.Keys)
            {
                if (texs[a] != null)
                {

                    texs[a].Factory.Dispose();

                    texs[a].Dispose();
                    Console.WriteLine(a + " disposen " + texs[a].IsDisposed);


                }
            }


            foreach (var a in otos)
            {
                a.Value.dispo();

            }
            otos.Clear();
            texs.Clear();

            motions.Clear();
            characters.Clear();
            otos.Clear();

            //基本ビットを追加
            {

                foreach (var a in basebitnames)
                {
                    settextobit(a.Key, a.Value);
                }
            }

        }
        /// <summary>
        /// ムービーをつくるスクリプトをロードする。使うならmovieというフォルダがexeと同じところに必要
        /// </summary>
        /// <param name="file">.\movie\*.txtの*部分</param>
        /// <returns>ロードしたムービー</returns>
        static public movie.Movie loadmovie(string file)
        {

            var aa = Path.GetExtension(file);
            if (aa != ".txt") file += ".txt";
            file = slashformat(file);
            string loaddata = "";

            string dir = @".\movie\" + file;

            //ファイルを読込
            try
            {
                using (var r = new System.IO.StreamReader(@".\movie\" + file))
                {
                    loaddata = r.ReadToEnd();
                }

            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }

            ScriptOptions a = ScriptOptions.Default
       .WithReferences(Assembly.GetEntryAssembly())
       .WithImports("System", "System.Windows.Forms", "System.Threading.Tasks", "System.Text", "System.Linq", "System.Collections.Generic", "Microsoft.CodeAnalysis.Scripting"
       , "Band_Brave_Journey.Character", "Band_Brave_Journey"
       , "Band_Brave_Journey.Entity.wazatoka", "Band_Brave_Journey.Entity.sousakei", "Band_Brave_Journey.Entity", "Band_Brave_Journey.Entity.debufftoka");


            var Q = CSharpScript.Create(loaddata, options: a, globalsType: typeof(movie.Movie));
            //Q.Compile();
            Delegate runner = Q.CreateDelegate();
            return new movie.Movie(runner);

        }
        
        /// <summary>
        /// loadfiketokaの時に呼び出されるイベント。
        /// </summary>
        static public EventHandler<loadfileEventArgs> loading;
        /// <summary>
        /// tex,oto,character,motionフォルダにあるアイテムを全てロードする
        /// event
        /// </summary>
        static public void loadfiletoka()
        {
            var m = new motion();
            m.addmoves(new texpropman(200, "a", 471, 0.5f));
            m.addmoves(new moveman(1, true));
            m.addmoves(new idouman(200, 0, 0, 360 / 100));





            
            if (Directory.Exists(@".\motion"))
            {
                string[] filesM = System.IO.Directory.GetFiles(@".\motion", "*.c2m", System.IO.SearchOption.AllDirectories);
                for (int i = 0; i < filesM.Count(); i++)
                {
                    Console.WriteLine(filesM[i]);
                    loadmotion(filesM[i].Replace(@".\motion\", @""));
                    loading?.Invoke(null, new loadfileEventArgs(filesM.Length, 1,i, filesM[i]));
                }
            }
            if (Directory.Exists(@".\oto"))
            {
                string[] filesO = System.IO.Directory.GetFiles(@".\oto", "*.wav", System.IO.SearchOption.AllDirectories);
                for (int i = 0; i < filesO.Count(); i++)
                {
                    Console.WriteLine(filesO[i]);
                    loadoto(filesO[i].Replace(@".\oto\", @""));
                    loading?.Invoke(null, new loadfileEventArgs(filesO.Length, 1, i, filesO[i]));
                }
            }
            if (Directory.Exists(@".\tex"))
            {
                string[] filesT = System.IO.Directory.GetFiles(@".\tex", "*.bmp", System.IO.SearchOption.AllDirectories);
                for (int i = 0; i < filesT.Count(); i++)
                {

                    Console.WriteLine(filesT[i]);
                    ldtex(filesT[i].Replace(@".\tex\", @""));
                    loading?.Invoke(null, new loadfileEventArgs(filesT.Length, 1, i, filesT[i]));
                }
            }
            if (Directory.Exists(@".\character"))
            {
                string[] filesC = System.IO.Directory.GetFiles(@".\character", "*.c2c", System.IO.SearchOption.AllDirectories);
                for (int i = 0; i < filesC.Count(); i++)
                {

                    Console.WriteLine(filesC[i]);
                    loadcharacter(filesC[i].Replace(@".\character\", @""));
                    loading?.Invoke(null, new loadfileEventArgs(filesC.Length, 1, i, filesC[i]));
                }
            }
        }
        /// <summary>
        /// 0~wの範囲でランダムな整数を発生させる
        /// </summary>
        /// <param name="w"></param>
        /// <param name="minusToo">-w~wの範囲にする</param>
        /// <returns></returns>
        static public float whrandhani(float w,bool minusToo=false)
        {
            if (minusToo) 
            {

                return (float)r.NextDouble() * w*2-w;
            }
            return (float)r.NextDouble() * w;

        }
        /// <summary>
        /// Listからランダムに一つピックする
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static public T pickone<T>(List<T>list)
         
        {
            if (list.Count > 0) 
            {
                return list[r.Next() % list.Count];
            }
            return default(T);
        }
        /// <summary>
        /// 配列からランダムに一つピックする
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static public T pickone<T>(params T[] list)

        {
            if (list.Length > 0)
            {
                return list[r.Next() % list.Length];
            }
            return default(T);
        }
     
        /// <summary>
        /// nより小さい自然数数(0含む)を返す
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        static public int randmods(int w)
        {
            return r.Next()%w;

        }
        /// <summary>
        /// +1か-1を返す
        /// </summary>
        /// <param name="per">+1を返す確率</param>
        /// <returns></returns>
        static public float plusminus(float per=50)
        {
            var a = r.NextDouble() * 100;
            if (per >= a) 
            {
                return 1;
            }
            return -1;

        }
        /// <summary>
        /// +1か-1を返す
        /// </summary>
        /// <param name="ok">+1を返すか</param>
        /// <param name="plus">ok=trueの解き返すほう</param>
        /// <returns></returns>
        static public float plusminus(bool ok,bool plus=true)
        {
            if (plus) 
            {
                if (ok) return 1;
                return -1;
            }
            if (ok) return -1;
            return 1;

        }
        /// <summary>
        /// なんとなくの確率でTrueを返す
        /// </summary>
        /// <param name="per">パーセント(100で確実よもちろん)</param>
        /// <returns></returns>
        static public bool percentin(float per)
        {
            var a = (double)r.Next()/(double)int.MaxValue*100;
            return per >= a;
        }

        /// <summary>
        /// モーションをスクリプトから作る
        /// </summary>
        /// <param name="script">スクリプトか？</param>
        /// <param name="speed">再生速度</param>
        /// <param name="yobidasi">呼び出してくれるクラス</param>
        /// <returns></returns>
        static public motion buildMotion(string script,float speed=1) 
        {
//            var work = new motion();
//            work.sp = speed;

            script = "var work=new motion();\n"+script+";\nreturn work;";
            ScriptOptions a = ScriptOptions.Default
            .WithReferences(Assembly.GetEntryAssembly())
            .WithImports("System", "System.Collections.Generic", "Charamaker2.Character", "Charamaker2"
            , "Charamaker2.maker");

            var Q = CSharpScript.Create(script, options: a);
            var runner = Q.CreateDelegate();
            var run = (Delegate)runner;
            //runner();
            var ret = (motion)runner().Result;
            if (ret != null)
            {
                ret.sp = speed;
            }
            return ret;
        }
    }

    /// <summary>
    /// 鳴らした音を保存しとくクラス
    /// </summary>
    public class otoman
    {

        public IXAudio2SourceVoice sorce;
        public WaveFormat wvf;
        public AudioBuffer buf;

        public otoman(IXAudio2SourceVoice s, WaveFormat wf, AudioBuffer ab)
        {
            //Console.WriteLine("oukQ");
            sorce = s;
            wvf = wf;
            buf = ab;
            //     Console.WriteLine("oukQQWER");
        }
        public void dispo()
        {
            if (!sorce.IsDisposed)
            {
                wvf = null;
                sorce.DestroyVoice();

                buf.Dispose();
                sorce.Dispose();

            }
        }



    }
    /// <summary>
    /// モーションをセーブするためのクラス
    /// </summary>
    [Serializable]
    public class motionsaveman
    {
        public string text;
        public motion m;
        public motionsaveman() { }
        public motionsaveman(motionsaveman mm)
        {
            text = mm.text;
            m = new motion(mm.m);
        }

        public DataSaver ToSave() 
        {
            var d = new DataSaver();
            d.packAdd("script", text, true);
            d.linechange();
            d.packAdd("motion",m.ToSave());
            return d;
        }
        /// <summary>
        /// escapeせずに渡して
        /// </summary>
        /// <param name="d">escapeせずに渡して</param>
        /// <returns></returns>
        static public motionsaveman ToLoad(DataSaver d) 
        {
            var ms=new motionsaveman();

            ms.text = d.unPackDataS("script");
            ms.m = motion.ToLoad(d.unPackDataD("motion").escaped());
            return ms;
        }

    }
    /// <summary>
    /// loadfiletokaのときのイベント
    /// </summary>
    public class loadfileEventArgs 
    {
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="max">最大ファイルの数</param>
        /// <param name="num">今ロードしたファイルの数</param>
        /// <param name="loaded">ロードを終えたファイルの数</param>
        /// <param name="filename">ロードしたファイルの名前の代表</param>
        public loadfileEventArgs(int max,int num,int loaded,string filename) 
        {
            this.max = max;
            this.num = num;
            this.loaded = loaded;
            name = filename;
            type = name.Replace("*."," ");
        }
        /// <summary>
        /// ロードするファイルの総数
        /// </summary>
        public int max;
        /// <summary>
        /// ロードしたファイルの数
        /// </summary>
        public int num;
        /// <summary>
        /// ロードを終えたファイルの数
        /// </summary>
        public int loaded;
        /// <summary>
        /// ロードしたファイルの名前の代表
        /// </summary>
        public string name;
        /// <summary>
        /// ロードしたファイルの拡張子
        /// </summary>
        public string type;
        /// <summary>
        /// 既にロードしたファイルの比
        /// </summary>
        public float loadedhi { get { if (max <= 0) return 1;return loaded / max; } }
        /// <summary>
        /// 今ロードしたファイルの比
        /// </summary>
        public float numhi { get { if (max <= 0) return 1; return num / max; } }

    }

}
