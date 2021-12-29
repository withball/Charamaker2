using Charamaker2;
using Charamaker2.Character;
using Charamaker2.input;
using Charamaker2.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAVE_THE_SANTACOS_
{
    //これでステージの切り替えを実現する
    class stc 
    {
        public stage s;
    }
    //場面の基底クラス。
    class stage
    {
        public stage(stc s)
        { 
            stc = s;
            hyo = fileman.makehyojiman();
            //なんとなく乱数をリセットしてる
            fileman.r = new Random(DateTime.Now.Year+ DateTime.Now.Month+ DateTime.Now.Day+ DateTime.Now.Millisecond+DateTime.Now.Hour+DateTime.Now.Minute+DateTime.Now.Second);
        }
       protected stc stc;
         protected   hyojiman hyo;
        public virtual void frame(inputin i) 
        {
            //ここにフレーム毎の処理を書く
            hyo.hyoji();
            //基底だからこれぐらいでいいと思う
        }
        //さらに背景をランダムで生成するとか、天候要素とかそういう汎用的なのはここにメソッドとして書いとくと便利
    }
    class test : stage 
    {
        public test(stc s) : base(s) 
        {
            fileman.playbgm("tooryanse");
            iron = new ironman(hyo,sc);
            iron.add();
            gamenu = new Rectangle(-hyo.ww, -hyo.wh, hyo.ww*3, hyo.wh*3, 0);
            Entity.ents = new List<Entity>();
            hari = new picture(0, hyo.wh - 80, 100, 80, 80, 40, 40, 0, false, 0.8f, "def", new Dictionary<string, string> { { "def", "meter" } });
            hyo.addpicture(hari);
            hari = new picture(0, hyo.wh - 80, 100.1f, 80, 80, 40, 40, 0, false, 0.8f, "def", new Dictionary<string, string> { { "def", "hari" } });
            hyo.addpicture(hari);
        }
        picture hari;
        ironman iron;
        Shape gamenu;
        const float sc = 0.5f;
        float persu = 50;
        float syutugen = 2;
        int cou = 0;
        public override void frame(inputin i)
        {
            persu += fileman.whrandhani(200) / 100 - 1;
            persu = Math.Min(85, persu);
            persu = Math.Max(50, persu);
            syutugen += 0.001f;
            syutugen = Math.Min(100, syutugen);
            syutugen = Math.Max(2f, syutugen);

            if (fileman.percentin(syutugen)) 
            {
                if (fileman.percentin(persu))
                {
                    cou++;
                    double kaku = fileman.whrandhani(2000) / 1000 * Math.PI;
                    float x = (float)Math.Cos(kaku) * (float)Math.Sqrt(hyo.ww * hyo.ww + hyo.wh * hyo.wh) * 0.8f;
                    float y = (float)Math.Sin(kaku) * (float)Math.Sqrt(hyo.ww * hyo.ww + hyo.wh * hyo.wh) * 0.8f;
                  
                    x += +hyo.ww / 2;
                    y += +hyo.wh / 2;
                    Entity e;
                    if (fileman.percentin(30))
                    {
                      e=  new Entity(hyo, new Circle(0, 0, 0, 0, 0), x, y, 0, 60 * sc, 60 * sc, "kanasimi", "human");
                    }
                    else if (fileman.percentin(50))
                    {
                        e = new Entity(hyo, new Circle(0, 0, 0, 0, 0), x, y, 0, 60 * sc, 60 * sc, "hiai", "human");
                    }
                    else 
                    {
                        e = new Entity(hyo, new Circle(0, 0, 0, 0, 0), x, y, 0, 60 * sc, 60 * sc, "gakkari", "human");
                    }
                    e.add();
                    List<double> rads = new List<double>();
                    rads.Add(Math.Atan2(0 - y, 0 - x));
                    rads.Add(Math.Atan2(0 - y, hyo.ww - x));
                    rads.Add(Math.Atan2(hyo.wh - y, 0 - x));
                    rads.Add(Math.Atan2(hyo.wh - y, hyo.ww - x));
                    double vvx=0, vvy=0;
                    foreach (var a in rads)
                    {
                        float wow = fileman.whrandhani(1001) / 1000;
                        vvx += wow*Math.Cos(a);
                        vvy += wow*Math.Sin(a);
                    }
                    float sp = 2 + fileman.whrandhani(500) / 100;
                    e.vx = sp * (float)Math.Cos(Math.Atan2(vvy, vvx));
                    e.vy = sp * (float)Math.Sin(Math.Atan2(vvy, vvx));
                   // Console.WriteLine(x+" :: "+y+" :: "+e.vx+" :: "+e.vy) ;
                }
                else 
                {
                    double kaku = fileman.whrandhani(2000) / 1000 * Math.PI;
                    float x = (float)Math.Cos(kaku) * (float)Math.Sqrt(hyo.ww * hyo.ww + hyo.wh * hyo.wh) * 1f;
                    float y = (float)Math.Sin(kaku) * (float)Math.Sqrt(hyo.ww * hyo.ww + hyo.wh * hyo.wh) * 1f;

                    x += +hyo.ww / 2;
                    y += +hyo.wh / 2;
                    var e = new Entity(hyo, new Circle(0, 0, 0, 0, 0),x, y, 0, 25*sc, 90*sc, "ramune","ramune");
                    e.add();
                    List<double> rads = new List<double>();
                    rads.Add(Math.Atan2(0 - y, 0 - x));
                    rads.Add(Math.Atan2(0 - y, hyo.ww - x));
                    rads.Add(Math.Atan2(hyo.wh - y, 0 - x));
                    rads.Add(Math.Atan2(hyo.wh - y, hyo.ww - x));
                    double vvx = 0, vvy = 0;
                    foreach (var a in rads)
                    {
                        float wow = fileman.whrandhani(1001) / 1000;
                        vvx += wow * Math.Cos(a);
                        vvy += wow * Math.Sin(a);
                    }
                    float sp = 2 + fileman.whrandhani(500) / 100;
                    e.vx = sp * (float)Math.Cos(Math.Atan2(vvy, vvx));
                    e.vy = sp * (float)Math.Sin(Math.Atan2(vvy, vvx));
                  
                }
            }
            {
                double to = -(Math.PI - Math.PI * iron.energy / 3000f) - hari.RAD;
                to *= 0.1f;
                if (to < -0.02f) 
                {
                    to = -0.02f;
                }
                if (to > 0.02f)
                {
                    to = 0.02f;
                }
                hari.RAD += to;
                // new message(hyo, 0, hyo.wh-32, 32, 20, 0, 0, 0, Math.Round(iron.energy).ToString());
            }
            float R = 0.5f, G = 0.5f, B = 0.5f;
            switch (score.getmode(cou,iron.cou,iron.dark,syutugen))
            {
                case 0:
                    R = 0;G = 1;B = 0;
                  
                    break;
                case 100:
                    R = 1f; G = 1f; B = 0f;
                    break;

                case -100:
                    R = 0.0f; G = 0.0f; B = 0f;
                    break;
                case 10:
                    R = 0.7f; G = 0.7f; B = 0.7f;
                    break;
            }

            message.hutidorin(1,hyo, hyo.ww-32*3, hyo.wh - 32, 32, 20, 20, 0, 0,syutugen.ToString(),R,G,B,false);


            new message( 0, 0, 32, 20, 0, 0, 0,  Math.Round(persu).ToString());
            var lis = new List<Entity>(Entity.ents);
            for (int j = lis.Count - 1; j >= 0; j--)
            {
                var eee = lis[j];
                eee.frame();
                if (!eee.s.atarun(gamenu))
                {
                    Entity.ents.RemoveAt(j);
                    if (eee.p.texname == "human"|| eee.p.texname == "softviran"|| eee.p.texname == "yami")
                    {
                      
                        fileman.playoto("hoaa");
                        if (eee.p.texname == "yami") 
                        {
                            var e = new viran(hyo, eee.p.gettx(), eee.p.getty(), sc);

                            float sp = 3.5f + fileman.whrandhani(500) / 100;
                            var kaku = gamenu.nasukaku(e.p.gettx(), e.p.getty()) + Math.PI;
                            e.vx = sp * (float)Math.Cos(kaku);
                            e.vy = sp * (float)Math.Sin(kaku);
                            e.frame();
                            e.frame();
                            e.frame();
                            e.add();
                        }
                        {
                         
                            var e = new viran(hyo, eee.p.gettx(), eee.p.getty(), sc);

                           

                            float sp = 3.5f + fileman.whrandhani(500) / 100;
                            var kaku = iron.s.nasukaku(e.p.gettx(), e.p.getty()) + Math.PI;
                            e.vx = sp * (float)Math.Cos(kaku);
                            e.vy = sp * (float)Math.Sin(kaku);
                            e.frame();
                            e.frame();
                            e.frame();
                            e.add();
                        }
                    }
                    else if(eee.p.texname!="iron")
                    {
                        Entity.ents.Remove(eee);
                    }

                }
            }
            {
              
                iron.frame2(i);
            }
           
            base.frame(i);
            if (iron.energy <= 0)
            {
                if (cou > 0)
                {
                    stc.s = new score(stc, cou, iron.cou, iron.dark,syutugen);
                }
                else 
                {
                    stc.s = new score(stc,0,0,0,0);
                }
            }
        }
    }
     class title : stage 
    {
        
        public title(stc s) : base(s) 
        {
          
            fileman.playbgm("stop");
        
        
            message.hutidorin(1,hyo, hyo.ww/2, hyo.wh /2-16, 32, 40, 20, 0, -1, "SAVE THE FACES!",0,0,0);

            if (SD.S.aku > 0)
            {
                message.hutidorin(1, hyo, hyo.ww / 2, hyo.wh / 2 + 16, 32, 40, 20, 0, -1, "悪Score:" + (SD.S.aku*100).ToString(), 0, 0, 0, false);
            }
            if (SD.S.zen > 0)
            {
                message.hutidorin(1, hyo, hyo.ww / 2, hyo.wh / 2 + 16+32, 32, 40, 20, 0, -1, "善Score:" + (SD.S.zen*100).ToString(), 0, 0, 0, false);
            }
            if (SD.S.zikeidan > 0)
            {
                message.hutidorin(1, hyo, hyo.ww / 2, hyo.wh / 2 + 16+60, 32, 40, 20, 0, -1, "自警団Score:" + (SD.S.zikeidan*100).ToString(), 0, 0, 0, false);
            }
            var p=new ironman(hyo,3).p;
            hyo.addpicture(p);
            p.settxy(hyo.ww/2, hyo.wh/2);
            message.hutidorin(2,hyo,0,0,32,100,0,0,-1,"左クリック:開始\n右クリック:説明",0,0,0,false);
        }
        public override void frame(inputin i)
        {
         
            base.frame(i);
            if (i.ok(MouseButtons.Left, itype.down)) 
            {
                stc.s = new test(stc);
            }
            if (i.ok(MouseButtons.Right, itype.down))
            {
                stc.s = new setumei(stc);
            }

        }

    }
    class setumei : stage
    {
    
        public setumei(stc s) : base(s)
        {
            fileman.playbgm("tooryanse");
            message.hutidorin(1,hyo, hyo.ww / 2, hyo.wh / 2 - 90, 32, 80, 45, 0, -1, "エネルギーが尽きるまで人々を救おう！\nエネルギーは移動と人を助けることでを消費される。\n" +
                "助けなかった人間はヴィランとなって襲い掛かってくる。\n"+"しかし、全ての人間を助けられるわけではない……\n"+"より多くの人々を救うのだ。", 0, 0, 0);

            message.hutidorin(2, hyo, 0, 0, 32, 100, 0, 0, -1, "ゲーム内操作\n左クリック:ブレーキ\n右クリック:ブレード", 0, 0, 0, false);
            var p = new ironman(hyo, 3).p;
            hyo.addpicture(p);
            p.settxy(hyo.ww / 2, hyo.wh / 2);
            
        }
        public override void frame(inputin i)
        {
          
            base.frame(i);
            if (i.ok(MouseButtons.Right, itype.up))
            {
                stc.s = new title(stc);
            }
        }

    }
    class score : stage
    {

        static public int getmode(int cou, int tasu, int dark, float ritu) 
        {
           
            if (cou < 40)
            {
                return 0;
         
            }
            else if (tasu / (float)cou > 0.7)
            {
                return 100;
             

            }
            else if (dark -tasu> 5)
            {
                return -100;
             
            }
            else
            {
                return 10;
               
            }
        }
        public score(stc s, int cou, int tasu, int dark, float ritu) : base(s)
        {
            int mode = getmode(cou, tasu, dark, ritu);
            fileman.playbgm("tooryanse");
            string text = "";
            switch (mode)
            {
                case 0:

                    SD.S.zikeidan = ritu;
                    text = "アイロンマン？ああ、\nあの最近出て来た奴のことか?";
                    break;
                case 100:
                    SD.S.zen = ritu;

                    text = "ありがとう！俺らのヒーロー！";
                    break;

                case -100:

                    SD.S.aku = ritu;
                    if (fileman.percentin(50))
                    {
                        text = "速報です。国際連合がアイロンマンを\n国際指名手配犯に指定しました。";
                    }
                    else
                    {
                        text = "その名前を出すな！！！\n俺の家族は……みんなそいつに殺されちまった……";
                    }
                    break;
                case 10:

                    SD.S.zikeidan = ritu;
                    if (fileman.percentin(50))
                    {
                        text = "アイロンマンは危険な自警団だ。\n今すぐに取り締まるべきだ。絶対な。";
                    }
                    else
                    {
                        text = "俺はアイロンマンのこと応援してるぜ！\nカッコイイいだろ！？あんな飛びまわって！";
                    }
                    break;
            }
        
            message.hutidorin(1, hyo, hyo.ww / 2, hyo.wh / 2 - 90, 32, 80, 45, 0, -1, text+"\nScore:" + (ritu*100).ToString(), 0, 0, 0);
            message.hutidorin(2, hyo, 0, 0, 32, 100, 0, 0, -1, "タイトルへ:右押しながら左クリック", 0, 0, 0, false);
            var p = new ironman(hyo, 3).p;
            hyo.addpicture(p);
            p.settxy(hyo.ww / 2, hyo.wh / 2);
            SD.savesave();
        }
        public override void frame(inputin i)
        {
           
            base.frame(i);
            if (i.ok(MouseButtons.Left, itype.down)&& i.ok(MouseButtons.Right, itype.ing))
            {
                stc.s = new title(stc);
            }
        }

    }
}
