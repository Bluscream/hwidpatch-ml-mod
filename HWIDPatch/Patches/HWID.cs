using System;
using System.Linq;
using UnityEngine;

namespace UnitySpoofer {
    static class HWIDPatch {
        internal const int HWID_LENGTH = 40;
        internal static string sessionHWID;
        public enum SpoofMode {
            None,
            Custom,
            Empty,
            Random_per_session,
            Random
        }
        internal static string GenerateHWID() {
            var random = new System.Random(Environment.TickCount);
            var bytes = new byte[HWID_LENGTH / 2];
            random.NextBytes(bytes);
            return string.Join("", bytes.Select(it => it.ToString("x2")));
        }
        internal static void Print() {
            Mod.Log("====== HWIDs ======");
            Mod.Log($"Current:\t{SystemInfo.deviceUniqueIdentifier}");
            Mod.Log($"Custom:\t{Preferences.spoofedHWID.Value}");
            Mod.Log($"Session:\t{sessionHWID}");
            Mod.Log("====================");
        }
        [HarmonyLib.HarmonyPatch(typeof(SystemInfo), nameof(SystemInfo.deviceUniqueIdentifier), methodType: HarmonyLib.MethodType.Getter)]
        static void Postfix(ref string __result) {
            Mod.Log($"deviceUniqueIdentifier getter called: {__result}");
            switch (Preferences.spoofHWIDMode.Value) {
                case SpoofMode.Empty: __result = ""; break;
                case SpoofMode.Custom: __result = Preferences.spoofedHWID?.Value ?? ""; break;
                case SpoofMode.Random_per_session:
                    if (string.IsNullOrEmpty(sessionHWID)) {
                        __result = sessionHWID = GenerateHWID();
                        Mod.Log($"Generated new session HWID: {sessionHWID}");
                    }
                    break;
                case SpoofMode.Random:
                    __result = GenerateHWID();
                    break;
            }
            Print();
        }
    }
}
