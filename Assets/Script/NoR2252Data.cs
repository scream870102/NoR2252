using UnityEngine;
namespace NoR2252.Models {
    [System.Serializable]
    public class NoR2252Data : Eccentric.Utils.TSingletonMonoBehavior<NoR2252Data> {
        public int [ ] Points = new int [ ] { 100, 80, 60, 20, 0 };
        public float [ ] TimeGrade = new float [ ] { 0.033f, 0.055f, 0.077f, 0.12f, 0.15f };
        public NoteColorAndSprite [ ] ColorsAndSprites = new NoteColorAndSprite [System.Enum.GetNames (typeof (ENoteType)).Length];
        // public Color [ ] NoteColor = new Color [System.Enum.GetNames (typeof (ENoteType)).Length];
        // public Sprite [ ] NoteSprite = new Sprite [System.Enum.GetNames (typeof (ENoteType)).Length];
        // public Sprite [ ] NoteOutLineSprite = new Sprite [System.Enum.GetNames (typeof (ENoteType)).Length];
        // public Color TapOutLineColor;
        // public Color TapPtcColor;
        // public Color FlickOutLineColor;
        // public Color FlickPtcColor;
        // public Color SlideHeadPtcColor;
        // public Color SlideChildPtcColor;
        // public Color LineColor;
        // public Color HoldPtcColor;
        [System.Serializable]
        public class NoteColorAndSprite {
            public Sprite MainS;
            public Sprite LineS;
            public Sprite OutlineS;
            public Color MainC;
            public Color OutLineC;
            public Color LineC;
            public Color LineBGC;
            public Color PtcC;
        }

    }
}
