using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreManager.Domain.Entity
{
    /// <summary>
    /// 单词表
    /// </summary>
    public class Words : IAggregateRoot
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 单词文件Id
        /// </summary>
        public Guid WordFileId { get; set; }

        /// <summary>
        /// 英文单词名
        /// </summary>
        public string WordName { get; set; }

        /// <summary>
        /// 音标
        /// </summary>
        public string Phonetic { get; set; }

        /// <summary>
        /// 收录的单词摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
