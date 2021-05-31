using System.Reflection;
using HarmonyLib;
using MusicTimestamp.MainPatch;
using UnityEngine;
using UnityModManagerNet;

namespace MusicTimestamp {
    internal static class Main {
        public static Text text;
        internal static UnityModManager.ModEntry Mod;
        private static Harmony _harmony;
        internal static bool IsEnabled { get; private set; }
        internal static MainSettings Settings { get; private set; }

        private static void Load(UnityModManager.ModEntry modEntry) {
            Mod = modEntry;
            Mod.OnToggle = OnToggle;
            Settings = UnityModManager.ModSettings.Load<MainSettings>(modEntry);
            Mod.OnGUI = Settings.OnGUI;
            Mod.OnSaveGUI = Settings.OnSaveGUI;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            IsEnabled = value;

            if (value) Start();
            else Stop();

            return true;
        }

        private static void Start() {
            _harmony = new Harmony(Mod.Info.Id);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            text = new GameObject().AddComponent<Text>();
            Object.DontDestroyOnLoad(text);
        }

        private static void Stop() {
            _harmony.UnpatchAll(Mod.Info.Id);
            _harmony = null;
            Object.DestroyImmediate(text);
            text = null;
        }
    }
}