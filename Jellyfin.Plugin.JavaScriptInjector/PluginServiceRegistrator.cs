using Jellyfin.Plugin.JavaScriptInjector.Services;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Jellyfin.Plugin.JavaScriptInjector
{
    public class PluginServiceRegistrator : IPluginServiceRegistrator
    {
        public void RegisterServices(IServiceCollection serviceCollection, IServerApplicationHost applicationHost)
        {
            serviceCollection.AddSingleton<StartupService>();
            serviceCollection.AddSingleton<IJavaScriptRegistrationService, JavaScriptRegistrationService>();

            // Request-time injection (Jellyfin 10.11 & 12): injects the loader
            // <script> into web index.html on every request via ASP.NET middleware.
            // Primary injection path -- see ScriptInjectionStartupFilter for details.
            // Kill-switchable via DisableScriptInjectionMiddleware. HAC fork: switched
            // off means nothing is injected; there is no on-disk fallback.
            serviceCollection.AddSingleton<IStartupFilter, ScriptInjectionStartupFilter>();
        }
    }
}
