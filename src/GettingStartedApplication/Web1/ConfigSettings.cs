﻿

namespace Web1
{
    using System.Fabric;
    using System.Fabric.Description;
    public class ConfigSettings
    {
        public ConfigSettings(StatelessServiceContext context)
        {
            context.CodePackageActivationContext.ConfigurationPackageModifiedEvent += this.CodePackageActivationContext_ConfigurationPackageModifiedEvent;
            this.UpdateConfigSettings(context.CodePackageActivationContext.GetConfigurationPackageObject("Config").Settings);
        }

        //public string GuestExeBackendServiceName { get; private set; }

        //public string StatefulBackendServiceName { get; private set; }

        //public string StatelessBackendServiceName { get; private set; }

        //public string ActorBackendServiceName { get; private set; }

        //public int ReverseProxyPort { get; private set; }


        private void CodePackageActivationContext_ConfigurationPackageModifiedEvent(object sender, PackageModifiedEventArgs<ConfigurationPackage> e)
        {
            this.UpdateConfigSettings(e.NewPackage.Settings);
        }

        private void UpdateConfigSettings(ConfigurationSettings settings)
        {
            //ConfigurationSection section = settings.Sections["MyConfigSection"];
            //this.GuestExeBackendServiceName = section.Parameters["GuestExeBackendServiceName"].Value;
            //this.StatefulBackendServiceName = section.Parameters["StatefulBackendServiceName"].Value;
            //this.StatelessBackendServiceName = section.Parameters["StatelessBackendServiceName"].Value;
            //this.ActorBackendServiceName = section.Parameters["ActorBackendServiceName"].Value;
            //this.ReverseProxyPort = int.Parse(section.Parameters["ReverseProxyPort"].Value);

            var appInsights = settings.Sections["ApplicationInsights"];
            var telemetryConfig = Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active;
            telemetryConfig.InstrumentationKey = appInsights.Parameters["InstrumentationKey"].Value;
        }
    }
}
