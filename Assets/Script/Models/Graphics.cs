using UnityEngine;
namespace NoR2252.Models.Graphics {
    [System.Serializable]
    public class RefObject {
        public Transform MainTf;
        public Transform LineTf;
        public Transform OutLineTf;
        public Transform LineBGTf;
        public Transform PtcTf;
        public Transform MaskTf;
        public SpriteRenderer Main;
        public SpriteRenderer Line;
        public SpriteRenderer OutLine;
        public SpriteRenderer LineBG;
        public ParticleSystem Ptc;
        public SpriteMask Mask;
        protected bool bAllEnable = true;
        public bool IsAllEnable {
            get {
                return bAllEnable;
            }
            set {
                bAllEnable = value;
                Main.enabled = value;
                Line.enabled = value;
                OutLine.enabled = value;
                LineBG.enabled = value;
                if (value == false){
                    Ptc.Stop ( );
                }
                Mask.enabled = value;
            }

        }

    }

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
