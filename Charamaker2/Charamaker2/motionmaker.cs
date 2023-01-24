using Charamaker2.Character;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;

namespace Charamaker2.maker
{
   public partial class motionmaker : Form
    {
        character sel;
        setu selll;
        public motion work;
        public motionmaker(charamaker c)
        {
            InitializeComponent();
            sel = c.sel;
            selll = c.selll;
        }
        public void changesel(character c) 
        {
            sel = c;
           

        }
        public motionmaker()
        {
            InitializeComponent();
        }

        private void motionmaker_Load(object sender, EventArgs e)
        {

        }

        private void applyb_Click(object sender, EventArgs e)
        {
            exbox.Text = "idouman(time,vx,vy,vsita=0,stop=false):キャラの移動" + Environment.NewLine
                + "zureman(time,wwariai,hwariai,stop=false):キャラの大きさの割合分だけいい感じにずれる" + Environment.NewLine
                + "setuidouman(time,setuname,vsita,vdx=0,vdx=0,stop=false):節の回転,dx,dyの移動" + Environment.NewLine
                + "setumageman(time, setuname, tosita, vsita,最短経路を自動で選択=true, stop = false):節を相対角度で曲げる" + Environment.NewLine
                + "radtoman(time,setuname,tosita,vsita,最短経路を自動で選択=true,stop=false):節とかを絶対角度で曲げる指定した名前が\"\"の時はキャラの回転になる" + Environment.NewLine
                + "texpropman(time,setuname,mirror(1->false;0->反転;-1->true),toopa=-1(変更しない),後続も同じ効果を適用するか=true(false かつキャラでキャラのミラーの変更),stop=false):ミラー(471で変更なし)、不透明度の変更。ミラーは瞬時、不透明度はゆっくり変更される。" + Environment.NewLine
                + "texchangeman(setuname,texname):テクスチャーの変更。瞬時に行われる。" + Environment.NewLine
                  + "Kopaman(time, setuname,  基準の不透明度から何倍か, bool kozokumo = true, bool stop = false):関節の不透明度を基準のを基準にして整える" + Environment.NewLine
                + "zchangeman(time,setuname,vz,後続も同じ効果を適用するか=true,stop=false):zを即座に移動する。" + Environment.NewLine
                 + "Kzchangeman(setuname,tosetuname,vz,後続も同じ効果を適用するか=true):zを基準の指定節のz+vzに即座に移動する。" + Environment.NewLine
                + "sizetokaman(time,setuname,vtx,vty,vw=0,vh=0,stop=false):中心の位置txty大きさwhを移動する。指定した名前が\"\"の時はキャラのが変更される" + Environment.NewLine
                + "scalechangeman(time,middletime,setuname,XXchangescaleXX,YYchangescaleYY,後続も同じ効果を適用するか=true,stop=false):指定した節以下のサイズ、中心の位置、節の位置をいっきに変更する指定した節が\"\"の時はすべてに適用される。また効果はtime時間で大きくなりmiddleが終わるまでそのまんま。その後time時間で元の大きさに戻る。middle=-1で元には戻らなくなる。" + Environment.NewLine
                + "tyusinchangeman(time,middletime,setuname,tyusinwariai,bool Y=false,add=false,stop=false):指定した節の中心の位置を指定した位置に変更する（add=trueにすると移動する形になる）。指定した節が\"\"の時はキャラに適用される。また効果はtime時間でずれてmiddleが終わるまでそのまんま。その後time時間で元の中心に戻る。middle=-1で元には戻らなくなる。" + Environment.NewLine
                + "dxchangeman(time,middletime,setuname,dxwariai(root依存),bool Y=false,add=false,stop=false):指定した節のdx,dyの位置を指定した位置に変更する（add=trueにすると移動する形になる）。指定した節が\"\"の時はしっぱい。また効果はtime時間でずれてmiddleが終わるまでそのまんま。その後time時間で元の中心に戻る。middle=-1で元には戻らなくなる。" + Environment.NewLine
                + "Kscalechangeman(time,setuname,XXX,YYY,mode=0 (1 X -1 Y),後続も同じ効果を適用するか=true,追加する形でスケール=false,stop=false):指定した節以下のサイズを基準相対で変更" + Environment.NewLine
                + "Ktyusinchangeman(time,setuname,XXX,YYY,mode=0 (1 X -1 Y),基準の中心から変更=false stop=false):指定した節(\"\"でキャラ)の中心を基準相対で変更" + Environment.NewLine
                + "Kdxychangeman(time,setuname,XXX,YYY,mode=0 (1 X -1 Y),基準のdxyから変更=false, stop=false):指定した節のdxdyを基準相対で変更" + Environment.NewLine
                + "new yureman(回数, 角速度, 角度, 揺れの幅, setuname,幅を大きさ依存に変える=false,角度を相対的にする=false,stop=false):節をいい感じに揺らすsetuname=\"\"のときはキャラが揺れる" + Environment.NewLine
                + "new zkaitenman(時間, 対象(\"\"でキャラ), はじめの角度, 終わりの角度, ,mode=1(1でXのみ-1でYのみ),後続にも適用するか=false,stop=false):いい感じに回転するがその合い札はサイズとか変更不可になる" + Environment.NewLine

                 + " 上記のどれかで " + Environment.NewLine + "work.addmoves(new scalechangeman(100, 100, \"\", 2));   てな具合で書く。" + Environment.NewLine + Environment.NewLine
            + " つかわないやつ " + Environment.NewLine + "zahyosetman(time, x, y, stop=false));中心をx,yに固定し続ける" + Environment.NewLine
            + "stopaaddman(100,stop=false);関節の回転をめっちゃ止める" + Environment.NewLine
            + "playotoman(otoname,volume=0);音を鳴らす" + Environment.NewLine;
         
            try
            {
                var mot=fileman.buildMotion(scriptbox.Text,(float)this.loopud.Value);
                sel.addmotion(mot);
                setmotionjouhou(mot);
            }
            catch (Exception ex)
            {

                exbox.Text += ex.StackTrace + " an " + ex.Message;
            }
        }

        private void motionsave(object sender, EventArgs e)
        {
            try
            {
                string script = scriptbox.Text;
                work = fileman.buildMotion(script);

                fileman.savemotion(script, work);

                exbox.Text += "ok";

            }
            catch { exbox.Text += "no"; }
        }
        internal motion motionmake(string script) 
        {
            try
            {
                work = new motion();

                
                ScriptOptions a = ScriptOptions.Default
                .WithReferences(Assembly.GetEntryAssembly())
                .WithImports("System", "System.Collections.Generic", "Charamaker2.Character", "Charamaker2");

                var Q = CSharpScript.Create(script, options: a, globalsType: typeof(motionmaker));
                var runner = Q.CreateDelegate();
                var run = (Delegate)runner;
                runner(this);

                
            }
            catch { }
            return work;
        }
        private void motionload(object sender, EventArgs e)
        {
           

                var l =fileman.loadmotion(true);
            if (l != null)
            {
                work = l.m;
                scriptbox.Text = l.text;

                setmotionjouhou(l);
            }

            
           
        }

        private void scriptbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void getsetusB_Click(object sender, EventArgs e)
        {
            string seturist="List<string>setus=new List<string>{";
            var lis = this.sel.getallsetu();
                for (int i=0;i<lis.Count;i++) 
            {

                seturist += "\"" + lis[i].nm + "\"";
                if (i < lis.Count - 1) 
                {
                    seturist += ",";
                }
            }
            seturist += "};\n";
            scriptbox.Text = seturist+scriptbox.Text;
        }

        private void quickmotionload(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) 
            {
               var m= fileman.loadmotion(quickload.Text);
                if (m != null)
                {
                    exbox.Text = m.text;
                    setmotionjouhou(m);
                    Clipboard.SetText(m.text);

                }
                else 
                {
                    if (Directory.Exists(@".\motion\"+quickload.Text))
                    {
                        exbox.Text = "";
                        string[] filesM = System.IO.Directory.GetFiles(@".\motion\"+quickload.Text, "*.c2m", System.IO.SearchOption.AllDirectories);
                        for (int i = 0; i < filesM.Count(); i++)
                        {
                            exbox.Text += filesM[i].Replace(@".\motion\", @"") + Environment.NewLine;

                        }
                    }
                }
         
            }
          
        }
        /// <summary>
        /// モーションの情報を表示する
        /// </summary>
        /// <param name="m"></param>
        protected void setmotionjouhou(motion m) 
        {
            timelabel.Text = m.rawendtime.ToString();
        }
        
        /// <summary>
        /// モーションの情報を表示する
        /// </summary>
        /// <param name="m"></param>
        protected void setmotionjouhou(motionsaveman m)
        {
            setmotionjouhou(m.m);
        }

        private void resized(object sender, EventArgs e)
        {
            
        }
    }
}
