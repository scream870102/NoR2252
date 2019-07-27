using UnityEngine;
namespace NoR2252.Models.Graphics {
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
        public SpriteRenderer LingBG;
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
                LingBG.enabled = value;
                if (value == false)
                    Ptc.Stop ( );
                Mask.enabled = value;
            }

        }

    }
}
