using Charamaker2;
using Charamaker2.Shapes;
using Charamaker2.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Charamaker2.input;

namespace omaegahajimetamonogataridaro.stage
{
    //こちらはシーンに当たるクラスのテンプレートです。
    public class STC //ステージを切り替えるためのデータ
    {
        public Stage s;

    }//STC.s=new stage(STC);とすることで切り替わる感じに

  //こいつがSceneに当たる
    public class Stage
    {
        protected STC stc;
        public hyojiman hyo;
        public Stage(STC s) 
        {
            stc = s;
            hyo = fileman.makehyojiman();
            setup();
        }
    
        virtual public void frame(inputin i) 
        {
            
            hyo.hyoji();
          
        }
        

        protected virtual void setup() 
        {
         
        }
        /// <summary>
        /// 背景を生成するメソッド
        /// </summary>
        /// <param name="yline">背景を生成する下線</param>
        /// <param name="ybure">y方向のブレ</param>
        /// <param name="sx">xの始点</param>
        /// <param name="ex">xの終点</param>
        /// <param name="ikutu">いくつの絵を生成するか</param>
        /// <param name="kaisuu">何週繰り返すか</param>
        /// <param name="haikeis">生成する背景のリスト。等確率でどれか一つが生成される。</param>
        /// <param name="assyuku">sx=-100,ex=100のときにスクロール割合が0.1のとき、10~-10に生成するようにするか</param>
        public void haikeiseiser(float yline, float ybure, float sx, float ex, float ikutu, float kaisuu, List<haikei> haikeis, bool assyuku = true)
        {
            hyojiman hyojiman = hyo;
            float dx = (ex - sx) / ikutu;
            int cou = 0;
            if (haikeis.Count > 0 && dx > 0)
            {

                for (int yuyu = 0; yuyu < kaisuu; yuyu++)
                {
                    float tx = 0;
                    for (int i = 0; i < ikutu; i++)
                    {
                        tx += fileman.r.Next() % (dx + (dx * i - tx));
                        int ttt = fileman.r.Next() % haikeis.Count();
                        float si = haikeis[ttt].size;
                        if (haikeis[ttt].sizebure > 0) si += fileman.r.Next() % (haikeis[ttt].sizebure * 2 + 1) - haikeis[ttt].sizebure;
                      
                        bool mir = fileman.percentin(50);

                        float ty = yline;
                        if (ybure > 0) ty += fileman.r.Next() % (ybure * 2 + 1) - ybure;
                        float pppxpxpx = (sx + tx);
                        if (assyuku) pppxpxpx *= haikeis[ttt].scrollwarix;
                        var p = new picture(pppxpxpx, ty - si, -100000 + cou, si, si, 0, si, 0, mir, 1, haikeis[ttt].name, new Dictionary<string, string> { { haikeis[ttt].name, haikeis[ttt].name } });
                        hyojiman.addhaikeipicture(new haikeidraws(haikeis[ttt].scrollwarix,haikeis[ttt].scrollwariy, p));
                  
                        cou++;
                    }
                }
            }
        }
        float raintime = 0;
        List<List<effectchara>> tenkos = new List<List<effectchara>>();
        int cou = 0;

        /// <summary>
        /// 天候を生成するメソッド。1フレームごとに呼び出す
        /// </summary>
        /// <param name="size">画像のサイズ</param>
        /// <param name="mitudo">画像の密度</param>
        /// <param name="kakudo">雨とかが降ってくる角度</param>
        /// <param name="kankaku">雨とかが生成される間隔</param>
        /// <param name="eff">天候を表すテクスチャー</param>
        /// <param name="time">何フレームぐらいで画面に表れて消えるのか。実質速度</param>
        /// <param name="cl">クロック</param>
        public void rainseiser(float size = 128, float mitudo = 3, float kakudo = 0, float kankaku = 10, string eff = @"effects\rain", float time = 50,float cl=1)
        {
            hyojiman hyojiman = hyo;
            if (raintime > 0)
            {
                raintime -= cl;
            }
            else
            {

                float raisi = size;
                float mitu = mitudo;
                double kaku = kakudo / 180 * Math.PI;
                if (kaku >= Math.PI / 3) kaku = Math.PI / 3;
                if (kaku <= -Math.PI / 3) kaku = -Math.PI / 3;
                raintime = kankaku;
                if (cou >= tenkos.Count()) tenkos.Add(new List<effectchara>());
                int ccc = 0;
                for (float i = -(hyojiman.ww + hyojiman.wh * (float)Math.Tan(kaku)) * 0.7f * mitu / raisi; i <= (hyojiman.ww + hyojiman.wh * (float)Math.Tan(kaku)) * (1 + 0.3f) * mitu / raisi; i++)
                {

                    float hhh = fileman.whrandhani(raisi * 2);
                    float www = fileman.whrandhani(raisi / 3) - raisi / 6;
                    float opapa = 0.6f - fileman.r.Next() % 30 / 100f;
                    double sikajika = kaku + (fileman.r.NextDouble() - 0.5) * Math.PI / 40;
                    if (tenkos[cou].Count() <= ccc)
                    {

                        var rain = new effectchara(hyojiman,time * 1.6f * 3f, hyojiman.camx + raisi / mitu * i + www, hyojiman.camy - raisi - hhh, raisi, raisi, raisi / 2, raisi / 2, 0,
                            new setu("rain", 0, 0, new picture(0, 0, 1000, raisi, raisi, raisi / 2, raisi / 2, sikajika, fileman.percentin(50), opapa, eff, new Dictionary<string, string> { { eff, eff } }), new List<setu>()));
                        tenkos[cou].Add(rain);
                    }
                    else
                    {
                        tenkos[cou][ccc].resetmotion();
                        tenkos[cou][ccc].x = hyojiman.camx + raisi / mitu * i + www;
                        tenkos[cou][ccc].y = hyojiman.camy - raisi - hhh;
                        tenkos[cou][ccc].w = raisi;
                        tenkos[cou][ccc].h = raisi;
                        tenkos[cou][ccc].tx = raisi / 2;
                        tenkos[cou][ccc].ty = raisi / 2;
                        tenkos[cou][ccc].core.p.OPA = opapa;
                        tenkos[cou][ccc].core.p.RAD = sikajika;
                        tenkos[cou][ccc].core.p.mir = fileman.percentin(50);
                        tenkos[cou][ccc].core.p.w = raisi;
                        tenkos[cou][ccc].core.p.h = raisi;
                        tenkos[cou][ccc].core.p.tx = raisi / 2;
                        tenkos[cou][ccc].core.p.ty = raisi / 2;
                        if (!tenkos[cou][ccc].core.p.textures.ContainsKey(eff))
                            tenkos[cou][ccc].core.p.textures.Add(eff, eff);

                        tenkos[cou][ccc].core.p.texname = eff;
                        tenkos[cou][ccc].time = time * 1.6f * 3f;
                        tenkos[cou][ccc].add();
                    }
                    tenkos[cou][ccc].addmotion(new motion(new idouman(time * 1.6f, hyojiman.wh / time * (float)Math.Tan(-sikajika), hyojiman.wh / time)));
                    ccc++;
                }
                cou++;
                if (cou > time * 1.6f / kankaku) cou = 0;
            }
        }
        float kumotime = 0;
        List<List<effectchara>> kumos = new List<List<effectchara>>();
        int coukumo = 0;
        public void kumoseiser(float size = 128, float mitudo = 2, float atusa = 50, float kankaku = 10, string eff = @"cloud", float time = 200)
        {
            hyojiman hyojiman = hyo;
            if (kumotime > 0)
            {
                kumotime -= 1;
            }
            else
            {

                float raisi = size;
                float mitu = mitudo;

                kumotime = kankaku;
                if (coukumo >= kumos.Count()) kumos.Add(new List<effectchara>());
                int ccc = 0;
                for (float i = -1; i <= (atusa) * (1) * mitu / raisi; i++)
                {

                    float hhh = fileman.whrandhani(atusa);
                    float www = fileman.whrandhani(raisi);
                    float opapa = fileman.whrandhani(30) / 100f + 0.4f;
                    if (kumos[coukumo].Count() <= ccc)
                    {
                        var rain = new effectchara(hyojiman,time * 1.6f * 3f, hyojiman.camx + raisi + www + hyojiman.ww, hyojiman.camy - raisi + i * raisi / mitu + hhh, raisi, raisi, raisi / 2, raisi / 2, 0,
                            new setu("rain", 0, 0, new picture(0, 0, 1010, raisi, raisi, raisi / 2, raisi / 2, 0, fileman.percentin(50), opapa, eff, new Dictionary<string, string> { { eff, eff } }), new List<setu>()));
                        kumos[coukumo].Add(rain);
                    }
                    else
                    {
                        kumos[coukumo][ccc].resetmotion();
                        kumos[coukumo][ccc].x = hyojiman.camx + raisi + www + hyojiman.ww;
                        kumos[coukumo][ccc].y = hyojiman.camy - raisi + i * raisi / mitu + hhh;
                        kumos[coukumo][ccc].w = raisi;
                        kumos[coukumo][ccc].h = raisi;
                        kumos[coukumo][ccc].tx = raisi / 2;
                        kumos[coukumo][ccc].ty = raisi / 2;
                        kumos[coukumo][ccc].core.p.OPA = opapa;
                        kumos[coukumo][ccc].core.p.RAD = 0;
                        kumos[coukumo][ccc].core.p.mir = fileman.percentin(50);
                        kumos[coukumo][ccc].core.p.w = raisi;
                        kumos[coukumo][ccc].core.p.h = raisi;
                        kumos[coukumo][ccc].core.p.tx = raisi / 2;
                        kumos[coukumo][ccc].core.p.ty = raisi / 2;
                        if (!kumos[coukumo][ccc].core.p.textures.ContainsKey(eff))
                            kumos[coukumo][ccc].core.p.textures.Add(eff, eff);

                        kumos[coukumo][ccc].core.p.texname = eff;
                        kumos[coukumo][ccc].time = time * 1.6f * 3f;
                        kumos[coukumo][ccc].add();
                    }
                    kumos[coukumo][ccc].addmotion(new motion(new idouman(time * 1.6f * 2, -hyojiman.ww / time, 0)));
                    kumos[coukumo][ccc].addmotion(new motion(new yureman((int)(time * 5), fileman.whrandhani(10) + 1, 90, fileman.whrandhani(kumos[coukumo][ccc].h / 10), "")));

                    ccc++;
                }
                coukumo++;
                if (coukumo > time * 1.6f / kankaku) coukumo = 0;
            }
        }

    }
    public class haikei
    {
        public string name;
        public float size;
        public float sizebure;
        public float scrollwarix;
        public float scrollwariy;
        /// <summary>
        /// 背景を生成するために使うクラス
        /// </summary>
        /// <param name="texnm">テクスチャーの名前</param>
        /// <param name="si">大きさ(幅と高さは等しくこれ)</param>
        /// <param name="bure">サイズのブレ</param>
        /// <param name="scx">x方向のスクロール割合</param>
        /// <param name="scy">y方向のスクロール割合</param>
        public haikei(string texnm, float si, float bure, float scx, float scy)
        {
            name = texnm;
            size = si;
            sizebure = bure;
            scrollwarix = scx;
            scrollwariy = scy;
        }
    }




}
