using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Memory;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace Paggles
{
    public sealed class Plugin : IDalamudPlugin
    {
        private IAddonLifecycle AddonLifecycle { get; init; }

        public Plugin(IAddonLifecycle addonLifecycle)
        {
            this.AddonLifecycle = addonLifecycle;
            AddonLifecycle.RegisterListener(AddonEvent.PreRequestedUpdate, "_DTR", PagglesTime);
        }

        public void Dispose()
        {
            AddonLifecycle.UnregisterListener(PagglesTime);
        }

        public unsafe void PagglesTime(AddonEvent type, AddonArgs args)
        {
            if (args is not AddonRequestedUpdateArgs updateArgs) return;

            const int stringArrayNum = 67;
            const int stringNum = 2;

            StringArrayData* strArray = *(StringArrayData**) (updateArgs.StringArrayData + (8 * stringArrayNum));
            string originalText = MemoryHelper.ReadStringNullTerminated((nint) strArray->ManagedStringArray[stringNum]);
            originalText = originalText.Replace("a.m.", "aggles");
            originalText = originalText.Replace("p.m.", "paggles");

            strArray->SetValue(stringNum, originalText, false, true, true);
        }
    }
}
