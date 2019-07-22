using UnityEngine;
namespace NoR2252.Models {
    [System.Serializable]
    public class NoR2252Data {
        public float PrepareTime;
        public int [ ] Points = new int [ ] { 100, 80, 60, 20, 0 };
        public float [ ] Time_Grade = new float [ ] { 0.033f, 0.055f, 0.077f, 0.12f, 0.15f };
        public Color [ ] NoteColor = new Color [System.Enum.GetNames (typeof (ENoteType)).Length];

    }
}
