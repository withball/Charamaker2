using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Mathematics;
using Vortice.Direct2D1;
using System.Numerics;
using System.Drawing;
using System.Web.UI.WebControls;
using Vortice.XAudio2.Fx;
using System.Security.Policy;
using Microsoft.VisualBasic.Logging;
using Vortice.DirectWrite;
/// <summary>
/// 図形を扱う名前空間
/// </summary>
namespace Charamaker2.Shapes
{
    /// <summary>
    /// ポイント,ベクトルを表すためのクラス
    /// </summary>
    public class FXY
    {
        public float X, Y;

        public float length { get { return (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)); }
            set { X = value * (float)Math.Cos(rad); Y = value * (float)Math.Sin(rad); } }
        public double rad { get { return Math.Atan2(Y, X); }
            set { var input = Shape.radseiki(value); X = length * (float)Math.Cos(input); Y = length * (float)Math.Sin(input); } }

        public FXY unit { get { var res = new FXY(X, Y); res.length = 1; return res; } }

        public FXY(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public FXY(FXY f)
        {
            this.X = f.X;
            this.Y = f.Y;
        }
        public FXY(float length, double rad)
        {
            this.X = length * (float)Math.Cos(rad);
            this.Y = length * (float)Math.Sin(rad);
        }
        public static FXY operator +(FXY a, FXY b)
        {
            return new FXY(a.X + b.X, a.Y + b.Y);
        }
        public static FXY operator -(FXY a, FXY b)
        {
            return new FXY(a.X - b.X, a.Y - b.Y);
        }
        public static bool operator ==(FXY a, FXY b)
        {
            bool anul = a is null;
            bool bnul = b is null;
            if (anul && bnul) return true;
            if (anul || bnul) return false;
            return a.X==b.X&&a.Y==b.Y;
        }
        public static bool operator !=(FXY a, FXY b)
        {
            bool anul = a is null;
            bool bnul = b is null;
            if (anul && bnul) return false;
            if (anul || bnul) return true;
            return !(a.X == b.X && a.Y == b.Y);
        }
        public static FXY operator *(FXY a, float b)
        {
            return new FXY(a.X * b, a.Y * b);
        }
        public static FXY operator /(FXY a, float b)
        {
            return new FXY(a.X / b, a.Y / b);
        }
       
        override public string ToString() 
        {
            return X + " :XY: " + Y;
        }
        
    }
    /// <summary>
    ///	接触したライン等表すクラス
    /// </summary>
    public class lineX
    {
        public FXY begin, end, bs;

        public float length { get { return (end - begin).length; } }

        public double rad { get { return (end - begin).rad; } }
        public double hosen
        {
            get
            {

                double rad = (end - begin).rad;
                double rad2 = (bs - begin).rad;

                if (Shape.radseiki(rad2 - rad) < 0)
                {
                    return Shape.radseiki(rad + Math.PI / 2);
                }
                else
                {
                    return Shape.radseiki(rad - Math.PI / 2);

                }
                return rad;
            }
        }
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="a">始まりの点</param>
        /// <param name="b">終わりの点</param>
        /// <param name="kijyun">辺の内側の中心点</param>
        public lineX(FXY a, FXY b, FXY kijyun)
        {
            begin = a;
            end = b;
            bs = kijyun;
        }/// <summary>
         /// 普通のコンストラクタ
         /// </summary>
         /// <param name="bx">開始点x</param>
         /// <param name="by">開始点y</param>
         /// <param name="ex">終了店x</param>
         /// <param name="ey">終了店y</param>
         /// <param name="kx">基準点(内側の点)x</param>
         /// <param name="ky">基準点y</param>
        public lineX(float bx,float by,float ex,float ey, float kx,float ky)
        {
            begin = new FXY(bx,by);
            end = new FXY(ex, ey);
            bs = new FXY(kx, ky);
        }

        /// <summary>
        /// 線の中間地点
        /// </summary>
        public FXY center 
        {
            get { return (begin + end) / 2; }
        }

        /// <summary>
        /// その点が辺の内側にあるか検知する
        /// </summary>
        /// <param name="poi">その点</param>
        /// <returns>内側であればtrue</returns>
        public bool onInside(FXY poi)
        {
            double r = rad;
            double kij = (bs - begin).rad;
            double p = (poi - begin).rad;
            kij = Shape.radseiki(kij - r);
            p = Shape.radseiki(p - r);
            return (kij>=0) == (p>=0);
        }
        /// <summary>
        /// 点と直線の距離を求めてくれる
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <returns>.lengthでちゃんとした距離</returns>
        public FXY getkyori(float px, float py) 
        {
            var res = new FXY(0, 0);
            double rad = -this.rad;
            double rad2 = -this.hosen;

            //TRACE(_T("rad:::%f  hos %f\n"), rad/M_PI*180, rad2 / M_PI * 180);
            float a1 = (float)Math.Sin(rad), b1 = (float)Math.Cos(rad), c1 = -this.begin.X, d1 = -this.begin.Y;

            float a2 = (float)Math.Sin(rad2), b2 = (float)Math.Cos(rad2), c2 = -px, d2 = -py;


            if (a1 == 0)
            {
                res.X = -c2;
                res.Y = -d1;
            }
            else if (b1 == 0)
            {
                res.X = -c1;
                res.Y = -d2;
            }
            else
            {
                res.X = (b1 * (a2 * c2 + d2 * b2) - b2 * (a1 * c1 + d1 * b1)) / (a1 * b2 - a2 * b1);
                res.Y = (a2 * (a1 * c1 + d1 * b1) - a1 * (a2 * c2 + d2 * b2)) / (a1 * b2 - a2 * b1);
            }
            return res;
        }
        override public string ToString()
        {
            return begin.ToString() + " :begin end: " + end.ToString() + " ->"
                + rad * 180 / Math.PI + " :rad hos: "+ hosen * 180 / Math.PI;
        }


        public bool crosses(lineX l)
        {
            double psx=this.begin.X, psy= this.begin.Y, pex= this.end.X, pey= this.end.Y
                , qsx= l.begin.X, qsy= l.begin.Y, qex= l.end.X, qey= l.end.Y;
            double c1 = (pex - psx) * (qsy - psy) - (pey - psy) * (qsx - psx);

            double c2 = (pex - psx) * (qey - psy) - (pey - psy) * (qex - psx);

            double c3 = (qex - qsx) * (psy - qsy) - (qey - qsy) * (psx - qsx);

            double c4 = (qex - qsx) * (pey - qsy) - (qey - qsy) * (pex - qsx);

            if (c1 * c2 < 0 && c3 * c4 < 0)
            {


                return true;
            }
            return false;
        }

        public static bool operator ==(lineX a, lineX b)
        {
            bool anul = a is null;
            bool bnul = b is null;
            if (anul && bnul) return true;
            if (anul || bnul) return false;
            return a.begin == b.begin&& a.end == b.end&&a.bs==b.bs;
        }
        public static bool operator !=(lineX a, lineX b)
        {

            bool anul = a is null;
            bool bnul = b is null;
            if (anul && bnul) return false;
            if (anul || bnul) return true;
            return a.begin != b.begin || a.end != b.end || a.bs != b.bs;
        }
    };

    /// <summary>
    /// 図形の基底クラス
    /// </summary>
    public class Shape
    {
        #region statics

        /// <summary>
        /// 図形二つの重なり。両側からやるよ
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool atarun(Shape a,Shape b) 
        {
            return a.kasanari(b);
        }
        /// <summary>
        /// 図形2つとそれぞれの位置フレーム前での重なり
        /// </summary>
        /// <returns></returns>
        public static bool atarun(Shape a,Shape pa, Shape b,Shape pb)
        {
            return atarun(gousei(a,pa), gousei(b, pb));
        }
        
        /// <summary>
        /// 二つの図形を合成する。結果は最大の面積を持つ図形になる
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Shape gousei(Shape a,Shape  b) 
        {
            var lis = a.getzettaipoints();
            lis.AddRange(b.getzettaipoints());

            var res = new List<FXY>();
            FXY tm = lis.First(),pm=lis.Last();

            for (int t=0;t<lis.Count;t++) 
            {
                if (tm.X < lis[t].X) 
                {
                    tm = lis[t];
                }
                if (pm.X > lis[t].X)
                {
                    pm = lis[t];
                }
            }
            res.Add(pm);
            FXY now = pm;
            double kaku = (now - tm).rad;
            do
            {
                var removes=new List<FXY>();
           //     Console.WriteLine(now.X+" noww "+now.Y+" lis.count "+lis.Count);
                tm = lis.First();
                for (int i=lis.Count-1;i>=0;i--) 
                {
                    double rad = radseiki2((lis[i] - now).rad-kaku);
                    if (lis[i] != now) 
                    {
                        if (radseiki2((tm - now).rad - kaku) > rad)
                        {
                            tm = lis[i];
                            removes.Clear();
                        }
                        else if(radseiki2((tm - now).rad - kaku) == rad)
                        {
                            if ((tm - now).length < (lis[i] - now).length)
                            {
                                var tmp = tm;
                                tm = lis[i];

                              removes.Add(tmp);
                            }
                            else 
                            {
                                removes.Add(lis[i]);
                            }
                        }
                    }
                }
                kaku = (tm-now).rad;

                now = tm;
                res.Add(now);
                lis.Remove(now);
                foreach (var c in removes) lis.Remove(c);
               // Console.WriteLine(now.X + " denden  " + now.Y + " lis.count " + lis.Count);
            } while (now != pm&&lis.Count>0);
            if(now==pm)res.RemoveAt(res.Count - 1);

          /*  foreach (var c in res) 
            {
                Console.WriteLine(c.X + " XY " + c.Y);
            }
            Console.WriteLine("da");
          */  FXY max=new FXY(res.First()), min = new FXY(res.First());
            for (int i = 1; i < res.Count; i++) 
            {
                if (max.X < res[i].X) max.X = res[i].X;
                if (max.Y < res[i].Y) max.Y = res[i].Y;
                if (min.X > res[i].X) min.X = res[i].X;
                if (min.Y > res[i].Y) min.Y = res[i].Y;
            }
            float www= max.X - min.X, hhh = max.Y - min.Y;

            foreach (var c in res)
            {
                if (www > 0) c.X = (c.X - min.X) / www;
                else c.X = 0;
                if (hhh > 0) c.Y = (c.Y - min.Y) / hhh;
                else c.Y = 0;
            }

            return new Shape(min.X,min.Y,www,hhh, 0,res);
        }

        /// <summary>
        /// ラジアンの正規化
        /// </summary>
        /// <param name="r">正規化するラジアン</param>
        /// <returns>-PI~PI正規化されたラジアン</returns>
        public static double radseiki(double r)
        {
            return Math.Atan2(Math.Sin(r), Math.Cos(r));
        }
        /// <summary>
        /// ラジアンの正規化2
        /// </summary>
        /// <param name="r">正規化するラジアン</param>
        /// <returns>0~2PIに正規化されたラジアン</returns>
        public static double radseiki2(double r)
        {
            var res = radseiki(r);
            if (res < 0) res = Math.PI * 2 + res;
            return res;
        }
        /// <summary>
        /// ラジアンを-90~90度に直す
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static double radkatamuki(double r)
        {
            r = radseiki(r);
            if (Math.Abs(r) > Math.PI / 2)
            {
                r += Math.PI;
            }
            return radseiki(r);
        }
        /// <summary>
        /// 線がクロスしているのかを判定する
        /// </summary>
        /// <param name="mom">{x1,y1,x2,y2}</param>
        /// <param name="mom2">{x1,y1,x2,y2}</param>
        /// <returns></returns>
        static protected bool crosses(double[] mom, double[] mom2)
        {
            return crosses(mom[0], mom[1], mom[2], mom[3], mom2[0], mom2[1], mom2[2], mom2[3]);
        }
        /// <summary>
           /// 線がクロスしているのかを判定する
           /// </summary>
           /// <returns></returns>
        static protected bool crosses(FXY ps,FXY pe,FXY qs,FXY qe)
        {
            return crosses(ps.X, ps.Y, pe.X, pe.Y, qs.X, qs.Y, qe.X, qe.Y);
        }
        /// <summary>
        /// 線がクロスしているのかを判定する
        /// </summary>
        /// <returns></returns>
        static protected bool crosses(double psx,double psy,double pex,double pey, double qsx, double qsy, double qex, double qey)
        {

            double c1 = (pex - psx) * (qsy - psy) - (pey - psy) * (qsx - psx);

            double c2 = (pex - psx) * (qey - psy) - (pey - psy) * (qex - psx);

            double c3 = (qex - qsx) * (psy - qsy) - (qey - qsy) * (psx - qsx);

            double c4 = (qex - qsx) * (pey - qsy) - (qey - qsy) * (pex - qsx);

            if (c1 * c2 < 0 && c3 * c4 < 0)
            {


                return true;
            }
            return false;
        }
        static double nasukaku(float ax,float ay,float bx,float by) 
        {
            return Math.Atan2(by - ay, bx - ax);
        }
        static float kyori(float ax, float ay, float bx, float by)
        {
            return (float)Math.Sqrt(Math.Pow(ax-bx,2)+ Math.Pow(ay - by, 2));
        }
        /// <summary>
        /// 図形内部の点を割り出す
        /// </summary>
        /// <param name="kaku">中心からの角度</param>
        /// <param name="nagwari">長さの割合</param>
        /// <param name="kaiten">trueならkakuが図形の回転型されたものになる</param>
        /// <returns></returns>
        public virtual FXY getinnerpoint(double kaku, float nagwari, bool kaiten=false) 
        {
            var c = getCenter();
            var lis = getzettaipoints(false);
            int idx = 1;
            for (int i = 1; i < lis.Count - 1; i++) 
            {
                if (radseiki((lis[i - 1] - c).rad-kaku)* radseiki((lis[i] - c).rad - kaku) <= 0) 
                { 
                    {
                        idx = i;
                        break;
                    }
                }
            }
            var L = lis[idx - 1] - c;
            var R = lis[idx] - c;
            float n = 0;
             var soi  = (new FXY(L.length,kaku)-L);

            if (R.X != 0)
            {
                n = Math.Max(Math.Abs(soi.X / R.X),n);
            }
            if(R.Y!=0)
            {
                n = Math.Max(Math.Abs(soi.Y / R.Y),n);
            }
            var Lwari = 1 / (n + 1);
            var res= L * Lwari * nagwari + R * (1 - Lwari) * nagwari;
            if (kaiten) res.rad -= rad;
            return res+c;
        }

        /// <summary>
        /// 点を線と重なるようにずらす幅を教えてくれる。どうしても重ならない場合は(0,0)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="l"></param>
        /// <param name="yokomugen">線が無限の長さだと解釈する</param>
        /// <param name="outer">外側の点だとしてもずらす</param>
        /// <returns></returns>
        public static FXY getzurasi(FXY point, lineX l, bool yokomugen = false,bool outer=false)
        {
            //	TRACE(_T("%f::%f ||||%f %f:idoukaku:%f %f  %f\n"),point.x,point.y, l.begin.x, l.begin.y, l.end.x, l.end.y, l.getrad());
           /* Console.WriteLine(point.X + " point " + point.Y);
            Console.WriteLine(l.begin.X + " lineB " + l.begin.Y);
            Console.WriteLine(l.end.X + " lineE " + l.end.Y);
            Console.WriteLine(l.bs.X + " lineK " + l.bs.Y);
         */
            if (!l.onInside(point)&&!outer)
            {
                double r = l.rad;
                double kij = (l.bs - l.begin).rad;
                double p = (point - l.begin).rad;
                kij = Shape.radseiki(kij - r);
                p = Shape.radseiki(p - r);
            //    Console.WriteLine(kij+ "::ohnooQQ::"+p+"  :->: "+point.ToString());
                return new FXY(float.NaN, float.NaN); ;
            }
            var res= l.getkyori(point.X, point.Y);

            // Console.WriteLine(res.X + " tentyoku " + res.Y);

            float bigx = Math.Max(l.begin.X, l.end.X);
            float minx = Math.Min(l.begin.X, l.end.X);
            float bigy = Math.Max(l.begin.Y, l.end.Y);
            float miny = Math.Min(l.begin.Y, l.end.Y);

            if (yokomugen ||
                ((minx <= res.X && res.X <= bigx) && (miny <= res.Y && res.Y <= bigy)
               ))
            {
               /* double r = l.rad;
                double kij = (l.bs - l.begin).rad;
                double p = (point - l.begin).rad;
                kij = Shape.radseiki(kij - r);
                p = Shape.radseiki(p - r);
                Console.WriteLine(kij + "::OKKKKKKQQ::" + p + "  :->: " + point.ToString());

                Console.WriteLine(minx + "::OKKKKKKQQ::" + bigx + " ->  " + miny + " :: " + bigy + " <- " + res.ToString());
                Console.WriteLine("ZZZZ " + (minx <= res.X && res.X <= bigx) + " :: " + (miny <= res.Y && res.Y <= bigy));
               */

                res.X -= point.X;
                res.Y -= point.Y;
               

                return res;
            }
           // Console.WriteLine("YkosugiTA");
            		//TRACE(_T("ohno:: %f %f\n"), res.x, res.y);
            return new FXY(float.NaN,0);


        }
        /// <summary>
        /// 図形を合成する
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Shape operator +(Shape a,Shape b)
        {
            return gousei(a,b);
        }
        #endregion
        #region define
        /// <summary>
        /// 図形の基本要素
        /// </summary>
        public float x, y,w,h;
        
        /// <summary>
        /// カクード
        /// </summary>
        protected double _rad;
        /// <summary>
        /// セットする際は-Pi＜＝rad＜＝Piの範囲にする。そして重心で回転させる
        /// </summary>
        public double rad
        {
            get { return _rad; }
            set
            {
                float x = gettx();
                float y = getty();
                _rad = value;
                settxy(x, y);
                _rad = radseiki(_rad);
            }
        }
        /// <summary>
        /// 図形の頂点の相対座標ども
        /// </summary>
        List<FXY> points;
   
      


        /// <summary>
        /// 絶対座標に直した各頂点を取得する
        /// </summary>
        /// <param name="kaburinasi">被らない範囲のやつだけ</param>
        /// <returns></returns>
        public List<FXY> getzettaipoints(bool kaburinasi = true)
        {
            List<FXY> res=new List<FXY>();

            if (kaburinasi)
            {
                for (int i = 1; i < points.Count - 1; i++)
                {
                    res.Add(new FXY(x + (float)Math.Cos(rad) * points[i].X*w - (float)Math.Sin(rad) * points[i].Y * h
                        , y + (float)Math.Sin(rad) * points[i].X * w + (float)Math.Cos(rad) * points[i].Y * h));
                }
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    res.Add(new FXY(x + (float)Math.Cos(rad) * points[i].X * w - (float)Math.Sin(rad) * points[i].Y * h
                      , y + (float)Math.Sin(rad) * points[i].X * w + (float)Math.Cos(rad) * points[i].Y * h));
                 
                }

              
            }
            return res;
        }

        /// <summary>
        /// 各頂点の相対座標を取得する
        /// </summary>
        /// <param name="kaburinasi">被らない範囲のやつだけ</param>
        /// <returns></returns>
        public List<FXY> getSoutaiPoints(bool kaburinasi = true)
        {
            List<FXY> res = new List<FXY>();

            if (kaburinasi)
            {
                for (int i = 1; i < points.Count - 1; i++)
                {
                    res.Add(new FXY(points[i].X * w,points[i].Y * h));
                }
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    res.Add(new FXY(points[i].X * w, points[i].Y * h));
                }
            }
            return res;
        }
        /// <summary>
        /// 図形が当たっているか
        /// </summary>
        /// <param name="p">1フレーム前の自分</param>
        /// <param name="e">相手</param>
        /// <param name="pe">1フレーム前の相手</param>
        /// <returns></returns>
        public bool atarun2(Shape p, Shape e, Shape pe) 
        {
            return atarun(this, p, e, pe);
        }/// <summary>
         /// 図形が当たっているか
         /// </summary>
         /// <param name="e"></param>
        public bool atarun( Shape e)
        {
            return atarun(this,e);
        }
        /// <summary>
        /// 図形の外周の長さを返す
        /// </summary>
        /// <returns></returns>
        public float gaisyuu
        {
            get
            {
                float sum = 0;
                for (int i = 1; i < points.Count - 1; i++)
                {

                    sum += (float)Math.Sqrt(Math.Pow(points[i + 1].Y - points[i].Y, 2) + Math.Pow(points[i + 1].X - points[i].X, 2));
                }
                return sum;
            }
        }
        /// <summary>
        /// 図形の面積を返す
        /// </summary>
        /// <returns></returns>
        public float menseki
        {
            get
            {
                var c = getSoutaiCenter();
                var points = getSoutaiPoints(false);
                float sum = 0;
                for (int i = 1; i < points.Count - 1; i++)
                {
                    var k = radseiki(nasukaku(points[i].X, points[i].Y, c.X, c.Y)
                        - nasukaku(points[i].X, points[i].Y, points[i + 1].X, points[i + 1].Y));
                    var kyo = kyori(points[i].X, points[i].Y, c.X, c.Y);
                    //TRACE(_T("%f aklfoaii\n"), fabs(kyo * kyo * sinf(k) * cosf(k)));
                    sum += (float)Math.Abs(kyo * kyo * Math.Sin(k) * Math.Cos(k));

                }
                //TRACE(_T("%f OMOSA\n"),sum);
                return sum;
            }
        }
        /// <summary>
        /// 頂点の座標をセットする
        /// </summary>
        /// <param name="points"></param>
        protected void setpoints(List<FXY>points) 
        {

            this.points = new List<FXY>();
            if (points.Count > 0)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    this.points.Add(new FXY(points[i]));
                }
                this.points.Add(points[0]);

                this.points.Insert(0, points[points.Count - 1]);
            }
        }
        #endregion

        /// <summary>
        /// 図形をいい感じに反転させる(方向を決める)メソッド大体のやつは左右等しいので意味ないけど
        /// </summary>
        /// <param name="mir">1で右向き、-1で左向きに0で普通の反転</param>
        public virtual void setMirror(int mir) 
        {
            
        }


        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="xx">ｘ座標</param>
        /// <param name="yy">ｙ座標</param>
        /// <param name="ww">幅</param>
        /// <param name="hh">高さ</param>
        /// <param name="radd">角度</param>
        /// <param name="points">図形の頂点の相対座標(w,hに対する比)ども(0~1)</param>
        public Shape(float xx, float yy, float ww, float hh, double radd, List<FXY> points)
        {

            x = xx;
            y = yy;
            w = ww;
            h = hh;
            _rad = Math.Atan2(Math.Sin(radd), Math.Cos(radd));


                setpoints(points);
            
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
        /// 中心の絶対座標を求める
        /// </summary>
        /// <returns></returns>
        public FXY getCenter() 
        {
            var points = getzettaipoints();
            FXY res=new FXY(0, 0);
            for (int i = 0; i < points.Count; i++)
            {

                res.X += points[i].X;
                res.Y += points[i].Y;
            }
            res.X = res.X / points.Count ;
            res.Y = res.Y / points.Count ;

            return res;
        }
        /// <summary>
        /// 中心の相対座標を求める
        /// </summary>
        /// <returns></returns>
        public FXY getSoutaiCenter()
        {
            FXY res = new FXY(0, 0);
            for (int i = 1; i < points.Count-1; i++)
            {

                res.X += points[i].X;
                res.Y += points[i].Y;
            }

            res.X = res.X / points.Count * w;
            res.Y = res.Y / points.Count * h;

            return res;
        }
        /// <summary>
        ///　重心のx座標を返す
        /// </summary>
        /// <returns>返されるのはx座標の値</returns>
        public float gettx()
        {          
            return getCenter().X;

        }
        /// <summary>
        ///　重心のy座標を返す
        /// </summary>
        /// <returns>返されるのはy座標の値</returns>
        public float getty()
        {
            return getCenter().Y;

        }
        /// <summary>
        /// 重心をxy座標にセットする。
        /// </summary>
        /// <param name="xx">セットするx座標</param>
        /// <param name="yy">セットするy座標</param>
        virtual public void settxy(float xx, float yy)
        {
            x += xx - gettx();
            y += yy - getty();

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
        /// 図形をピクチャーにかぶせるっていうかセットする
        /// </summary>
        /// <param name="p"></param>
        public void setto(picture p)
        {
            w = p.w;
            h = p.h;
            rad = p.RAD;
            x = p.x;
            y = p.y;
        }
        /// <summary>
        /// 図形をキャラクターにかぶせるっていうかセットする
        /// </summary>
        /// <param name="p"></param>
        public void setto(Character.character p)
        {
            w = p.w;
            h = p.h;
            rad = p.RAD;
            x = p.x;
            y = p.y;
        }
        /// <summary>
        /// 中心の座標を変えずにサイズを拡大縮小する
        /// </summary>
        /// <param name="sc">スケール</param>
        public void scale(float sc)
        {
            float x = gettx();
            float y = getty();
            w = this.w * sc;
            h = this.h * sc;

            settxy(x, y);
        }

        /// <summary>
        /// 図形を描画する
        /// </summary>
        /// <param name="hyojiman">描画するhyojiman</param>
        /// <param name="R">線の色</param>
        /// <param name="G">線の色</param>
        /// <param name="B">線の色</param>
        /// <param name="A">線の不透明度</param>
        /// <param name="hutosa">線のふとさ</param>
        /// <param name="begin">hyojimanのbegindrawをついでにするか(すると重くなるので外部でやるのがおススメ)</param>
        public void drawshape(hyojiman hyojiman, float R, float G, float B, float A,float hutosa=3, bool begin = false)
        {
            if (begin)
            {
                hyojiman.render.BeginDraw();
            }
            drawn(new Color4(R, G, B, A),hutosa, hyojiman);
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
        /// <param name="hutosa">線の太さ</param>
        protected void drawn(Color4 col,float hutosa, hyojiman hyo) 
        {
            var lis = getzettaipoints(false);

            for (int i = 1; i < lis.Count-1;i++) 
            {
                hyo.drawLine( lis[i-1].X, lis[i - 1].Y, lis[i].X, lis[i].Y, col,hutosa);
            }
        }
        /// <summary>
        /// その図形が図形内にあるかのどうかの判定。両側からやってくれる
        /// </summary>
        /// <param name="s">その図形</param>
        /// <param name="onis">1.0001とかでちょっとアバウトに判定してくれる</param>
        /// <returns>あるか</returns>
        public bool onhani(Shape s,float onis=1) 
        {
            foreach (var a in this.getzettaipoints()) 
            {
                if (s.onhani(a.X, a.Y,onis)) return true;
            }
            foreach (var a in s.getzettaipoints())
            {
                if (this.onhani(a.X, a.Y, onis)) return true;
            }
            return false;
        }

        /// <summary>
        /// その点が図形内にあるかのどうかの判定
        /// </summary>
        /// <param name="px">その点のx座標</param>
        /// <param name="py">その点のy座標</param>
        /// <param name="onis">1.0001とかでちょっとアバウトに判定してくれる</param>
        /// <returns>あるか</returns>
        virtual public bool onhani(float px, float py,float onis=1) 
        {
            FXY myCenter = new FXY(gettx(),getty());
            float XXX = px - myCenter.X;
            float YYY = py - myCenter.Y;
            var points = getzettaipoints(false);
            //TRACE(_T("%f :SOY: %f\n"), x, y);
            for (int i = 1; i < points.Count - 1; i++)
            {
                float x1 = points[i].X - myCenter.X;
                float y1 = points[i].Y - myCenter.Y;
                float x2 = points[i + 1].X - myCenter.X;
                float y2 = points[i + 1].Y - myCenter.Y;
                //Console.WriteLine(x1 + "::" + y1 + " これがオン範囲のあれや！ " + x2 + " :: " + y2);
               
                float t, s;
                if (y1 == 0)
                {
                    s = YYY / y2;
                    t = (XXX - x2 * s ) / x1;
                }
                else
                {
                    s = (XXX - YYY * x1 / y1) / (x2 - y2 * x1 / y1);
                    t = (YYY - y2 * s) / y1;
                }

                //Console.WriteLine(s+" +++ "+t+" = "+(s+t));
                
                if (s >= 0 && t >= 0 && s + t <= onis)
                {
                   // Console.WriteLine(s + " :onEQQEQEWQEQEhani: " + t);
                   // Console.WriteLine(points[i].X+" :onEQQEQEWQEQEhani: "+ points[i].Y);
                    return true;
                }
                else
                {
                    //TRACE(_T("%f :onhani: %f\n"), points[i].x, points[i].y);
                }

            }
            return false;
        }
        /// <summary>
        /// その線が図形と接触するかどうかの判定
        /// </summary>
        /// <param name="px">その点のx座標1</param>
        /// <param name="py">その点のy座標1</param>
        /// <param name="ppx">その点のx座標2</param>
        /// <param name="ppy">その点のy座標2</param>
        /// <returns></returns>
        virtual public bool onhani(float px, float py, float ppx, float ppy) 
        {
            FXY sp = new FXY(px - x, py - y);
            FXY spp = new FXY(ppx - x, ppy - y);

            sp.rad -= rad;
            spp.rad -= rad;

            bool Lsp = sp.X < 0, Rsp = sp.X > w;
            bool Usp = sp.Y < 0, Dsp = sp.Y > h;

            bool Lspp = spp.X < 0, Rspp = spp.X >  w;
            bool Uspp = spp.Y < 0, Dspp = spp.Y >  h;

            var lis = getzettaipoints(false);

            if ((Lsp&&Lspp)|| (Rsp && Rspp)|| (Usp && Uspp) || (Dsp && Dspp))
                {  return false; }
        


            if (onhani(px, py) || onhani(ppx, ppy)) return true;

            for (int i = 1; i < lis.Count - 1; i++) 
            {
                var mom1 = new double[] { lis[i].X, lis[i].Y, lis[i + 1].X, lis[i + 1].Y };
                var mom2 = new double[] { px,py,ppx,ppy };
                if (Shape.crosses(mom1, mom2)) return true; 
            }
            return false;
        }
        /// <summary>
        /// 図形の外周の辺のリストを手に入れる。引数無しで重心を中心とした相対座標で出してくれる
        /// </summary>
        /// <param name="x">中心座標x</param>>
        /// <param name="y">中心座標y</param>>
        /// <param name="rad">重心を中心とした回転角</param>>
        /// <returns>外周のリスト(相対座標){{x1,y1,x2,y2},{x1,y1,x2,y2}}</returns>
        protected List<double[]> getgaisyuus(float x = 0, float y = 0, double rad = 0) 
        {
            var res=new List<double[]>();

            FXY sc = getSoutaiCenter();

            for (int i = 1; i < points.Count - 1; i++) 
            {
                FXY one, two;
                one = new FXY(points[i].X * w, points[i].Y * h);
                two = new FXY(points[i + 1].X * w, points[i + 1].Y * h);

                one = one - sc;
                two = two - sc;

                one.rad += rad;
                two.rad += rad;

                res.Add(new double[] {one.X+x, one.Y + y, two.X + x, two.Y + y });

            }
            return res;
        }


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
        /// 頂点を複製する
        /// </summary>
        /// <returns></returns>
        protected List<FXY> clonepoints() 
        {
            var res = new List<FXY>();

            for (int i = 1; i < points.Count-1; i++) 
            {
                res.Add(new FXY(points[i]));
            }

            res.Add(points[0]);

            res.Insert(0, points[points.Count - 2]);
            return res;
        }
        /// <summary>
        /// 図形を複製する
        /// </summary>
        /// <returns>複製された図形</returns>
        virtual public Shape clone() 
        {
            return new Shape(x,y,w,h,rad,clonepoints());
        }
        /// <summary>
        /// 図形の重心からの最大の距離
        /// </summary>
        /// <returns>その射程</returns>
        public float syatei() 
        {
            float max = 0;
            var lis = getSoutaiPoints();
            var center=getSoutaiCenter();
            foreach (var a in lis) 
            {
                max = Math.Max(max, Math.Abs((a - center).length));
            }
            return max;
        }

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
        public double nasukaku(float px, float py)
        {
            return (float)Math.Atan2(py - getty(), px - gettx());
        }
        /// <summary>
        /// 接触していないことを判定する。接触していること判定できない
        /// </summary>
        /// <param name="s"></param>
        /// <returns>絶対に接触していないか</returns>
        public bool atattenai(Shape s) 
        {
            if (kyori(s) > syatei() + s.syatei()) return true;
            return false;
        }


        /// <summary>
        /// 図形同士が重なっているか調べる。両側からやる
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool kasanari(Shape s)
        {
            if (atattenai(s))
            {
                //Console.WriteLine("当たるわけねーだろ！");
                return false;
            }
                var points = getzettaipoints(false);
            var spoints = s.getzettaipoints(false);
            if (onhani(s)) 
            {
                return true;
            }
            for (int i = 1; i < points.Count - 1; i++)
            {

                if (s.onhani(points[i-1].X, points[i - 1].Y, points[i].X, points[i].Y))
                {
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// 指定した点に最もふさわしい辺を見つける
        /// </summary>
        public lineX getnearestline(FXY Z) 
        {
            return getnearestline(Z.X,Z.Y);
        }
        /// <summary>
        /// 指定した点に最もふさわしい辺を見つける
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <returns></returns>
        virtual public lineX getnearestline(float px,float py)
        {
            //Console.WriteLine("nearest");
            FXY center = new FXY(px, py);
            var points = getzettaipoints(false);

            FXY mycenter = getCenter();

            var linest = new FXY(center.X - mycenter.X, center.Y - mycenter.Y);
            linest.length += gaisyuu;

            linest.X += mycenter.X;
            linest.Y += mycenter.Y;
            var res = new lineX((points[0]), (points[0 + 1]), (mycenter));
            float mina = -1;

            //TRACE(_T("%f %f OIOI  %f %f\n"), mycenter.x, mycenter.y, linest.x, linest.y);
            for (int i = 1; i < points.Count() - 1; i++)
            {

                //TRACE(_T("%f %f OTYA  %f %f\n"), points[i].x, points[i].y, points[i+1].x, points[i+1].y);
                if (crosses(mycenter, linest, points[i], points[i + 1]))
                {
              //      Console.WriteLine("crossed!");
                    var tmp = new lineX((points[i]), (points[i + 1]), (mycenter));
                    FXY sco = getzurasi(center, tmp, true);
                    float score = sco.X * sco.X + sco.Y * sco.Y;
                    //	TRACE(_T("%f :KITAKORE:  %f %f\n",mina,sco.x,sco.y));
                    if (mina < 0)
                    {
                        res = tmp;
                        mina = score;

                    }
                    else if (score > mina)
                    {

                        res = tmp;
                        mina = score;
                    }
                    else if (score == mina)
                    {
                        // TRACE(_T("owattaaaaa\n"));
                        res = new lineX(new FXY(res.begin.X - tmp.end.X + tmp.begin.X, res.begin.Y - tmp.end.Y + tmp.begin.Y)
                            , new FXY(-res.begin.X + tmp.end.X + tmp.begin.X, -res.begin.Y + tmp.end.Y + tmp.begin.Y)
                            , (mycenter));
                        mina = score;
                    }
                }
                else 
                {
             //       Console.WriteLine("1: " + mycenter.ToString());
           //         Console.WriteLine("2: " + linest.ToString());
           //         Console.WriteLine("3: " + points[i].ToString());
          //          Console.WriteLine("4: " + points[i+1].ToString());
                }

            }
            //	TRACE(_T(" %f %f:idoukaku:%f %f  %f\n"), res.begin.x, res.begin.y, res.end.x, res.end.y,res.getrad());

         //   Console.WriteLine("END");
            return res;

        }

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
        protected bool tokeimawari(List<double[]> gaisyuu)
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
        /// 図形を単純に移動させる
        /// </summary>
        /// <param name="dx">移動するxの距離</param>
        /// <param name="dy">移動するyの距離</param>
        public void idou(float dx, float dy)
        {

            x += dx; y += dy;
        }
      
      

    }
    /// <summary>
    /// 四角形を表すクラス
    /// </summary>
    public class Rectangle : Shape
    {
        /// <summary>
        /// 四角形を作る。ちなみにこの時回転は指定したx,yを中心に行われる
        /// </summary>
        /// <param name="xx">x座標</param>
        /// <param name="yy">y座標</param>
        /// <param name="ww">幅</param>
        /// <param name="hh">高さ</param>
        /// <param name="radd">回転角度</param>
        public Rectangle(float xx=0, float yy=0, float ww = 0, float hh = 0, double radd = 0) 
            : base(xx, yy, ww, hh, radd,new List<FXY> {new FXY(0,0), new FXY(1, 0), new FXY(1, 1), new FXY(0, 1) })
        {

        }
        /// <summary>
        /// 図形を複製する
        /// </summary>
        /// <returns>複製された図形</returns>
        public override Shape clone()
        {
            var res = new Rectangle(x, y, w, h, rad);
            res.settxy(gettx(), getty());
            return res;
        }

    }
    /// <summary>
    /// 三角形を表すクラス。
    /// しかし|>こういう左の辺がy軸と平行で右の先端が左の辺のy座標の間にあるな形の三角形しか作れない。
    /// </summary>
    public class Triangle : Shape
    {
        int _houkou = 1;
        int _basehoukou = 1;
        /// <summary>
        /// 三角形の先の方向。1で右-1で左2で下
        /// </summary>
        int basehoukou { get { return _basehoukou; } set { if (-2 <= value && value <= 2 && value != 0) _basehoukou = value; } }
        int houkou { get { return _houkou; } 
            set {
                int pre = _houkou;
                if (-2 <= value && value <= 2 && value != 0) _houkou = value;
                if(pre!=_houkou) changehoukou();
            }
        
        }

        public override void setMirror(int mir)
        {
            if (Math.Abs(houkou) == 1)
            {
                if (mir > 0)
                {
                    houkou = basehoukou*1;
                }
                else if (mir < 0)
                {
                    houkou = basehoukou * -1;
                }
                else
                {
                    houkou *= -1;
                }
            }
            base.setMirror(mir);
        }
        float _haji;
        /// <summary>
        /// 先端の高さの割合hajiは0<=<=1の範囲で変化する。
        /// </summary>
        float haji { get { return _haji; } set { _haji = value; if (_haji < 0) _haji = 0; if (_haji > 1) _haji = 1; } }
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
        public Triangle(float xx=0, float yy=0, float ww = 0, float hh = 0, float hajih = 0.5f, double radd = 0, int hou = 1) 
            : base(xx, yy, ww, hh, radd,new List<FXY>())
        {
            haji = hajih;
            houkou = hou;
            basehoukou = hou;
            changehoukou();
        }
        /// <summary>
        /// 三角形の方向を変えたり変えなかったり
        /// </summary>
        protected void changehoukou() 
        {
            List<FXY> lis = new List<FXY>();
            switch (houkou)
            {
                case 1:
                    lis = new List<FXY> { new FXY(0, 0), new FXY(1, haji), new FXY(0, 1) };
                    break;
                case -1:
                    lis = new List<FXY> { new FXY(0, haji), new FXY(1, 0), new FXY(1, 1) };
                    break;
                case 2:
                    lis = new List<FXY> { new FXY(0, 0), new FXY(0, 1), new FXY(haji, 1) };
                    break;
                case -2:
                    lis = new List<FXY> { new FXY(haji, 0), new FXY(1, 1), new FXY(0, 1) };
                    break;
            }
            setpoints(lis);
        }


        public override Shape clone()
        {
            var res = new Triangle(x, y, w, h, haji, rad, houkou);
            res.settxy(gettx(), getty());
            return res;
        }
    }
    /// <summary>
    /// 円を表すクラス
    /// </summary>
    public class Circle : Shape
    {
        int kinji = 5;
        /// <summary>
        /// 1角の角度
        /// </summary>
        double onerad { get { return 2 * Math.PI / kinji; } }
        /// <summary>
        /// 1辺の長さ
        /// </summary>
        float onelen
        {
            get
            {
                return (float)Math.Cos(rad / 2);
            }
        }
        /// <summary>
        /// 円を作り出す。円はあたり判定の時、多角形に近似される。
        /// </summary>
        /// <param name="xx">x座標</param>
        /// <param name="yy">y座標</param>
        /// <param name="ww">幅</param>
        /// <param name="hh">高さ</param>
        /// <param name="radd">回転角</param>
        /// <param name="pointkinji">近似する多角形の画数</param>
        public Circle(float xx=0, float yy=0, float ww = 0, float hh = 0, double radd = 0, int pointkinji = 20) 
            : base(xx, yy, ww, hh, radd,new List<FXY>())
        {
            if (pointkinji > kinji)
            {
                kinji = pointkinji;
                //  Console.WriteLine(pointkinji+"aslfkjaslslkinji"+kinji);
            }
         
            double rad = 2 * Math.PI / kinji;
            float nag = (float)Math.Cos(rad / 2);
            //	TRACE(_T("%f Circle %f  %f\n"), nag * w, nag * h,nag);
            List<FXY> lis = new List<FXY> {  };
            for (int i = 0; i < kinji; i++)
            {
                lis.Add(new FXY(0.5f+0.5f*(float)Math.Cos(rad*i), 0.5f + 0.5f * (float)Math.Sin(rad * i)));
            }
            setpoints(lis);
        }

        public override Shape clone()
        {
            var res = new Circle(x, y, w, h, rad,kinji);
            res.settxy(gettx(), getty());
            res.kinji = kinji;
            return res;
        }
        public override bool onhani(float px, float py,float onis=1)
        {
            float dx = px - gettx();
            float dy = py - getty();

            double ddx = dx * Math.Cos(-rad) - dy * Math.Sin(-rad);
            double ddy = dx * Math.Sin(-rad) + dy * Math.Cos(-rad);



            return (ddx * ddx / (w / 2 * w / 2) + ddy * ddy / (h / 2 * h / 2) <= onis);
        }
        public override bool onhani(float px, float py, float ppx, float ppy)
        {
            var c = getCenter();



            float spx = px - c.X;
            float spy = py - c.Y;
            float sppx = ppx - c.X;
            float sppy = ppy - c.Y;

            float 
                fpx=spx*(float)Math.Cos(-rad)-spy*(float)Math.Sin(-rad), 
                fpy = spx * (float)Math.Sin(-rad) + spy * (float)Math.Cos(-rad), 
                fppx = sppx * (float)Math.Cos(-rad) - sppy * (float)Math.Sin(-rad), 
                fppy = sppx * (float)Math.Sin(-rad) + sppy * (float)Math.Cos(-rad);


            var kyo = new lineX(fpx, fpy, fppx, fppy, 0, 0).getkyori(0,0);

            return Math.Abs(kyo.X)<=Math.Abs(w/2)&& Math.Abs(kyo.Y) <= Math.Abs(h /2);

        }
        /*
        /// <summary>
        /// 指定した点に最もふさわしい辺を見つける
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <returns></returns>
        override public lineX getnearestline(float px, float py)
        {
            if (w != 0 && h != 0)
            {

                var c = getCenter();
                var k = new FXY(px, py);
                k -= c;
                k.rad -= this.rad;
                k.X /= w/2;
                k.Y /= h/2;

                var tyu = new FXY(0.5f, k.rad);
                var nags = new FXY(onelen/2, k.rad+Math.PI/2);

                var res = new lineX(tyu+nags, tyu - nags, new FXY(0,0));

                res.begin.X *= w / 2;
                res.begin.Y *= h / 2;
                res.end.X *= w / 2;
                res.end.Y *= h / 2;
                res.begin.rad += this.rad;
                res.end.rad += this.rad;

                res.begin += c;
                res.end += c;
                res.bs += c;

                return res;
            }
            return new lineX(getCenter(), getCenter(), getCenter());
        }*/

    }
}
