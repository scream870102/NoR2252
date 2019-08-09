using Eccentric;

using NoR2252.Models;

using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class HoldNoteView : TapNoteView {
        readonly int LINE_HEIGHT_FACTOR = 1;
        readonly float LINE_WIDTH=0.5f;
        int lineHeight = 0;
        //SpriteRenderer lineRenderer;
        //SpriteRenderer bgLineRenderer;
        //Animator lineAnimator;
        public HoldNoteView (GameNote note, NoteViewRef VRef):
            base (note,VRef) {
                //this.lineRenderer = lineRenderer;
                //this.bgLineRenderer = bgLineRenderer;
                //this.lineAnimator = lineAnimator;
                VRef.lineRenderer.size = new Vector2 (LINE_WIDTH, 0f);
                lineHeight = Mathf.CeilToInt (Note.Info.Duration) * LINE_HEIGHT_FACTOR;
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
            if (IsRendering) {
                if (NoR2252Application.VideoTime <= Note.Info.startTime) {
                    float lineH = lineHeight * (Math.InverseProbability ((Note.Info.startTime - NoR2252Application.VideoTime)/ NoR2252Application.PreLoad));
                    VRef.bgLineRenderer.size = new Vector2 (LINE_WIDTH, lineH);
                }
                if (NoR2252Application.VideoTime >= Note.Info.startTime && (Note.Strategy as HoldStrategy).IsHolding) {
                    Vector2 size = new Vector2 (LINE_WIDTH, 0f);
                    size.y = lineHeight * Math.InverseProbability ((Note.Info.endTime - NoR2252Application.VideoTime) / Note.Info.Duration);
                    VRef.lineRenderer.size = size;
                    VRef.bgLineRenderer.size=new Vector2(LINE_WIDTH,lineHeight);
                }
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
                VRef.lineAnimator.SetTrigger ("FadeOut");
                VRef.lineRenderer.size = new Vector2 (0f, 0f);
            }
        }
        public override void OnSpawn ( ) {
            base.OnSpawn ( );
            VRef.lineRenderer.enabled=true;
            VRef.bgLineRenderer.enabled=true;
            VRef.lineAnimator.SetTrigger ("FadeIn");
        }

    }
}
