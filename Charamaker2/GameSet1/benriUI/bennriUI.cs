using Charamaker2;
using Charamaker2.input;
using System;
using System.Collections.Generic;
using Charamaker2.Shapes;
using System.Text;
using Charamaker2.Character;

namespace GameSet1.benriUI
{
    /// <summary>
    /// 便利なUIの元。
    /// 要件としてあるのはstart,endで開始終了ができ、frameでhyojimanのcamがずれても対応できるってこと。
    /// </summary>
    public abstract class bennriUI
    {
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
