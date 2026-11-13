#nullable enable
using Newtonsoft.Json.Linq;

namespace Jellyfin.Plugin.JavaScriptInjector
{
    /// <summary>
    /// The interface other plugins use, through reflection, to register scripts.
    /// HAC fork: turned off. Only scripts an admin enters on the config page are
    /// served, so another plugin cannot add JavaScript that runs in every browser.
    /// The methods stay so callers find them; they never change anything.
    /// </summary>
    public static class PluginInterface
    {
        /// <summary>
        /// Refuses every registration.
        /// </summary>
        /// <param name="payload">Ignored.</param>
        /// <returns>Always false.</returns>
        public static bool RegisterScript(JObject payload)
        {
            _ = payload;
            return false;
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="scriptId">Ignored.</param>
        /// <returns>Always false.</returns>
        public static bool UnregisterScript(string scriptId)
        {
            _ = scriptId;
            return false;
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="pluginId">Ignored.</param>
        /// <returns>Always 0.</returns>
        public static int UnregisterAllScriptsFromPlugin(string pluginId)
        {
            _ = pluginId;
            return 0;
        }
    }
}
