using UnityEngine;
using UnityModManagerNet;

namespace MusicTimestamp {
    public class MainSettings : UnityModManager.ModSettings, IDrawable {
        [Draw("")] public string TextTemplate = "음악 시간 : <NowMinute>:<NowSecond> / <TotalMinute>:<TotalSecond>";
        #if DEBUG
        [Draw("")] public string NotPlaying = "";
        #endif
        #if RELEASE
        [Draw("")] public string NotPlaying = "플레이 중이 아님";
        #endif
        [Draw("위치 X좌표(Position X)")] public int PositionX = 0;
        [Draw("위치 Y좌표(Position Y)")] public int PositionY = 180;
        [Draw("글자 크기(Font Size)")] public int TextSize = 50;
        [Draw("텍스트 그림자 진하기(1~100)", Min = 0, Max = 100)] public int TextShadow = 50;

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