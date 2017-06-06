using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreManager.Domain.Entity
{
    /// <summary>
    /// 有道翻译实体类
    /// </summary>
    public class YouDaoFanYi : IAggregateRoot
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 单词Id
        /// </summary>
        public Guid WordId { get; set; }

        /// <summary>
        /// 单词注释
        /// </summary>
        public string Annotation { get; set; }

        /// <summary>
        /// 输入发音地址
        /// </summary>
        public string SpeakUrl { get; set; }

        /// <summary>
        /// 翻译发音地址
        /// </summary>
        public string TSpeakUrl { get; set; }
    }
}
