using System;
using System.Collections.Generic;
using System.Text;
using Charamaker2;
using Charamaker2.input;

namespace GameSet1
{
    /// <summary>
    /// シーンの変更のためのバッファー的な奴
    /// </summary>
    public class SceneManager 
    {
        /// <summary>
        /// ここに好きなシーンが入る！そしてs.frame()
        /// </summary>
        public Scene s=null;
        
    }
    /// <summary>
    /// シーンだお
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// SceneManager
        /// </summary>
        public SceneManager sm;
        /// <summary>
        /// 表示マン
        /// </summary>
        public readonly hyojiman hyo;
        /// <summary>
        /// 次のシーン
        /// </summary>
        public Scene next;
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="s">シーンマネージャ</param>
        public Scene(SceneManager s) 
        {
            sm = s;
            hyo = fileman.makehyojiman();
        }
        private bool _started=false;
        /// <summary>
        /// start(),end()が複数回発動しないようにするフラグ。startでtrueになる。
        /// 直接いじってもいいけどstart,endで変えてくれるんだけど
        /// </summary>
        protected bool started { get { return _started; } set { _started = value; } }
        /// <summary>
        /// シーンを開始したいときに発動してね。
        /// smにこれが代入されてnextがない時は何かしらを代入しておくといい
        /// </summary>
        virtual public void start() 
        {
            if (!started)
            {
                sm.s = this;
                if (next == null) next = this;
                _started = true;
            }
        }

        /// <summary>
        /// 画面の描画。標準は画面表示だけよ
        /// </summary>
        /// <param name="i">入力情報</param>
        /// <param name="cl">クロック時間</param>
        virtual public void frame(inputin i,float cl)
        {
            hyo.hyoji(cl);
        }
        /// <summary>
        /// 標準はnextをスタートしてstartedをfalseにするだけ
        /// </summary>
        virtual public void end()
        {
            if (started)
            {
                next.start();
                _started = false;
            }
        }
    }
}
