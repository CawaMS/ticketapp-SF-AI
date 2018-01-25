# ticketapp-SF-AI

## Overview
This this a sample Service Fabric application that is configured to use Application Insights for monitoring and diagnostics. This app is intended to generate errors randomly to show how exceptions can be correlated to requests in Application Insights

## How to configure the app to use App Insights

Currently a few steps are required to configure a Service Fabric application to use Application Insights

### Prerequisite
Make sure you have an Application Insights resource provisioned in your Azure Subscription. Follow instructions at [Create an Application Insights resource](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-create-new-resource)

### Configure App Insights IKEY
Follow instructions at [
Using Service Fabric with Application Insights](https://github.com/Azure-Samples/service-fabric-dotnet-getting-started/blob/dev/appinsights/ApplicationInsights.md)

### Install Application Insights NuGet Packages
* Microsoft.ApplicationInsights.ServiceFabric.Native
* Microsoft.ApplicationInsights.AspNetCore
* Microsoft.ApplicationInsights.EventSourceListener
* Microsoft.ApplicationInsights.DependencyCollector
* Microsoft.aspnet.teleemtrycorrelation
* All dependencies for above

### Add Code to initialize telemetry client
A few lines of code is required to initialize the telemetry client for sending data to the right Application Insights

#### Web1 ASP.NET core stateless Service
In Web1.cs

Add the following using statements

``` csharp
using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.EventSourceListener;
```

Add the following code in WebHostBuilder ConfigureServices:

``` csharp
.AddSingleton<ITelemetryInitializer>((serviceProvider) => FabricTelemetryInitializerExtension.CreateFabricTelemetryInitializer(serviceContext))
.AddSingleton<ITelemetryModule>((serviceProvider) => CreateEventSourceTelemetryModule())
```
Add the following method in the Web1 class

``` csharp
private EventSourceTelemetryModule CreateEventSourceTelemetryModule()
        {
            var module = new EventSourceTelemetryModule();
            module.Sources.Add(new EventSourceListeningRequest() { Name = "Microsoft-ServiceFabric-Services", Level = EventLevel.Verbose });
            module.Sources.Add(new EventSourceListeningRequest() { Name = "MyCompany-GettingStartedApplication-WebService", Level = EventLevel.Verbose });
            return module;
        }
```

In ConfigSettings.cs

Add the following using statements
```csharp
using System.Fabric;
using System.Fabric.Description;
```

Create a private method UpdateConfigSettings

``` csharp
private void UpdateConfigSettings(ConfigurationSettings settings)
{

    var appInsights = settings.Sections["ApplicationInsights"];
    var telemetryConfig = Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active;
    telemetryConfig.InstrumentationKey = appInsights.Parameters["InstrumentationKey"].Value;
}

```

Call the method in ConfigSettings constructor

``` csharp
public ConfigSettings(StatelessServiceContext context)
      {
          context.CodePackageActivationContext.ConfigurationPackageModifiedEvent += this.CodePackageActivationContext_ConfigurationPackageModifiedEvent;
          this.UpdateConfigSettings(context.CodePackageActivationContext.GetConfigurationPackageObject("Config").Settings);
      }
```

#### StatelessBackendService

In StatelessBackendService.cs
Add the following using statements

```csharp
using Microsoft.ApplicationInsights.Extensibility;
   using Microsoft.ApplicationInsights;
   using Microsoft.ApplicationInsights.ServiceFabric;
   using Microsoft.ServiceFabric.Services.Remoting.V1.FabricTransport.Runtime;
   using Microsoft.ApplicationInsights.ServiceFabric.Remoting.Activities;
```

Add the following code to the constructor of StatelessBackendService

```csharp
public StatelessBackendService(StatelessServiceContext context)
            : base(context)
        {
            var telemetryConfig = TelemetryConfiguration.Active;

            // Replace the fabric telemetry initializer, if there is one, with one that has the rich context
            for (int i = 0; i < telemetryConfig.TelemetryInitializers.Count; i++)
            {
                if (telemetryConfig.TelemetryInitializers[i] is FabricTelemetryInitializer)
                {
                    telemetryConfig.TelemetryInitializers[i] = FabricTelemetryInitializerExtension.CreateFabricTelemetryInitializer(context);
                    break;
                }
            }

            var config = context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            var appInsights = config.Settings.Sections["ApplicationInsights"];
            telemetryConfig.InstrumentationKey = appInsights.Parameters["InstrumentationKey"].Value;

            this.telemetryClient = new TelemetryClient(telemetryConfig);
        }
```





## How to run the app

//TODO

### Debug the app locally

//TODO

### Deploy to Azure

//TODO

## E2E trace viewer in Application Insights

//TODO
