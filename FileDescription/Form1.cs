using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace FileDescription
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        String al = "";
        String description = "软媒";
        FileStream fs = null;
        StreamWriter sw = null;
        FileStream fs1 = null;
        StreamWriter sw1 = null;
        private void button1_Click(object sender, EventArgs e)
        {
            fs = new FileStream("D:\\log.log", FileMode.OpenOrCreate);
            sw = new StreamWriter(fs, Encoding.Default);
            fs1 = new FileStream("D:\\log1.log", FileMode.OpenOrCreate);
            sw1 = new StreamWriter(fs, Encoding.Default);
            DirectoryInfo d = new DirectoryInfo(@"C:\");
            new Thread(gets).Start();
        }
        private void gets(){
            GetAll(new DirectoryInfo(@"C:\"));
            al = "完成";
        }

        private void GetAll(DirectoryInfo dir)//搜索文件夹中的文件
        {
            try
            {
                ArrayList FileList = new ArrayList();

                FileInfo[] allFile = dir.GetFiles();
                foreach (FileInfo fi in allFile)
                {
                    String de = FileVersionInfo.GetVersionInfo(fi.FullName).FileDescription;
                    if (String.IsNullOrEmpty(de) || de.IndexOf(description) > 0)
                    {
                        sw.WriteLine(de + "<---->" + fi.FullName);
                        sw.Flush();
                        Console.WriteLine(de + "<---->" + fi.FullName);
                    }
                }

                DirectoryInfo[] allDir = dir.GetDirectories();
                foreach (DirectoryInfo d in allDir)
                {
                    GetAll(d);
                }
            }
            catch (System.Exception ex)
            {
                sw1.WriteLine(dir.ToString());
                sw1.Flush();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = al;
        }
    }
}
