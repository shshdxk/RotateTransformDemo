using System;

namespace Rep
{
    /// <summary>
    /// 清洁度类
    /// </summary>
    public class Clean
    {
        #region 成员变量
        /// <summary>
        /// 作为锁使用，没有特别意义
        /// </summary>
        private static Object o = new Object();
        private volatile static Clean _clean = null;
        /// <summary>
        /// 清洁值
        /// </summary>
        public int CleanPoint { get; set; }
        /// <summary>
        /// 清洁值上限
        /// </summary>
        public int MaxCleanPoint { get; set; }
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        private Clean() { }
        /// <summary>
        /// 双重锁单例模式
        /// </summary>
        /// <returns>EatDrink对象</returns>
        public static Clean getInstance()
        {
            if (_clean == null)
            {
                lock (o)
                {
                    if (_clean == null)
                    {
                        _clean = new Clean();
                    }
                }
            }
            return _clean;
        }
        #endregion
    }
}
