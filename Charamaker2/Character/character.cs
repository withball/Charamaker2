﻿using Charamaker2.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// キャラクターを扱うための名前空間
/// </summary>
namespace Charamaker2.Character
{
    /// <summary>
    /// 関節。pictureを一枚持ち、いい感じに構成する
    /// </summary>
    [Serializable]
    public class setu
    {
        /// <summary>
        /// 節の前。一つのキャラクター内で被ってはいけない。あと""もやめてくれ
        /// </summary>
       public string nm;
        /// <summary>
        /// ピクチャーだよ！
        /// </summary>
        public picture p;
        /// <summary>
        /// 下にある節の塊
        /// </summary>
        public List<setu> sts = new List<setu>();
        /// <summary>
        /// 前の物体からの位置。前がキャラクターなら中心からの位置、節ならpictureの左上からの位置。
        /// </summary>
        public float dx, dy;
        /// <summary>
        /// 節のコンストラクタ
        /// </summary>
        /// <param name="name">節の名前</param>
        /// <param name="ddx">節の位置x</param>
        /// <param name="ddy">節の位置y</param>
        /// <param name="pic">ピクチャー</param>
        /// <param name="kansetu">下の関節のリスト。nullで自動</param>
        public setu(string name, float ddx, float ddy, picture pic, List<setu> kansetu=null)
        {
            nm = name;
            dx = ddx;
            dy = ddy;
            p = pic;
            if (kansetu == null)
            {
                sts = new List<setu>();
            }
            else 
            {
                sts = kansetu;
            }
        }
        /// <summary>
        /// コピーするためのコンストラクタ
        /// </summary>
        /// <param name="s">コピー元</param>
        public setu(setu s)
        {
            nm = s.nm;
            p =  (picture)s.p.clone();
            foreach (var a in s.sts)
            {
                sts.Add(a.clone());
            }
            dx = s.dx;
            dy = s.dy;

        }
        /*   他のバージョンから移植するときこんな風にしたりする
         *   public setu(Band_Brave_Journey.Character.setu s)
           {
               nm = s.nm;

               p = new picture(s.p.x, s.p.y, s.p.z, s.p.w, s.p.h, s.p.tx, s.p.ty, s.p.RAD, s.p.mir, s.p.OPA, s.p.texname,new Dictionary<string, string>(s.p.textures));

               foreach (var a in s.sts)
               {
                   sts.Add(new setu(a));
               }
               dx = s.dx;
               dy = s.dy;


           }*/
        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        public setu() { }
        /// <summary>
        /// ツリーの中にある節を一つ見つける。深さ優先探索。
        /// </summary>
        /// <param name="name">見つける節の名前</param>
        /// <returns>見つかった節</returns>
        public setu GetSetu(string name)
        {
            if (nm == name) return this;
            setu res = null;
            for (int i = 0; i < sts.Count; i++)
            {
                var tmp = sts[i].GetSetu(name);
                if (tmp != null) res = tmp;

            }
            return res;

        }
        /// <summary>
        /// ツリーの中にある節の親を一つ見つける。深さ優先探索。
        /// </summary>
        /// <param name="name">見つける節の名前</param>
        /// <returns>見つかった親</returns>
        public setu Getrootsetu(string name)
        {

            setu res = null;
            for (int i = 0; i < sts.Count; i++)
            {
                if (sts[i].nm == name) return this;
                var tmp = sts[i].Getrootsetu(name);
                if (tmp != null) res = tmp;

            }
            return res;
        }
        /// <summary>
        /// 節を消去する。もちろんその下の節も消滅する。
        /// </summary>
        /// <param name="name">消す節の名前</param>
        /// <param name="hyojiman">picsから消すのでその表示マンも必要なのだ！</param>
        public void Removesetu(string name, hyojiman hyojiman)
        {

            if (name == nm)
            {
                for (int t = sts.Count() - 1; t >= 0; t--)
                {
                    Removesetu(sts[t].nm, hyojiman);
                }
            }

            for (int i = sts.Count() - 1; i >= 0; i--)
            {
                sts[i].Removesetu(name, hyojiman);
                if (sts[i].nm == name)
                {
                    hyojiman.removepicture(sts[i].p);
                    sts.RemoveAt(i);

                }
            }

        }
        /// <summary>
        /// 自分と下にある節の列を取得する
        /// </summary>
        /// <returns>得られたリスト</returns>
        public List<setu> getallsetu()
        {
            var l = new List<setu>();

            foreach (var kk in sts)
            {
                l.AddRange(kk.getallsetu());
            }
            l.Add(this);
            return l;

        }
        /// <summary>
        /// 節とかの位置を整えるためのメソッド
        /// </summary>
        public void frame()
        {
            foreach (var a in sts)
            {
                if (p.mir)
                    a.p.settxy(p.x + (p.w - a.dx) * (float)Math.Cos(p.RAD) - (a.dy) * (float)Math.Sin(p.RAD)
                        , p.y + (p.w - a.dx) * (float)Math.Sin(p.RAD) + (a.dy) * (float)Math.Cos(p.RAD));
                else
                    a.p.settxy(p.x + (a.dx) * (float)Math.Cos(p.RAD) - (a.dy) * (float)Math.Sin(p.RAD)
                   , p.y + (a.dx) * (float)Math.Sin(p.RAD) + (a.dy) * (float)Math.Cos(p.RAD));
                a.frame();
            }

        }
        /// <summary>
        /// 節とかの大きさなどを変えるためのメソッド
        /// </summary>
        /// <param name="sc">スケール</param>
        public void scalechange(float sc)
        {
            foreach (var a in getallsetu())
            {
                a.p.w *= sc;
                a.p.h *= sc;
                a.p.tx *= sc;
                a.p.ty *= sc;
                a.dx *= sc;
                a.dy *= sc;

            }
        }
        /// <summary>
        /// コピーするためのメソッド。下の奴らとかはコピーしない。
        /// </summary>
        /// <param name="s">コピー元</param>
        public void copy(setu s)
        {
            nm = s.nm;
            p.copy(s.p);
            dx = s.dx;
            dy = s.dy;
        }
        /// <summary>
        /// クローンするメソッド
        /// </summary>
        /// <returns></returns>
        virtual public setu clone() 
        {
            return new setu(this);
        }
        /// <summary>
        /// セーブする
        /// </summary>
        /// <returns></returns>
        virtual public DataSaver ToSave()
        {
            var d = new DataSaver();

            d.packAdd("name", nm);
            d.linechange();

            d.packAdd("dx", dx.ToString());
            d.packAdd("dy", dy.ToString());
            d.linechange();

            var dd = p.ToSave();
            dd.indent();
            d.packAdd("picture",dd);
            d.linechange();

            var setus = new DataSaver();
            foreach (var a in sts)
            {
                setus.packAdd(a.nm, a.ToSave());
            }
            setus.indent(1);
            d.packAdd("sts", setus);
            return d;
        }

        /// <summary>
        /// ロードする。
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        static public setu ToLoad(DataSaver d)
        {
            var setus = new List<setu>();
            var dd = d.unPackDataD("sts");
            var lis = dd.getAllPacks();
            foreach (var a in lis)
            {
                var set = ToLoad(dd.unPackDataD(a));
                setus.Add(set);
            }
            return new setu(d.unPackDataS("name", "core"), d.unPackDataF("dx",0), d.unPackDataF("dy", 0),picture.ToLoad(d.unPackDataD("picture")),setus);
        }

    }
    /// <summary>
    /// キャラクターを構成するためのクラス。
    /// イメージは棒人間に肉を乗せる感じ。
    /// </summary>
    [Serializable]
    public class character
    {
        /// <summary>
        /// ピクチャー一枚だけのキャラクターを作る
        /// </summary>
        /// <param name="tex">テクスチャー</param>
        /// <param name="z">Z</param>
        /// <param name="widthscale">横幅をここにそろえる</param>
        /// <param name="hin">やっぱ縦幅にする</param>
        /// <param name="txp">中心点の位置x</param>
        /// <param name="typ">中心点の位置y</param>
        /// <param name="opa">透明度</param>
        /// <param name="rad">角度</param>
        /// <param name="tx">中心の位置x</param>
        /// <param name="ty">中心の位置y</param>
        /// <param name="mirror">反転</param>
        /// <returns></returns>
        public static character onepicturechara(string tex, float widthscale, float z = 0, bool hin = false, float txp = 0.5f, float typ = 0.5f, float opa = 1, double rad = 0, float tx = 0,float ty=0,bool mirror=false) 
        {

            var si = fileman.gettexsize(tex);
            float scale ;
            if (hin)
            {
                scale = widthscale / (float)si.Height;
            }
            else 
            {
                scale = widthscale / (float)si.Width;
            } 
            
            var res = new character(0, 0, si.Width * scale, si.Height * scale, si.Width * scale * txp, si.Height * scale * typ, rad,
                new setu("core", 0, 0, picture.onetexpic(tex,widthscale,z,hin,txp,typ,opa,rad,0,0,mirror)));
            res.settxy(tx, ty);
            return res;
        }
        /// <summary>
        /// ピクチャー一枚だけのキャラクターを作る
        /// </summary>
        /// <param name="pic">ピクチャー</param>
        /// <returns></returns>
        public static character onepicturechara(picture pic)
        {

         

            var res = new character(0, 0, pic.w, pic.h, pic.tx, pic.ty, pic.RAD,
                new setu("core", 0, 0, pic));
            res.settxy(pic.gettx(), pic.getty());
            return res;
        }

        /// <summary>
        ///図形と図形の重心の距離を測る
        /// </summary>
        /// <param name="s">その図形の片割れ</param>
        /// <returns>距離</returns>
        public float kyori(character s)
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
        public double nasukaku(character s)
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
        /// xy座標
        /// </summary>
        public float x,y;
        /// <summary>
        /// 幅と高さ
        /// </summary>
        public float w,h;
        /// <summary>
        /// ラジアン角度
        /// </summary>
        protected double rad;
        /// <summary>
        /// ラジアン角度。-PI＜＝＜＝Pi
        /// </summary>
        public double RAD { get { return rad; } set {  float x = gettx(), y = getty(); rad = value; rad = Shape.radseiki(rad); settxy(x, y); } }

        /// <summary>
        /// 中心点の位置
        /// </summary>
        public float tx,ty;


        /// <summary>
        /// 画像の中心点xを返す。
        /// </summary>
        public float TX { get { if (mirror) return w - tx; else return tx; } }
        /// <summary>
        /// 画像の中心点yを返す
        /// </summary>
        public float TY { get { return ty; } }
        /// <summary>
        /// 核となる一つの節
        /// </summary>
        public setu core;
        /// <summary>
        /// 持っているモーション
        /// </summary>
        protected List<motion> motions = new List<motion>();
        /// <summary>
        /// 基準となるキャラクター。
        /// </summary>
        protected character kijyun = null;
        /// <summary>
        /// キャラクターを反転させる奴
        /// </summary>
        public bool mirror { get { return _mirror; } set { _mirror = value; } }
        /// <summary>
        /// 素のミラー
        /// </summary>
        protected bool _mirror = false;
        /// <summary>
        /// キャラクターのコンストラクタ。この瞬間基準がセットされる
        /// </summary>
        /// <param name="xx">左上x座標</param>
        /// <param name="yy">左上y座標</param>
        /// <param name="ww">幅</param>
        /// <param name="hh">高さ</param>
        /// <param name="ttx">中心x座標</param>
        /// <param name="tty">中心y座標</param>
        /// <param name="sita">ラディアン角度</param>
        /// <param name="cor">核となる節</param>
        public character(float xx, float yy, float ww, float hh, float ttx, float tty, double sita, setu cor)
        {
            x = xx;
            y = yy;
            w = ww;
            h = hh;
            tx = ttx;
            ty = tty;
            rad = Shape.radseiki(sita);
            core = cor;
            setkijyuns();
        }
        /// <summary>
        /// コピーするためのコンストラクタ。
        /// </summary>
        /// <param name="c">コピー元</param>
        /// <param name="setkijyun">基準をセット/コピーするか</param>
        /// <param name="motion">モーションまでもコピーするか</param>
        public character(character c, bool setkijyun = true, bool motion = false)
        {
            x = c.x;
            y = c.y;
            w = c.w;
            h = c.h;
            tx = c.tx;
            ty = c.ty;
            rad = c.RAD;
            core = (setu)c.core.clone();
            
            _mirror = c._mirror;
            premir = c._mirror;
            if (setkijyun)
            {
                if (c.kijyun == null) setkijyuns();
                else
                {
                    kijyun = new character(c.kijyun, false, false);
                }
            }

            if (motion) copymotion(c);
        }
        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        public character() { }
        /*別バージョンからわっしょいしてくるためのやーつ
      public character(Band_Brave_Journey.Character.character c)
      {
          x = c.x;
          y = c.y;
          w = c.w;
          h = c.h;
          tx = c.tx;
          ty = c.ty;
          rad = c.RAD;
          core = new setu(c.core);
          setkijyuns();
      }*/
        /// <summary>
        /// 節のツリーの中にある節を一つ見つける。深さ優先探索。
        /// </summary>
        /// <param name="name">見つける節の名前</param>
        /// <returns>見つかった節</returns>
        public setu GetSetu(string name)
        {
            return core.GetSetu(name);

        }
        /// <summary>
        /// 節のツリーの中にある節の親を一つ見つける。深さ優先探索。
        /// </summary>
        /// <param name="name">見つける節の名前</param>
        /// <returns>見つかった親</returns>
        public setu Getrootsetu(string name)
        {
            return core.Getrootsetu(name);
        }
        /// <summary>
        /// 節のリストにあるやつを消去する。もちろんその下の節も消滅する。
        /// </summary>
        /// <param name="name">消す節の名前</param>
        /// <param name="hyojiman">picsから消すのでその表示マンも必要なのだ！</param>
        public void Removesetu(string name, hyojiman hyojiman)
        {
            core.Removesetu(name, hyojiman);
        }
        /// <summary>
        /// これに所属しているすべての節を取得する
        /// </summary>
        /// <returns>得られたリスト</returns>
        public List<setu> getallsetu()
        {
            return core.getallsetu();
        }
        /// <summary>
        /// 基準をセットするメソッド
        /// </summary>
        public void setkijyuns()
        {
            kijyun = new character(this, false);
            kijyun.kijyun = null;
        }
        /// <summary>
        /// 基準をゲットする。基準がnullならそのまま自分を返す。
        /// </summary>
        /// <returns>基準か何かのキャラクター</returns>
        public character getkijyun()
        {
            if (kijyun != null) return kijyun;
            return this;
        }
        /// <summary>
        /// 自身を基準と同一のものとする。節以下は新しくインスタンスを作る。
        /// </summary>
        /// <param name="hyo">表示を変えるために必要</param>
        public void resettokijyun(hyojiman hyo)
        {
            character c = getkijyun();

            w = c.w;
            h = c.h;
            tx = c.tx;
            ty = c.ty;
            rad = c.RAD;
            _mirror = c._mirror;
            this.sinu(hyo);
            core =(setu)Activator.CreateInstance(kijyun.core.GetType(),kijyun.core);
            this.resethyoji(hyo);
            premir = false;
            soroeru();
        }

        /// <summary>
        /// 自身を基準と同一のものとする
        /// </summary>
        /// <param name="OPA">不透明度も同一とするか</param>
        /// <param name="TEX">テクスチャーも同一とするか</param>
        public void resettokijyun(bool OPA=true,bool TEX =true)
        {
            character c = getkijyun();

            w = c.w;
            h = c.h;
            tx = c.tx;
            ty = c.ty;
            rad = c.RAD;
            _mirror = c._mirror;
            premir = false;
            foreach (var a in kijyun.core.getallsetu())
            {
                var b = this.core.GetSetu(a.nm);
                if (b != null)
                {
                    float opa = b.p.OPA;
                    string tex = b.p.texname;

                    b.copy(a);
                    if (!OPA)
                    {
                        b.p.OPA = opa;

                    }
                    if (!TEX)
                    {
                        b.p.texname = tex;
                    }
                }
            }
            c.soroeru();
        }
        /// <summary>
        /// 不透明度のみを基準とそろえる
        /// </summary>
        public void refreshopa()
        {
            character c = getkijyun();

            foreach (var a in kijyun.core.getallsetu())
            {
                var b = this.core.GetSetu(a.nm);
                if (b != null)
                {
                    b.p.OPA=a.p.OPA;
                }
            }
        }
       
        /// <summary>
        /// ピクチャーのミラーのみを基準とそろえる
        /// </summary>
        public void refreshmir()
        {
            character c = getkijyun();

            foreach (var a in kijyun.core.getallsetu())
            {
                var b = this.core.GetSetu(a.nm);
                if (b != null)
                {
                    b.p.mir = a.p.mir;
                }
            }
        }


        /// <summary>
        /// 角度以外の要素を基準に整える
        /// </summary>
        /// <param name="OPA">不透明度も同一とするか</param>
        /// <param name="TEX">テクスチャーも同一とするか</param>
        /// <param name="MIR">反転も同一とするか</param>
        public void refreshtokijyun(bool OPA = true, bool TEX = true,bool MIR=true)
        {
            var tyusin = gettxy();
            character c = getkijyun();
            w = c.w;
            h = c.h;
            tx = c.tx;
            ty = c.ty;
            //rad = c.rad;
            //premir =false;
           // Console.WriteLine("QQQQQQQ");

            foreach (var a in kijyun.core.getallsetu())
            {
                var b = this.core.GetSetu(a.nm);
                if (b != null)
                {
                    float opa = b.p.OPA;
                    string tex = b.p.texname;
                    double kaku = b.p.RAD;
                    b.copy(a);
                    b.p.RAD = kaku;
                    if (MIR)
                    {
                        if (mirror) b.p.mir = !b.p.mir;
                    }
                    if (!OPA)
                    {
                        b.p.OPA = opa;

                    }
                    if (!TEX)
                    {
                        b.p.texname = tex;
                    }
                }
            }
            settxy(tyusin);
            soroeru();
        }

   
      /// <summary>
      /// モーションをコピーする
      /// </summary>
      /// <param name="c">コピー元</param>
      /// <param name="="reset">モーションを最初から行うかfalseは割と特殊になる</param> 

        public void copymotion(character c,bool reset=true)
        {
            if (reset)
            {
                foreach (var a in c.motions)
                {
                    addmotion(new motion(a));
                }
            }
            else 
            {
                foreach (var a in c.motions)
                {
                    motions.Add(new motion(a));
                }
            }
        }
        /// <summary>
        /// 角度をコピーする
        /// </summary>
        /// <param name="c">コピー元</param>
        public void copykakudo(character c)
        {
            if (c.premir)
            {
                c.kijyuhanten();
            }
            if (this.premir)
            {
                this.kijyuhanten();
            }

            foreach (var a in c.core.getallsetu())
            {
                foreach (var b in core.getallsetu())
                {
                    if (a.nm == b.nm)
                    {
                        b.p.RAD = a.p.RAD;
                        break;
                    }
                }
            }
            RAD = c.RAD;
            if (c.premir)
            {
                c.kijyuhanten();
            }
            if (this.premir)
            {
                this.kijyuhanten();
            }
        }
        /// <summary>
        /// hyojimanに全ての節のピクチャーを追加する。
        /// </summary>
        /// <param name="hyojiman">追加するところ</param>
        virtual public void resethyoji(hyojiman hyojiman)
        {
            var l = core.getallsetu();
            foreach (var lll in l)
            {
                hyojiman.addpicture(lll.p);
            }
        }
        /// <summary>
        /// zを変化させた時、hyojimanに再度追加することでソートを発生させるメソッド
        /// </summary>
        /// <param name="hyojiman">その表示マン</param>
        public void zchanged(hyojiman hyojiman)
        {
            var l = core.getallsetu();
            foreach (var lll in l)
            {
                hyojiman.addpicture(lll.p);
            }
        }
        /// <summary>
        /// 1フレーム前ミラーしてたかどうか。角度コピーとかで操作してね
        /// </summary>
        public bool premir = false;
        /// <summary>
        /// フレームを行う。節を整えて、モーションを行う。
        /// </summary>
        /// <param name="cl">モーションに使うクロック</param>
        virtual public void frame(float cl=1)
        {
            startmotion(true);
            
            for (int i = motions.Count() - 1; i >= 0; i--)
            {
                motions[i].frame(this,cl);
                if (motions[i].owari)
                {
                    motions.RemoveAt(i);

                }
            }
            endmotion(true);
           
           
        }
        /// <summary>
        /// モーションを動かす前に呼ぶじゃないと反転がおかしくなる
        /// </summary>
        /// <param name="gati">こちらはcharacter.frame内のみで呼びます。ほっといて！</param>
        public void startmotion(bool gati = false) 
        {
            if (premir)
            {
                kijyuhanten();
                if (gati) premir = false;
            }
        }
        /// <summary>
        /// モーションを動かした後に呼ぶじゃないと反転がおかしくなる
        /// </summary>
        /// <param name="gati">こちらはcharacter.frame内のみで呼びます。ほっといて！</param>
        public void endmotion(bool gati=false)
        {
            if ((gati&&mirror)||(!gati&&premir))
            {
                if(gati)premir = true;
                kijyuhanten();
            }
            soroeru();
        }
        /// <summary>
        /// 基準をもとに反転を行う
        /// </summary>
        void kijyuhanten()
        {
            var lis = core.getallsetu();
            foreach (var a in lis)
            {
                a.p.mir = !a.p.mir;
            }
            var kijyun = getkijyun();
            {

                foreach (var a in lis)
                {
                    var b = kijyun.core.GetSetu(a.nm);
                    if (b != null)
                    {
                  
                        var ki = a.p.RAD - b.p.RAD;
                        a.p.RAD = b.p.RAD - ki;
                    }
                }
            }
            {
                var ki = RAD - kijyun.RAD;
                RAD = kijyun.RAD - ki;
            }
        }
        /// <summary>
        /// 節を整えたりする。
        /// </summary>
        public void soroeru()
        {
            float txt = TX, tyt = TY;
            if (!core.p.mir)
            {
                core.p.settxy(x + (txt + core.dx) * (float)Math.Cos(RAD) - (tyt + core.dy) * (float)Math.Sin(RAD)
                   , y + (txt + core.dx) * (float)Math.Sin(RAD) + (tyt + core.dy) * (float)Math.Cos(RAD));
            }
            else
            {
                core.p.settxy(x + (txt - core.dx) * (float)Math.Cos(RAD) - (tyt + core.dy) * (float)Math.Sin(RAD)
                   , y + (txt - core.dx) * (float)Math.Sin(RAD) + (tyt + core.dy) * (float)Math.Cos(RAD));
            }

            core.frame();
        }
        /// <summary>
        /// キャラクターのサイズを変える
        /// </summary>
        /// <param name="sc">スケール</param>
        /// <param name="setkijyun">基準にも拡大を適用するか</param>
        public void scalechange(float sc,bool setkijyun=true)
        {
            w *= sc;
            h *= sc;
            tx *= sc;
            ty *= sc;

            core.scalechange(sc);
            if (setkijyun)
            {
                if (kijyun != null) 
                {
                    kijyun.w *= sc;
                    kijyun.h *= sc;
                    kijyun.tx *= sc;
                    kijyun.ty *= sc; 
                    kijyun.core.scalechange(sc);
                }
            }
        }

        /// <summary>
        /// キャラクター上の一点のxy座標を取得する(回転の影響を考慮してるってこと)
        /// </summary>
        /// <param name="ww">左上を0としたときのキャラクターの点の位置</param>
        /// <param name="hh">左上を0としたときのキャラクターの点の位置</param>
        /// <returns>返されるのはxy座標の値</returns>
        public FXY getcxy(float ww, float hh)
        {

            return new FXY(getcx(ww,hh),getcy(ww,hh));
        }
        /// <summary>
        /// キャラクター上の一点のx座標を取得する(回転の影響を考慮してるってこと)
        /// </summary>
        /// <param name="ww">左上を0としたときのキャラクターの点の位置</param>
        /// <param name="hh">左上を0としたときのキャラクターの点の位置</param>
        /// <returns>返されるのはx座標の値</returns>
        public float getcx(float ww, float hh)
        {
            float W; if (mirror) W = w - ww; else W = ww;
            float H; if (mirror && 1 == 0) H = h - hh; else H = hh;
            float rx = x + W * (float)Math.Cos(RAD) - H * (float)Math.Sin(RAD);
            return rx;
        }
        /// <summary>
        /// キャラクター上の一点のy座標を取得する(回転の影響を考慮してるってこと)
        /// </summary>
        /// <param name="ww">左上を0としたときのキャラクターの点の位置</param>
        /// <param name="hh">左上を0としたときのキャラクターの点の位置</param>
        /// <returns>返されるのはy座標の値</returns>
        public float getcy(float ww, float hh)
        {

            float W; if (mirror) W = w - ww; else W = ww;
            float H; if (mirror && 1 == 0) H = h - hh; else H = hh;
            float ry = y + W * (float)Math.Sin(RAD) + H * (float)Math.Cos(RAD);
            return ry;
        
        }
        /// <summary>
        /// 中心点のxy座標を返す。
        /// </summary>
        /// <returns>xy座標</returns>
        public FXY gettxy()
        {
            return new FXY(gettx(),getty());

        }
        /// <summary>
        /// 中心点のx座標を返す。
        /// </summary>
        /// <returns>x座標</returns>
        public float gettx()
        {
            return x + TX * (float)Math.Cos(RAD) - TY * (float)Math.Sin(RAD);

        }
        /// <summary>
        /// 中心点のy座標を返す。
        /// </summary>
        /// <returns>y座標</returns>
        public float getty()
        {
            return y + (TX) * (float)Math.Sin(RAD) + TY * (float)Math.Cos(RAD);
            
        }
        /// <summary>
        /// 画像上の任意の一点をxy座標にセットする
        /// </summary>
        /// <param name="xx">セットするx座標</param>
        /// <param name="yy">セットするy座標</param>
        /// <param name="cx">画像上のwの点</param>
        /// <param name="cy">画像上のhの点</param>
        public void setcxy(float xx, float yy, float cx, float cy)
        {
            x += xx - getcx(cx, cy);
            y += yy - getcy(cx, cy);
            soroeru();
        }
        /// <summary>
        /// 画像上の任意の一点をxy座標にセットする
        /// </summary>
        /// <param name="xy">セットするxy座標</param>
        /// <param name="cx">画像上のwの点</param>
        /// <param name="cy">画像上のhの点</param>
        public void setcxy(FXY xy, float cx, float cy)
        {
            setcxy(xy.X, xy.Y, cx, cy);
        }
        /// <summary>
        /// 中心をxy座標にセットする。
        /// </summary>
        /// <param name="xx">セットするx座標</param>
        /// <param name="yy">セットするy座標</param>
        public void settxy(float xx, float yy)
        {
            x = xx - TX * (float)Math.Cos(RAD) + TY * (float)Math.Sin(RAD);
            y = yy - TX * (float)Math.Sin(RAD) - TY * (float)Math.Cos(RAD);
            soroeru();
        }
        /// <summary>
        /// 中心をxy座標にセットする。
        /// </summary>
        /// <param name="xy">セットするxy座標</param>
        public void settxy(FXY xy)
        {
            settxy(xy.X, xy.Y);
        }
        /// <summary>
        /// 移動する奴
        /// </summary>
        /// <param name="dx">x方向の移動量</param>
        /// <param name="dy">y方向の移動量</param>
        public void idouxy(float dx, float dy)
        {
            x += dx;
            y += dy;
            soroeru();
        }
        /// <summary>
        /// 移動する奴
        /// </summary>
        /// <param name="dxy">xy方向の移動量</param>
        public void idouxy(FXY dxy)
        {
            idouxy(dxy.X,dxy.Y);
        }
        /// <summary>
        /// 回転とミラーしている方向に移動する奴
        /// </summary>
        /// <param name="dx">w方向の移動量</param>
        /// <param name="dy">h方向の移動量</param>
        public void wowidouxy(float dx, float dy)
        {
            if (mirror) dx*=-1;
            x += dx * (float)Math.Cos(RAD) - dy * (float)Math.Sin(RAD);
            y += dx * (float)Math.Sin(RAD) + dy * (float)Math.Cos(RAD);
            soroeru();
        }

        /// <summary>
        /// zの大きさを定数倍する
        /// </summary>
        /// <param name="bai">倍率</param>
        /// <param name="setkijyun">基準にも同じ効果を適用するか</param>
        public void zbai(float bai,bool setkijyun=true)
        {
            foreach (var c in core.getallsetu())
            {
                c.p.z *= bai;
            }
            if (setkijyun && kijyun != null) 
            {
                foreach (var c in kijyun.core.getallsetu())
                {
                    c.p.z *= bai;
                }
            }
        }
        /// <summary>
        /// モーションをすべて消す
        /// </summary>
        public void resetmotion()
        {
            motions.Clear();
        }
        /// <summary>
        /// 終わっていないモーションを終わらせる
        /// </summary>
        /// <param name="power">終わらせるフレームの時間</param>
        /// <param name="bunkatu">フレームを分割するか</param>
        /// <returns>全て終わっていたらtrue</returns>
        public bool endmotions(float power=10000,bool bunkatu=true) 
        {
            bool res = true;
            var mmm = new List<motion>(this.motions);
            foreach (var a in mmm) 
            {
                if (!a.loop) 
                {
                    if (bunkatu) 
                    {
                        for (int i = 0; i < power && !a.owari; i++) 
                        {
                            if (mirror) kijyuhanten();
                            a.frame(this,1);
                            if (mirror) kijyuhanten();
                            this.soroeru();
                        }
                    }
                    else
                    {
                        a.frame(this, power);
                    }
                    res = false;
                }
            }
            return res;
        }
        /// <summary>
        /// モーションを追加する
        /// </summary>
        /// <param name="m">追加するモーション</param>
        /// <param name="sento">最後に実行されるところに入れるか</param>
        public void addmotion(motion m, bool sento = true)
        {
            if (sento) motions.Insert(0, m);
            else motions.Add(m);
            m.start(this);
        }
        /// <summary>
        /// ムーブを追加する。moveが一つの時だけ使ってね
        /// </summary>
        /// <param name="move">追加するムーブ。motionに直される</param>
        /// <param name="sento">最後に実行されるところに入れるか</param>
        public void addmotion(moveman move, bool sento = true)
        {
            var m = new motion(move);
            if (sento) motions.Insert(0, m);
            else motions.Add(m);
            m.start(this);
        }
        /// <summary>
        /// hyojimanから全てを消す
        /// </summary>
        /// <param name="hyojiman">消すhyojiman</param>
        virtual public void sinu(hyojiman hyojiman)
        {

            var l = core.getallsetu();
            foreach (var lll in l)
            {

                hyojiman.removepicture(lll.p);


            }


        }
        /// <summary>
        /// 特定のタイプのムーブを全てのモーションから消すE:
        /// </summary>
        /// <param name="t">消すmoveのタイプ</param>
        public void removemoves(Type t)
        {
            foreach (var a in motions) a.removemoves(t);
        }
        /// <summary>
        /// クローンするメソッド
        /// </summary>
        /// <returns></returns>
       virtual public character clone() 
        {
            return new character(this);
        }

        /// <summary>
        /// セーブする。セーブされるのは基準の方はセーブされない。
        /// </summary>
        /// <returns></returns>
        virtual public DataSaver ToSave()
        {
            var d = new DataSaver();

            d.packAdd("x", x.ToString());
            d.packAdd("y", y.ToString());
            d.linechange();

            d.packAdd("w", w.ToString());
            d.packAdd("h", h.ToString());
            d.linechange();
            d.packAdd("tx", tx.ToString());
            d.packAdd("ty", ty.ToString());
            d.packAdd("rad", rad.ToString());
            d.linechange();
            d.packAdd("core", core.ToSave());

            return d;
        }

        /// <summary>
        /// ロードする。基準はロード時点のものがセットされる。
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        static public character ToLoad(DataSaver d)
        {
            return new character(d.unPackDataF("x",0), d.unPackDataF("y", 0)
                , d.unPackDataF("w", 0), d.unPackDataF("h", 0)
                , d.unPackDataF("tx", 0), d.unPackDataF("ty", 0) , d.unPackDataF("rad", 0)
                ,setu.ToLoad(d.unPackDataD("core")));
        }

    }
    /// <summary>
    /// カメラを自動的に追跡してくれるキャラクター。effectcharaのついてくキャラをこいつにして追跡する節をcoreにするといい感じ。
    /// </summary>
    [Serializable]
    public class camchara : effectchara
    {/// <summary>
    /// 普通のコンストラクタ
    /// </summary>
    /// <param name="timer">持続時間</param>
    /// <param name="dx">coreのdx</param>
    /// <param name="dy">coreのdy</param>
    /// <param name="hyo">追跡する表示マン</param>
        public camchara(float timer, float dx, float dy, hyojiman hyo) : base()
        {
            hyojiman = hyo;
            core = new setu("core", dx, dy, new picture(0, 0, 0, 0, 0, 0, 0, 0, false, 0, "", new Dictionary<string, string>()), new List<setu>());
            x = 0;
            y = 0;
            w = 0;
            h = 0;
            tx = 0;
            ty = 0;
            rad = 0;
            time = timer;
            resethyoji(hyojiman);
            hyojiman.effects.Add(this);
            frame();
        }

        public camchara(camchara cam) : base(cam)
        {
            hyojiman = cam.hyojiman;
          
            time = cam.time;
            resethyoji(hyojiman);
            hyojiman.effects.Add(this);
            frame();
        }
        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        public camchara() { }
        /// <summary>
        /// フレーム処理
        /// </summary>
        /// <param name="cl">クロック</param>
        public override void frame(float cl=1)
        {
            base.frame(cl);
            settxy(hyojiman.camx, hyojiman.camy);
        }
        public override character clone()
        {
            return new camchara(this);
        }
    }
    /// <summary>
    /// エッフェクト。フレーム処理はhyojiman側で行われる。
    /// </summary>
    [Serializable]
    public class effectchara : character
    {
        /// <summary>
        /// 残り生存時間
        /// </summary>
        public float time = 0;
        /// <summary>
        /// くっつくキャラクター
        /// </summary>
        public character on = null;
        /// <summary>
        /// くっつく節""でキャラクターそのものにくっつく
        /// </summary>
        public string onsetu="";
        bool kaitable = false;
        /// <summary>
        /// 自分を保持してるキャラクター
        /// </summary>
        protected hyojiman hyojiman;
        /// <summary>
        /// 表示マン中に自分が存在していないフラグ
        /// </summary>
        public bool sinderu { get { return !hyojiman.effects.Contains(this); } }
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="hyo">追加する表示マン</param>
        /// <param name="timer">生存時間</param>
        /// <param name="xx">左上x座標</param>
        /// <param name="yy">左上y座標</param>
        /// <param name="ww">幅</param>
        /// <param name="hh">高さ</param>
        /// <param name="ttx">中心x座標</param>
        /// <param name="tty">中心y座標</param>
        /// <param name="rrad">ラディアン角度</param>
        /// <param name="cor">核となる節</param>
        /// <param name="tuiteku">ついていくキャラクター。nullでついていかない</param>
        /// <param name="tuisetu">ついていく節の名前""でキャラクター本体</param>
        /// <param name="kaitenawaseru">このキャラクターの回転を合わせるか</param>
        /// <param name="addy">hyojimanにaddしてしまうか</param>
        public effectchara(hyojiman hyo, float timer, float xx, float yy, float ww, float hh, float ttx, float tty, double rrad, setu cor, character tuiteku = null, string tuisetu = "", bool kaitenawaseru = false, bool addy = true) : base(xx, yy, ww, hh, ttx, tty, rrad, cor)
        {
            hyojiman = hyo;
            time = timer;
            on = tuiteku;
            onsetu = tuisetu;
            kaitable = kaitenawaseru;
            if (addy) add();
        }
        /// <summary>
        /// 表示マンに追加するメソッド
        /// </summary>
        virtual public void add()
        {
            resethyoji(hyojiman);
            if (!hyojiman.effects.Contains(this))
                hyojiman.effects.Add(this);
        }
        /// <summary>
        /// コピーするためのコンストラクタ
        /// </summary>
        /// <param name="eff">コピー元</param>
        /// <param name="addy">表示マンに追加するか</param>
        /// <param name="hyo">nullならコピー元の表示マンに</param>
        /// <param name="setkijyun">基準をコピーするか</param>
        /// <param name="motion">モーションをコピーするか</param>
        public effectchara(effectchara eff, bool addy = true, hyojiman hyo = null,bool setkijyun=true,bool motion=false) : base(eff,setkijyun,motion)
        {
            time = eff.time;
            on = eff.on;
            onsetu = eff.onsetu;
            kaitable = eff.kaitable;

            if (hyo == null) hyojiman = eff.hyojiman;
            else hyojiman = hyo;
            if (addy) add();
        }
        /// <summary>
        /// キャラクターをエフェクトに変換するためのコンストラクタ
        /// </summary>
        /// <param name="hyo">追加する表示マン</param>
        /// <param name="timee">生存時間</param>
        /// <param name="c">素となるキャラクター</param>
        /// <param name="onn">ついていくキャラクター。nullでついていかない</param>
        /// <param name="tuisetu">ついていく節の名前""でキャラクター本体</param>
        /// <param name="kaitenawaseru">このキャラクターの回転を合わせるか</param>
        /// <param name="addy">hyojimanにaddしてしまうか</param>
        /// <param name="setkijyun">基準をコピーするか</param>
        /// <param name="motion">モーションをコピーするか</param>
        public effectchara(hyojiman hyo, float timee, character c, character onn = null, string tuisetu = "", bool addy = true, bool kaitenawaseru = false
            , bool setkijyun = true, bool motion = false) : base(c,setkijyun,motion)
        {
            hyojiman = hyo;
            time = timee;
            on = onn;
            onsetu = tuisetu;
            kaitable = kaitenawaseru;
            if (addy) add();

        }
        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        public effectchara()  {}
        /// <summary>
        /// フレーム処理。表示マンが行ってくれる
        /// </summary>
        /// <param name="cl"></param>
        public override void frame(float cl=1)
        {
            base.frame(cl);
            if (on != null)
            {
                var a = on.core.GetSetu(onsetu);
                if (a != null)
                {
                    settxy(a.p.gettx(), a.p.getty());
                    if (kaitable)
                    {
                        double kakun = a.p.RAD - RAD;
                        RAD += kakun;
                        foreach (var bb in core.getallsetu())
                        {
                            bb.p.RAD += kakun;
                        }

                    }
                }
                else
                {
                    settxy(on.gettx(), on.getty());
                    if (kaitable)
                    {

                        double kakun = on.RAD - RAD;
                        RAD += kakun;
                        foreach (var bb in core.getallsetu())
                        {
                            bb.p.RAD += kakun;
                        }
                    }
                }
                soroeru();

            }
            if (time > 0) time -= cl;
            else
            {
                sinu(hyojiman);
            }
        }
        /// <summary>
        /// hyoujimanからも削除し、消えるためのメソッド
        /// </summary>
        /// <param name="hyojiman"></param>
        public override void sinu(hyojiman hyojiman)
        {
            base.sinu(hyojiman);
            hyojiman.effects.Remove(this);

        }
        public override character clone()
        {
            return new effectchara(this);
        }

    }
    /// <summary>
    /// エフェクトの背景版
    /// </summary>
    [Serializable]
    public class haikeieff:effectchara
    {
        /// <summary>
        /// キャラクターを背景のエフェクトにするメソッド
        /// </summary>
        /// <param name="hyo">追加する表示マン</param>
        /// <param name="time">生存時間</param>
        /// <param name="scrolx">スクロール割合x</param>
        /// <param name="scroly">スクロール割合y</param>
        /// <param name="c">素となるキャラクター</param>
        /// <param name="addin">追加するか</param>
        public haikeieff(hyojiman hyo, float time, float scrolx, float scroly, character c,bool addin=false) : base(hyo, time, c,addy:addin) 
        {
            scx = scrolx;
            scy = scroly;
        }
        /// <summary>
        /// コピーするためのコンストラクタ
        /// </summary>
        /// <param name="eff">コピー元</param>
        /// <param name="addy">表示マンに追加するか</param>
        /// <param name="hyo">nullならコピー元の表示マンに</param>
        /// <param name="setkijyun">基準をコピーするか</param>
        /// <param name="motion">モーションをコピーするか</param>
        public haikeieff(haikeieff eff, bool addy = true, hyojiman hyo = null,bool setkijyun=true,bool motion=false) : base(eff,false,hyo,setkijyun,motion)
        {
          
            scx = eff.scx;
            scy = eff.scy;
            if (addy) add();

        }
        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        public haikeieff() { }
       
        public override void add()
        {
            addtohaikei(scx,scy, hyojiman);
            if (!hyojiman.effects.Contains(this))
                hyojiman.effects.Add(this);
        }
        public override void sinu(hyojiman hyojiman)
        {
            sinuhaikei(hyojiman);
            hyojiman.effects.Remove(this);
        }
        float scx, scy;
        List<haikeidraws> ds = new List<haikeidraws>();
        /// <summary>
        /// 背景に追加するためのメソッド
        /// </summary>
        /// <param name="scrollx">スクロール割合x</param>
        /// <param name="scrolly">スクロール割合y</param>
        /// <param name="hyojiman">追加する表示マン</param>
        public void addtohaikei(float scrollx, float scrolly, hyojiman hyojiman)
        {
           // Console.WriteLine("asuhjasdighbasdoimnh");
            var li = core.getallsetu();

            li.Sort((a, b) => (int)((-a.p.z + b.p.z)*100) );
            foreach (var lll in li)
            {
              //  Console.WriteLine(lll.nm+" :haikeisnaufni: "+lll.p.z);
                var ddd = new haikeidraws(scrollx, scrolly, lll.p);
                ds.Add(ddd);
                hyojiman.addhaikeipicture(ddd);

            }
        }
        /// <summary>
        /// sinuの背景版
        /// </summary>
        /// <param name="hyojiman">削除する表示マン</param>
        public void sinuhaikei(hyojiman hyojiman)
        {
           
            foreach (var lll in ds)
            {
                hyojiman.removehaikeipicture(lll);

            }
            ds.Clear();
        }
        public override character clone()
        {
            return new haikeieff(this);
        }
    }
    /// <summary>
    /// いい感じにセリフを表示するためのクラス。使うに堪えない
    /// </summary>
    [Serializable]
    public class serif : effectchara
    {
        /// <summary>
        /// セリフの中身
        /// </summary>
        public message m;
        /// <summary>
        /// メッセージの方のタイマー
        /// </summary>
        public float TIMEA { get { return m.TIMEA; } }
        /// <summary>
        /// サイズと高さ
        /// </summary>
        protected float size, takasa;

        /// <summary>
        /// 普通のコンストラクタ
        /// <paramref name="z">z</paramref>
        /// <param name="hyo">追加する表示マン</param>
        /// <param name="time">生存時間</param>
        /// <param name="tuiteku">ついていくキャラクター</param>
        /// <param name="habamoji">文字の幅</param>
        /// <param name="speed">一文字が表示されるために必要な時間</param>
        /// <param name="text">セリフの内容</param>
        /// <param name="scale">文字の大きさ（基本はキャラクターの大きさに依存）</param>
        /// <param name="R">色</param>
        /// <param name="G">色</param>
        /// <param name="B">色</param>
        /// <param name="takaa">セリフが表示される高さ</param>
        /// <param name="textuer">ウィンドウのテクスチャー</param>
        public serif(hyojiman hyo, float time, character tuiteku, int habamoji, float speed, string text, float scale = 1, float R = 0, float G = 0, float B = 0, float takaa = 1, float z = 99999, string textuer = "serifwindow", bool fade = false) : base(hyo, 100, tuiteku.x, tuiteku.y, 0, 0, 0, 0, 0, new setu("window", 0, 0, new picture(0, -10000, z, 0, 0, 0, 0, 0, false, 1, "def", new Dictionary<string, string> { { "def", textuer } }), new List<setu>()), tuiteku,"",false,false)
        {

            takasa = takaa;
            size = scale;
            float si = (tuiteku.w + tuiteku.h) / 5 * size;
            if (si <= 0) si = 1;
            m = new message(tuiteku.x, tuiteku.y, (int)si, habamoji, 0, speed, time, text, R, G, B, true, z);
            add();

            if (fade) 
            {
                var mot=new motion(new moveman(m.texthyojitime, true));
                mot.addmoves(new Kopaman(time, "", 0));
                this.addmotion(mot);
                m.setfadeout(m.texthyojitime, time);
            }
        }
        /// <summary>
        /// コピーするためのコンストラクタ。フェードがうまくコピーできないかも
        /// </summary>
        /// <param name="eff">コピー元</param>
        /// <param name="addy">追加するか</param>
        /// <param name="hyo">追加する表示マン(nullならコピー元のを使う)</param>
        /// <param name="setkijyun">基準をコピーするか</param>
        /// <param name="motion">モーションをコピーするか</param>
        public serif(serif eff, bool addy = true, hyojiman hyo = null,bool setkijyun= true, bool motion = false) : base(eff, addy, hyo, setkijyun, motion)
        {
            m = new message(m);
            size = eff.size;
        }
        /// <summary>
        /// カラコン
        /// </summary>
        public serif() : base()
        {

        }
        /// <summary>
        /// フレーム処理。文字に応じてウィンドウの幅を変えたり
        /// </summary>
        /// <param name="cl">クロック</param>
        public override void frame(float cl = 1)
        {
            base.frame(cl);
            SetSerif();
        }
     
        /// <summary>
        /// セリフの座標とかをちゃんとセットする
        /// </summary>
        protected void SetSerif()
        {
            if (m.sinderu) time =0;
            else { time = 100; }
            core.p.w = m.W + m.SIZE/10;
            core.p.h = m.H*4f/2f;

            core.dx = -core.p.w * 1f / 8f;
            core.dy =(-core.p.h- on.ty) * takasa;
            m.x = core.p.x + m.SIZE/10;
            m.y = core.p.y;
        }
        
        public override void sinu(hyojiman hyojiman)
        {
    
            base.sinu(hyojiman);
            m.remove(hyojiman);
        }
        public override void resethyoji(hyojiman hyojiman)
        {
            m.add(hyojiman);
            base.resethyoji(hyojiman);
            SetSerif();

        }
        public override character clone()
        {
            return new serif(this);
        }

    }



   



}