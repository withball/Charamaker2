using Charamaker2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Charamaker2.input;

namespace omaegahajimetamonogataridaro
{
    //こちらはForm.csに当たるクラスのテンプレートです。
    //同梱してあるScene.cs,Savedata.csを使ってます
    public partial class display : Form
    {
        public inputin i;//入力をつかさどる
        public STC stc;//ステージをつかさどる
        
        public display()
        {
            
            InitializeComponent();
            this.ClientSize = new Size((int)(1200 ), (int)(675 ));
            //基本となるサイズを設定

            SD.loadsave();
          //セーブデータをロード
            
            if (SD.S == null)//ロードできなかったら作成
            {
                SD.S = new SD();
            }
      
            fileman.setinguping(this,SD.S.gsit);//ファイルマンをセットアップして

            this.ClientSize = new Size((int)(1200 * SD.S.gsit), (int)(675 * SD.S.gsit));
            //サイズを画質に合わせて変える
            
            SD.S.setvols();
            //ボリュームの設定を適用する

            i = new inputin();
            

       //stcを作って
            stc = new STC();
            
            //最初のステージを。
            stc.s = new title(stc);
        }
        private void display_Load(object sender, EventArgs e)
        {

        }
        
        private void keydown(object sender, KeyEventArgs e)
        {
         
            i.down(e.KeyCode,SD.S.converts);//入力する

            inputin.raw.down(e.KeyCode);//キーコンフィグのための生の入力
        }
        
        private void keyup(object sender, KeyEventArgs e)
        {
            i.up(e.KeyCode, SD.S.converts);

            inputin.raw.up(e.KeyCode);
        }
        

        private void ticked(object sender, EventArgs e)
        {
            //マウスポインターの座標をセットして
            i.setpointer(stc.s.hyo, this);
            //一応生のもやっとく
            inputin.raw.setpointer(stc.s.hyo, this);
            //ステージのフレームを行う
            stc.s.frame(i);
          
            i.topre();//入力を後ろへ
            inputin.raw.topre();//生のも
        }

        private void mousedown(object sender, MouseEventArgs e)
        {
            i.down(e.Button, SD.S.converts);
            inputin.raw.down(e.Button);
        }

        private void mouseup(object sender, MouseEventArgs e)
        {

            inputin.raw.up(e.Button);
            i.up(e.Button, SD.S.converts);
        }

        private void resized(object sender, EventArgs e)//リサイズされたとき、画面の比を固定する
        {
            int sum = this.ClientSize.Width + this.ClientSize.Height;
            this.ClientSize = new Size(sum*1200/(1200+675), sum * 675 / (1200 + 675));
        }

  
        private void shown(object sender, EventArgs e)
        {
            //画面が表示された瞬間、いい感じの画面を作ってからファイルとかをロードする。
            var kako = new stage.story.kakkoiisyugou(new STC(),stc.s);
            kako.frame(new inputin());
          //  System.Threading.Thread.Sleep(1000);
            fileman.loadfiletoka();
        }
    }
   
}
