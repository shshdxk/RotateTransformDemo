using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using Iplugins;
using System.IO;
using System.Diagnostics;
using WinSystem;

namespace Pet
{
    static class Program
    {
        static bool glExitApp = false;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            

            //处理未捕获的异常
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            //Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理非UI线程异常
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            glExitApp = true;//标志应用程序可以退出

            //Application.Run(new FormArtifact());
            Process instance = RunningInstance();
            if (instance == null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FishForm());
            }
            else
            {
                //HandleRunningInstance(instance);
            }
        }


        /// <summary>
        /// 检查程序是否已运行
        /// </summary>
        /// <returns></returns>
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 提示已运行，并将已运行的程序置顶
        /// </summary>
        /// <param name="instance"></param>
        public static void HandleRunningInstance(Process instance)
        {
            MessageBox.Show("程序已运行，请勿重复运行！！！");
            Win32Api.SetForegroundWindow(instance.MainWindowHandle);
            Win32Api.ShowWindowAsync(instance.MainWindowHandle, Win32Api.WS_SHOWNORMAL);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //_log.Error("CurrentDomain_UnhandledException");
            //_log.Error("IsTerminating : " + e.IsTerminating.ToString());
            //_log.Error(e.ExceptionObject.ToString());
            //_log.Error("程序出错重启！！！！！！！！！！！！！！！！！");
            //CmdStartCTIProc(Application.ExecutablePath, "cmd params");
            Application.Restart();
            while (true)
            {//循环处理，否则应用程序将会退出
                if (glExitApp)
                {//标志应用程序可以退出，否则程序退出后，进程仍然在运行
                    //LogHelper.Save("ExitApp");
                    //CmdStartCTIProc(Application.ExecutablePath, "cmd params");
                    return;
                }
                System.Threading.Thread.Sleep(2 * 1000);
            };
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //_log.Error("Application_ThreadException:" + e.Exception.Message);
            //CmdStartCTIProc(Application.ExecutablePath, "cmd params");
            //_log.Error(e.Exception);
            //throw new NotImplementedException();
            String message = e.Exception.Source + "：" + e.Exception.Message;
            MessageBox.Show(message,"警告");
        }

        static void CmdStartCTIProc(string sExePath, string sArguments)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = false;
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.Start();
            p.StandardInput.WriteLine(sExePath + " " + sArguments);
            p.StandardInput.WriteLine("exit");
            p.Close();

            System.Threading.Thread.Sleep(2000);//必须等待，否则重启的程序还未启动完成；根据情况调整等待时间
        }
    }
}