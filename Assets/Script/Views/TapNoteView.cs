using Eccentric;
using EU = Eccentric.Utils;
using NoR2252.Models;
using NG = NoR2252.Models.Graphics;
using NoR2252.View.Action;

using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class TapNoteView : NoteView {
        public TapNoteView (GameNote note, NoteViewRef VRef):
            base (note, VRef) {

            }

        public override void Render ( ) {
            if (IsRendering) {
                // note inside
                //note outside
                //about color
                if (Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.GOOD] <= NoR2252Application.VideoTime) { } else if (Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS] <= NoR2252Application.VideoTime) { } else { }
                if (bClearing) {
                    if (timer.IsFinished)
                        OnCleared ( );
                }
            }

        }

        public override void OnClear (ENoteGrade grade) {
            base.OnClear (grade);
            if (grade != ENoteGrade.UNKNOWN && !bClearing) {
                VRef.animator.SetTrigger ("FadeOut");
                bClearing = true;
                if (grade == ENoteGrade.PERFECT || grade == ENoteGrade.GREAT) { }
            }
        }
        public override void OnSpawn ( ) {
            float speed = 1f / NoR2252Application.PreLoad;
            VRef.animator.speed = speed;
            VRef.animator.SetTrigger ("FadeIn");
            base.OnSpawn ( );
        }

    }
}
