using System;
using HarmonyLib;
using UnityEngine;

namespace MusicTimestamp.MainPatch {
    public class Text : MonoBehaviour {
        public static string Content = "";
        
        void OnGUI() {
            if (Main.Settings.TextShadow) {
                GUIStyle shadow = new GUIStyle();
                shadow.fontSize = Main.Settings.TextSize;
                shadow.font = RDString.GetFontDataForLanguage(SystemLanguage.Korean).font;
                shadow.normal.textColor = Color.black;
                
                GUI.Label(new Rect(Main.Settings.PositionX + 12, Main.Settings.PositionY - 8, Screen.width, Screen.height), Content, shadow);
            }

            GUIStyle style = new GUIStyle();
            style.fontSize = Main.Settings.TextSize;
            style.font = RDString.GetFontDataForLanguage(RDString.language).font;
            style.normal.textColor = Color.white;
    
            GUI.Label(new Rect(Main.Settings.PositionX + 10, Main.Settings.PositionY - 10, Screen.width, Screen.height), Content, style);
        }
    }

    [HarmonyPatch(typeof(scrController), "PlayerControl_Update")]
    
    internal static class ChangeText {
        private static void Prefix(scrController __instance) {
            if (!scrController.instance.paused && scrConductor.instance.isGameWorld) {
                if (!scrConductor.instance.song.clip) return;
                
                TimeSpan nowt = TimeSpan.FromSeconds(scrConductor.instance.song.time);
                TimeSpan tott = TimeSpan.FromSeconds(scrConductor.instance.song.clip.length);
                Text.Content = Main.Settings.TextTemplate
                    .Replace("<NowMinute>", nowt.Minutes.ToString())
                    .Replace("<NowSecond>", nowt.Seconds.ToString("00"))
                    .Replace("<TotalMinute>", tott.Minutes.ToString())
                    .Replace("<TotalSecond>", tott.Seconds.ToString("00"));
            }
            else {
                Text.Content = Main.Settings.NotPlaying;
            }
        }
    }
    
    [HarmonyPatch(typeof(scrController), "FailAction")]

    internal static class ChangeTextOnFail {
        private static void Prefix(scrController __instance) {
            Text.Content = Main.Settings.NotPlaying;
        }
    }
}