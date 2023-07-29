using Charamaker2.maker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Charamaker2
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //SharpDX.Configuration.EnableObjectTracking = true;
            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);
            
          
                Application.Run(new charamaker());
            
            //   Console.WriteLine("yo! "+SharpDX.Diagnostics.ObjectTracker.ReportActiveObjects());
        }
        
    }
}
