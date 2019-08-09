using NoR2252.Models.Graphics;
using UnityEngine;
namespace NoR2252.Models {
    [System.Serializable]
    public class NoR2252Data : Eccentric.Utils.TSingletonMonoBehavior<NoR2252Data> {
        public int [ ] Points = new int [ ] { 100, 80, 60, 20, 0 };
        public float [ ] TimeGrade = new float [ ] { 0.05f, 0.075f, 0.1f, 0.125f, 0.15f };
        public float SlideChildGradeFactor = 1.5f;
    }
}
