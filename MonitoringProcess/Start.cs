using System;
using System.Collections.Generic;
using System.Text;
using Iplugins;

namespace MonitoringProcess
{
    public class Start : IPetPlug
    {
        private MonitoringProcess mp = null;
        private Menu[] menus = { };
        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialization() {
            menus = new Menu[]{ new Menu("工具", 2), new Menu("进程监控", 1) };
        }
        /// <summary>
        /// 打开插件
        /// </summary>
        public void OpenPlug()
        {
            if (mp == null || mp.IsDisposed)
            {
                mp = new MonitoringProcess();
            }
            mp.Show();
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public Menu[] GetMenu()
        {
            return menus;
        }
        /// <summary>
        /// 关闭插件
        /// </summary>
        public void Close()
        {
            if (mp != null && !mp.IsDisposed)
            {
                mp.Close();
            }
        }
    }
}
