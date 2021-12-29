using Charamaker2;
using Charamaker2.Character;
using Charamaker2.input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAVE_THE_SANTACOS_
{
    public partial class Form1 : Form
    {
        /*
           サンタコスにしてあげよう。移動とサンタコス化にはエネルギーが必要だぞ！
        　　エネルギーはラムネを取ることで補給できる。エネルギーが尽きたらゲームオーバーだ！
        　　ヒーローは無限の力を持つわけではない。救える命と救えない命があるのだ……。そこを見誤り、救えるはずのより多くの人間を殺してはならない。
        　　例えその人間が愛する人だったとしても……ヒーローならば……。
        　　その力を発揮し、責務を果たせ！アイロンマン！
         
         */
        public Form1()
        {
            InitializeComponent();
            
            //セーブをロードする！画質とかの設定があるのでsetingupの前に！
            SD.loadsave();
            if (SD.S == null) 
            {
                SD.S = new SD();
            }

            fileman.setinguping(this, 1.2f);
            this.Size = new Size((int)(this.Size.Width * 1.4f), (int)(this.Size.Width * 1.2f));
            hyo = fileman.makehyojiman();
            stc.s = new title(stc);
            stc.s.frame(i);
            //ロードの時は全て止まっちゃうので一回表示しとく
            fileman.loadfiletoka();
            
        }
        hyojiman hyo;
        inputin i = new inputin();
        
        //こういうのをセーブデータにぶち込んどけばキーコンフィグの出来上がり。スペースキーがLeftと同等の働きをするよ！
        List<IPC> converts = new List<IPC> {new IPC(Keys.Space,MouseButtons.Left) };
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //入力の接続このゲームはキーボードは使わないが全部接続しとくわ！
        private void mousedown(object sender, MouseEventArgs e)
        {

            i.down(e.Button,converts);
            //キーコンフィグのする場合はrawに入れとかないと生の入力が追えなくなっちゃう。
            inputin.raw.down(e.Button);
        }

        private void mouseup(object sender, MouseEventArgs e)
        {
            i.up(e.Button, converts);
        }

        private void keydown(object sender, KeyEventArgs e)
        {
            i.down(e.KeyCode, converts);
        }

        private void keyup(object sender, KeyEventArgs e)
        {
            i.up(e.KeyCode, converts);
        }
        //こいつの中身を変えて場面を変更する。
        stc stc = new stc();
        private void ticked(object sender, EventArgs e)
        {
            //マウスのセットと
            i.setpointer(hyo,this);
            //場面のフレームと
            stc.s.frame(i);
            //入力のフレーム処理
            i.topre();
        }

        private void shown(object sender, EventArgs e)
        {
        }

   
    }
}
