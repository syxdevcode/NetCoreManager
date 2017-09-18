﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreManager.Component.Tools.Lib
{
    /// <summary>
    /// 常用公共操作
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// 获取类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static Type GetType<T>()
        {
            var type = typeof(T);
            return Nullable.GetUnderlyingType(type) ?? type;
        }
    }
}
