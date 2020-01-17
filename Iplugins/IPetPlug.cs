using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iplugins
{
    public interface IPetPlug
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Initialization();
        /// <summary>
        /// 打开插件
        /// </summary>
        void OpenPlug();
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        Menu[] GetMenu();
        /// <summary>
        /// 关闭时调用
        /// </summary>
        void Close();
    }
}
