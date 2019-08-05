using NoR2252.Models.Graphics;
using UnityEngine;
namespace NoR2252.Models {
    [System.Serializable]
    public class NoR2252Data : Eccentric.Utils.TSingletonMonoBehavior<NoR2252Data> {
        public int [ ] Points = new int [ ] { 100, 80, 60, 20, 0 };
        public float [ ] TimeGrade = new float [ ] { 0.1f, 0.2f, 0.3f, 0.35f, 0.4f };
        public NoteColorAndSprite [ ] ColorsAndSprites = new NoteColorAndSprite [System.Enum.GetNames (typeof (ENoteType)).Length];
        public float SlideChildGradeFactor = 1.5f;
    }
}
