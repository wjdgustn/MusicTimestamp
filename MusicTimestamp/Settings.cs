using UnityModManagerNet;

namespace MusicTimestamp {
    public class MainSettings : UnityModManager.ModSettings, IDrawable {
        [Draw("위치 X좌표")] public int PositionX = 0;
        [Draw("위치 Y좌표")] public int PositionY = 120;

        public override void Save(UnityModManager.ModEntry modEntry) {
            UnityModManager.ModSettings.Save(this, modEntry);
        }
        
        public void OnChange() {
            
        }
    }
}