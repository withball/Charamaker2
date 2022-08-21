using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Charamaker2;
using System.IO;
using System.IO.Compression;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.MixedReality.WebRTC;
using Charamaker2.input;

/// なんかわからんけど、mrwebrtc.dllがないって言われたらpackages->MixedRealityWebrtc->runtimes->win10-x86->nativにある
/// やつをexeのとこまで持ってけ！

namespace C2WebRTCP2P
{
    
    /// <summary>
    /// hyojimanとinputinを交換し合う通信を行うためのクラス。setupしてから使ってね
    /// </summary>
    public static class supertusin
    {
       
        /// <summary>
        /// セットアップするやつ
        /// </summary>
        /// <param name="stun">使用するSTUNサーバーのUrl</param>
        /// <param name="messages">通信フォームに追加されるメッセージ8つ</param>
        static public void setup(string stun= @"stun:stun.l.google.com:19302", List<string>messages=null) 
        {
           
            _stun = stun;
            if (messages == null)
            {
                messages = new List<string>();
                messages.Add("通信相手にこの文字列をコピーして渡す");
                messages.Add("通信相手から文字列を受け取って右に入力する");
                messages.Add("多分通信は無理だ");
                messages.Add("通信は厳しそうだが");
                messages.Add("相手に左のメッセージを渡す");
                messages.Add("後は待てばいいと思うが……");
                messages.Add(" たぶんネットワークエラー");
                messages.Add("通信文字列エラー");
            }
            formmess = messages;


           
        }
        /// <summary>
        /// 通信フォームのエラーとかメッセージ列
        /// </summary>
        public static List<string> formmess;
        /// <summary>
        /// 現在使用中のpeerconectionのリスト
        /// </summary>
        public static  List<PeerConnection> pcs = new List<PeerConnection>();
        /// <summary>
        /// プロセス終了時にpeerconectionが生きていたらおかしくなるので呼び出せな
        /// </summary>
        static public void shutdown()
        {
            foreach (var a in pcs) 
            {
                a.Dispose();
            }
        }


        static string _stun;
        /// <summary>
        /// STUNサーバー
        /// </summary>
       static public string stun { get { return _stun; } }

        static  serverman3 sv;
        static  clientman3 cl;
        /// <summary>
        /// 通信を確立したのちに使用する。接続を開始する。
        /// </summary>
        static public void starts() 
        {
            if (sv != null) sv.resetcount();
            if (cl != null) cl.resetcount();
        }
        /// <summary>
        /// 通信を確立したのちに使用する。接続を停止する。
        /// </summary>
        static public void ends()
        {
            if (sv != null) sv.termcount();
            if (cl != null) cl.termcount();
        }
        /// <summary>
        /// 接続してる情報をリセットする
        /// </summary>
        static public void clear() 
        {
            sv?.dispose(); 
            cl?.dispose();
            sv = null;
            cl = null;

            tuusinform.cleanupforms();
        }
        /// <summary>
        /// 通信を開始したのちに使用する。hyojimanを受け取る
        /// </summary>
        /// <returns>受け取ったhyojiman</returns>
        static public hyojiman getinhyoji()
        {
            return cl.get();
        }
        /// <summary>
        /// 通信を開始したのちに使用する。inputinを受け取る
        /// </summary>
        /// <returns>受け取ったinputin</returns>
        static public inputin getininput()
        {
            return sv.get();
        }
        /// <summary>
        /// 通信を開始したのちに使用する。通信相手にデータを送る。
        /// </summary>
        /// <param name="hyo">送るデータ</param>
        static public void cast(hyojiman hyo)
        {
            sv.cast(hyo);
        } 
        /// <summary>
          /// 通信を開始したのちに使用する。通信相手にデータを送る。
          /// </summary>
          /// <param name="i">送るデータ</param>
        static public void cast(inputin i) 
        {
            cl.cast(i);
        }
        /// <summary>
        /// このプロセスがサーバーを立てているか
        /// </summary>
        static public bool servon { get { return sv != null; } }
        
        /// <summary>
        /// このプロセスがクライアントを立てているか
        /// </summary>
        static public bool clieon { get { return cl != null; } }
       
        /// <summary>
        /// このプロセスのサーバーと別プロセスのクライアントが接続できているか
        /// あくまで接続ができてるだけで、データが送られてきてるかとかは感知できない。
        /// </summary>
        static public bool servcon { get { return servon&&sv.connected; } }
        /// <summary>
        /// このプロセスのクライアントと別プロセスのサーバーが接続できているか。
        /// データのやり取りができているかはcast,getを行わないとわからない
        /// </summary>
        static public bool cliecon { get { return clieon&&cl.connected; } }

        /// <summary>
        /// サーバーを立てる。
        /// 通信の接続はtuusinformで行う
        /// もう立ってるなら前のは削除する
        /// </summary>
        static public void setsv()
        {
            if (sv !=null) sv.dispose();

             sv = new serverman3();
            new tuusinform(sv.pc, true).Show(); ;

        }

        /// <summary>
        /// クライアントを立てる。
        /// 通信の接続はtuusinformで行う
        /// もう立ってるなら前のは削除する
        /// </summary>
        static public void setcli()
        {
            if (cl != null) cl.dispose();
            cl = new clientman3();
            new tuusinform(cl.pc, false).Show();
        }


    }
    /// <summary>
    /// 全てのサーバー、クライアントに共通しそうな部分
    /// </summary>
    public abstract class kyotuman
    {
        int cou = 0;
        int maxcou = 80;
        /// <summary>
        /// 通信しているかの判定に使用するカウントをリセットする
        /// </summary>
        public void resetcount() { cou = 0; jyu.Clear(); }
        /// <summary>
        /// 通信しているかの判定に使用するカウントを終わらせる
        /// </summary>
        public void termcount() { cou = maxcou+1; jyu.Clear(); }

        /// <summary>
        /// 通信が確立されているか
        /// </summary>
        public bool connected { get { if (pc == null) return false; return cou <= maxcou && pc.IsConnected; } }


        /// <summary>
        /// 通信するために必要なperrconection
        /// </summary>
        public PeerConnection pc;

        /// <summary>
        /// 受信したオブジェクトを入れとく
        /// </summary>
       List<object> jyu = new List<object>();
        int maxbuf = 4;

        /// <summary>
        /// バッファーに格納されている情報を取り出す。
        /// </summary>
        /// <returns></returns>
        protected object getn()
        {

            //Console.WriteLine(jyu.Count + "  SSSjyunokazu");

            if (jyu.Count > 0)
            {
                cou = 0;
                var a = jyu[0];
                jyu.RemoveAt(0);
                return a;
            }
            //  Console.WriteLine(cou+" naiyoooo");
            cou++;
            return null;
        }

        /// <summary>
        /// 受信したものを格納するメソッド
        /// </summary>
        /// <param name="a">受信</param>
        /// <returns>バッファーがあふれたか</returns>
        protected bool jyusin(object a)
        {
            jyu.Add(a);
            if (jyu.Count > maxbuf)
            {
                jyu.RemoveAt(0);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="cou">接続状態の最大カウント</param>
        /// <param name="buf">バッファーの数</param>
         public kyotuman(int cou, int buf) 
        {
            maxcou = cou;
            maxbuf = buf;

            pc = new PeerConnection();
            supertusin.pcs.Add(pc);
            var llis = new List<IceServer>();

            Console.WriteLine(supertusin.stun+" aoshfoa");
            llis.Add(new IceServer { Urls = { supertusin.stun } });

            var config = new PeerConnectionConfiguration
            {
                IceServers = llis,
                SdpSemantic = SdpSemantic.UnifiedPlan,
                IceTransportType = IceTransportType.All,
                BundlePolicy = BundlePolicy.Balanced
            };
            var a = pc.InitializeAsync(config);

            a.Wait();



            pc.Connected += () => {
                Console.WriteLine("PeerConnection: connected.");
            };
            pc.RenegotiationNeeded += () => {
                Console.WriteLine("PeerConnection: detected.");
            };

            pc.IceStateChanged += (IceConnectionState newState) => {
                Console.WriteLine($"ICE state: {newState}");
            };
        }
        /// <summary>
        /// データを圧縮する
        /// </summary>
        /// <param name="src">データ</param>
        /// <returns>圧縮されたデータ</returns>
        static public byte[] asyukku(byte[] src)
        {
            using (var ms = new MemoryStream())
            {
                using (var ds = new DeflateStream(ms, CompressionMode.Compress, true/*msは*/))
                {
                    ds.Write(src, 0, src.Length);
                }


                ms.Position = 0;
                byte[] comp = new byte[ms.Length];
                ms.Read(comp, 0, comp.Length);
                //  Console.WriteLine(src.Length + " : : " + comp.Length);
                return comp;
            }

        }
        /// <summary>
        /// データを解凍する
        /// </summary>
        /// <param name="src">データ</param>
        /// <returns>解凍されたデータ</returns>
        static public byte[] kaitou(byte[] src)
        {
            try
            {


                using (var ms = new MemoryStream(src))
                using (var ds = new DeflateStream(ms, CompressionMode.Decompress))
                {
                    using (var dest = new MemoryStream())
                    {
                        ds.CopyTo(dest);

                        dest.Position = 0;
                        byte[] decomp = new byte[dest.Length];
                        dest.Read(decomp, 0, decomp.Length);
                        return decomp;
                    }
                }
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// データを送信するメソッド
        /// </summary>
        /// <param name="b">送信するデータ</param>
        protected  void castn(byte[] b) 
        {   try
            {
                b = asyukku(b);
                
                // if (fileman.percentin(80))
                {
                    foreach (var d in pc.DataChannels)
                    {

                        d.SendMessage(b);
                        break;
                    }
                }

            }
            catch (Exception eee) { Console.WriteLine("Error on Cast! " + eee.ToString()); }
        }
        /// <summary>
        /// PEERCONECTIONとかをdisposeする
        /// </summary>
        public void dispose()
        {

            supertusin.pcs.Remove(pc);
            pc.Close();

            pc.Dispose();


        }
    }
   /// <summary>
   /// サーバーであるクラス。クライアントにhyojimanを送る
   /// </summary>
    public class serverman3:kyotuman
    {
       /// <summary>
       /// 普通のコンストラクタ
       /// </summary>
       /// <param name="cou">接続判定のカウント</param>
       /// <param name="buf">バッファーの数</param>
        public serverman3(int cou=80,int buf=4):base(cou,buf)
        {
            //  var s = new tuusinform(true, true);
            //  s.Show();
            
            pc.DataChannelAdded +=(DataChannel chan)=> 
            {
                chan.MessageReceived += SDatarecieved;
            };
            
            var a = pc.AddDataChannelAsync("D1", true,true);
          
                a.Wait();
            

        }
        private void SDatarecieved(byte[] e)
        {
            try
            {
                var a = inputin.andbyte(kaitou(e));
              
                if (a != null)
                {
                    jyusin(a);
                }
            }
            catch (Exception eee) { Console.WriteLine("DATATECIEVEERROR! " + eee.ToString()); }
        }
        /// <summary>
        /// 受け取りメソッド
        /// </summary>
        /// <returns></returns>
        public inputin get()
        {
            return (inputin)getn();
        }
        /// <summary>
        /// 送信メソッド
        /// </summary>
        /// <param name="h"></param>
        public void cast(hyojiman h)
        {
            if (pc.DataChannels.Count < 2)
            {

                return;
            }
            try
            {
                var b = hyojiman.andbyte(h);
                b = asyukku(b);
                {
                    foreach (var d in pc.DataChannels)
                    {

                        d.SendMessage(b);
                        break;
                    }
                }

            }
            catch (Exception eee) { Console.WriteLine("Error on Cast! " + eee.ToString()); }
        }

      
      
     

    }
    /// <summary>
    /// hyojimanを受け取り、inputinを送信するクライアント
    /// </summary>
    public class clientman3 : kyotuman
    {
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="cou">通信継続判定のためのカウント</param>
        /// <param name="buf">バッファーの数</param>
        public clientman3(int cou=80, int buf=4) : base(cou, buf)
        {



            pc.DataChannelAdded += (DataChannel chan) =>
            {
                chan.MessageReceived += CDatarecieved;
            };


            var a = pc.AddDataChannelAsync("D1", true, true);
            
                a.Wait();
           
        }
        
        private void CDatarecieved(byte[] e)
        {

            try
            {


                var a = hyojiman.andbyte(kaitou(e));
                if (a != null)
                {
                    jyusin(a);

                }

            }
            catch (Exception aea) { Console.WriteLine("DATA Recieve Error" + aea); }

        }
/// <summary>
///     受け取りメソッド
/// </summary>
/// <returns></returns>
        public hyojiman get()
        {

            return (hyojiman)getn();
        }
        /// <summary>
        /// 送信メソッド
        /// </summary>
        /// <param name="i"></param>
        public void cast(inputin i)
        {

            if (pc.DataChannels.Count < 2)
            {
                return;
            }
            try
            {

                var b = inputin.andbyte(i);

                castn(b);
            }
            catch
            {

            }
        }
    }


}
