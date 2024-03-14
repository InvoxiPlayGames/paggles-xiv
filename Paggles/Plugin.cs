using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Memory;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System;
using Dalamud.Configuration;
using Newtonsoft.Json;
using Dalamud.Interface.Windowing;
using System.Globalization;

namespace Paggles
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        [JsonProperty] public bool UseSystemLocale = true;
    }

    public sealed class Plugin : IDalamudPlugin
    {
        private IAddonLifecycle AddonLifecycle { get; init; }
        private DalamudPluginInterface PluginInterface { get; init; }

        public WindowSystem WindowSystem = new("PagglesPlugin");

        public Configuration config { get; init; }
        private ConfigWindow ConfigWindow { get; init; }

        public Plugin(DalamudPluginInterface pluginInterface,
            IAddonLifecycle addonLifecycle)
        {
            this.AddonLifecycle = addonLifecycle;
            this.PluginInterface = pluginInterface;
            AddonLifecycle.RegisterListener(AddonEvent.PreRequestedUpdate, "_DTR", PagglesTime);
            this.config = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.ConfigWindow = new ConfigWindow(this);
            this.WindowSystem.AddWindow(this.ConfigWindow);
            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void SaveConfig()
        {
            this.PluginInterface.SavePluginConfig(this.config);
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            AddonLifecycle.UnregisterListener(PagglesTime);
        }
        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }

        public unsafe void PagglesTime(AddonEvent type, AddonArgs args)
        {
            if (args is not AddonRequestedUpdateArgs updateArgs) return;

            const int stringArrayNum = 67;
            const int stringNum = 2;

            StringArrayData* strArray = *(StringArrayData**) (updateArgs.StringArrayData + (8 * stringArrayNum));
            string originalText = MemoryHelper.ReadStringNullTerminated((nint) strArray->ManagedStringArray[stringNum]);
            if (config.UseSystemLocale)
            {
                originalText = originalText.Replace("a.m.", CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator);
                originalText = originalText.Replace("p.m.", CultureInfo.CurrentCulture.DateTimeFormat.PMDesignator);
            } else
            {
                originalText = originalText.Replace("a.m.", "aggles");
                originalText = originalText.Replace("p.m.", "paggles");
            }

            strArray->SetValue(stringNum, originalText, false, true, true);
        }
    }
}
