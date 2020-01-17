using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;
using System.Threading;
using System.Reflection;
using System.IO;

namespace MonitoringProcess
{
    public partial class MonitoringProcess : Form
    {
        private string thisPath = "";
        Dictionary<String, Process> processAll = new Dictionary<String, Process>();
        Object exitO = new Object();
        Boolean exit = false;
        Boolean canExist = false;
        public MonitoringProcess()
        {
            InitializeComponent();
            new Thread(
                delegate ()
                {
                    initFile();
                }
            ).Start();
            new Thread(
                delegate()
                {
                    getAllProcessToListView();
                    timerProcess.Start();
                }
            ).Start();
        }
        public delegate void AddList(ListViewItem lvi);
        public AddList addList;
        /// <summary>
        /// 在进程列表中移除一个进程
        /// </summary>
        /// <param name="process">进程</param>
        public delegate void RemoveList(Process process);
        public RemoveList removeList;
        /// <summary>
        /// 初始化配置文件
        /// </summary>
        private void initFile() {
            thisPath = Assembly.GetExecutingAssembly().Location;
            thisPath = thisPath.Substring(0, thisPath.LastIndexOf('\\'));
            // 初始化关闭进程文件
            string fileCloseProcess = thisPath + "\\closeProcessName";
            if (!File.Exists(fileCloseProcess)) {
                FileStream fsObj = null;
                try
                {
                    byte[] Save = global::MonitoringProcess.Properties.Resources.closeProcessName;
                    fsObj = new FileStream(fileCloseProcess, FileMode.CreateNew);
                    fsObj.Write(Save, 0, Save.Length);
                }
                finally
                {
                    if (fsObj != null)
                    {
                        fsObj.Close();
                    }
                }
            }
        }

        private void buttonGetProcess_Click(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// 获取进程列表，并加入到listview中
        /// </summary>
        private void getAllProcessToListView()
        {
            canExist = false;
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                lock (exitO)
                {
                    String ID = process.Id + "";
                    if (!processAll.ContainsKey(ID))
                    {
                        processAll.Add(ID, process);
                        ListViewItem lvi = new ListViewItem();
                        lvi.Name = ID;
                        lvi.Text = ID;
                        lvi.SubItems.Add(process.ProcessName);
                        lvi.SubItems.Add(GetProcessUserName(process.Id));
                        lvi.SubItems.Add(getProcessDescription(process));
                        if (exit)
                        {
                            break;
                        }
                        blindExited(process);
                        BeginInvoke(addList, lvi);
                    }
                }
            }
            List<string> keys = new List<string>(processAll.Keys);
            for (int i = processAll.Count-1; i > -1; i--)
            {
                Process process = processAll[keys[i]];
                if (!haveProcess(processes, process))
                {
                    RemoveListP(process);
                }
            }

            canExist = true;
        }

        private Boolean haveProcess(Process[] processList, Process process)
        {
            foreach (Process p in processList)
            {
                if (p.Id == process.Id)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 添加列表中一项
        /// </summary>
        /// <param name="lvi"></param>
        private void AddListP(ListViewItem lvi)
        {
            if (exit)
            {
                return;
            }
            listViewProcess.Items.Add(lvi);
        }
        /// <summary>
        /// 删除列表中的一项
        /// </summary>
        /// <param name="process"></param>
        private void RemoveListP(Process process)
        {
            lock (exitO)
            {
                processAll.Remove(process.Id + "");
                listViewProcess.Items.RemoveByKey(process.Id + "");
            }
            
        }
        /// <summary>
        /// 获取进程描述信息
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        private string getProcessDescription(Process process)
        {
            try
            {
                return FileVersionInfo.GetVersionInfo(process.MainModule.FileName).FileDescription;
            }
            catch (System.Exception)
            {
                return "";
            }
        }
        static EventHandler eventH = null;
        /// <summary>
        /// 绑定进程退出事件
        /// </summary>
        /// <param name="process">进程</param>
        private void blindExited(Process process)
        {
            try
            {
                process.EnableRaisingEvents = true;
                process.Exited += eventH;
            }
            catch (System.Exception)
            {
            }
        }

        /// <summary>
        /// 进程退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void process_Exited(object sender, EventArgs e)
        {
            Process process = (Process)sender;
            Console.WriteLine(process.Id + process.ProcessName);
            BeginInvoke(removeList, process);
        }
        /// <summary>
        /// 获取进程对应的用户
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private string GetProcessUserName(int pid)
        {
            try {
                ManagementObject mo = new ManagementObject("Win32_Process.Handle=\"" + pid + "\"");
                ManagementBaseObject outPar = mo.InvokeMethod("GetOwner", null, null);
                return outPar["User"].ToString();
            }catch(Exception){
                return "SYSTEM";
            }
        }

        private void MonitoringProcess_Load(object sender, EventArgs e)
        {
            canExist = true;
            exit = false;
            addList = new AddList(AddListP);
            removeList = new RemoveList(RemoveListP);
            eventH = new EventHandler(process_Exited);
        }

        private void MonitoringProcess_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerProcess.Stop();
            lock (exitO)
            {
                exit = true;
            }
            while (!canExist)
            {
                Thread.Sleep(10);
            }
            UnBlindProcessExited();
            processAll = null;
        }
        /// <summary>
        /// 解绑所有 process 退出事件
        /// </summary>
        private void UnBlindProcessExited()
        {
            foreach (Process process in processAll.Values)
            {
                process.Exited -= eventH;
                process.EnableRaisingEvents = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(listViewProcess.Items[3].Name);
            timerProcess.Start();
        }

        int currentCol = -1;
        bool sort;
        /// <summary>
        /// 表头单击进行排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewProcess_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            string Asc = ((char)0x25bc).ToString();
            string Des = ((char)0x25b2).ToString();

            if (sort == false)
            {
                sort = true;
                string oldStr = this.listViewProcess.Columns[e.Column].Text.TrimEnd((char)0x25bc, (char)0x25b2, ' ');
                this.listViewProcess.Columns[e.Column].Text = oldStr + Des;
            }
            else if (sort == true)
            {
                sort = false;
                string oldStr = this.listViewProcess.Columns[e.Column].Text.TrimEnd((char)0x25bc, (char)0x25b2, ' ');
                this.listViewProcess.Columns[e.Column].Text = oldStr + Asc;
            }
            listViewProcess.ListViewItemSorter = new ListViewItemComparer(e.Column, sort, (Type)listViewProcess.Columns[e.Column].Tag);
            this.listViewProcess.Sort();
            int rowCount = this.listViewProcess.Items.Count;
            if (currentCol != -1)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    if (e.Column != currentCol)
                        this.listViewProcess.Columns[currentCol].Text = this.listViewProcess.Columns[currentCol].Text.TrimEnd((char)0x25bc, (char)0x25b2, ' ');
                }
            }

            for (int i = 0; i < rowCount; i++)
            {
                currentCol = e.Column;
            }
        }

        private void timerProcess_Tick(object sender, EventArgs e)
        {
            getAllProcessToListView();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf('\\'));
            byte[] Save = global::MonitoringProcess.Properties.Resources.closeProcessName;
            FileStream fsObj = new FileStream(path + "\\closeProcessName", FileMode.CreateNew);
            fsObj.Write(Save, 0, Save.Length);
            fsObj.Close();
        }
    }
}
