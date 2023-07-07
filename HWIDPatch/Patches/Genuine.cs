using System;
using Harmony;
using UnityEngine;

namespace UnitySpoofer {
    static class genuineCheckAvailablePatch {
        [HarmonyPatch(typeof(Application))]
        [HarmonyPatch(nameof(Application.genuineCheckAvailable), PropertyMethod.Getter)]
        [Obsolete]
        static class Patch {
            static bool Postfix(bool __result) {
                Mod.Log($"Application.spoofGenuineCheckAvailable getter called: {__result}");
                if (Preferences.spoofGenuineCheckAvailable.Value) {
                    Mod.Log($"Presenting spoofed Application.genuineCheckAvailable");
                    __result = true;
                }
                return __result;
            }
        }
    }
    internal class genuinePatch {
        [HarmonyPatch(typeof(Application))]
        [HarmonyPatch(nameof(Application.genuine), PropertyMethod.Getter)]
        [Obsolete]
        static class Patch {
            static bool Postfix(bool __result) {
                Mod.Log($"Application.genuine getter called: {__result}");
                if (Preferences.spoofGenuine.Value) {
                    Mod.Log($"Presenting spoofed Application.spoofGenuine");
                    __result = true;
                }
                return __result;
            }
        }
    }
}
