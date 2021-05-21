using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Common.Settings
{
    public class App<TAppSettings> where TAppSettings : new()
    {
        private IConfigurationRoot _config;

        public IConfiguration Configuration
        {
            get
            {
                if (_config != null)
                {
                    return _config;
                }

                var builder = new ConfigurationBuilder()
                  .SetBasePath(AppContext.BaseDirectory)
                  .AddJsonFile("appsettings.json",
                    optional: true,
                    reloadOnChange: true);

                return _config = builder.Build();
            }
        }

        private TAppSettings _appSettings;

        public TAppSettings Settings
        {
            get
            {
                if (_appSettings == null)
                {
                    _appSettings = new TAppSettings();
                    Configuration.Bind(_appSettings);
                }
                return _appSettings;
            }
        }
    }
}
