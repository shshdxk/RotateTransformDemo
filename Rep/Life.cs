using System;

namespace Rep
{
    public class Life
    {
        #region 成员变量
        /// <summary>
        /// 作为锁使用，没有特别意义
        /// </summary>
        private static Object o = new Object();
        private volatile static Life _life = null;
        /// <summary>
        /// 饱食度
        /// </summary>
        public EatDrink EatDrink { get; set; }
        /// <summary>
        /// 清洁度
        /// </summary>
        public Clean Clean { get; set; }
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        private Life()
        {
            EatDrink = EatDrink.getInstance();
            Clean = Clean.getInstance();
        }
        /// <summary>
        /// 双重锁单例模式
        /// </summary>
        /// <returns>Life对象</returns>
        public static Life getInstance()
        {
            if (_life == null)
            {
                lock (o)
                {
                    if (_life == null)
                    {
                        _life = new Life();
                    }
                }
            }
            return _life;
        }
    }
        #endregion
}