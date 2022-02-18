using System;
using System.Collections.Generic;
using System.Text;

namespace GameSet1
{
    /// <summary>
    /// エンテティに追加していろいろ動作させるクラス
    /// </summary>
    public class Waza
    {
        /// <summary>
        /// 便利ショトカeのEM
        /// </summary>
        protected EntityManager EM { get { return e.EM; } }

        /// <summary>
        /// 便利ショトカeのEMのhyoji
        /// </summary>
        protected Charamaker2.hyojiman hyoji { get { return e.EM.hyoji; } }
        /// <summary>
        /// 現在時間
        /// </summary>
        protected float timer=0;
        /// <summary>
        /// 終了時間
        /// </summary>
        protected float end;
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="end">終了時間</param>
        public Waza(float end) 
        {
            this.end = end;
        }
        private Entity _e=null;
        /// <summary>
        /// この技が追加されてるエンテティ
        /// </summary>
        protected Entity e { get { return _e; } }
        /// <summary>
        /// 技をエンテティに追加する。複数のに追加しないでね
        /// 追加できたらOnaddを呼び出す。
        /// </summary>
        /// <param name="e">追加するエンテティ</param>
        /// <returns>追加されたかどうか</returns>
        virtual public bool add(Entity e) 
        {
            if(e.addWaza(this))
            {
                this._e = e;
                
                onAdd();
                return true;
            }
            return false;

        }
        /// <summary>
        /// 技をエンテティから削除する。
        /// 削除できたらonRemoveを呼び出す
        /// </summary>
        /// <returns></returns>
        virtual public bool remove()
        {
            if (e!=null&&e.removeWaza(this))
            {

                onRemove();
                return true;
            }
            return false;

        }
        /// <summary>
        /// 残りの消費できる時間を算出する
        /// </summary>
        /// <param name="cl">このぐらい消費したいです！</param>
        /// <returns>消費できる時間</returns>
        protected float nokoritime(float cl) 
        {
            var t = end - timer;
            if (t < 0) t = 0;
            if (t< cl) return t;
            return cl;
        }
        /// <summary>
        /// フレーム処理。
        /// Onframeを呼び出した後にタイマーが進められる
        /// </summary>
        /// <param name="cl">クロック時間</param>
        virtual public void frame(float cl) 
        {
           
            onFrame(nokoritime(cl));
            timer += cl;
        }
        /// <summary>
        /// フレーム処理の時に呼び出されるメソッド
        /// オーバーライドしてね
        /// </summary>
        /// <param name="cl">クロック時間</param>
        virtual protected void onFrame(float cl) 
        {
            
        }
        
        /// <summary>
        /// 技がエンテティに追加されたときに発動するメソッド
        /// オーバーライドしてね。
        /// </summary>
        virtual protected void onAdd()
        {

        }
        /// <summary>
        /// 技がエンテティから削除されたときに発動するメソッド
        /// オーバーライドしてね。
        /// ここで自分を新しく追加するとかやったら無限ループするからやめてね。
        /// </summary>
        virtual protected void onRemove()
        {

        }


    }
}
