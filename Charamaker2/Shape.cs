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
        public Shape(float xx, float yy, float ww, float hh, double radd = 0)
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
        /// 相手のgetpointsをも呼び出していい感じの法線ベクトルを返す。おそらく物理的な奴向きき？
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

            if (syatei() + s.syatei() >= kyori(s))
            {

                foreach (var a in s.getpoints(gettx(), getty(), this.syatei()))
                {
                    if (onhani(a.X, a.Y, s.gettx(), s.getty()))
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
            if (syatei()*2 + now.syatei()+pre.syatei() >= kyori(now))
            {
                var a=now.getpoints(gettx(), getty(), this.syatei());
                var b = pre.getpoints(gettx(), getty(), this.syatei());
           
                for(int i=0;i<a.Count()&&i<b.Count();i++)
                {
                    if (onhani(a[i].X,a[i].Y,b[i].X,b[i].Y))
                    {

                        return true;
                    }
                }

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
            if (s.atarin(this)) 
            {
              
                return true;
            }
           
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
            
            if (atarin2(snow,spre))
            {
                return true;
            }
            if (snow.atarin2(this,pre))
            {
                return true;
            }/*これはテスト中
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
        public Rectangle(float xx, float yy, float ww, float hh, double radd) : base(xx, yy, ww, hh, radd) 
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
        /// <summary>
        /// 相手の図形と接触しているかを調べる
        /// </summary>
        /// <param name="s"></param>
        /// <returns>当たってるか</returns>
        protected override bool atarin(Shape s)
        {
        
            if (syatei() + s.syatei() >= kyori(s)) 
            {
              
                foreach (var a in s.getpoints(gettx(),getty(),this.syatei())) 
                {
                    if (onhani(a.X, a.Y,s.gettx(),s.getty()))
                    {

                        return true;
                    }
                }
            
            }
          
            return false;
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
            foreach (var b in a) 
            {
                var t = (float)Math.Sqrt(w/2*w/2+h/2*h/2)*(float)Math.Cos(Math.Atan2(b.Y,b.X)+kaku);
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
            if (onhani(px, py) || onhani(ppx, ppy)) return true;
            // Console.WriteLine("qwiqjo hafoudhfasuiv ");
            float dx = px - gettx();
            float dy = py - getty();

            double ddx = dx * Math.Cos(-rad) - dy * Math.Sin(-rad);
            double ddy = dx * Math.Sin(-rad) + dy * Math.Cos(-rad);


            float fx = ppx - gettx();
            float fy = ppy - getty();

            double ffx = fx * Math.Cos(-rad) - fy * Math.Sin(-rad);
            double ffy = fx * Math.Sin(-rad) + fy * Math.Cos(-rad);


            double p1x = ddx,
        p1y = ddy,
        q1x = ffx,
        q1y = ffy;
            List<List<double>> mom = new List<List<double>>();
            mom.Add(new List<double> { -w / 2, h / 2, w / 2, h / 2 });
            mom.Add(new List<double> { -w / 2, -h / 2, w / 2, -h / 2 });
            mom.Add(new List<double> { w / 2, h / 2, w / 2, -h / 2 });
            mom.Add(new List<double> { -w / 2, h / 2, -w / 2, -h / 2 });
            foreach (var a in mom)
            {
              
            //    foreach (var b in a) Console.WriteLine("aa " + b);
                
                double p2x = a[0],
                 p2y = a[1],
                 q2x = a[2],
                 q2y = a[3];
                double c1 = (q1x - p1x) * (p2y - p1y) - (q1y - p1y) * (p2x - p1x);

                double c2 = (q1x - p1x) * (q2y - p1y) - (q1y - p1y) * (q2x - p1x);

                double c3 = (q2x - p2x) * (p1y - p2y) - (q2y - p2y) * (p1x - p2x);

                double c4 = (q2x - p2x) * (q1y - p2y) - (q2y - p2y) * (q1x - p2x);

                if (c1 * c2 < 0 && c3 * c4 < 0)
                {
                    double det = (p1x - q1x) * (q2y - p2y) - (q2x - p2x) * (p1y - q1y);
                    double t = ((q2y - p1y) * (q2x - q1x) + (p2x - q2x) * (q2y - q1y)) / det;
                 

                    return true;
                }

            }
          
            return false;

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
            int k1=0,k2=0,k3=0,k4 = 0;
            double res=rad;
            foreach (var p in s.getpoints(gettx(), getty(), syatei())) 
            {
                float dx = p.X - gettx();
                float dy = p.Y - getty();

                double ddx = dx * Math.Cos(-rad) - dy * Math.Sin(-rad);
                double ddy = dx * Math.Sin(-rad) + dy * Math.Cos(-rad);

                if (ddx >= w/2) k1 += 1;
                if (ddy >= h/2) k2 += 1;
                if (ddx <= -w/2) k3 += 1;
                if (ddy <= -h/2) k4 += 1;
              //  Console.WriteLine(k1 + " :: " + k2 + " :qqq: " + k3 + " :: " + k4);
            }
            if (k1 >= k2 && k1 >= k3 && k1 >= k4)
            {
                res += 0;
            }
            else if (k2 >= k1 && k2 >= k3 && k2 >= k4) 
            {
                res+= Math.PI/2;
            }
            else if (k3 >= k2 && k3 >= k1 && k3 >= k4) 
            {
                res += Math.PI;
            }
            else if (k4 >= k2 && k4 >= k3 && k4 >= k1)
            {
                res-= Math.PI / 2;
            }
         //   Console.WriteLine(k1 + " :: " + k2 + " :rwq: " + k3 + " :: " + k4);
           return Math.Atan2(Math.Sin(res), Math.Cos(res));

        }


        protected override void drawn(Color4 col,hyojiman hyo)
        {
            var bruh=hyo.render.CreateSolidColorBrush(col);
            
            hyo.render.DrawLine(new PointF(getcx(0,0) - hyo.camx, getcy(0, 0) - hyo.camy)
                ,new PointF( getcx(w, 0) - hyo.camx, getcy(w, 0) - hyo.camy),bruh,2);

            hyo.render.DrawLine(new PointF(getcx(w, 0) - hyo.camx, getcy(w, 0) - hyo.camy)
                , new PointF(getcx(w, h) - hyo.camx, getcy(w, h) - hyo.camy), bruh, 2);

            hyo.render.DrawLine(new PointF(getcx(w, h) - hyo.camx, getcy(w, h) - hyo.camy)
                , new PointF(getcx(0, h) - hyo.camx, getcy(0, h) - hyo.camy), bruh, 2);

            hyo.render.DrawLine(new PointF(getcx(0, h) - hyo.camx, getcy(0, h) - hyo.camy)
                , new PointF(getcx(0, 0) - hyo.camx, getcy(0, 0) - hyo.camy), bruh, 2);
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
        /// <param name="radd">回転角度</param>
        public Triangle(float xx, float yy, float ww, float hh,float hajih=0.5f, double radd=0) : base(xx, yy, ww, hh, radd)
        {
            haji = hajih;
        }
        public override float gettx()
        {
            return (getcx(0, 0) + getcx(0, h) + getcx(w, h*haji)) / 3;
        }
        public override float getty()
        {
            return (getcy(0, 0) + getcy(0, h) + getcy(w, h*haji)) / 3;
        }
        public override Shape clone()
        {
            var res = new Triangle(x, y, w, h,haji, rad);
            res.settxy(gettx(), getty());
            return res;
        }

      
        override public float getsaikyo(double kaku)
        {
            var a = new List<PointF>();
            a.Add(new PointF(getcx(0,0)-gettx(), getcy(0, 0) - getty()));
            a.Add(new PointF(getcx(0, h) - gettx(), getcy(0, h) - getty()));
            a.Add(new PointF(getcx(w, h*haji) - gettx(), getcy(w, h*haji) - getty()));



            float res = 0;

            foreach (var b in a)
            {
                var t = (float)Math.Sqrt(b.X*b.X + b.Y*b.Y) * (float)Math.Cos(Math.Atan2(b.Y, b.X)+kaku );
                if (t > res) res = t;
           //     Console.WriteLine(t+" sad "+(float)Math.Sqrt(b.X * b.X + b.Y * b.Y)  + " :saikyo: "+ (float)Math.Cos(Math.Atan2(b.Y, b.X) + kaku));
            }


            return Math.Abs(res);
        }
        public override float gethokyo(double kaku)
        {
            float dx = syatei() * (float)Math.Cos(kaku) ;
            float dy = syatei() * (float)Math.Sin(kaku) ;

       

            double a = kaku;

            float res = (float)Math.Sqrt(Math.Pow(getcx(0,0)-gettx(),2)+ Math.Pow(getcy(0, 0) - getty(),2));
            var aaa = new List<PointF>();
            aaa.Add(new PointF(getcx(0, 0) - gettx(), getcy(0, 0) - getty()));
            aaa.Add(new PointF(getcx(w, h * haji) - gettx(), getcy(w, h * haji) - getty()));
            aaa.Add(new PointF(getcx(0, h) - gettx(), getcy(0, h) - getty()));


            for (int i = 0; i < 2; i++)
            {

                var ka = Math.Atan2(aaa[i].Y, aaa[i].X) - a;
                var kb = Math.Atan2(aaa[i + 1].Y, aaa[i + 1].X) - a;
                if (Math.Atan2(Math.Sin(ka), Math.Cos(ka)) - Math.Atan2(Math.Sin(kb), Math.Cos(kb)) <= Math.PI && Math.Atan2(Math.Sin(ka), Math.Cos(ka)) * Math.Atan2(Math.Sin(kb), Math.Cos(kb)) < 0)
                {
                   

                    //  var kakun = Math.Atan2(aaa[i + 1].Y - aaa[i].Y, aaa[i + 1].X - aaa[i].X) - Math.Atan2(aaa[i + 1].Y, aaa[i + 1].X);
                    //  res = (float)Math.Sqrt(aaa[i + 1].X * aaa[i + 1].X + aaa[i + 1].Y * aaa[i + 1].Y) * (float)Math.Sin(kakun);
                    var kakun2 = Math.Atan2(-aaa[i].Y + aaa[i + 1].Y, -aaa[i].X + aaa[i + 1].X) - Math.Atan2(-aaa[i].Y, -aaa[i].X);
                    res = (float)Math.Sqrt(aaa[i].X * aaa[i].X + aaa[i].Y * aaa[i].Y) * (float)Math.Sin(kakun2);
                }
               
            }
            return Math.Abs(res)     ;

        }
        public override List<PointF> getpoints(float px, float py, float syatei)
        {

            var res = base.getpoints(px, py, syatei);
            res.Add(new PointF(getcx(0, 0), getcy(0, 0)));
            res.Add(new PointF(getcx(0, h), getcy(0, h)));
            res.Add(new PointF(getcx(w, h*haji), getcy(w, h*haji)));



            return res;
        }

        public override bool onhani(float px, float py)
        {

            float dx = px - getcx(0,h/2);
            float dy = py - getcy(0,h/2);

            double ddx = dx * Math.Cos(-rad) - dy * Math.Sin(-rad);
            double ddy = dx * Math.Sin(-rad) + dy * Math.Cos(-rad);

            double uh = 0,sh=0;
            if (w != 0)
            {
                sh = -  h * (1- ddx / w )* haji;
                uh =   h * (1- ddx / w )* (1 - haji);
            }
           // Console.WriteLine(haji+" ha "+ddx + " " + ddy + "  asl; " +sh+" :saf: "+uh);

            return (0 <= ddx && ddx <= w ) && (sh  <= ddy && ddy <= uh);
        }
        public override bool onhani(float px, float py, float ppx, float ppy)
        {
            if (onhani(px, py) || onhani(ppx, ppy)) return true;
            // Console.WriteLine("qwiqjo hafoudhfasuiv ");
            float dx = px - gettx();
            float dy = py - getty();

         

            float fx = ppx - gettx();
            float fy = ppy - getty();

           

            double p1x = dx,
        p1y = dy,
        q1x = fx,
        q1y = fy;
            List<List<double>> mom = new List<List<double>>();
            mom.Add(new List<double> { getcx(0, 0) - gettx(), getcy(0, 0) - getty(), getcx(0, h) - gettx(), getcy(0, h) - getty() });
            mom.Add(new List<double> { getcx(0, h) - gettx(), getcy(0, h) - getty(), getcx(w, h*haji) - gettx(), getcy(w, h*haji) - getty() });
            mom.Add(new List<double> { getcx(w, h * haji) - gettx(), getcy(w, h * haji) - getty(), getcx(0, 0) - gettx(), getcy(0, 0) - getty() });
            foreach (var a in mom)
            {
      
             //   Console.WriteLine((a[0]+gettx())+" :aanqw: " + (a[1] + getty())+" |qwei| "+(a[2]+gettx())+" :aanqwe: "+(a[3]+getty()));

                double p2x = a[0],
                 p2y = a[1],
                 q2x = a[2],
                 q2y = a[3];
                double c1 = (q1x - p1x) * (p2y - p1y) - (q1y - p1y) * (p2x - p1x);

                double c2 = (q1x - p1x) * (q2y - p1y) - (q1y - p1y) * (q2x - p1x);

                double c3 = (q2x - p2x) * (p1y - p2y) - (q2y - p2y) * (p1x - p2x);

                double c4 = (q2x - p2x) * (q1y - p2y) - (q2y - p2y) * (q1x - p2x);

                if (c1 * c2 < 0 && c3 * c4 < 0)
                {
                    double det = (p1x - q1x) * (q2y - p2y) - (q2x - p2x) * (p1y - q1y);
                    double t = ((q2y - p1y) * (q2x - q1x) + (p2x - q2x) * (q2y - q1y)) / det;

                   
                    return true;
                }
              
            }

            return false;

        }
        public override double gethosen(float px, float py)
        {
           
            float dx = px - gettx();
            float dy = py - getty();
 
            double a = Math.Atan2(dy, dx);

            double res=Math.PI+rad; 
            
            var aaa = new List<PointF>();
            aaa.Add(new PointF(getcx(0, 0) - gettx(), getcy(0, 0) - getty()));
            aaa.Add(new PointF(getcx(w, h * haji) - gettx(), getcy(w, h * haji) - getty()));
            aaa.Add(new PointF(getcx(0, h) - gettx(), getcy(0, h) - getty()));
            for (int i = 0; i < 2; i++)
            {
                var ka = Math.Atan2(aaa[i].Y, aaa[i].X) - a;
                 var kb = Math.Atan2(aaa[i + 1].Y, aaa[i + 1].X) - a;
                   if (Math.Atan2(Math.Sin(ka), Math.Cos(ka)) - Math.Atan2(Math.Sin(kb), Math.Cos(kb))<=Math.PI && Math.Atan2(Math.Sin(ka),Math.Cos(ka))* Math.Atan2(Math.Sin(kb), Math.Cos(kb))<0)
                {
                
                    res = Math.Atan2(aaa[i + 1].Y - aaa[i].Y, aaa[i + 1].X - aaa[i].X) ;

                    if (w >= 0) res -= Math.PI / 2;
                    else res += Math.PI / 2;
                }
            }
            
            return Math.Atan2(Math.Sin(res), Math.Cos(res));
        }
        public override double gethosen2(Shape s)
        {
            int k1 = 0, k2 = 0, k3 = 0;
            double res = rad;
            foreach (var p in s.getpoints(gettx(), getty(), syatei()))
            {
                var koun = nasukaku(p.X, p.Y);

                var kkkk1 = nasukaku(gettx() + (getcx(w, h * haji) - getcx(0, 0)), getty() + (getcy(w, h * haji) - getcy(0, 0)));
                var kkkk1e = Math.Atan2(Math.Sin(kkkk1 + Math.PI), Math.Cos(kkkk1 + Math.PI));

                var kkkk2 = nasukaku(gettx() + (getcx(0, h ) - getcx(w, h*haji)), getty() + (getcy(0, h) - getcy(w, h*haji)));
                var kkkk2e = Math.Atan2(Math.Sin(kkkk2 + Math.PI), Math.Cos(kkkk2 + Math.PI));

                var kkkk3 = nasukaku(gettx() + (getcx(0, 0) - getcx(0, h)), getty() + (getcy(0, 0) - getcy(0, h)));
                var kkkk3e = Math.Atan2(Math.Sin(kkkk3+Math.PI), Math.Cos(kkkk3 + Math.PI));



                if (Math.Atan2(Math.Sin(koun-kkkk2),Math.Cos(koun-kkkk1))<=0&& Math.Atan2(Math.Sin(koun - kkkk1e), Math.Cos(koun - kkkk1e)) >= 0) k1 += 1;
                if (Math.Atan2(Math.Sin(koun - kkkk2), Math.Cos(koun - kkkk2)) <= 0 && Math.Atan2(Math.Sin(koun - kkkk2e), Math.Cos(koun - kkkk2e)) >= 0) k2 += 1;
                if (Math.Atan2(Math.Sin(koun - kkkk3), Math.Cos(koun - kkkk3)) <= 0 && Math.Atan2(Math.Sin(koun - kkkk3e), Math.Cos(koun - kkkk3e)) >= 0) k3 += 1;


            }
            if (k1 >= k2 && k1 >= k3 )
            {

                res = nasukaku(gettx() + (getcx(w, h * haji) - getcx(0, 0)), getty() + (getcy(w, h * haji) - getcy(0, 0)));

                if (w >= 0) res -= Math.PI / 2;
                else res += Math.PI / 2;
            }
            else if (k2 >= k1 && k2 >= k3 )
            {

                res = nasukaku(gettx() + (getcx(0, h) - getcx(w, h * haji)), getty() + (getcy(0, h) - getcy(w, h * haji)));

                if (w >= 0) res -= Math.PI / 2;
                else res += Math.PI / 2;
            }
            else if (k3 >= k2 && k3 >= k1)
            {

                res = nasukaku(gettx() + (getcx(0, 0) - getcx(0, h)), getty() + (getcy(0, 0) - getcy(0, h)));

                if (w >= 0) res -= Math.PI / 2;
                else res += Math.PI / 2;
            }
            return Math.Atan2(Math.Sin(res), Math.Cos(res));

        }


        protected override void drawn(Color4 col, hyojiman hyo)
        {
            var bruh = hyo.render.CreateSolidColorBrush(col);
            hyo.render.DrawLine(new PointF(getcx(0, 0) - hyo.camx, getcy(0, 0) - hyo.camy)
                , new PointF(getcx(w, h * _haji) - hyo.camx, getcy(w, h*_haji) - hyo.camy), bruh, 2);

            hyo.render.DrawLine(new PointF(getcx(0, h) - hyo.camx, getcy(0, h) - hyo.camy)
                , new PointF(getcx(w, h * _haji) - hyo.camx, getcy(w, h * _haji) - hyo.camy), bruh, 2);

            hyo.render.DrawLine(new PointF(getcx(0, 0) - hyo.camx, getcy(0, 0) - hyo.camy)
                , new PointF(getcx(0, h) - hyo.camx, getcy(0, h) - hyo.camy), bruh, 2);

         
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
        public Circle(float xx, float yy, float ww, float hh, double radd,int pointkinji=20) : base(xx, yy, ww, hh, radd)
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
            if (onhani(px, py) || onhani(ppx, ppy)) return true;
            //  Console.WriteLine("qwiqjo hafoudhfasuiv ");
            float dx = px - gettx();
            float dy = py - getty();

            double ddx = dx * Math.Cos(-rad) - dy * Math.Sin(-rad);
            double ddy = dx * Math.Sin(-rad) + dy * Math.Cos(-rad);


            float fx = ppx - gettx();
            float fy = ppy - getty();

            double ffx = fx * Math.Cos(-rad) - fy * Math.Sin(-rad);
            double ffy = fx * Math.Sin(-rad) + fy * Math.Cos(-rad);


            double p1x = ddx,
        p1y = ddy,
        q1x = ffx,
        q1y = ffy;
            double kaku = Math.Atan2(q1y - p1y, q1x - p1x);
            List<List<double>> mom = new List<List<double>>();
            for (int i = 0; i < kinji; i +=1)
            {
                var nag = (float)(w * h / 4 / Math.Sqrt(w * w / 4 * Math.Sin((i + 1) * Math.PI / kinji*2) * Math.Sin((i + 1) * Math.PI / kinji*2) 
                    + h * h / 4 * Math.Cos((i + 1) * Math.PI / kinji * 2) * Math.Cos((i + 1) * Math.PI / kinji * 2)));
                var nag2 = (float)(w * h / 4 / Math.Sqrt(w * w / 4 * Math.Sin((i ) * Math.PI / kinji * 2) * Math.Sin((i) * Math.PI / kinji * 2)
                   + h * h / 4 * Math.Cos((i ) * Math.PI / kinji * 2) * Math.Cos((i ) * Math.PI / kinji * 2)));

                mom.Add(new List<double> { nag * Math.Cos((i+1)*Math.PI/kinji*2), nag * Math.Sin((i + 1) * Math.PI /kinji*2)
                    ,nag2 * Math.Cos((i)*Math.PI/kinji*2), nag2 * Math.Sin((i ) * Math.PI / kinji*2) });
            }
          //  mom.Add(new List<double> { 0, 0, w * Math.Cos(kaku + Math.PI / 2), h * Math.Sin(kaku + Math.PI / 2) });
         //   mom.Add(new List<double> { 0, 0, w * Math.Cos(kaku - Math.PI / 2), h * Math.Sin(kaku - Math.PI / 2) });
            foreach (var a in mom)
            {
               // foreach (var b in a) Console.WriteLine("aa " + b);

                double p2x = a[0],
                 p2y = a[1],
                 q2x = a[2],
                 q2y = a[3];
                double c1 = (q1x - p1x) * (p2y - p1y) - (q1y - p1y) * (p2x - p1x);

                double c2 = (q1x - p1x) * (q2y - p1y) - (q1y - p1y) * (q2x - p1x);

                double c3 = (q2x - p2x) * (p1y - p2y) - (q2y - p2y) * (p1x - p2x);

                double c4 = (q2x - p2x) * (q1y - p2y) - (q2y - p2y) * (q1x - p2x);

                if (c1 * c2 < 0 && c3 * c4 < 0)
                {
                    //Console.WriteLine(px + " -> " + ppx + " :: " + py + " -> " + ppy + " " + gettx() + "::" + getty() + " unchon " + p2x + " -> " + q2x + " :: " + p2y + " -> " + q2y);
                    double det = (p1x - q1x) * (q2y - p2y) - (q2x - p2x) * (p1y - q1y);
                    double t = ((q2y - p1y) * (q2x - q1x) + (p2x - q2x) * (q2y - q1y)) / det;
                    
                    return true;
                }

            }
            
            return false;

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
            var bruh = hyo.render.CreateSolidColorBrush(col);
            hyo.render.Transform = Matrix3x2.CreateRotation((float)rad, new Vector2(gettx() - hyo.camx, getty() - hyo.camy));
            hyo.render.DrawEllipse(new Ellipse(new PointF(gettx()-hyo.camx, getty() - hyo.camy), w/2,h/2), bruh, 2);


            bruh.Dispose();
        }

        public override float syatei()
        {
            return (float)Math.Sqrt(Math.Pow(Math.Max(w/2,h/2),2));
        }
    }
}
