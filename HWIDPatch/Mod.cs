using System;
using System.Linq;
using MelonLoader;
using UnityEngine;

[assembly:MelonInfo(typeof(UnitySpoofer.Mod), "UnitySpoofer", "1.0.0", "Bluscream, knah", "https://github.com/Bluscream/hwidpatch-ml-mod")]
[assembly:MelonGame]

namespace UnitySpoofer
{
    public class Mod : MelonMod
    {
        internal static void Log(object message, LogType type = LogType.Log) {
            if (!Preferences.EnableLogging.Value) return;
            var msg = message.ToString();
            MelonLogger.Msg(msg);
        }

        public override void OnInitializeMelon() {
            Preferences.Init();
        }
    }
}