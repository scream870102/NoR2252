using NoR2252.Models;

using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class TapNoteView : NoteView {
        public TapNoteView (GameNote note, NoteViewRef VRef):
            base (note, VRef) {

            }

        public override void Render ( ) {
            if (bClearing) {
                if (timer.IsFinished)
                    OnCleared ( );
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
