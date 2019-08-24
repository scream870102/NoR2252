using Eccentric;

using NoR2252.Models;

using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class HoldNoteView : TapNoteView {
        readonly float LINE_HEIGHT_FACTOR = 1;
        readonly float LINE_BASIC_HEIGHT = 1f;
        readonly float MAX_LINE_HEIGH = 5f;
        readonly float LINE_WIDTH = 0.3f;
        readonly float LINE_OFFSET = 0.5f;
        float lineHeight = 0;
        public HoldNoteView (GameNote note, NoteViewRef VRef):
            base (note, VRef) {
                lineHeight = Mathf.CeilToInt (Note.Info.Duration) * LINE_HEIGHT_FACTOR + LINE_BASIC_HEIGHT - LINE_OFFSET;
                lineHeight = Mathf.Clamp (lineHeight, 1, MAX_LINE_HEIGH);
                //set position
                Vector3 linePos = VRef.lineRenderer.transform.localPosition;
                Vector3 lineRot = VRef.lineRenderer.transform.rotation.eulerAngles;
                //if note on the top part of screen
                if (note.Info.pos.y > NoR2252Application.CurrentSheet.screenSize.y / 2f) {
                    linePos.y = -Mathf.Abs (linePos.y);
                    lineRot.z = 180f;
                }
                //if note on the bottom part of screen
                else {
                    linePos.y = Mathf.Abs (linePos.y);
                    lineRot.z = 0f;
                }
                //set the position
                VRef.bgLineRenderer.transform.localPosition = linePos;
                VRef.bgLineRenderer.transform.rotation = Quaternion.Euler (lineRot);
                VRef.lineRenderer.transform.localPosition = linePos;
                VRef.lineRenderer.transform.rotation = Quaternion.Euler (lineRot);
            }

        public override void Render ( ) {
            if (NoR2252Application.VideoTime <= Note.Info.startTime) {
                float lineH = lineHeight * (Math.InverseProbability ((Note.Info.startTime - NoR2252Application.VideoTime) / NoR2252Application.PreLoad));
                VRef.bgLineRenderer.size = new Vector2 (LINE_WIDTH, lineH);
            }
            if (NoR2252Application.VideoTime >= Note.Info.startTime && (Note.Strategy as HoldStrategy).IsHolding) {
                Vector2 size = new Vector2 (LINE_WIDTH, 0f);
                size.y = lineHeight * Math.InverseProbability ((Note.Info.endTime - NoR2252Application.VideoTime) / Note.Info.Duration);
                VRef.lineRenderer.size = size;
                VRef.bgLineRenderer.size = new Vector2 (LINE_WIDTH, lineHeight);
            }
            if (bClearing) {
                if (timer.IsFinished)
                    OnCleared ( );
            }

        }

        public override void OnClear (ENoteGrade grade) {
            base.OnClear (grade);
            if (grade != ENoteGrade.UNKNOWN && !bClearing) {
                VRef.lineAnimator.SetTrigger ("FadeOut");
                VRef.lineRenderer.size = new Vector2 (0f, 0f);
            }
        }
        public override void OnSpawn ( ) {
            base.OnSpawn ( );
            VRef.lineRenderer.enabled = true;
            VRef.bgLineRenderer.enabled = true;
            VRef.lineAnimator.SetTrigger ("FadeIn");
        }

    }
}
