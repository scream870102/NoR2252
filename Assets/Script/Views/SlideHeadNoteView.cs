using Eccentric;
using NG = NoR2252.Models.Graphics;
using NoR2252.Models;
using EU = Eccentric.Utils;
using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class SlideHeadNoteView : TapNoteView {
        public SlideHeadNoteView (GameNote note, NG.RefObject refs) : base (note, refs) {
            cAs = NoR2252Data.Instance.ColorsAndSprites [(int) ENoteType.SLIDE_HEAD];
            ParticleSystem.MainModule ptcMain = refs.Ptc.main;
            ptcMain.startColor = cAs.PtcC;
            refs.Main.sprite = cAs.MainS;
            refs.Main.color = cAs.MainC;
            refs.OutLine.sprite = cAs.OutlineS;
            refs.OutLine.color = cAs.OutLineC;

        }
    }
}
