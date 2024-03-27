using System.Reflection;
using HarmonyLib;
using MusicTimestamp.MainPatch;
using System.IO;
using UnityEngine;
using UnityModManagerNet;

namespace MusicTimestamp {
    #if DEBUG
    [EnableReloading]
    #endif
    
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
            
            #if DEBUG
            Mod.OnUnload = Stop;
            #endif
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            IsEnabled = value;

            if (value) Start();
            else Stop(modEntry);

            return true;
        }
        
#if DEBUG
        private static FileSystemWatcher watcher;
        private static void OnDllChanged(object source, FileSystemEventArgs e) {
	        Mod.Logger.Log($"{Mod.Info.Id} mod change detected! Reloading...");
	        watcher.Changed -= OnDllChanged;
	        watcher.Dispose();
	        watcher = null;
	        Mod.GetType().GetMethod("Reload", BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(Mod, null);
        }
#endif

        private static void Start() {
            _harmony = new Harmony(Mod.Info.Id);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            
#if DEBUG
            watcher = new FileSystemWatcher(Mod.Path);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.dll";

            watcher.Changed += OnDllChanged;

            watcher.EnableRaisingEvents = true;
#endif

            text = new GameObject().AddComponent<Text>();
            Object.DontDestroyOnLoad(text);
        }

        private static bool Stop(UnityModManager.ModEntry modEntry) {
            _harmony.UnpatchAll(Mod.Info.Id);
            #if RELEASE
            _harmony = null;
            #endif
            
            Object.DestroyImmediate(text);
            text = null;

            return true;
        }
    }
}