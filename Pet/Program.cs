//#####################################################################################
//★★★★★★★           http://www.cnpopsoft.com [华普软件]           ★★★★★★★
//★★★★★★★        华普软件 - VB6 & C#.NET专业论文与源码荟萃        ★★★★★★★
//#####################################################################################

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Pet
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FishForm());
        }
    }
}