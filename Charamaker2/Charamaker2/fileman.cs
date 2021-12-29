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

namespace Charamaker2
{
    /// <summary>
    /// ファイルの入出力・このシステムのセットアップを扱うクラス。
    /// setingupingでセットアップは完了する。
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
        static ID2D1HwndRenderTarget rendertarget;
        /// <summary>
        /// ガシーツ
        /// </summary>
        static float gasyu = 1;
        /// <summary>
        /// 画面のサイズを設定し、hyojimanを生成可能にする。
        /// </summary>
        /// <param name="bairitu">画質の倍率</param>
        /// <param name="Hand">ハンドラー</param>
        /// <param name="wi">画面の幅</param>
        /// <param name="hei">画面の高さ</param>
        static private void resizen(float bairitu,IntPtr Hand,float wi,float hei)
        {
            D2D1.D2D1CreateFactory<ID2D1Factory>(FactoryType.SingleThreaded, out fac);
            var renpro = new RenderTargetProperties();
            var hrenpro = new HwndRenderTargetProperties();
            hrenpro.Hwnd = Hand;
            System.Drawing.Size si = new System.Drawing.Size((int)(wi * bairitu), (int)(hei * bairitu));
            hrenpro.PixelSize = si;

            rendertarget = fac.CreateHwndRenderTarget(renpro, hrenpro);
            gasyu = bairitu;
        }
        /// <summary>
        /// hyojimanを取得する
        /// </summary>
        /// <returns>新しいhyojiman</returns>
        static public hyojiman makehyojiman() 
        {
            var aa = new hyojiman(rendertarget);
            aa.bairitu = gasyu;
            return aa;
        }
        /// <summary>
        /// セットアップをする。
        /// 画像の表示、音の再生が使用可能になる。
        /// </summary>
        /// <param name="f">素となるフォーム</param>
        /// <param name="bai">画質の倍率</param>
        static public void setinguping(Form f,float bai=1)
        {
            resizen(bai, f.Handle, f.ClientSize.Width, f.ClientSize.Height);
            fileman.resetfileman( f.Handle);

        }
        /// <summary>
        /// セットアップをする。
        /// 画像の表示、音の再生が使用可能になる。
        /// </summary>
        /// <param name="f">素となるユーザーコントロール</param>
        /// <param name="bai">画質の倍率</param>
        static public void setinguping(UserControl f,float bai=1)
        {
            resizen(bai, f.Handle, f.ClientSize.Width, f.ClientSize.Height);

            fileman.resetfileman( f.Handle);

        }

        /// <summary>
        /// 読み込んだテクスチャーを保存しとく
        /// </summary>
        static Dictionary<string, ID2D1Bitmap> texs = new Dictionary<string, ID2D1Bitmap>();
        /// <summary>
        /// 読み込んだモーションを保存しとく
        /// </summary>
        static Dictionary<string, motionsaveman> motions = new Dictionary<string, motionsaveman>();
        /// <summary>
        /// 読み込んだキャラクターを保存しとく
        /// </summary>
        static Dictionary<string, character> characters = new Dictionary<string, character>();
        /// <summary>
        /// 読み込んだ音を保存しとく
        /// </summary>
        static Dictionary<string, otoman> otos = new Dictionary<string, otoman>();
        /// <summary>
        /// 今ならしている音を保存しとく。ちゃんとメモリ開放できるように。
        /// </summary>
        static List<otoman> oton = new List<otoman>();
        /// <summary>
        /// 乱数ロット
        /// </summary>
        static public Random r = new Random();
        /// <summary>
        /// bitmapテクスチャーを読み込む。既に読み込んでいた場合は読み込まずに返す。
        /// .bmpはつけてもつけなくてもいい
        /// </summary>
        /// <param name="file">.\tex\に続くファイルパス</param>
        /// <param name="reset">強制的に再読み込みする</param>
        /// <returns>ロードしたテクスチャー</returns>
        static public ID2D1Bitmap ldtex(string file, bool reset = false)
        {
            var aa = Path.GetExtension(file);
            if (aa != ".bmp") file += ".bmp";
            if (!texs.ContainsKey(file) || reset)
            {
                Console.WriteLine(file + "load sitao!");
                string fi = @".\tex\" + file ;
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
                                    if (B == 254 && R == 0 && G == 254) tempStream.Write(gaba);
                                    else
                                        tempStream.Write(rgba);


                                }
                            }
                            // 読み込み元のBitmapのロックを解除する
                            bitmap.UnlockBits(bitmapData);
                            tempStream.Position = 0;

                            // 変換したデータからBitmapを生成して返す

                            var size = new System.Drawing.Size(bitmap.Width, bitmap.Height);
                            var bitmapProperties = new BitmapProperties(new PixelFormat(Vortice.DXGI.Format.R8G8B8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied));

                            var result = rendertarget.CreateBitmap(size, tempStream.BasePointer, stride, bitmapProperties);
                            if (texs.ContainsKey(file))
                            {
                                texs[file].Dispose();
                                texs[file] = result;
                            }
                            else texs.Add(file, result);
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
        /// 作成したキャラクターをダイアログから保存する。拡張子は.c2cにしなよ
        /// </summary>
        /// <param name="c">保存するキャラクター</param>
        static public void savecharacter(character c)
        {
            var saveData = c;

            System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @".\character";
            sfd.FileName = "character" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second+".c2c";
            if (sfd.ShowDialog() == DialogResult.OK)
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
        /// <summary>
        /// キャラクターをダイアログからロードする。.c2cとか関係なくロードできるのかな？
        /// </summary>
        /// <param name="reset">既にロードしていた場合もロードし直す</param>
        /// <returns>ロードしたキャラクター</returns>
        static public character loadcharacter(bool reset = false)
        {
            character res = null;
            System.Windows.Forms.OpenFileDialog sfd = new OpenFileDialog();
            sfd.InitialDirectory = @".\character";
            sfd.FileName = "character" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second+".c2c";
            if (sfd.ShowDialog() == DialogResult.OK)
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
                    catch { }

                }
                if (characters.ContainsKey(file))
                {
                    res = characters[file];
                }
            }
            return new character(res);

        }
        /// <summary>
        /// 作成したモーションをスクリプトと合わせてセーブする。拡張子は.c2mにしなよ
        /// </summary>
        /// <param name="s">モーションを作ったスクリプト</param>
        /// <param name="m">モーション本体</param>
        static public void savemotion(string s, motion m)
        {
            var saveData = new motionsaveman();
            saveData.m = m;
            saveData.text = s;
            System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @".\motion";
            sfd.FileName = "motion" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second+".c2m";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                //指定したパスにファイルを保存する
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
        /// <summary>
        /// モーションをダイアログからロードする。
        /// </summary>
        /// <param name="reset">ロードされている場合も再度ロードする</param>
        /// <returns>ロードしたモーション</returns>
        static public motionsaveman loadmotion(bool reset = false)
        {

            motionsaveman res = null;
            System.Windows.Forms.OpenFileDialog sfd = new OpenFileDialog();
            sfd.InitialDirectory = @".\motion";
            sfd.FileName = "motion" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second+".c2m";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string file = sfd.SafeFileName;
                if (!motions.ContainsKey(file) || reset)
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
                        if (!motions.ContainsKey(file))
                        {
                            motions.Add(file, ((motionsaveman)loadedData));
                        }
                        else
                        {
                            motions[file] = ((motionsaveman)loadedData);
                        }
                    }
                    catch { }
                    res = (motionsaveman)loadedData;
                }
                

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
        static public motion ldmotion(string file, float sp = 1, bool reset = false) 
        {
            return loadmotion(file, sp, reset).m;
        }
        /// <summary>
        /// モーションファイル(.c2m)をロードしそのまま返す。モーションの編集するならこっち。
        /// </summary>
        /// <param name="file">.\motion\に続くパス.c2mは書かなくてよい</param>
        /// <param name="sp">モーションのスピード</param>
        /// <param name="reset">再ロードする</param>
        /// <returns>ロードしたモーションファイル</returns>
        static public motionsaveman loadmotion(string file, float sp = 1, bool reset = false)
        {

            var a = Path.GetExtension(file);
            if (a != ".c2m") file += ".c2m";
            //   Console.WriteLine(file + " motion load");
            motionsaveman res = null;
         
            if (!motions.ContainsKey(file) || reset)
            {
                Object loadedData = null;
                string dir = @".\motion\" + file;
                Console.WriteLine(file + " motion load");



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
                catch { }

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
       /// 作成したキャラクターをロードする。
       /// </summary>
       /// <param name="file">.\character\*.c2cの*部分.c2cは書いてもいいし</param>
       /// <param name="scale">キャラクターのスケール</param>
       /// <param name="reset">再ロードする</param>
       /// <returns>ロードしたキャラクター</returns>
        static public character loadcharacter(string file, float scale = 1, bool reset = false)
        {

            var a = Path.GetExtension(file);
            if (a != ".c2c") file += ".c2c";
            character res = null;
            if (!characters.ContainsKey(file) || reset)
            {
                Object loadedData = null;

                string dir = @".\character\" + file;



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
                catch { }

            }
            if (characters.ContainsKey(file))
            {
                res = characters[file];
            }

            var ret = new character(res);
            ret.scalechange(scale);
            //         res.x = -1000;
            //         res.y = -1000;
            return ret;
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
        static void loadoto(string file)
        {
            var a = Path.GetExtension(file);
            if (a != ".wav") file += ".wav";


             Console.WriteLine(file + "  otoload");
            if (File.Exists(@".\oto\" + file ) == false) return;
            var reader = new BinaryReader(File.OpenRead(@".\oto\" + file ));

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
            var a = Path.GetExtension(file);
            if (a != ".wav") file += ".wav";

            vol = vol * glovolkou;

            if (!otos.ContainsKey(file))
            {

                loadoto(file);


                // Console.WriteLine(otos.Count() + "otocount");

            }
            if (otos.ContainsKey(file))
            {
                var otoman = otos[file];

                var nbuf = audio.CreateSourceVoice(otoman.wvf);



                nbuf.SubmitSourceBuffer(otoman.buf);

                // Lock the secondary buffer to write wave data into it.


                nbuf.SetVolume(vol);
                oton.Add(new otoman(nbuf, otoman.wvf, otoman.buf));

                nbuf.Start();
                if (oton.Count > 50)
                {
                    oton[0].dispo();
                    oton.RemoveAt(0);

                }
            }
        }

        static otoman bgmman;
        static string nowbgm = "";
        /// <summary>
        /// bgmを流す。bgmは一つしか流せない
        /// </summary>
        /// <param name="file">.\oto\bgm\*.wavの*部分。""とすることで無音にできる</param>
        /// <param name="butu">おなじbgmを流したときに最初から再生するか</param>
        static public void playbgm(string file, bool butu = false)
        {
            var a = Path.GetExtension(file);
            if (a != ".wav") file += ".wav";
            if (file == ".wav")
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
        /// ファイルマンを初期化する
        /// </summary>
        /// <param name="hand"></param>
        static private void resetfileman( IntPtr hand)
        {
            if (audio != null)
            {
                audio.Dispose();
                MV.Dispose();
            }

            audio = Vortice.XAudio2.XAudio2.XAudio2Create();
            MV = audio.CreateMasteringVoice();
   
            r = new Random();
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

        }
        /// <summary>
        /// ムービーをつくるスクリプトをロードする
        /// </summary>
        /// <param name="file">.\movie\*.txtの*部分</param>
        /// <returns>ロードしたムービー</returns>
        static public movie.Movie loadmovie(string file)
        {

            var aa = Path.GetExtension(file);
            if (aa != ".txt") file += ".txt";
            string loaddata = "";

            string dir = @".\movie\" + file;

            //ファイルを読込
            try
            {
                using (var r = new System.IO.StreamReader(@".\movie\" + file ))
                {
                    loaddata = r.ReadToEnd();
                }

            }
            catch { }

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
        /// tex,oto,character,motionフォルダにあるアイテムを全てロードする
        /// </summary>
        static public void loadfiletoka()
        {
              var m = new motion();
            m.addmoves(new texpropman(200, "a", 471, 0.5f));
            m.addmoves(new moveman(1, true));
            m.addmoves(new idouman(200, 0, 0, 360 / 100));
       

          
            string[] filesM = System.IO.Directory.GetFiles(@".\motion", "*.c2m", System.IO.SearchOption.AllDirectories);
            string[] filesO = System.IO.Directory.GetFiles(@".\oto", "*.wav", System.IO.SearchOption.AllDirectories);

            string[] filesT = System.IO.Directory.GetFiles(@".\tex", "*.bmp", System.IO.SearchOption.AllDirectories);
            string[] filesC = System.IO.Directory.GetFiles(@".\character", "*.c2c", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < filesM.Count();i++) 
            {
                Console.WriteLine(filesM[i]);
                loadmotion(filesM[i].Replace(@".\motion\", @""));
            }
            for (int i = 0; i < filesO.Count(); i++)
            {
                Console.WriteLine(filesO[i]);
                loadoto(filesO[i].Replace(@".\oto\", @""));
            }
            for (int i = 0; i < filesT.Count(); i++)
            {

                Console.WriteLine(filesT[i]);
                ldtex(filesT[i].Replace(@".\tex\", @""));
            }
            for (int i = 0; i < filesC.Count(); i++)
            {

                Console.WriteLine(filesC[i]);
                loadcharacter(filesC[i].Replace(@".\character\", @""));
            }
          
            
            foreach (var a in characters)
            {
                foreach (var b in a.Value.core.getallsetu())
                {
                    foreach (var c in b.p.textures)
                    {
                        ldtex(c.Value);
                    }
                }
            }
         
        }
        static public float whrandhani(float w)
        {
            if (w == 0)
            {
                return 0;
            }
            else if (w < 0)
            {
                return -(r.Next() % (Math.Abs(w) + 1));
            }
            else
            {
                return r.Next() % (w + 1);
            }

        }
        static public bool percentin(float per)
        {
            var a = r.NextDouble() * 100;
            return per >= a;
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
    }

}
