using UnityEngine;
using UnityModManagerNet;

namespace MusicTimestamp {
    public class MainSettings : UnityModManager.ModSettings, IDrawable {
        [Draw("위치 X좌표(Position X)")] public int PositionX = 0;
        [Draw("위치 Y좌표(Position Y)")] public int PositionY = 120;
        [Draw("글자 크기(Font Size)")] public int TextSize = 50;
        [Draw("")] public string TextTemplate = "음악 시간 : <NowMinute>:<NowSecond> / <TotalMinute>:<TotalSecond>";
        [Draw("")] public string NotPlaying = "플레이 중이 아님";

        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(this, modEntry);
        }
        
        public void OnChange() {
            
        }

        public void OnGUI(UnityModManager.ModEntry modEntry) {
            GUILayout.Label("플레이 중 텍스트 형식 (Playing text)");
            TextTemplate = GUILayout.TextField(TextTemplate);

            GUILayout.Label("플레이중이지 않을 때 텍스트 (Not playing text)");
            NotPlaying = GUILayout.TextField(NotPlaying);
            
            Main.Settings.Draw(modEntry);
        }
        
        public void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            Main.Settings.Save(modEntry);
        }
    }
}