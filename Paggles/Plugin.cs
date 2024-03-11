using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace Paggles
{
    public sealed class Plugin : IDalamudPlugin
    {
        private IAddonLifecycle AddonLifecycle { get; init; }

        private float? original = null;

        public Plugin(IAddonLifecycle addonLifecycle)
        {
            this.AddonLifecycle = addonLifecycle;

            AddonLifecycle.RegisterListener(AddonEvent.PreSetup, "_DTR", ResetPaggles);
            AddonLifecycle.RegisterListener(AddonEvent.PostRequestedUpdate, "_DTR", PagglesTime);
        }

        public void Dispose()
        {
            AddonLifecycle.UnregisterListener(ResetPaggles);
            AddonLifecycle.UnregisterListener(PagglesTime);
        }

        public void ResetPaggles(AddonEvent type, AddonArgs args)
        {
            original = null;
        }

        public unsafe void PagglesTime(AddonEvent type, AddonArgs args)
        {
            AtkUnitBase* dtrButtonBase = (AtkUnitBase*)args.Addon;
            AtkComponentButton* dtrTimeButton = dtrButtonBase->GetButtonNodeById(16);
            AtkTextNode *textNode = dtrTimeButton->ButtonTextNode;

            string originalText = textNode->NodeText.ToString();
            originalText = originalText.Replace("a.m.", "aggles");
            originalText = originalText.Replace("p.m.", "paggles");

            textNode->SetText(originalText);
            textNode->ResizeNodeForCurrentText();

            if (original is null)
                original = dtrButtonBase->RootNode->X;

            float diff = dtrButtonBase->RootNode->Width - textNode->AtkResNode.Width;
            dtrButtonBase->RootNode->X = (float)(original + diff);
        }
    }
}
