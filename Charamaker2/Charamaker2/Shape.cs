using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Mathematics;
using Vortice.Direct2D1;
using System.Numerics;
using System.Drawing;
/// <summary>
/// 図形を扱う名前空間
/// </summary>
namespace Charamaker2.Shapes
{
    /// <summary>
    /// 図形の基底クラス
    /// </summary>
    public abstract class Shape
    {
        /// <summary>
        /// 図形の基本要素
        /// </summary>
        public float x, y, w, h;
        /// <summary>
        /// カクード
        /// </summary>
        protected double _rad;
        /// <summary>
        /// セットする際は-Pi＜＝rad＜＝Piの範囲にする。そして重心で回転させる
        /// </summary>
        public double rad { get { return _rad; }
            set {
                float x = gettx();
                float y = getty();
                _rad = value;
                settxy(x, y);
                _rad = Math.Atan2(Math.Sin(_rad), Math.Cos(_rad));
            } }
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="xx">ｘ座標</param>
        /// <param name="yy">ｙ座標</param>
        /// <param name="ww">幅</param>
        /// <param name="hh">高さ</param>
        /// <param name="radd">角度</param>
        public Shape(float xx, float yy, float ww, float hh, double radd )
        {
            
            x = xx;
            y = yy;
            w = ww;
            h = hh;
            _rad = Math.Atan2(Math.Sin(radd), Math.Cos(radd));
        }
        /// <summary>
        /// 図形上の一点のx座標を取得する(回転の影響を考慮してるってこと)
        /// </summary>
        /// <param name="ww">左上を0としたときの図形の点の位置</param>
        /// <param name="hh">左上を0としたときの図形の点の位置</param>
        /// <returns>返されるのはx座標の値</returns>
        public float getcx(float ww, float hh)
        {
            float W = ww;
            float H = hh;
            float rx = x + W * (float)Math.Cos(rad) - H * (float)Math.Sin(rad);
            return rx;
        }
        /// <summary>
        /// 図形上の一点のy座標を取得する(回転の影響を考慮してるってこと)
        /// </summary>
       /// <param name="ww">左上を0としたときの図形の点の位置</param>
        /// <param name="hh">左上を0としたときの図形の点の位置</param>
        /// <returns>返されるのはy座標の値</returns>
         public float getcy(float ww, float hh)
        {
            float W = ww;
            float H = hh;
            float ry = y + W * (float)Math.Sin(rad) + H * (float)Math.Cos(rad);
            return ry;
        }
        /// <summary>
        ///　重心のx座標を返す
        /// </summary>
        /// <returns>返されるのはx座標の値</returns>
        virtual public float gettx()
        {

            return x + w / 2 * (float)Math.Cos(rad) - h / 2 * (float)Math.Sin(rad);

        }
        /// <summary>
         ///　重心のy座標を返す
         /// </summary>
         /// <returns>返されるのはy座標の値</returns>
        virtual public float getty()
        {
            return y + w / 2 * (float)Math.Sin(rad) + h / 2 * (float)Math.Cos(rad);

        }
        /// <summary>
        /// 重心をxy座標にセットする。
        /// </summary>
        /// <param name="xx">セットするx座標</param>
        /// <param name="yy">セットするy座標</param>
        virtual public void settxy(float xx, float yy)
        {
            x += xx - gettx();
            y += yy-getty();
            
        }
        /// <summary>
        /// 図形上の任意の一点をxy座標にセットする
        /// </summary>
        /// <param name="xx">セットするx座標</param>
        /// <param name="yy">セットするy座標</param>
        /// <param name="cw">図形上のwの点</param>
        /// <param name="ch">図形上のhの点</param>
        public void setcxy(float xx, float yy, float cw, float ch)
        {
            x = xx - cw * (float)Math.Cos(rad) + ch * (float)Math.Sin(rad);
            y = yy - cw * (float)Math.Sin(rad) - ch * (float)Math.Cos(rad);
        }
        /// <summary>
        /// 任意の点で図形の角度をセットする
        /// </summary>
        /// <param name="trad">回転角</param>
        /// <param name="cw">図形上のwの点</param>
        /// <param name="ch">図形上のhの点</param>
        public void setradcxy(double trad, float cw, float ch)
        {
            trad = Math.Atan2(Math.Cos(trad), Math.Cos(trad));
            float x = getcx(cw, ch);
            float y = getcy(cw, ch);
            rad = trad;
            setcxy(x, y, cw, ch);
        }
        /// <summary>
        /// 図形を描画する
        /// </summary>
        /// <param name="hyojiman">描画するhyojiman</param>
        /// <param name="R">線の色</param>
        /// <param name="G">線の色</param>
        /// <param name="B">線の色</param>
        /// <param name="A">線の不透明度</param>
        /// <param name="begin">hyojimanのbegindrawをついでにするか(すると重くなるので外部でやるのがおススメ)</param>
        public void drawshape(hyojiman hyojiman, float R, float G, float B, float A, bool begin = false)
        {
            if (begin)
            {
                hyojiman.render.BeginDraw();
            }
            drawn(new Color4(R,G,B,A), hyojiman);
            if (begin)
            {
                hyojiman.render.EndDraw();
            }

        }
        /// <summary>
        /// 図形それぞれの描画の部分の本体
        /// </summary>
        /// <param name="col">カラー</param>
        /// <param name="hyo">表示する画面</param>
        abstract protected void drawn(Color4 col, hyojiman hyo);
        /// <summary>
        /// その点が図形内にあるかのどうかの判定
        /// </summary>
        /// <param name="px">その点のx座標</param>
        /// <param name="py">その点のy座標</param>
        /// <returns>あるか</returns>
        abstract public bool onhani(float px, float py);
        /// <summary>
        /// その線が図形と接触するかどうかの判定
        /// </summary>
        /// <param name="px">その点のx座標1</param>
        /// <param name="py">その点のy座標1</param>
        /// <param name="ppx">その点のx座標2</param>
        /// <param name="ppy">その点のy座標2</param>
        /// <returns></returns>
        abstract public bool onhani(float px, float py,float ppx,float ppy);
        /// <summary>
        /// 図形の外周の辺のリストを手に入れる。引数無しで相対座標で出してくれる
        /// </summary>
        /// <param name="x">中心座標x</param>>
        /// <param name="y">中心座標y</param>>
        /// <param name="rad">回転角</param>>
        /// <returns>外周のリスト(相対座標){{x1,y1,x2,y2},{x1,y1,x2,y2}}</returns>
        abstract protected List<double[]> getgaisyuus(float x = 0, float y = 0,double rad=0);
    
        
        /// <summary>
        /// 外周を絶対座標にして返す
        /// </summary>
        /// <returns>外周のリスト(絶対座標)</returns>
        protected List<double[]> getgaisyuus2()
        {
            var lis = getgaisyuus();
            double a, b, c, d;
            for (int i = 0; i < lis.Count; i++)
            {

                a = lis[i][0] * Math.Cos(rad) - lis[i][1] * Math.Sin(rad);
                b = lis[i][0] * Math.Sin(rad) + lis[i][1] * Math.Cos(rad);

                c = lis[i][2] * Math.Cos(rad) - lis[i][3] * Math.Sin(rad);
                d = lis[i][2] * Math.Sin(rad) + lis[i][3] * Math.Cos(rad);
                lis[i][0] = a + gettx();
                lis[i][1] = b + getty();
                lis[i][2] = c + gettx();
                lis[i][3] = d + getty();
            }
            return lis;
        }
        /// <summary>
        /// 図形を複製する
        /// </summary>
        /// <returns>複製された図形</returns>
        abstract public Shape clone();
        /// <summary>
        /// 図形の重心からの最大の距離
        /// </summary>
        /// <returns>その射程</returns>
        abstract public float syatei();

        /// <summary>
        ///図形と図形の重心の距離を測る
        /// </summary>
        /// <param name="s">その図形の片割れ</param>
        /// <returns>距離</returns>
        public float kyori(Shape s)
        {
            return (float)Math.Sqrt(Math.Pow(s.gettx() - gettx(), 2) + Math.Pow(s.getty() - getty(), 2));
        }
       
        /// <summary>
        /// 図形の重心とある座標の距離を測る
        /// </summary>
        /// <param name="px">そのx座標</param>
        /// <param name="py">そのy座標</param>
        /// <returns>距離</returns>
        public float kyori(float px, float py)
        {
            return (float)Math.Sqrt(Math.Pow(px - gettx(), 2) + Math.Pow(py - getty(), 2));
        }
        /// <summary>
        /// 図形と図形の重心の紡ぐ線の角度を計る
        /// </summary>
        /// <param name="s">その図形</param>
        /// <returns>角度</returns>
        public double nasukaku(Shape s)
        {
            return (float)Math.Atan2(s.getty() - getty(), s.gettx() - gettx());
        }
        /// <summary>
        /// 図形の重心とある座標の紡ぐ線の角度を測る
        /// </summary>
        /// <param name="px">そのx座標</param>
        /// <param name="py">そのy座標</param>
        /// <returns>角度</returns>
        public double nasukaku(float px,float py)
        {
            return (float)Math.Atan2(py - getty(), px - gettx());
        }
        /// <summary>
        /// 指定した点に必要であろう図形を構成する頂点を返す。
        /// </summary>
        /// <param name="px">その点のx座標</param>
        /// <param name="py">その点のy座標</param>
        /// <param name="syatei">何かに使えるかなと思ってたけど使ってないやーつ</param>
        /// <returns>ポイントの束</returns>
        virtual public List<PointF> getpoints(float px, float py, float syatei) 
        {
            var res = new List<PointF>();
     
            res.Add(new PointF(gettx(),getty()));
            return res;
        }
        /// <summary>
        /// 指定した点に適した法線ベクトルを返す
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <returns></returns>
        abstract public double gethosen(float px, float py);
        /// <summary>
        /// 外周が時計回りで並んでいるかどうか
        /// </summary>
        /// <returns>時計回りか？</returns>
        public bool tokeimawari2() 
        {
           return tokeimawari(getgaisyuus());
        }
        /// <summary>
        /// 外周が時計回りで並んでいるかどうか
        /// </summary>
        /// <param name="gaisyuu">なんでもいいから外周</param>
        /// <returns></returns>
        protected bool tokeimawari(List<double[]>gaisyuu) 
        {
            if (gaisyuu.Count >= 2) 
            {
                var a = Math.Atan2(gaisyuu[0][3] - gaisyuu[0][1], gaisyuu[0][2] - gaisyuu[0][0]);
                var b = Math.Atan2(gaisyuu[1][3] - gaisyuu[1][1], gaisyuu[1][2] - gaisyuu[1][0]);
                return Math.Atan2(Math.Sin(b - a), Math.Cos(b - a)) >= 0;
            }
            return true;
        }
        /// <summary>
        /// 対象の角の法線ベクトルを出す
        /// </summary>
        /// <param name="gaisyuu">なんでもいいから外周</param>
        /// <param name="taisyo"></param>
        /// <returns></returns>
        protected double hosenton(List<double[]> gaisyuu, double taisyo) 
        {
            if (tokeimawari(gaisyuu))
            {
                return Math.Atan2(Math.Sin(taisyo - Math.PI), Math.Cos(taisyo - Math.PI));
            }
            else
            {
                return Math.Atan2(Math.Sin(taisyo + Math.PI), Math.Cos(taisyo + Math.PI));
            }
        }
        /// <summary>
        /// gethosen2のための角度割り出しマン
        /// </summary>
        /// <param name="s">相手の図形</param>
        /// <param name="gaisyu">自分の絶対外周</param>
        /// <param name="hoss">絶対外周の法線ベクトルが外側を向くための補正角度</param>
        /// <returns></returns>
        protected double tasuuketun(Shape s, List<double[]> gaisyu)
        {
            var hoss = Math.PI / 2;
            if (tokeimawari(gaisyu)) 
            {
                hoss *= -1;
            }
            List<double> k = new List<double> ();
            var aas = s.getpoints(s.gettx(),s.getty(), s.syatei());
            double res = 0;
            List<double> kkkke = new List<double>();
            for (int i = 0; i < gaisyu.Count; i++)
            {
                kkkke.Add(Math.Atan2(gaisyu[i][3] - gaisyu[i][1], gaisyu[i][2] - gaisyu[i][0]));
                k.Add(0);
          
            }

            for (int i = 0; i < aas.Count; i++)
            {
                for (int t = 0; t < kkkke.Count; t++)
                {
                    var kp = Math.Abs(
                        Math.Atan2(Math.Sin(Math.Atan2(-aas[i].Y + (gaisyu[t][3] + gaisyu[t][1]) / 2, -aas[i].X + (gaisyu[t][2] + gaisyu[t][0]) / 2) - kkkke[t] + hoss)
                                 , Math.Cos(Math.Atan2(-aas[i].Y + (gaisyu[t][3] + gaisyu[t][1]) / 2, -aas[i].X + (gaisyu[t][2] + gaisyu[t][0]) / 2) - kkkke[t] + hoss))
                        );
                     k[t] +=kp;
                }
               

            }
            int cou = 0;
          
            for (int i = 0; i < k.Count; i++)
            {
                //  Console.WriteLine(Math.Atan2(Math.Sin(kkkke[i] + hoss), Math.Cos(kkkke[i] + hoss)) / Math.PI * 180 + " hyou : " + k[i]);
                  if (k.Min() == k[i])
                   {
                       res += kkkke[i];
                       cou++;
                   }
        //kkk           Console.WriteLine(kkkke[i]*180/Math.PI + "aslfk -> "+k[i]);
               
          
            }
            if (cou==k.Count)
            {
                return nasukaku(s);
            }

         //   res = Math.Atan2(py, px);
           
      //kkk      Console.WriteLine(res/cou * 180 / Math.PI + " VYVYVYVYVV " + hoss*180/Math.PI);

            res = Math.Atan2(Math.Sin(res / cou + hoss), Math.Cos(res / cou + hoss));
            return res;
        }

        /// <summary>
        /// 相手のgetpointsをも呼び出していい感じの法線ベクトルを返す。おそらく物理的な奴向き？
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        abstract public double gethosen2(Shape s);
        /// <summary>
        /// 相手の図形と接触しているかを調べる
        /// </summary>
        /// <param name="s"></param>
        /// <returns>当たってるか</returns>
        virtual protected bool atarin(Shape s)
        {
            var mom = getgaisyuus(gettx(),getty(),rad);
            int i;
            /*
            double x0 = mom[0][0];
            double x1 = mom[0][0];
            double y0 = mom[0][1];
            double y1 = mom[0][1];
            bool fx0 = true, fx1 = true, fy0 = true, fy1 = true;

            
            for (i = 1; i < mom.Count; i++)
            {
                if (mom[i][0] < x0) x0 = mom[i][0];
                if (x1 < mom[i][0]) x1 = mom[i][0];
                if (mom[i][1] < y0) y0 = mom[i][1];
                if (y1 < mom[i][1]) y1 = mom[i][1];
            }
            */

            {
                var polis = s.getgaisyuus(s.gettx(),s.getty(),s.rad);
                /*
                for (i = 0; i < polis.Count; i++)
                {
                    if (fx0 && polis[i][0] >= x0)
                    {
                        fx0 = false;
                        if (!fx1 && !fy0 && !fy1) break;
                    }
                    if (fx1 && x1 >= polis[i][0])
                    {
                        fx1 = false;

                        if (!fx0 && !fy0 && !fy1) break;
                    }
                    if (fy0 && polis[i][1] >= y0)
                    {
                        fy0 = false;

                        if (!fx1 && !fx0 && !fy1) break;
                    }
                    if (fy1 && y1 >= polis[i][1])
                    {
                        fy1 = false;

                        if (!fx1 && !fy0 && !fx0) break;
                    }
                }
                if (fx0 || fx1 || fy0 || fy1) return false;
                */
                for (i=0;i<polis.Count;i++)
                {
                    if (onhani((float)polis[i][0], (float)polis[i][1]) ) return true;
                }
                for (i = 0; i < polis.Count; i++)
                {
                    if (crosses(polis[i][0], polis[i][1], polis[i][2], polis[i][3], mom))
                    {

                        return true;
                    }
                }

            }

            return false;
        }/// <summary>
        /// 過去の相手も考慮して接触しているか調べる
        /// </summary>
        /// <param name="now">今の相手</param>
        /// <param name="pre">昔の相手</param>
        /// <returns></returns>
        virtual protected bool atarin2(Shape now, Shape pre)
        {
            var mom = getgaisyuus(gettx(),getty(),rad);
            int i;
            /*
            double x0 = mom[0][0];
            double x1 = mom[0][0];
            double y0 = mom[0][1];
            double y1 = mom[0][1];
            bool fx0=true, fx1 = true, fy0 = true, fy1 = true;

            
            for (i = 1; i < mom.Count; i++) 
            {
                if (mom[i][0] < x0) x0 = mom[i][0];
                if (x1 < mom[i][0]) x1 = mom[i][0];
                if (mom[i][1] < y0) y0 = mom[i][1];
                if (y1 < mom[i][1]) y1 = mom[i][1];
            }*/
            {
                var a = now.getgaisyuus(now.gettx(),now.getty(),now.rad);
                var b = pre.getgaisyuus(pre.gettx(), pre.getty(), pre.rad);
                /*
                for (i = 0; i < a.Count; i++)
                {
                    if (fx0 && a[i][0] >= x0)
                    {
                        fx0 = false;
                        if (!fx1 && !fy0 && !fy1) break;
                    }
                    if (fx1 && x1 >= a[i][0])
                    {
                        fx1 = false;

                        if (!fx0 && !fy0 && !fy1) break;
                    }
                    if (fy0 && a[i][1] >= y0)
                    {
                        fy0 = false;

                        if (!fx1 && !fx0 && !fy1) break;
                    }
                    if (fy1 && y1 >= a[i][1])
                    {
                        fy1 = false;

                        if (!fx1 && !fy0 && !fx0) break;
                    }
                }
                for (i = 0; i < b.Count; i++)
                {
                    if (fx0 && b[i][0] >= x0)
                    {
                        fx0 = false;
                        if (!fx1 && !fy0 && !fy1) break;
                    }
                    if (fx1 && x1 >= b[i][0])
                    {
                        fx1 = false;

                        if (!fx0 && !fy0 && !fy1) break;
                    }
                    if (fy0 && b[i][1] >= y0)
                    {
                        fy0 = false;

                        if (!fx1 && !fx0 && !fy1) break;
                    }
                    if (fy1 && y1 >= b[i][1])
                    {
                        fy1 = false;

                        if (!fx1 && !fy0 && !fx0) break;
                    }
                }

                if (fx0 || fx1 || fy0 || fy1)
                {
                    return false;
                }*/
                for (i=0;i<a.Count;i++) 
                {
                    if (onhani((float)a[i][0], (float)a[i][1]) ) return true;
                }
                for (i = 0; i < a.Count() && i < b.Count(); i++)
                {

                    if (crosses(a[i][0], a[i][1], b[i][0], b[i][1], mom))//今と昔の点を結ぶ線
                    {

                        return true;
                    }

                }
                for (i = 0; i < a.Count() && i < b.Count(); i++)
                {

                    if (crosses(a[i][0], a[i][1], b[i][2], b[i][3], mom))//今の点と昔の次の点を結ぶ線
                    {

                        return true;
                    }

                }
                /*  軽くするために
                 for (int i = 0; i < a.Count(); i++)
                  {
                      if (crosses(a[i][0], a[i][1], a[i][2], a[i][3], mom))
                      {

                          return true;
                      }
                  }
                  for (int i = 0; i < b.Count(); i++)
                  {
                      if (crosses(b[i][0], b[i][1], b[i][2], b[i][3], mom))
                      {

                          return true;
                      }
                  }*/

            }

            return false;
        }
        /// <summary>
        /// 過去の点と今の点を比較して最も移動した点の移動距離を返す。使い道は謎である。
        /// </summary>
        /// <param name="pre"></param>
        /// <returns></returns>
        public float getsaipreidou(Shape pre) 
        {
            var a = this.getpoints(gettx(), getty(), this.syatei());
            var b = pre.getpoints(pre.gettx(), pre.getty(), pre.syatei());
            float res = 0, temp ;
            for (int i = 0; i < a.Count() && i < b.Count(); i++)
            {
                temp = (float)Math.Sqrt(Math.Pow(a[i].X - b[i].X, 2) + Math.Pow(a[i].Y - b[i].Y, 2));
                if (res < temp) res = temp;
            }
            return res;
        }
        /// <summary>
        /// 図形が互いに接触しているか調べる。結局atarinを双方向で呼び出してるだけ。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool atarun(Shape s) 
        {
            if (atarin(s))
            {
               
                return true;
            }
           /*いらない？
            if (s.atarin(this)) 
            {
              
                return true;
            }
           */
            return false;
        }
        /// <summary>
        /// 自分の過去との動きを相手に適用し、それを影として受け取る
        /// </summary>
        /// <param name="pre">過去の自分</param>
        /// <param name="shadow">影にする相手</param>
        /// <returns>影</returns>
        public Shape kagenbomb(Shape pre, Shape shadow) 
        {
            var kage = shadow.clone();
            {
                float dx = kage.gettx() - pre.gettx();
                float dy = kage.getty() - pre.getty();

                kage.settxy(this.gettx() + dx * (float)Math.Cos(this.rad - pre.rad) - dy * (float)Math.Sin(this.rad - pre.rad)
                    , this.getty() + dx * (float)Math.Sin(this.rad - pre.rad) + dy * (float)Math.Cos(this.rad - pre.rad));
            }
            return kage;
        }
        /// <summary>
        /// 図形がお互いに接触しているか過去の姿も見て調べる
        /// </summary>
        /// <param name="pre">過去の自分</param>
        /// <param name="snow">今の相手</param>
        /// <param name="spre">過去の相手</param>
        /// <returns></returns>
        public bool atarun2(Shape pre,Shape snow ,Shape spre)
        {
            bool do1 = this == pre, do2 = snow == spre ;
          
            if (do1 && do2) 
            {
                return atarun(snow);
            }
            
            if (do1) 
            {
                if (atarin(snow)) 
                {
                    return true;
                }
            }
            else
            {
                if (atarin2(snow, spre))
                {
                    return true;
                }
            }
            if (do2)
            {
                if (snow.atarin(this))
                {
                    return true;
                }
            }
            else
            {
                if (snow.atarin2(this, pre))
                {
                    return true;
                }
            }

            /*これはテスト中
            var kage = this.kagenbomb(pre, snow);
            if (atarin2(snow, kage))
            {
                return true;
            }

            kage = snow.kagenbomb(spre, this);
            if (snow.atarin2(this, kage))
            {

                return true;
            }*/

            return false;
        }
        /// <summary>
        /// 図形を単純に移動させる
        /// </summary>
        /// <param name="dx">移動するxの距離</param>
        /// <param name="dy">移動するyの距離</param>
        public void idou(float dx, float dy) 
        {
          
            x += dx;y += dy;
        }
        /// <summary>
        /// 重心を通る角度方向と垂直な直線を考える。その線が図形と接触している間角度方向に線を動かす。
        /// その線と重心の距離を返す。
        /// </summary>
        /// <param name="kaku">任意の角度</param>
        /// <returns>重心と線の距離</returns>
        abstract public float getsaikyo(double kaku);
        /// <summary>
        /// 重心から辺までの法線ベクトルの距離を返す
        /// </summary>
        /// <param name="kaku">法線ベクトル(自分のgethosenで手に入れた奴で)</param>
        /// <returns>距離</returns>
        abstract public float gethokyo(double kaku);
        /// <summary>
        /// 線がクロスしているのかを判定する
        /// </summary>
        /// <param name="mom">{x1,y1,x2,y2}</param>
        /// <param name="mom2">{x1,y1,x2,y2}</param>
        /// <returns></returns>
        static protected bool crosses(double[]mom, double[] mom2) 
        {

            double c1 = (mom[2] - mom[0]) * (mom2[1] - mom[1]) - (mom[3] - mom[1]) * (mom2[0] - mom[0]);

            double c2 = (mom[2] - mom[0]) * (mom2[3] - mom[1]) - (mom[3] - mom[1]) * (mom2[2] - mom[0]);

            double c3 = (mom2[2] - mom2[0]) * (mom[1] - mom2[1]) - (mom2[3] - mom2[1]) * (mom[0] - mom2[0]);

            double c4 = (mom2[2] - mom2[0]) * (mom[3] - mom2[1]) - (mom2[3] - mom2[1]) * (mom[2] - mom2[0]);

            if (c1 * c2 < 0 && c3 * c4 < 0)
            {


                return true;
            }
            return false;
        }
      
        /// <summary>
        /// 一本の辺と辺の集合がクロスしているかを判定する
        /// </summary>
        /// <param name="px">x1</param>
        /// <param name="py">y1</param>
        /// <param name="ppx">x2</param>
        /// <param name="ppy">y2</param>
        /// <param name="mom">相対座標の辺の集合{{x1,y1,x2,y2},{x1,y1,x2,y2},{x1,y1,x2,y2}}こんなの</param>
        /// <param name="soutaika">x1~y2を図形からの相対座標にするか</param>>
        /// <returns></returns>
        virtual protected bool crosses(double px, double py, double ppx, double ppy, List<double[]> mom,bool soutaika=false) 
        {
            double p1x;
            double p1y;
            double q1x;
            double q1y;
            if (soutaika)
            {
                double dx = px - gettx();
                double dy = py - getty();

                p1x = dx * Math.Cos(-rad) - dy * Math.Sin(-rad);
                p1y = dx * Math.Sin(-rad) + dy * Math.Cos(-rad);


                double fx = ppx - gettx();
                double fy = ppy - getty();

                q1x = fx * Math.Cos(-rad) - fy * Math.Sin(-rad);
                q1y = fx * Math.Sin(-rad) + fy * Math.Cos(-rad);
            }
            else 
            {
                p1x = px;
                p1y = py;
                q1x = ppx;
                q1y = ppy;
            }
            //座標系を相対にする
          
           

            for (int i=0;i<mom.Count;i++)
            {
                    //    foreach (var b in a) Console.WriteLine("aa " + b);

            
                if (((q1x - p1x) * (mom[i][1] - p1y) - (q1y - p1y) * (mom[i][0] - p1x) )*
                      ((q1x - p1x) * (mom[i][3] - p1y) - (q1y - p1y) * (mom[i][2] - p1x))<0
                    && ((mom[i][2] - mom[i][0]) * (p1y - mom[i][1]) - (mom[i][3] - mom[i][1]) * (p1x - mom[i][0]) )
                    * ((mom[i][2] - mom[i][0]) * (q1y - mom[i][1]) - (mom[i][3] - mom[i][1]) * (q1x - mom[i][0])) < 0)
                {
                  //  double det = (p1x - q1x) * (q2y - p2y) - (q2x - p2x) * (p1y - q1y);
                  //  double t = ((q2y - p1y) * (q2x - q1x) + (p2x - q2x) * (q2y - q1y)) / det;


                    return true;
                }

            }
            return false;
        }
    }
    /// <summary>
    /// 四角形を表すクラス
    /// </summary>
    public class Rectangle :Shape
    {
        /// <summary>
        /// 四角形を作る。ちなみにこの時回転は指定したx,yを中心に行われる
        /// </summary>
        /// <param name="xx">x座標</param>
        /// <param name="yy">y座標</param>
        /// <param name="ww">幅</param>
        /// <param name="hh">高さ</param>
        /// <param name="radd">回転角度</param>
        public Rectangle(float xx, float yy, float ww=0, float hh=0, double radd=0) : base(xx, yy, ww, hh, radd) 
        {
            
        }
        /// <summary>
        /// 図形を複製する
        /// </summary>
        /// <returns>複製された図形</returns>
        public override Shape clone()
        {
            var res=new Rectangle(x, y, w, h, rad);
            res.settxy(gettx(), getty());
            return res;
        }
 
        override public float getsaikyo(double kaku)
        {
            var a = new List<PointF>();
            a.Add(new PointF(-w/2,-h/2));
            a.Add(new PointF(w/2,-h/2));
            a.Add(new PointF(-w/2,h/2));
            a.Add(new PointF(w/2,h/2));

            float res=0;
            kaku -= rad;
            for (int i=0;i<a.Count;i++) 
            {
                var t = (float)Math.Sqrt(w/2*w/2+h/2*h/2)*(float)Math.Cos(Math.Atan2(a[i].Y, a[i].X)+kaku);
                if (t > res) res = t;
               
            }


            return Math.Abs(res);
        }
        public override float gethokyo(double kaku)
        {
            kaku -= rad;
            kaku = Math.Atan2(Math.Sin(kaku), Math.Cos(kaku));
            var kaku2 = Math.Atan2(h, w);
            if (-kaku2 <= kaku && kaku < kaku2) return Math.Abs(w / 2);
            else if (kaku2 <= kaku && kaku < Math.PI - kaku2) return Math.Abs(h / 2);
            else if (kaku2 - Math.PI <= kaku && kaku < -kaku2) return Math.Abs(-h / 2);
            else return Math.Abs(-w / 2);
        }
        public override List<PointF> getpoints(float px, float py,float syatei)
        {
            
            var res = base.getpoints(px, py, syatei);
            res.Add(new PointF(getcx(0, 0), getcy(0, 0)));
            res.Add(new PointF(getcx(0, h), getcy(0, h)));
            res.Add(new PointF(getcx(w, h), getcy(w, h)));
            res.Add(new PointF(getcx(w, 0), getcy(w, 0)));


            return res;
        }

        public override bool onhani(float px, float py)
        {

            float dx = px - gettx();
            float dy = py - getty();

            double ddx = dx * Math.Cos(-rad) - dy * Math.Sin(-rad);
            double ddy = dx * Math.Sin(-rad) + dy * Math.Cos(-rad);

            return (-w / 2 <= ddx && ddx <=  w / 2) && (- h / 2 <= ddy && ddy <=  h / 2);
        }
        public override bool onhani(float px, float py,float ppx,float ppy)
        {
            //自分の辺を用意して全てに対して当たってるか判定する

            // Console.WriteLine("qwiqjo hafoudhfasuiv ");

            List<double[]> mom = getgaisyuus();
         
          
            return crosses(px,py,ppx,ppy,mom);

        }
        protected override List<double[]> getgaisyuus(float x=0, float y=0, double rad=0)
        {
            var mom = new List<double[]>();


            mom.Add(new double[] { (-w / 2)*Math.Cos(rad)-(+h / 2)*Math.Sin(rad)+x, (-w / 2) * Math.Sin(rad) + (+h / 2) * Math.Cos(rad)+y
                , (+w / 2)*Math.Cos(rad)-(+h / 2)*Math.Sin(rad)+x, (+w / 2) * Math.Sin(rad) + (+h / 2) * Math.Cos(rad)+y });
            mom.Add(new double[] { mom[0][2],mom[0][3]
                , (+w / 2)*Math.Cos(rad)-(-h / 2)*Math.Sin(rad)+x, (+w / 2) * Math.Sin(rad) + (-h / 2) * Math.Cos(rad)+y });
            mom.Add(new double[] { mom[1][2], mom[1][3]
                , (-w / 2)*Math.Cos(rad)-(-h / 2)*Math.Sin(rad)+x, (-w / 2) * Math.Sin(rad) + (-h / 2) * Math.Cos(rad)+y  });
            mom.Add(new double[] { mom[2][2], mom[2][3], mom[0][0], mom[0][1] });
            return mom;
        }
        public override double gethosen(float px, float py)
        {
            float dx = px - gettx();
            float dy = py - getty();

            double ddx = dx * Math.Cos(-rad) - dy * Math.Sin(-rad);
            double ddy = dx * Math.Sin(-rad) + dy * Math.Cos(-rad);

            double b = Math.Atan2(h, w);
            double a = Math.Atan2(ddy, ddx);
            double res ;
            if (-b <= a && a <= b) res= rad;
            else if (b <= a && a <= Math.PI - b) res= Math.PI / 2 + rad;
            else if(-Math.PI + b <= a && a <= -b) res= -Math.PI / 2 + rad;
            else res=  Math.PI+rad;
            return Math.Atan2(Math.Sin(res), Math.Cos(res));
        }
        public override double gethosen2(Shape s)
        {
           
           return tasuuketun(s,getgaisyuus2());

        }
       


        protected override void drawn(Color4 col,hyojiman hyo)
        {
            var bruh=hyo.render.CreateSolidColorBrush(col);
            float bai = hyo.bairitu;
            hyo.render.DrawLine(new PointF((getcx(0,0) - hyo.camx)*bai, (getcy(0, 0) - hyo.camy) * bai)
                ,new PointF( (getcx(w, 0) - hyo.camx) * bai, (getcy(w, 0) - hyo.camy) * bai), bruh, 3 * bai);

            hyo.render.DrawLine(new PointF((getcx(w, 0) - hyo.camx) * bai, (getcy(w, 0) - hyo.camy) * bai)
                , new PointF((getcx(w, h) - hyo.camx) * bai, (getcy(w, h) - hyo.camy) * bai), bruh, 3 * bai);

            hyo.render.DrawLine(new PointF((getcx(w, h) - hyo.camx) * bai, (getcy(w, h) - hyo.camy) * bai)
                , new PointF((getcx(0, h) - hyo.camx) * bai, (getcy(0, h) - hyo.camy) * bai), bruh, 3 * bai);

            hyo.render.DrawLine(new PointF((getcx(0, h) - hyo.camx) * bai, (getcy(0, h) - hyo.camy) * bai)
                , new PointF((getcx(0, 0) - hyo.camx) * bai, (getcy(0, 0) - hyo.camy) * bai), bruh, 3 * bai);
            bruh.Dispose();
        }

        public override float syatei()
        {
          return (float)Math.Sqrt(w/2 * w/2 + h/2 * h/2);
        }
    }
    /// <summary>
    /// 三角形を表すクラス。
    /// しかし|>こういう左の辺がy軸と平行で右の先端が左の辺のy座標の間にあるな形の三角形しか作れない。
    /// </summary>
    public class Triangle : Shape
    {
        int _houkou=1;
        /// <summary>
        /// 三角形の先の方向。1で右-1で左2で下
        /// </summary>
        public int houkou { get { return _houkou; } set { if (-2 <= value && value <= 2&&value!=0) _houkou = value; } }

        float _haji;
        /// <summary>
        /// 先端の高さの割合hajiは0<=<=1の範囲で変化する。
        /// </summary>
        public float haji { get { return _haji; }set { _haji = value;if (_haji < 0) _haji = 0;if (_haji > 1) _haji = 1; } }
        /// <summary>
        /// 三角形を作る。左辺が底で右が先端。
        /// </summary>
        /// <param name="xx">x座標</param>
        /// <param name="yy">y座標</param>
        /// <param name="ww">幅</param>
        /// <param name="hh">高さ</param>
        /// <param name="hajih">高さと先端の高さの割合。0で左上の角が90°になる</param>
        /// <param name="hou">三角形の先端の方向1で右-1で左2で下</param>
        /// <param name="radd">回転角度</param>
        public Triangle(float xx, float yy, float ww=0, float hh=0,float hajih=0.5f, double radd=0,int hou=1) : base(xx, yy, ww, hh, radd)
        {
            haji = hajih;
            houkou = hou;
        }

        
        public override float gettx()
        {
            switch (_houkou) {
                case 1:
                    return (getcx(0, 0) + getcx(0, h) + getcx(w, h * haji)) / 3;
                case 2:
                    return (getcx(0, 0) + getcx(w, 0) + getcx(w * haji, h)) / 3;
                case -1:
                    return (getcx(w, 0) + getcx(w, h) + getcx(0, h * haji)) / 3;
                case -2:
                    return (getcx(0, h) + getcx(w, h) + getcx(w * haji, 0)) / 3;
                default:
                    return (getcx(0, 0) + getcx(0, h) + getcx(w, h * haji)) / 3; 
            } 
        }
        
        public override float getty()
        {
            switch (_houkou)
            {
                case 1:
                    return (getcy(0, 0) + getcy(0, h) + getcy(w, h * haji)) / 3;
                case 2:
                    return (getcy(0, 0) + getcy(w, 0) + getcy(w * haji, h)) / 3;
                case -1:
                    return (getcy(w, 0) + getcy(w, h) + getcy(0, h * haji)) / 3;
                case -2:
                    return (getcy(0, h) + getcy(w, h) + getcy(w * haji, 0)) / 3;
                default:
                    return (getcy(0, 0) + getcy(0, h) + getcy(w, h * haji)) / 3;
            }
        }
        public override Shape clone()
        {
            var res = new Triangle(x, y, w, h,haji, rad,houkou);
            res.settxy(gettx(), getty());
            return res;
        }
        /// <summary>
        /// 重心をxy座標にセットする。
        /// </summary>
        /// <param name="xx">セットするx座標</param>
        /// <param name="yy">セットするy座標</param>
        override public void settxy(float xx, float yy)
        {
            x += xx - gettx();
            y += yy - getty();

        }
        /// <summary>
        /// 中心からの天の位置を取得する
        /// </summary>
        /// <returns></returns>
   
        override public float getsaikyo(double kaku)
        {
            var a = getsoutaipoints();
           



            float res = 0;

            for(int i=0;i<a.Count;i++)
            {
                var raad = Math.Atan2(Math.Sin(Math.Atan2(a[i].Y, a[i].X) - kaku), Math.Cos(Math.Atan2(a[i].Y, a[i].X) - kaku)) + Math.PI;
                var t = (float)Math.Sqrt(a[i].X* a[i].X + a[i].Y* a[i].Y) * (float)Math.Cos(raad);
                if (t > res) res = t;
               // Console.WriteLine(kaku*180/Math.PI+" ksja: "+ Math.Atan2(a[i].Y, a[i].X)/Math.PI*180+" asljok "+ Math.Sqrt(a[i].X * a[i].X + a[i].Y * a[i].Y));
          //     Console.WriteLine(t+" sad " + " :saikyo: "+raad/Math.PI*180);
            }


            return res;
        }
        protected List<PointF> getsoutaipoints(bool ho = false)
        {
            var a = new List<PointF>();
            switch (_houkou)
            {
                case 1:
                    a.Add(new PointF(getcx(0, 0) - gettx(), getcy(0, 0) - getty()));
                    a.Add(new PointF(getcx(0, h) - gettx(), getcy(0, h) - getty()));
                    a.Add(new PointF(getcx(w, h * haji) - gettx(), getcy(w, h * haji) - getty()));

                    break;
                case 2:
                    a.Add(new PointF(getcx(0, 0) - gettx(), getcy(0, 0) - getty()));
                    a.Add(new PointF(getcx(w, 0) - gettx(), getcy(w, 0) - getty()));
                    a.Add(new PointF(getcx(w * haji, h) - gettx(), getcy(w * haji, h) - getty()));
                    break;
                case -1:
                    a.Add(new PointF(getcx(w, 0) - gettx(), getcy(w, 0) - getty()));
                    a.Add(new PointF(getcx(w, h) - gettx(), getcy(w, h) - getty()));
                    a.Add(new PointF(getcx(0, h * haji) - gettx(), getcy(0, h * haji) - getty()));
                    break;
                case -2:
                    a.Add(new PointF(getcx(0, h) - gettx(), getcy(0, h) - getty()));
                    a.Add(new PointF(getcx(w, h) - gettx(), getcy(w, h) - getty()));
                    a.Add(new PointF(getcx(w * haji, 0) - gettx(), getcy(w * haji, 0) - getty()));
                    break;
                default:
                    a.Add(new PointF(getcx(0, 0) - gettx(), getcy(0, 0) - getty()));
                    a.Add(new PointF(getcx(0, h) - gettx(), getcy(0, h) - getty()));
                    a.Add(new PointF(getcx(w, h * haji) - gettx(), getcy(w, h * haji) - getty()));
                    break;
            }
            if (ho)
            {
                var t = a[2];
                a[2] = a[1];
                a[1] = t;
            }
            return a;
        }
      
        public override float gethokyo(double kaku)
        {
            float dx = syatei() * (float)Math.Cos(kaku) ;
            float dy = syatei() * (float)Math.Sin(kaku) ;

       

            double a = kaku;

            var aaa = getsoutaipoints(true);
            float res = (float)Math.Sqrt(Math.Pow(aaa[0].X, 2) + Math.Pow(aaa[0].Y, 2));

            for (int i = 0; i < 2; i++)
            {

                var ka = Math.Atan2(aaa[i].Y, aaa[i].X) - a;
                var kb = Math.Atan2(aaa[i + 1].Y, aaa[i + 1].X) - a;
                if (Math.Abs(Math.Atan2(Math.Sin(ka), Math.Cos(ka))) +Math.Abs(Math.Atan2(Math.Sin(kb), Math.Cos(kb))) <= Math.PI 
                    && Math.Atan2(Math.Sin(ka), Math.Cos(ka)) * Math.Atan2(Math.Sin(kb), Math.Cos(kb)) < 0)
                {
                   

                    //  var kakun = Math.Atan2(aaa[i + 1].Y - aaa[i].Y, aaa[i + 1].X - aaa[i].X) - Math.Atan2(aaa[i + 1].Y, aaa[i + 1].X);
                    //  res = (float)Math.Sqrt(aaa[i + 1].X * aaa[i + 1].X + aaa[i + 1].Y * aaa[i + 1].Y) * (float)Math.Sin(kakun);
                    var kakun2 = Math.Atan2(-aaa[i].Y + aaa[i + 1].Y, -aaa[i].X + aaa[i + 1].X) - Math.Atan2(-aaa[i].Y, -aaa[i].X);
                    res = (float)Math.Sqrt(aaa[i].X * aaa[i].X + aaa[i].Y * aaa[i].Y) * (float)Math.Sin(kakun2);
                }
               
            }
            return Math.Abs(res)  ;

        }
        public override List<PointF> getpoints(float px, float py, float syatei)
        {

            var res = new List<PointF>();
            res.Add(new PointF(gettx(), getty()));
            switch (_houkou)
            {
                case 1:
                    res.Add(new PointF(getcx(0, 0) , getcy(0, 0) ));
                    res.Add(new PointF(getcx(0, h) , getcy(0, h) ));
                    res.Add(new PointF(getcx(w, h * haji) , getcy(w, h * haji) ));
                    return res;
                case 2:
                    res.Add(new PointF(getcx(0, 0) , getcy(0, 0) ));
                    res.Add(new PointF(getcx(w, 0) , getcy(w, 0) ));
                    res.Add(new PointF(getcx(w * haji, h) , getcy(w * haji, h) ));
                    return res;
                case -1:
                    res.Add(new PointF(getcx(w, 0) , getcy(w, 0) ));
                    res.Add(new PointF(getcx(w, h) , getcy(w, h) ));
                    res.Add(new PointF(getcx(0, h * haji) , getcy(0, h * haji) ));
                    return res;
                case -2:
                    res.Add(new PointF(getcx(0, h) , getcy(0, h) ));
                    res.Add(new PointF(getcx(w, h) , getcy(w, h) ));
                    res.Add(new PointF(getcx(w * haji, 0) , getcy(w * haji, 0) ));
                    return res;
                default:
                    res.Add(new PointF(getcx(0, 0) , getcy(0, 0) ));
                    res.Add(new PointF(getcx(0, h) , getcy(0, h) ));
                    res.Add(new PointF(getcx(w, h * haji) , getcy(w, h * haji) ));
                    return res;
            }



            return res;
        }

        public override bool onhani(float px, float py)
        {
            var aas = new List<PointF>();
            switch (houkou) 
            {
                case 1:
                    aas.Add(new PointF(0, 0));
                    aas.Add(new PointF(0, h));
                    aas.Add(new PointF(w, h * haji));
                    break;
                case 2:
                    aas.Add(new PointF(0, 0));
                    aas.Add(new PointF(w, 0));
                    aas.Add(new PointF(w * haji, h));
                    break;
                case -1:
                    aas.Add(new PointF(w, 0));
                    aas.Add(new PointF(w, h));
                    aas.Add(new PointF(0, h * haji));
                    break;
                case -2:

                    aas.Add(new PointF(0, h));
                    aas.Add(new PointF(w, h));
                    aas.Add(new PointF(w * haji, 0));
                    break;
                default:
                    aas.Add(new PointF(0, 0));
                    aas.Add(new PointF(0, h));
                    aas.Add(new PointF(w, h * haji));
                    break;
            }
            float dx = px - getcx((-aas[0].X + aas[1].X)/2, (-aas[0].Y + aas[1].Y)/2) ;
            float dy = py - getcy((-aas[0].X + aas[1].X) / 2, (-aas[0].Y + aas[1].Y) / 2);

            double ddx = dx * Math.Cos(-rad) - dy * Math.Sin(-rad);
            double ddy = dx * Math.Sin(-rad) + dy * Math.Cos(-rad);

            double uh = 0,sh=0;
            switch (houkou)
            {
                case 1:
                    if (w != 0)
                    {
                        sh = -h * (1 - ddx / w) * haji;
                        uh = h * (1 - ddx / w) * (1 - haji);
                    }
                    return (0 <= ddx && ddx <= w) && (sh <= ddy && ddy <= uh);
                case 2:
                    if (h != 0)
                    {
                        sh = -w * (1 - ddy / h) * haji;
                        uh = w * (1 - ddy / h) * (1 - haji);
                    }
                    return (0 <= ddy && ddy <= h) && (sh <= ddx && ddx <= uh);
                case -1:
                    if (w != 0)
                    {
                        sh = -h * ( ddx / w) * haji;
                        uh = h * ( ddx / w) * (1 - haji);
                    }
                    return (0 <= ddx && ddx <= w) && (sh <= ddy && ddy <= uh);
                case -2:
                    if (h != 0)
                    {
                        sh = -w * ( ddy / h) * haji;
                        uh = w * (ddy / h) * (1 - haji);
                    }
                     return (0 <= ddy && ddy <= h) && (sh <= ddx && ddx <= uh);
                default:
                    if (w != 0)
                    {
                        sh = -h * (1 - ddx / w) * haji;
                        uh = h * (1 - ddx / w) * (1 - haji);
                    }
                    return (0 <= ddx && ddx <= w) && (sh <= ddy && ddy <= uh);
            }
           
           // Console.WriteLine(haji+" ha "+ddx + " " + ddy + "  asl; " +sh+" :saf: "+uh);

          
        }
        public override bool onhani(float px, float py, float ppx, float ppy)
        {


            List<double[]> mom = getgaisyuus();
            return crosses(px, py, ppx, ppy, mom);

        }
        protected override List<double[]> getgaisyuus(float x=0,float y=0,double rad=0)
        {
            List<double[]> mom = new List<double[]>();
            float x1, x2, x3, y1, y2, y3;
            switch (_houkou)
            {
                case 1:
                     x1 = -w / 3; y1 = -(h + h * haji) / 3;
                     x2 = -w / 3; y2 = h - (h + h * haji) / 3;
                     x3 = w * 2 / 3; y3 = h * haji - (h + h * haji) / 3;
                    break;
                case 2:
                     y1 = -h / 3; x1 = -(w + w * haji) / 3;
                     y2 = -h / 3; x2 = w - (w + w * haji) / 3;
                     y3 = h * 2 / 3; x3 = w * haji - (w + w * haji) / 3;
                    break;
                case -1:
                     x1 = w / 3; y1 = -(h + h * haji) / 3;
                     x2 = w / 3; y2 = h - (h + h * haji) / 3;
                     x3 = -w * 2 / 3; y3 = h * haji - (h + h * haji) / 3;
                    break;
                case -2:
                    y1 = h / 3; x1 = -(w + w * haji) / 3;
                    y2 = h / 3; x2 = w - (w + w * haji) / 3;
                    y3 = -h * 2 / 3; x3 = w * haji - (w + w * haji) / 3;
                    break;
                default:
                     x1 = -w / 3; y1 = -(h + h * haji) / 3;
                     x2 = -w / 3; y2 = h - (h + h * haji) / 3;
                     x3 = w * 2 / 3; y3 = h * haji - (h + h * haji) / 3;
                    break;
            }
           

            mom.Add(new double[] { x1 * Math.Cos(rad) - y1 * Math.Sin(rad) + x, x1 * Math.Sin(rad) + y1 * Math.Cos(rad) + y
                ,x2 * Math.Cos(rad) - y2 * Math.Sin(rad) + x, x2 * Math.Sin(rad) + y2 * Math.Cos(rad) + y});
            mom.Add(new double[] { mom[0][2], mom[0][3], x3 * Math.Cos(rad) - y3 * Math.Sin(rad) + x, x3 * Math.Sin(rad) + y3 * Math.Cos(rad) + y });
            mom.Add(new double[] { mom[1][2],mom[1][3], mom[0][0],mom[0][1] });
          
            return mom;
        }
        public override double gethosen(float px, float py)
        {
           
            float dx = px - gettx();
            float dy = py - getty();
 
            double a = Math.Atan2(dy, dx);

            double res=Math.PI+rad;

            var aaa = getgaisyuus(0,0,0);
            for (int i = 0; i < aaa.Count; i++)
            {
                var ka = Math.Atan2(aaa[i][1], aaa[i][0]) - a;
                 var kb = Math.Atan2(aaa[i][3], aaa[i][2]) - a;
                if (Math.Abs(Math.Atan2(Math.Sin(ka), Math.Cos(ka)) - Math.Atan2(Math.Sin(kb), Math.Cos(kb))) <= Math.PI && Math.Atan2(Math.Sin(ka), Math.Cos(ka)) * Math.Atan2(Math.Sin(kb), Math.Cos(kb)) < 0)
                {

                    res = Math.Atan2(aaa[i][3] - aaa[i][1], aaa[i][2] - aaa[i][0]);
                    res = hosenton(getgaisyuus(),res);
                }
            }
            
            return Math.Atan2(Math.Sin(res), Math.Cos(res));
        }
      
        public override double gethosen2(Shape s)
        {
                 
            return tasuuketun(s,getgaisyuus2());

        }
  


        protected override void drawn(Color4 col, hyojiman hyo)
        {
            var bruh = hyo.render.CreateSolidColorBrush(col);
           
            float bai = hyo.bairitu;
            var aas = getpoints(gettx(),getty(),syatei());
            hyo.render.DrawLine(new PointF((aas[1].X - hyo.camx) * bai, (aas[1].Y - hyo.camy) * bai)
                , new PointF((aas[3].X - hyo.camx) * bai, (aas[3].Y - hyo.camy) * bai), bruh, 3*bai);

            hyo.render.DrawLine(new PointF((aas[2].X - hyo.camx) * bai, (aas[2].Y - hyo.camy) * bai)
                , new PointF((aas[3].X - hyo.camx) * bai, (aas[3].Y - hyo.camy) * bai), bruh, 3 * bai);

            hyo.render.DrawLine(new PointF((aas[1].X - hyo.camx) * bai, (aas[1].Y - hyo.camy) * bai)
                , new PointF((aas[2].X - hyo.camx) * bai, (aas[2].Y - hyo.camy) * bai), bruh, 3 * bai);
         
            /*var bruh2 = hyo.render.CreateSolidColorBrush(new Color4(1 - col.R, 1 - col.G, 1 - col.B, col.A));
            foreach (var a in getgaisyuus(gettx(),getty(),rad)) 
            {
                hyo.render.DrawLine(new PointF((gettx()+(float)a[0] - hyo.camx) * bai, (getty() + (float)a[1] - hyo.camy) * bai)
             , new PointF((gettx() + (float)a[2] - hyo.camx) * bai, (getty() + (float)a[3] - hyo.camy) * bai), bruh2, 4 * bai);
            }
            bruh2.Dispose();*/
        bruh.Dispose();
        }

        public override float syatei()
        {
            return (float)Math.Sqrt(w / 2 * w / 2 + h / 2 * h / 2);
        }
    }
    /// <summary>
    /// 円を表すクラス
    /// </summary>
    public class Circle : Shape
    {
        int kinji = 5;
        /// <summary>
        /// 円を作り出す。円はあたり判定の時、多角形に近似される。
        /// </summary>
        /// <param name="xx">x座標</param>
        /// <param name="yy">y座標</param>
        /// <param name="ww">幅</param>
        /// <param name="hh">高さ</param>
        /// <param name="radd">回転角</param>
        /// <param name="pointkinji">近似する多角形の画数</param>
        public Circle(float xx, float yy, float ww=0, float hh=0, double radd=0,int pointkinji=20) : base(xx, yy, ww, hh, radd)
        {
            if (pointkinji > kinji)
            {
                kinji = pointkinji;
              //  Console.WriteLine(pointkinji+"aslfkjaslslkinji"+kinji);
            }
        }

        public override Shape clone()
        {
            var res = new Circle(x, y, w, h, rad);
            res.settxy(gettx(), getty());
            res.kinji = kinji;
            return res;
        }

      

        override public float getsaikyo(double kaku)
        {
            kaku -= rad;
            return (float)(w * h / 4 / Math.Sqrt(w * w / 4 * Math.Sin(kaku) * Math.Sin(kaku) + h * h / 4 * Math.Cos(kaku) * Math.Cos(kaku)));

        }
        public override float gethokyo(double kaku)
        {
            kaku -= rad;
            return (float)(w * h / 4 / Math.Sqrt(w * w / 4 * Math.Sin(kaku) * Math.Sin(kaku) + h * h / 4 * Math.Cos(kaku) * Math.Cos(kaku)));
         }
        /// <summary>
        /// 指定した点に必要であろうこの円を構成する頂点を返す。
        /// 点の表側のみ多角形で近似し反対側は点一個で済ませている
        /// </summary>
        /// <param name="px">その点のx座標</param>
        /// <param name="py">その点のy座標</param>
        /// <param name="syatei">何かに使えるかなと思ってたけど使ってないやーつ</param>
        /// <returns>ポイントの束</returns>
        public override List<PointF> getpoints(float px, float py,float syatei)
        {
            //表の部分だけ多角形に近似し、裏は点一個で済ます。
            var res = base.getpoints(px, py, syatei);
            double kaku = Math.Atan2(py - getty(), px - gettx());
            for (double i = kaku-Math.PI/2-rad; i < kaku+Math.PI/2-rad; i += Math.PI / kinji)
            {
                var nag = (float)(w * h / 4 / Math.Sqrt(w * w / 4 * Math.Sin(i) * Math.Sin(i) + h * h / 4 * Math.Cos(i) * Math.Cos(i)));
                var iii= new PointF(nag * (float)Math.Cos(i ),  nag * (float)Math.Sin(i ));
                var ppp= new PointF(gettx()+iii.X * (float)Math.Cos(rad) - iii.Y * (float)Math.Sin(rad),
                    getty()+iii.X * (float)Math.Sin(rad) + iii.Y * (float)Math.Cos(rad));
                res.Add(ppp);
              
            }
            {
                var i = kaku + Math.PI;
                var nag = (float)(w * h / 4 / Math.Sqrt(w * w / 4 * Math.Sin(i) * Math.Sin(i) + h * h / 4 * Math.Cos(i) * Math.Cos(i)));
                var iii = new PointF(nag * (float)Math.Cos(i), nag * (float)Math.Sin(i));
                var ppp = new PointF(gettx() + iii.X * (float)Math.Cos(rad) - iii.Y * (float)Math.Sin(rad),
                    getty() + iii.X * (float)Math.Sin(rad) + iii.Y * (float)Math.Cos(rad));
                res.Add(ppp);

            }

            return res;
        }

        public override bool onhani(float px, float py)
        {
            float dx = px - gettx();
            float dy = py - getty();

            double ddx = dx * Math.Cos(-rad) - dy * Math.Sin(-rad);
            double ddy = dx * Math.Sin(-rad) + dy * Math.Cos(-rad);


           
            return (ddx * ddx / (w / 2 * w / 2) + ddy * ddy / (h / 2 * h / 2) <= 1);
        }
        public override bool onhani(float px, float py, float ppx, float ppy)
        {

            List<double[]> mom = getgaisyuus();
     //       Console.WriteLine(px+" :x: "+ppx+" "+py + " :y: " + ppy+"  kkl "+ crosses(px, py, ppx, ppy, mom));
            return crosses(px,py,ppx,ppy,mom);

        }
        protected override List<double[]> getgaisyuus(float x = 0,float y = 0,double rad=0)
        {
            var mom = new List<double[]>();
            double nag, nag2;
            double x1, y1, x2, y2;
            for (int i = 0; i < kinji; i++)
            {
                if (i == 0 || i == kinji - 1)
                {
                    nag = (w * h / 4 / Math.Sqrt(w * w / 4 * Math.Sin((i + 1) * Math.PI / kinji * 2) * Math.Sin((i + 1) * Math.PI / kinji * 2)
                        + h * h / 4 * Math.Cos((i + 1) * Math.PI / kinji * 2) * Math.Cos((i + 1) * Math.PI / kinji * 2)));

                    nag2 = (w * h / 4 / Math.Sqrt(w * w / 4 * Math.Sin((i) * Math.PI / kinji * 2) * Math.Sin((i) * Math.PI / kinji * 2)
                       + h * h / 4 * Math.Cos((i) * Math.PI / kinji * 2) * Math.Cos((i) * Math.PI / kinji * 2)));

                    x1 = nag * Math.Cos((i + 1) * Math.PI / kinji * 2);
                    y1 = nag * Math.Sin((i + 1) * Math.PI / kinji * 2);
                    x2 = nag2 * Math.Cos((i) * Math.PI / kinji * 2);
                    y2 = nag2 * Math.Sin((i) * Math.PI / kinji * 2);
                    mom.Add(new double[] {x1*Math.Cos(rad)-y1*Math.Sin(rad)+x,x1*Math.Sin(rad)+y1*Math.Cos(rad)+y,
                    x2*Math.Cos(rad)-y2*Math.Sin(rad)+x,x2*Math.Sin(rad)+y2*Math.Cos(rad)+y});
                }
                else
                {
                    nag = (float)(w * h / 4 / Math.Sqrt(w * w / 4 * Math.Sin((i + 1) * Math.PI / kinji * 2) * Math.Sin((i + 1) * Math.PI / kinji * 2)
                           + h * h / 4 * Math.Cos((i + 1) * Math.PI / kinji * 2) * Math.Cos((i + 1) * Math.PI / kinji * 2)));
                    x1 = nag * Math.Cos((i + 1) * Math.PI / kinji * 2);
                    y1 = nag * Math.Sin((i + 1) * Math.PI / kinji * 2);

                    mom.Add(new double[] {mom[i-1][2],mom[i-1][3],
                        x1*Math.Cos(rad)-y1*Math.Sin(rad)+x,x1*Math.Sin(rad)+y1*Math.Cos(rad)+y});
                }
            }
            return mom;
        }
        public override double gethosen(float px, float py)
        {
            float dx = px - gettx();
            float dy = py - getty();

            return Math.Atan2(dy, dx);
        }
        public override double gethosen2(Shape s)
        {
            return gethosen(s.gettx(), s.getty());
        }
        protected override void drawn(Color4 col, hyojiman hyo)
        {
            float bai = hyo.bairitu;
            var bruh = hyo.render.CreateSolidColorBrush(col);
            hyo.render.Transform = Matrix3x2.CreateRotation((float)rad, new Vector2(gettx() - hyo.camx, getty() - hyo.camy));
            hyo.render.DrawEllipse(new Ellipse(new PointF((gettx()-hyo.camx) * bai, (getty() - hyo.camy) * bai), w/2*bai,h/2*bai), bruh, 3*bai);


            bruh.Dispose();
        }

        public override float syatei()
        {
            return (float)Math.Sqrt(Math.Pow(Math.Max(w/2,h/2),2));
        }
    }
}
