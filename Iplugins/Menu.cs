using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iplugins
{
    public class Menu
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        private int _index = 0;
        /// <summary>
        /// 构造方法，创建只有菜单名称的菜单
        /// </summary>
        /// <param name="menuName">菜单名称</param>
        public Menu(String menuName)
        {
            this.MenuName = menuName;
        }
        /// <summary>
        /// 构造方法，创建带有菜单名称和菜单图标的菜单
        /// </summary>
        /// <param name="menuName"></param>
        /// <param name="menuIco"></param>
        public Menu(String menuName, String menuIco)
        {
            this.MenuName = menuName;
            this.MenuIco = menuIco;
        }
        /// <summary>
        /// 构造方法，创建带有菜单名称和菜单顺序的菜单
        /// </summary>
        /// <param name="menuName"></param>
        /// <param name="index"></param>
        public Menu(String menuName, int index)
        {
            this.MenuName = menuName;
            this._index = index > 0 ? index : 0; ;
        }
        /// <summary>
        /// 构造方法，创建带有菜单名称、菜单图标和菜单顺序的菜单
        /// </summary>
        /// <param name="menuName"></param>
        /// <param name="menuIco"></param>
        /// <param name="index"></param>
        public Menu(String menuName, String menuIco, int index)
        {
            this.MenuName = menuName;
            this.MenuIco = menuIco;
            this._index = index > 0 ? index : 0; ;
        }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public String MenuName { get; set; }
        /// <summary>
        /// 菜单图片
        /// </summary>
        public string MenuIco { get; set; }
        /// <summary>
        /// 菜单顺序，最小为0，0表示默认排在末尾
        /// </summary>
        public int Index
        {
            get
            {
                return this._index > 0 ? this._index : 0;
            }
            set
            {
                this._index = value > 0 ? value : 0;
            }
        }
    }
}
