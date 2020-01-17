using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;

namespace Pet
{
    public class DynamicMenu
    {
        /// <summary>
        /// 菜单
        /// </summary>
        private static ContextMenuStrip _contextMenuStrip = null;
        ///<summary>
        /// 存放插件的集合
        ///</summary>
        private static Dictionary<String, Object> plugins = new Dictionary<String, Object>();
        ///<summary>
        /// 载入所有插件
        ///</summary>
        public static void LoadAllPlugs(ContextMenuStrip contextMenuStrip)
        {
            _contextMenuStrip = contextMenuStrip;
            //获取插件目录(plugins)下所有文件
            DirectoryInfo[] filePaths = (new DirectoryInfo(Application.StartupPath + @"\plugins")).GetDirectories();
            foreach (DirectoryInfo filePath in filePaths)
            {
                FileInfo[] files = filePath.GetFiles("*.DLL");
                foreach (FileInfo fileP in files)
                {
                    String file = fileP.FullName;
                    try
                    {
                        //载入dll
                        Assembly ab = Assembly.LoadFrom(file);
                        Type[] types = ab.GetTypes();
                        foreach (Type t in types)
                        {
                            Console.WriteLine(t);
                            //如果某些类实现了预定义的IMsg.IMsgPlug接口，则认为该类适配与主程序(是主程序的插件)
                            if (t.GetInterface("IPetPlug") != null)
                            {
                                if (!plugins.ContainsKey(t.FullName))
                                {
                                    Object selObj = ab.CreateInstance(t.FullName);
                                    plugins.Add(t.FullName, selObj);
                                    t.GetMethod("Initialization").Invoke(selObj, null);
                                    MethodInfo GetMenu = t.GetMethod("GetMenu");
                                    Object menuO = GetMenu.Invoke(selObj, null);
                                    Iplugins.Menu[] menu = (Iplugins.Menu[])menuO;
                                    setMenu(menu, selObj);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }
        /// <summary>
        /// 动态生成插件菜单
        /// </summary>
        /// <param name="menus">菜单</param>
        private static void setMenu(Iplugins.Menu[] menus, Object o)
        {
            ToolStripItemCollection item = _contextMenuStrip.Items;
            for (int i = 0; i < menus.Length; i++)
            {
                Iplugins.Menu om = menus[i];
                String text = om.MenuName;
                String name = text + "ToolStripMenuItem";
                ToolStripMenuItem menu = null;
                if (!item.ContainsKey(name))
                {
                    menu = new ToolStripMenuItem(text);
                    menu.Name = name;
                    if (!String.IsNullOrEmpty(om.MenuIco))
                    {
                        try
                        {
                            menu.Image = new Bitmap(om.MenuIco);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (i == menus.Length - 1)
                    {
                        menu.Tag = o;
                        menu.Click += new EventHandler(menu_Click);
                    }
                    InsertMenu(item, menu, om.Index);
                }
                else
                {
                    menu = (ToolStripMenuItem)(item.Find(name, false)[0]);
                }
                item = menu.DropDownItems;
            }
        }
        /// <summary>
        /// 将菜单按指定顺序插入到指定位置
        /// </summary>
        /// <param name="item">菜单组</param>
        /// <param name="menu">要插入的菜单</param>
        /// <param name="ind">要插入的顺序</param>
        private static void InsertMenu(ToolStripItemCollection item, ToolStripMenuItem menu, int ind)
        {
            if (ind == 0 || item.Count == 0)
            {
                // 插入位置为0或是没有菜单，在菜单末尾插入菜单
                item.Insert((item.Count > 0 ? item.Count - 1 : 0), menu);
            }
            else
            {
                int insertIndex = 0;
                foreach (ToolStripMenuItem oneMenu in item)
                {
                    if (oneMenu.MergeIndex > ind)
                    {
                        item.Insert(insertIndex, menu);
                        break;
                    }
                    insertIndex++;
                }
                if (insertIndex == item.Count)
                {
                    item.Insert(item.Count - 1, menu);
                }
            }
        }
        /// <summary>
        /// 菜单的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void menu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tool = (ToolStripMenuItem)sender;
            Object o = tool.Tag;
            Type t = o.GetType();
            MethodInfo OpenPlug = t.GetMethod("OpenPlug");
            OpenPlug.Invoke(o, null);
        }

        public static void closeAllPlugins()
        {
            foreach (object o in plugins.Values)
            {
                closePlugin(o);
            }
        }

        public static void closePlugin(object o)
        {
            if (o != null)
            {
                try
                {
                    Type t = o.GetType();
                    MethodInfo Close = t.GetMethod("Close");
                    Close.Invoke(o, null);
                }
                catch (Exception) { }
            }
        }
    }
}
