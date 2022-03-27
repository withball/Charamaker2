using Charamaker2;
using Charamaker2.input;
using System;
using System.Collections.Generic;
using Charamaker2.Shapes;
using System.Text;

namespace GameSet1.benriUI
{
    /// <summary>
    /// クリック+キーで移動アンドセレクトできる便利めのインターフェース
    /// </summary>
    public class buttoninterface
    {
        List<picture> buttons = new List<picture>();
        picture now;
        /// <summary>
        /// 指す奴
        /// </summary>
       public readonly picture selecter;
        hyojiman hyo;
        Shape kata;
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="hyo">表示マン</param>
        /// <param name="nowindex">最初に選択するボタンの番号</param>
        /// <param name="selecter">選択中を表すカーソル</param>
        /// <param name="buttons">左から右の順番にしてほしい</param>
        /// <param name="buttonkata">ボタンの形(x,y,w,h,radは関係ないよ)</param>
        public buttoninterface(hyojiman hyo, int nowindex, picture selecter, List<picture> buttons,Shape buttonkata)
        {
            kata = buttonkata;
            this.buttons = buttons;
            foreach (var a in buttons)
            {
                a.add(hyo);
            }
            this.selecter = selecter;
            selecter.add(hyo);
            now = buttons[nowindex];
            selecter.settxy(now.gettx(), now.getty());
            this.hyo = hyo;
        }
        /// <summary>
        ///UIを破壊するときに呼び出せば？
        /// </summary>
        public void sinu() 
        {
            foreach (var a in buttons) a.remove(hyo);
            selecter.remove(hyo);
        }
        /// <summary>
        /// クリックで選択したボタンを全部返す
        /// </summary>
        /// <param name="i">クリック位置</param>
        /// <returns>クリックしたボタン</returns>
        public List<picture> clickeds(inputin i)
        {
            var res = new List<picture>();
            var lis = hyo.picturegets(i.x, i.y,kata);
            foreach (var a in buttons)
            {
                if (lis.Contains(a))
                {
                    res.Add(a);
                }
            }
            return res;
        }
        /// <summary>
        /// 選択中のボタンを返す
        /// </summary>
        /// <returns>選択中のボタン(1だけどこっちのが便利だろ？)</returns>
        public List<picture> selected()
        {
            var res = new List<picture>();
            if (now != null) res.Add(now);
            return res;
        }
        /// <summary>
        /// 選んでる奴をセットする
        /// </summary>
        /// <param name="p">選ぶ奴</param>
        /// <returns>選べたか</returns>
        public bool select(picture p) 
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
            picture res = null;
            var picx = new List<picture>();
            var picy = new List<picture>();

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
}
