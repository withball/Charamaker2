using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SAVE_THE_SANTACOS_
{
    
    [Serializable]
    public class SD
    {
        //ロードしてるデータのイメージでstaticでインスタンスを置いとくと便利かも
        static public SD S;
        public float aku { get { return _aku; }set { if (value > aku) _aku = value; } }
        public float zen { get { return _zen; } set { if (value > zen) _zen = value; } }
        public float zikeidan { get { return _zikeidan; } set { if (value > zikeidan) _zikeidan = value; } }
       protected float _aku = 0, _zen = 0, _zikeidan = 0;

        public SD()
        {
         

        }
        static public void savesave()
        {

            var saveData = S;
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
            catch (Exception e) { Console.WriteLine("sfa " + e.ToString()); }


        }

    }
}
