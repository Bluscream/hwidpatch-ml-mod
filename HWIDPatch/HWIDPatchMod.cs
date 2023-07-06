using System;
using System.Linq;
using HWIDPatch;
using MelonLoader;
using UnityEngine;
using Harmony;

[assembly:MelonInfo(typeof(HwidPatchMod), "HWIDPatch (Mono)", "2.0.0", "Bluscream, knah", "https://github.com/Bluscream/hwidpatch-ml-mod")]
[assembly:MelonGame]

namespace HWIDPatch
{
    public class HwidPatchMod : MelonMod
    {
        internal static MelonPreferences_Entry<string> spoofedHWID;

        internal static void Log(object message, LogType type = LogType.Log) {
            //if (!Preferences.EnableLogging.Value) return;
            var msg = message.ToString();
            MelonLogger.Msg(msg);
        }

        public override void OnInitializeMelon()
        {
            try
            {
                var category = MelonPreferences.CreateCategory("HWIDPatch", "HWID Patch");
                spoofedHWID = category.CreateEntry("HWID", "", is_hidden: true);

                var newId = spoofedHWID.Value;
                if (newId.Length != SystemInfo.deviceUniqueIdentifier.Length)
                {
                    var random = new System.Random(Environment.TickCount);
                    var bytes = new byte[SystemInfo.deviceUniqueIdentifier.Length / 2];
                    random.NextBytes(bytes);
                    newId = string.Join("", bytes.Select(it => it.ToString("x2")));
                    Log("Generated and saved a new HWID");
                    spoofedHWID.Value = newId;
                    category.SaveToFile(false);
                }

                Log("Patched HWID; below two should match:");
                Log($"Current: {SystemInfo.deviceUniqueIdentifier}");
                Log($"Target:  {newId}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.ToString());
            }
        }
    }
    [HarmonyPatch(typeof(SystemInfo))]
    [HarmonyPatch(nameof(SystemInfo.deviceUniqueIdentifier), PropertyMethod.Getter)]
    [Obsolete]
    static class Patch {
        static string Postfix(string __result) {
            HwidPatchMod.Log($"deviceUniqueIdentifier getter called: {__result}");
            var newId = HwidPatchMod.spoofedHWID.Value;
            if (!string.IsNullOrEmpty(newId)) {
                HwidPatchMod.Log($"Presenting spoofed HWID: {newId}");
                return newId;
            }
            return __result;
        }
    }
}