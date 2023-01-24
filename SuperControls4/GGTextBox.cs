using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperControls4
{
    /// <summary>
    /// 画面外に置くことで真価を発揮するテキストボックス。
    /// Scene.startで追加してScene.endで削除すると良き
    /// </summary>
    public partial class GGTextBox : TextBox
    {
        ContainerControl f;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="f">フォーカスを外したりしたいので親フォームちょうだい</param>
        public GGTextBox(ContainerControl f)
        {
            this.f = f;
            KeyDown += endinput;
        }
        /// <summary>
        /// こいつをアクティブにするメソッド
        /// </summary>
        public void Active() 
        {
            f.ActiveControl = this;
        }

        /// <summary>
        /// こいつを非活性化するメソッド
        /// </summary>
        public void EndActive()
        {
            if(f.ActiveControl == this)f.ActiveControl=null;
       }
        private void endinput(object sender,KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) 
            {
                EndActive();
            }
        }
        /// <summary>
        /// 都合上文字が画面外なので見えない。これをmessageとかで表示するとカーソルの位置が見えていい感じかもしれない
        /// </summary>
        public string superText 
        {
            get
            {
                if (f.ActiveControl == this)
                {
                    var a = this.SelectionStart;
                    return this.Text.Insert(a, "|");
                }
                return this.Text;
            }
        }
    }
}
