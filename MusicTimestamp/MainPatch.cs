using System;
using System.CodeDom;
using System.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityModManagerNet;

namespace MusicTimestamp.MainPatch {
    public class Text : MonoBehaviour {
        public static string Content = "";
        
        void OnGUI() {
            GUIStyle style = GUI.skin.GetStyle("MusicTimestamp_text");
            style.fontSize = Main.Settings.TextSize;
            style.font = RDString.GetFontDataForLanguage(RDString.language).font;
            style.normal.textColor = Color.white;
    
            GUI.Label(new Rect(Main.Settings.PositionX + 10, Main.Settings.PositionY - 10, Screen.width, Screen.height), Content, "MusicTimestamp_text");
        }
    }

    [HarmonyPatch(typeof(scrController), "PlayerControl_Update")]
    
    internal static class ChangeText {
        private static void Prefix(scrController __instance) {
            if (!scrController.instance.paused && scrConductor.instance.isGameWorld) {
                if (!scrConductor.instance.song.clip) return;
                
                TimeSpan nowt = TimeSpan.FromSeconds(scrConductor.instance.song.time);
                TimeSpan tott = TimeSpan.FromSeconds(scrConductor.instance.song.clip.length);
                Text.Content = $"음악 시간 : {nowt.Minutes}:{nowt.Seconds.ToString("00")} / {tott.Minutes}:{tott.Seconds.ToString("00")}";
            }
            else {
                Text.Content = "";
            }
        }
    }
}