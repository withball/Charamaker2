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
    /// クリック+キーで移動アンドセレクトできる便利めのインターフェース
    /// start,frame忘れずに！！
    /// </summary>
    public class buttoninterface:bennriUI
    {

        List<character> buttons = new List<character>();


        character now;
        /// <summary>
        /// 指す奴
        /// </summary>
       public readonly character selecter;
      
        Shape kata;
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="hyo">表示マン</param>
        /// <param name="nowindex">最初に選択するボタンの番号</param>
        /// <param name="selecter">選択中を表すカーソル</param>
        /// <param name="buttons">左から右の順番にしてほしい</param>
        /// <param name="buttonkata">ボタンの形(x,y,w,h,radは関係ないよ)</param>
        public buttoninterface(hyojiman hyo, int nowindex, picture selecter, List<picture> buttons,Shape buttonkata):base(hyo)
        {
            kata = buttonkata;

            
            foreach (var a in buttons)
            {
                this.buttons.Add(new character(a.x,a.y,a.w,a.h,a.w/2,a.h/2,a.RAD,new setu("core",0,0,a)));
                
            }
            now = this.buttons[nowindex];
            this.selecter = new character(selecter.x, selecter.y, selecter.w, selecter.h, selecter.w / 2, selecter.h / 2, selecter.RAD, new setu("core", 0, 0, selecter));
            
            this.hyo = hyo;
        }
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="hyo">表示マン</param>
        /// <param name="nowindex">最初に選択するボタンの番号</param>
        /// <param name="selecter">選択中を表すカーソル</param>
        /// <param name="buttons">左から右の順番にしてほしい</param>
        /// <param name="buttonkata">ボタンの形(x,y,w,h,radは関係ないよ)</param>
        public buttoninterface(hyojiman hyo, int nowindex, character selecter, List<character> buttons, Shape buttonkata):base(hyo)
        {
            kata = buttonkata;


            this.buttons = buttons;
         
            this.selecter = selecter;
            now = this.buttons[nowindex];
            this.hyo = hyo;
        }

        /// <summary>
        /// フレームで呼び出す奴
        /// </summary>
        /// <param name="cl"></param>
        override public void frame(float cl)
        {
            base.frame(cl);
            selecter.frame(cl);
            foreach (var a in buttons) a.frame(cl);
        }
        protected override void zure(float dx, float dy)
        {
            foreach (var a in buttons) 
            {
                a.idouxy(dx,dy);
            }
            selecter.idouxy(dx, dy);
        }
        /// <summary>
        /// UIの開始
        /// </summary>
        override public void start() 
        {
            foreach (var a in this.buttons)
            {
                a.resethyoji(hyo);
            }
            this.selecter.resethyoji(hyo);
         
            this.selecter.settxy(now.gettx(), now.getty());
        }

        /// <summary>
        ///UIを破壊するときに呼び出せば？
        /// </summary>
        override public void end() 
        {
            foreach (var a in buttons) a.sinu(hyo);
            selecter.sinu(hyo);
        }
        /// <summary>
        /// クリックで選択したボタンを全部返す
        /// </summary>
        /// <param name="x">クリック位置</param>
        /// <param name="y">クリック位置</param>
        /// <returns>クリックしたボタン</returns>
        public List<character> clickeds(float x,float y)
        {
            var res = new List<character>();
            for (int j = 0; j < buttons.Count; j++) 
            {
                kata.setto(buttons[j]);
                if (kata.onhani(x, y)) 
                {
                    res.Add(buttons[j]);
                }
            }
            
            return res;
        }
        /// <summary>
        /// 選択中のボタンを返す
        /// </summary>
        /// <returns>選択中のボタン(1だけどこっちのが便利だろ？)</returns>
        public List<character> selected()
        {
            var res = new List<character>();
            if (now != null) res.Add(now);
            return res;
        }
        /// <summary>
        /// 選んでる奴をセットする
        /// </summary>
        /// <param name="p">選ぶ奴</param>
        /// <returns>選べたか</returns>
        public bool select(character p) 
        {
            if (buttons.Contains(p)) 
            {
                if (now != p)
                {
                    now = p;
                    selecter.settxy(now.gettx(), now.getty());
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 最もそれらしいボタンに移動する
        /// </summary>
        /// <param name="plus">プラスか</param>
        /// <param name="y">y方向か</param>
        public void selectsaikinbosi(bool plus, bool y)
        {
            character res = null;
            var picx = new List<character>();
            var picy = new List<character>();

            foreach (var a in buttons)
            {
                int i;
                bool go = false;
                for (i = 0; i < picx.Count; i++)
                {
                    if (picx[i].x >= a.x)
                    {
                        go = true;
                        break;
                    }
                }
                if (go)
                {
                    picx.Insert(i, a);
                }
                else
                {
                    picx.Add(a);
                }
                go = false;
                for (i = 0; i < picy.Count; i++)
                {
                    if (picy[i].y >= a.y)
                    {
                        go = true;
                        break;
                    }
                }
                if (go)
                {
                    picy.Insert(i, a);
                }
                else
                {
                    picy.Add(a);
                }
            }
            //  foreach (var a in picy) Console.WriteLine(a.texname+" askfna;l ");
            if (y)
            {
                int i = picy.IndexOf(now);
                if (plus)
                {
                    i = Math.Min(i + 1, picy.Count - 1);
                }
                else
                {
                    i = Math.Max(i - 1, 0);
                }
                res = picy[i];
            }
            else
            {
                int i = picx.IndexOf(now);
                if (plus)
                {
                    i = Math.Min(i + 1, picx.Count - 1);
                }
                else
                {
                    i = Math.Max(i - 1, 0);
                }
                res = picx[i];
            }
            if (res != null)
            {
                now = res;
                selecter.settxy(now.gettx(), now.getty());
            }
        }
    }
    /// <summary>
    /// キャラクターがローリングするUI
    /// </summary>
    public class rolling : bennriUI
    {
        /// <summary>
        /// キャラクター共
        /// </summary>
        List<character> charas = new List<character>();
        /// <summary>
        /// 選択はできないエフェクト系のキャラクター
        /// startしたのちに追加するのであればresethyojiは別途呼び出してくれ
        /// </summary>
        public List<character> adds ;
        /// <summary>
        /// 名前通り
        /// </summary>
        /// <param name="c"></param>
        public void addcharas(character c) 
        {
            charas.Add(c);
        }
        /// <summary>
         /// 名前通り
         /// </summary>
         /// <param name="c"></param>
        public void removecharas(character c)
        {
            charas.Remove(c);
            if (now == c) now = null;
        }

        /// <summary>
        /// セレクトキャラクタ-
        /// </summary>
        character now=null;
        /// <summary>
        /// インデックス
        /// 
        /// </summary>
        public int nowidx  { get { if (now == null) return -1; return charas.IndexOf(now); } }


        Shape kata;
        float time, timer = 0;

        /// <summary>
        ///i番目のキャラクターを返す
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public character getchara(int i) 
        {
            if(i>=0&&i<charas.Count)            return charas[i];
            return null;
        }
        /// <summary>
        /// 全てのキャラクターを返す
        /// </summary>
        /// <returns></returns>
        public List<character> getchara()
        {
            return new List<character>(charas);
        }

        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="hyo">表示マン</param>
        /// <param name="time">回転時間</param>
        /// <param name="nowindex">最初に選択するボタンの番号</param>
        /// <param name="buttons">左から右の順番にしてほしい</param>
        /// <param name="buttonkata">ボタンの形(x,y,w,h,radは関係ないよ)</param>
        public rolling(hyojiman hyo, int nowindex,float time, List<character> buttons, Shape buttonkata,List<character>adds=null) : base(hyo)
        {
            kata = buttonkata;
            this.time = time;

            this.charas = buttons;
            if (adds == null)
            {
                this.adds = new List<character>();
            }
            else 
            {
                this.adds = adds;
            }

            now = this.charas[0];

            idou(nowidx);

            this.hyo = hyo;
        }

     

        /// <summary>
        /// フレームで呼び出す奴
        /// </summary>
        /// <param name="cl"></param>
        override public void frame(float cl)
        {
            base.frame(cl);
            foreach (var a in charas) a.frame(cl);
            foreach (var a in adds) a.frame(cl);
            roll(cl);
        }
        
        protected override void zure(float dx, float dy)
        {
            foreach (var a in charas)
            {
                a.idouxy(dx, dy);
            }
            foreach (var a in adds)
            {
                a.idouxy(dx, dy);
            }
        }
        /// <summary>
        /// UIの開始
        /// </summary>
        override public void start()
        {
            foreach (var a in this.charas)
            {
                a.resethyoji(hyo);
            }

            foreach (var a in this.adds)
            {
                a.resethyoji(hyo);
            }

        }

        /// <summary>
        ///UIを破壊するときに呼び出せば？
        /// </summary>
        override public void end()
        {
            foreach (var a in charas) a.sinu(hyo);
            foreach (var a in adds) a.sinu(hyo);
        }
        /// <summary>
        /// クリックで選択したボタンを全部返す
        /// </summary>
        /// <param name="x">クリック位置</param>
        /// <param name="y">クリック位置</param>
        /// <returns>クリックしたボタン</returns>
        public List<character> clickeds(float x, float y)
        {
            var res = new List<character>();
            for (int j = 0; j < charas.Count; j++)
            {
                kata.setto(charas[j]);
                if (kata.onhani(x, y))
                {
                    res.Add(charas[j]);
                }
            }

            return res;
        }
        /// <summary>
        /// 選択中のボタンを返す
        /// </summary>
        /// <returns>選択中のボタン(1だけどこっちのが便利だろ？)</returns>
        public List<character> selected()
        {
            var res = new List<character>();
            if (now != null) res.Add(now);
            return res;
        }
        /// <summary>
        /// 選んでる奴をセットする
        /// </summary>
        /// <param name="p">選ぶ奴</param>
        /// <returns>選べたか</returns>
        public bool select(character p)
        {
            if (charas.Contains(p))
            {
                if (now != p)
                {
                    now = p;
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// 指定したキャラクターに移動
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool idou(character c)
        {
            int p = charas.IndexOf(c);
            if (p != -1) 
            {
                var s = charas.IndexOf(now);
                
                return scroll(p, s); ;
            }
            return false;

        }
        /// <summary>
        /// 指定した距離移動
        /// </summary>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public bool idou( int d)
        {
            var s = charas.IndexOf(now);

            return scroll(s + d, s); ;

        }
        protected bool scroll(int to,int st) 
        {
            while (to >= charas.Count) 
            {
                to -= charas.Count;
            }
            while (to < 0)
            {
                to += charas.Count;
            }
            if (timer <= 0&&to-st!=0) 
            {
                now = charas[to];
                float dx = hyo.ww/2+precamx-now.gettx();
                rollpower = dx;
                timer = time;
                return true;
            }
            return false;
        }
        float rollpower = 0;
        /// <summary>
        /// キャラクターたちを回転させたりするメソッド
        /// </summary>
        /// <param name="cl"></param>
        protected void roll(float cl) 
        {

            if (timer > 0)
            {
                timer -= cl;
                float clll = cl;
                if (timer < 0) clll += timer;
                foreach (var a in charas)
                {
                    a.idouxy(rollpower * clll / time, 0);
                }
                foreach (var a in adds)
                {
                    a.idouxy(rollpower * clll / time, 0);
                }
            }
        }



    }
}
