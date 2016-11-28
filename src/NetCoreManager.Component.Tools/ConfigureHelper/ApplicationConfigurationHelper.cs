using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NetCoreManager.Component.Tools.ConfigureHelper.ConfigureModel;

namespace NetCoreManager.Component.Tools.ConfigureHelper
{
    public class ApplicationConfigurationHelper
    {
        private readonly IOptions<ApplicationConfiguration> _appConfiguration;

        public ApplicationConfigurationHelper(IOptions<ApplicationConfiguration> appConfiguration)
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
}
