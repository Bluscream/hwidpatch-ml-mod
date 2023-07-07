using MelonLoader;
using UnityEngine;

namespace UnitySpoofer {
    internal static class Preferences {
        internal static MelonPreferences_Entry<bool> EnableLogging { get; private set; }
        internal static MelonPreferences_Entry<HWIDPatch.SpoofMode> spoofHWIDMode { get; private set; } = new MelonPreferences_Entry<HWIDPatch.SpoofMode>() { Value = HWIDPatch.SpoofMode.None };
        internal static MelonPreferences_Entry<string> spoofedHWID { get; private set; }
        internal static MelonPreferences_Entry<bool> spoofGenuine { get; private set; }
        internal static MelonPreferences_Entry<bool> spoofGenuineCheckAvailable { get; private set; }

        public static void Init() {
            var category = MelonPreferences.CreateCategory(baseCategoryName, "Unity Spoofer");
            EnableLogging = category.CreateEntry(nameof(EnableLogging), true, "Enable Logging", "Wether to enable logging to MelonLoader's Console", is_hidden: true);
            spoofHWIDMode = category.CreateEntry(nameof(spoofHWIDMode), HWIDPatch.SpoofMode.Random_per_session, "Hardware ID Spoof Mode", "Wether to spoof your unique device indentifier");
            spoofedHWID = category.CreateEntry(nameof(spoofedHWID), "", "Spoofed Hardware ID", "The unique device identifier to spoof");
            if (spoofedHWID.Value.Length != HWIDPatch.HWID_LENGTH) {
                spoofedHWID.Value = HWIDPatch.GenerateHWID();
                category.SaveToFile(false);
                Mod.Log($"Generated and saved new HWID: {spoofedHWID.Value}");
            }
            spoofGenuine = category.CreateEntry(nameof(spoofGenuine), true, "Spoof Application.genuine", "Wether to spoof UnityEngine.Application.genuine");
            spoofGenuineCheckAvailable = category.CreateEntry(nameof(spoofGenuineCheckAvailable), true, "Spoof Application.genuineCheckAvailable", "Wether to spoof UnityEngine.Application.genuineCheckAvailable");
            MelonPreferences.Save();
        }
        private static readonly string baseCategoryName = "UnitySpoofer";
    }
}
