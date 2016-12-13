using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace NetCoreManager.Component.Tools.Service
{
    /// <summary>
    /// 读取配置文件
    /// </summary>
    public class ApplicationConfigurationService
    {
        private readonly IOptions<ApplicationConfiguration> _appConfiguration;

        public ApplicationConfigurationService(IOptions<ApplicationConfiguration> appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public ApplicationConfiguration AppConfigurations
        {
            get
            {
                return _appConfiguration.Value;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
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
