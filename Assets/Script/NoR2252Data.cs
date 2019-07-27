using NoR2252.Models.Graphics;
namespace NoR2252.Models {
    [System.Serializable]
    public class NoR2252Data : Eccentric.Utils.TSingletonMonoBehavior<NoR2252Data> {
        public int [ ] Points = new int [ ] { 100, 80, 60, 20, 0 };
        public float [ ] TimeGrade = new float [ ] { 0.033f, 0.055f, 0.077f, 0.12f, 0.15f };
        public NoteColorAndSprite [ ] ColorsAndSprites = new NoteColorAndSprite [System.Enum.GetNames (typeof (ENoteType)).Length];
        

    }
}
