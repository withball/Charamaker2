using Charamaker2;
using Charamaker2.input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace GameSet1
{
    /// <summary>
    /// セーブデータのクラス。標準で画質や音量のデータを搭載
    /// </summary>
    [Serializable]
    public class SD
    {
        /// <summary>
        /// 最初のセーブデータロードするよ
        /// </summary>
        /// <param name="savefile"></param>
        /// <typeparam name="T">セーブデータのタイプ</typeparam>
        static public void setup<T>(string savefile="auto") 
        where T:SD
        {
            loadsave<T>(savefile);
            if (SD.S == null)
            {
          //      Console.WriteLine(typeof(T)+" ");
                SD.S = (SD)Activator.CreateInstance(typeof(T));
            }
        }
        /// <summary>
        /// 現在ロードしてるセーブデータ
        /// </summary>
        static public SD S;
        float mastervol = 1, kouvol = 1, bgmvol = 0.3f, gasitu = 1f;
        /// <summary>
        /// マスターボリューム
        /// </summary>
        public float mvol { get { return mastervol; } set { mastervol = value; setvols(); } }
        /// <summary>
        /// 効果音のボリューム
        /// </summary>
        public float kvol { get { return kouvol; } set { kouvol = value; setvols(); } }
        /// <summary>
        /// BGMのボリューム
        /// </summary>
        public float bvol { get { return bgmvol; } set { bgmvol = value; setvols(); } }
        /// <summary>
        /// 画質。こればかりは起動時にfileman.setingupに代入する
        /// </summary>
        public float gsit { get { return gasitu; } set { gasitu = value; if (gasitu < 0.5f) gasitu = 0.5f; if (gasitu > 1.5f) gasitu = 1.5f; } }
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        public SD()
        {
            resetIPC();

        }
        /// <summary>
        /// キーコンフィグ
        /// </summary>
        public List<IPC> converts;

        /// <summary>
        /// キーコンフィグのリセット
        /// </summary>
        virtual public void resetIPC()
        {
            converts = new List<IPC> { new IPC(Keys.W,Keys.W), new IPC(Keys.S, Keys.S), new IPC(Keys.A, Keys.A), new IPC(Keys.D, Keys.D)
                ,new IPC(Keys.E,Keys.E),new IPC(Keys.Q,Keys.Q),new IPC(Keys.R,Keys.R), new IPC(Keys.Space, Keys.Space)
                ,new IPC(Keys.Escape,Keys.Escape),new IPC(MouseButtons.Left,MouseButtons.Left),new IPC(MouseButtons.Right,MouseButtons.Right)
                ,new IPC(MouseButtons.XButton1,MouseButtons.XButton1),new IPC(MouseButtons.XButton2,MouseButtons.XButton2),new IPC(Keys.T,Keys.T)
        ,new IPC(Keys.D1,Keys.D1),new IPC(Keys.D2,Keys.D2),new IPC(Keys.D3,Keys.D3),new IPC(Keys.D4,Keys.D4),new IPC(Keys.D5,Keys.D5)
            ,new IPC(Keys.D6,Keys.D6),new IPC(Keys.D7,Keys.D7),new IPC(Keys.D8,Keys.D8),new IPC(Keys.D9,Keys.D9),new IPC(Keys.D0,Keys.D0) };
        }
        /// <summary>
        /// ボリュームを設定する
        /// </summary>
        public void setvols()
        {
            fileman.glovol = mastervol;
            fileman.glovolbgm = bgmvol;
            fileman.glovolkou = kouvol;

            mastervol = fileman.glovol;
            bgmvol = fileman.glovolbgm;
            kouvol = fileman.glovolkou;
        }

        /// <summary>
        /// セーブする
        /// </summary>
        /// <param name="name">ファイルの名前</param>
        /// <typeparam name="T">セーブデータのタイプ</typeparam>
        static public void savesave<T>(string name="auto")
            where T:SD
        {

            var saveData = S;
            string dir = @".\save\";
          
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
                    var res = (T)loadedData;
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
        /// <summary>
        /// セーブデータをロードする
        /// </summary>
        /// <param name="file">ファイルの名前</param>
        /// <typeparam name="T">セーブデータのタイプ</typeparam>
        static public void loadsave<T>(string file ="auto")
            where T:SD 
        {
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
            if(File.Exists(dir))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (var fs = File.OpenRead(dir))
                {

                    loadedData = binaryFormatter.Deserialize(fs);
                    fs.Close();
                    res = (T)loadedData;
                }
                SD.S = res;
            }


        }

    }
}
