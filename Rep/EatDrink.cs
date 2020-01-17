using System;

namespace Rep
{
    /// <summary>
    /// 饱食度类
    /// </summary>
    public class EatDrink
    {
        #region 成员变量
        /// <summary>
        /// 作为锁使用，没有特别意义
        /// </summary>
        private static Object o = new Object();
        private volatile static EatDrink _eatDrink = null;
        /// <summary>
        /// 饥饿值
        /// </summary>
        public int EatPoint { get; set; }
        /// <summary>
        /// 饥饿值上限
        /// </summary>
        public int MaxEatPoint { get; set; }
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        private EatDrink() { }
        /// <summary>
        /// 双重锁单例模式
        /// </summary>
        /// <returns>EatDrink对象</returns>
        public static EatDrink getInstance()
        {
            if (_eatDrink == null)
            {
                lock (o)
                {
                    if (_eatDrink == null)
                    {
                        _eatDrink = new EatDrink();
                    }
                }
            }
            return _eatDrink;
        }
        #endregion
    }
}
