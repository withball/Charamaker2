using Charamaker2;
using Charamaker2.input;
using System;
using System.Collections.Generic;
using Charamaker2.Shapes;
using System.Text;
using Charamaker2.Character;
using DevExpress.Utils.Design;

namespace GameSet1.benriUI
{
    /// <summary>
    /// 便利なUIの元。
    /// 要件としてあるのはstart,endで開始終了ができ、frameでhyojimanのcamがずれても対応できるってこと。
    /// </summary>
    public abstract class bennriUI
    {
        /// <summary>
        /// キャラクターでクリック判定を取る
        /// </summary>
        /// <param name="x">ポインタx</param>
        /// <param name="y">ポインタy</param>
        /// <param name="c">キャラクター</param>
        /// <param name="abr">判定を取る節の名前とかを含んだレシピ</param>
        /// <returns></returns>
       public  static bool onclick(float x, float y, character c, ABrecipie abr)
        {
            var ab = new ataribinding(c, abr);
            ab.frame();
            foreach(var a in ab.getallatari())
            {
                if(a.onhani(x, y))return true;
            }
            return false;
        }
        /// <summary>
        /// キャラクターでクリック判定を取る
        /// </summary>
        /// <param name="x">ポインタx</param>
        /// <param name="y">ポインタy</param>
        /// <param name="c">キャラクター</param>
        /// <param name="abr">判定を取る節の名前とかを含んだレシピ</param>
        /// <returns></returns>
        public static bool onclick(float x, float y, picture c, ABrecipie abr)
        {
            var ab = new ataribinding(character.onepicturechara(c), abr);
            ab.frame();
            foreach (var a in ab.getallatari())
            {
                if (a.onhani(x, y)) return true;
            }
            return false;
        }
        /// <summary>
        /// キャラクターでクリック判定を取る
        /// </summary>
        /// <param name="c1">ポインタx</param>
        /// <param name="abr1">判定を取る節の名前とかを含んだレシピ</param>
        /// <param name="c2">キャラクター</param>
        /// <param name="abr2">判定を取る節の名前とかを含んだレシピ</param>
        /// <returns></returns>
        public static bool onclick(character c1,ABrecipie abr1, character c2, ABrecipie abr2)
        {
            var ab1 = new ataribinding(c1, abr1);
            ab1.frame();

            var ab2 = new ataribinding(c2, abr2);
            ab2.frame();
            foreach (var a in ab1.getallatari())
            {
                foreach (var b in ab2.getallatari()) 
                {
                    if(a.atarun(b))return true;
                }
            }
            return false;
        }
        /// <summary>
        /// キャラクターでクリック判定を取る
        /// </summary>
        /// <param name="c1">ポインタx</param>
        /// <param name="abr1">判定を取る節の名前とかを含んだレシピ</param>
        /// <param name="c2">キャラクター</param>
        /// <param name="abr2">判定を取る節の名前とかを含んだレシピ</param>
        /// <returns></returns>
        public static bool onclick(picture c1, ABrecipie abr1, picture c2, ABrecipie abr2)
        {
            var ab1 = new ataribinding(character.onepicturechara(c1), abr1);
            ab1.frame();

            var ab2 = new ataribinding(character.onepicturechara(c2), abr2);
            ab2.frame();
            foreach (var a in ab1.getallatari())
            {
                foreach (var b in ab2.getallatari())
                {
                    if (a.atarun(b)) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 表示マン
        /// </summary>
        protected hyojiman hyo;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hyo"></param>
        public bennriUI(hyojiman hyo) 
        {
            this.hyo = hyo;
            precamx = hyo.camx;
            precamy = hyo.camy;
            
        }
        /// <summary>
        /// 開始時のメソッド
        /// </summary>
        virtual public void start() 
        {
            zure(precamx, precamy);
        }

        /// <summary>
        /// UIをずらすメソッド
        /// </summary>
        abstract protected void zure(float dx, float dy);
        protected float precamx,precamy;


        /// <summary>
        /// フレームメソッド
        /// </summary>
        virtual public void frame(float cl) 
        {

            zure(hyo.camx - precamx, hyo.camy - precamy);
            precamx = hyo.camx;
            precamy = hyo.camy;

        }

        /// <summary>
        /// 終了メソッド
        /// </summary>
        abstract public void end();
        /*
        /// <summary>
        /// コマンドのリスト
        /// </summary>
        protected List<EventHandler<EventArgs>> comannds;
        /// <summary>
        /// インデックスで管理しているコマンドを呼び出す
        /// </summary>
        /// <param name="i">何番目？</param>
        /// <param name="o"></param>
        protected void comand(int i,object o) 
        {
            if (i < comannds.Count) 
            {
                comannds[i]?.Invoke(o, new EventArgs());
            }
        }
        */

    }
}
