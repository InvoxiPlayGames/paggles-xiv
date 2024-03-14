using Dalamud.Interface.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using System.Numerics;

namespace Paggles
{
    public class ConfigWindow : Window
    {
        private Plugin plugin;

        public ConfigWindow(Plugin plugin) : base("Paggles Config",
            ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse |
            ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse |
            ImGuiWindowFlags.AlwaysAutoResize)
        {
            this.plugin = plugin;
        }

        public override void Draw()
        {
            if (ImGui.Checkbox("Use System Locale", ref plugin.config.UseSystemLocale))
            {
                plugin.SaveConfig();
            }
        }
    }
}
