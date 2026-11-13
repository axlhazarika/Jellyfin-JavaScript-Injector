using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Jellyfin.Plugin.JavaScriptInjector.Configuration;
using Jellyfin.Plugin.JavaScriptInjector.Helpers;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.JavaScriptInjector
{
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        private readonly IApplicationPaths _appPaths;
        private readonly ILogger<Plugin> _logger;

        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, ILogger<Plugin> logger, IServiceProvider serviceProvider)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
            _appPaths = applicationPaths;
            _logger = logger;
            ServiceProvider = serviceProvider;
        }

        public static Plugin? Instance { get; private set; }

        public IServiceProvider ServiceProvider { get; }

        public override string Name => "JavaScript Injector (HAC)";

        // HAC fork: its own id, so no catalogue entry for the upstream plugin can
        // ever update or replace it.
        public override Guid Id => Guid.Parse("559ef066-e27f-4961-bd06-8ea1bdfc924c");

        public string IndexHtmlPath => Path.Combine(_appPaths.WebPath, "index.html");

        public override void OnUninstalling()
        {
            try
            {
                var indexPath = IndexHtmlPath;
                if (!File.Exists(indexPath))
                {
                    _logger.LogError("Could not find index.html at path: {Path}", indexPath);
                    return;
                }

                var content = File.ReadAllText(indexPath);
                var newRegex = new Regex($"{JavascriptHelper.StartComment}[\\s\\S]*?{JavascriptHelper.EndComment}\\s*", RegexOptions.Multiline);
                if (newRegex.IsMatch(content))
                {
                    content = newRegex.Replace(content, string.Empty);
                    File.WriteAllText(indexPath, content);
                    _logger.LogInformation("Successfully removed the JavaScript Injector script from index.html during uninstall.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to remove script from index.html during uninstall.");
            }

            base.OnUninstalling();
        }

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = Name,
                    DisplayName = "JS Injector",
                    EnableInMainMenu = true,
                    EmbeddedResourcePath = $"{GetType().Namespace}.Configuration.configPage.html",
                    MenuIcon = "code",
                }
            };
        }
    }
}
