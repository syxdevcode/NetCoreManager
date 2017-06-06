using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace NetCoreManager.Domain.Entity
{
    /// <summary>
    /// 单词文件主表
    /// </summary>
    public class WordFile: IAggregateRoot
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
