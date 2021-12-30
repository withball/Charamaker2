using Charamaker2;
using Charamaker2.input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace omaegahajimetamonogataridaro
{
    //こちらはセーブデータに当たるクラスのテンプレートです
    [Serializable]
    public class SD
    {
        static public SD S;//楽にアクセスできるように。ロードされてるセーブデータのイメージ

        //音量,画質,キーコンフィグの保存
        float mastervol = 1, kouvol = 1, bgmvol = 0.3f, gasitu = 1f;
        public float mvol { get { return mastervol; } set { mastervol = value; setvols(); } }
        public float kvol { get { return kouvol; } set { kouvol = value; setvols(); } }
        public float bvol { get { return bgmvol; } set { bgmvol = value; setvols(); } }

        public float gsit { get { return gasitu; } set { gasitu = value; if (gasitu < 0.5f) gasitu = 0.5f; if (gasitu > 1.5f) gasitu = 1.5f; } }
        public List<IPC> converts = new List<IPC> { new IPC(Keys.W,Keys.W), new IPC(Keys.S, Keys.S), new IPC(Keys.A, Keys.A), new IPC(Keys.D, Keys.D)
                ,new IPC(Keys.E,Keys.E),new IPC(Keys.Q,Keys.Q),new IPC(Keys.R,Keys.R), new IPC(Keys.Space, Keys.Space)
                ,new IPC(Keys.Escape,Keys.Escape),new IPC(MouseButtons.Left,MouseButtons.Left),new IPC(MouseButtons.Right,MouseButtons.Right)
                ,new IPC(MouseButtons.XButton1,MouseButtons.XButton1),new IPC(MouseButtons.XButton2,MouseButtons.XButton2) };
        void resetIPC()
        {
            converts = new List<IPC> { new IPC(Keys.W,Keys.W), new IPC(Keys.S, Keys.S), new IPC(Keys.A, Keys.A), new IPC(Keys.D, Keys.D)
                ,new IPC(Keys.E,Keys.E),new IPC(Keys.Q,Keys.Q),new IPC(Keys.R,Keys.R), new IPC(Keys.Space, Keys.Space)
                ,new IPC(Keys.Escape,Keys.Escape),new IPC(MouseButtons.Left,MouseButtons.Left),new IPC(MouseButtons.Right,MouseButtons.Right)
                ,new IPC(MouseButtons.XButton1,MouseButtons.XButton1),new IPC(MouseButtons.XButton2,MouseButtons.XButton2) };
        }
        //他にもセーブしたい奴をここに置く
        //シリアライズできないデータの場合は[NonSerialized]をつけて何か他の方法で保存しとく

        public SD()
        {
          
        }
       
     
      
      //音をfilemanに反映する
        public void setvols()
        {
            fileman.glovol = mastervol;
            fileman.glovolbgm = bgmvol;
            fileman.glovolkou = kouvol;

            mastervol = fileman.glovol;
            bgmvol = fileman.glovolbgm;
            kouvol = fileman.glovolkou;
        }

        //今のセーブデータをセーブするメソッド
        static public void savesave()
        {

            var saveData = S;
            S.setcharanum();
            string dir = @".\save\";
            string name = "auto";
            if (Directory.Exists(dir))
            {
            }
            else
            {
                Directory.CreateDirectory(dir);
            }
            BinaryFormatter bF = new BinaryFormatter();
            if (saveData != null)
            {
                Stream fileStream2 = File.Open(dir + name + "checkyou", FileMode.Create);
                bF.Serialize(fileStream2, saveData);
                fileStream2.Close();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (var fs = File.OpenRead(dir + name + "checkyou"))
                {

                    var loadedData = binaryFormatter.Deserialize(fs);
                    fs.Close();
                    var res = (SD)loadedData;
                    if (res != null)
                    {
                        Stream fileStream = File.Open(dir + name, FileMode.Create);

                        bF.Serialize(fileStream, saveData);
                        fileStream.Close();
                        Console.WriteLine("save : OK OK OK");
                    }
                }
            }

        }
        //セーブデータをロードしてDにセットするメソッド
        static public void loadsave()
        {
            string file = "auto";
            SD res = null;

            Object loadedData = null;
            string dir = @".\save\";
            if (Directory.Exists(dir))
            {
            }
            else
            {
                Directory.CreateDirectory(dir);
            }
             dir += file;

            //ファイルを読込
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (var fs = File.OpenRead(dir))
                {

                    loadedData = binaryFormatter.Deserialize(fs);
                    fs.Close();
                    res = (SD)loadedData;
                }
                SD.S = res;
            }
            catch (Exception e){ Console.WriteLine("sfa "+e.ToString()); }

          
        }

    }
}
