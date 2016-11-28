using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreManager.Component.Tools.ConfigureHelper.ConfigureModel
{
    public class ApplicationConfiguration
    {
        #region 属性成员

        /// <summary>
        /// 用户密码盐
        /// </summary>
        public string PwdSalt { get; set; }

        /// <summary>
        /// 文件上传路径
        /// </summary>
        public string FileUpPath { get; set; }

        /// <summary>
        /// 允许上传的文件格式
        /// </summary>
        public string AttachExtension { get; set; }

        /// <summary>
        /// 图片上传最大值KB
        /// </summary>
        public int AttachImagesize { get; set; }
        #endregion
    }
}
