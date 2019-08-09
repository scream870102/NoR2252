using Eccentric;
using NG = NoR2252.Models.Graphics;
using NoR2252.Models;
using EU = Eccentric.Utils;
using NoR2252.View.Action;

using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class SlideChildNoteView : TapNoteView {
        readonly float LINE_WIDTH=0.5f;
        float timeOffset;
        float lineHeight;
        // SpriteRenderer lineRenderer;
        // SpriteRenderer bgLineRenderer;
        // Animator lineAnimator;
        public SlideChildNoteView (GameNote note, NoteViewRef VRef):
            base (note, VRef) {
                // this.lineRenderer = lineRenderer;
                // this.bgLineRenderer = bgLineRenderer;
                // this.lineAnimator = lineAnimator;
            }
        public override void Render ( ) {
            if (IsRendering) {
                if (NoR2252Application.VideoTime <= Note.Info.startTime) {
                    float lineH = lineHeight * (Math.InverseProbability ((Note.Info.startTime - NoR2252Application.VideoTime) / NoR2252Application.PreLoad));
                    VRef.bgLineRenderer.size = new Vector2 (LINE_WIDTH, lineH);
                }
                float offset=Note.Info.endTime-NoR2252Application.VideoTime;
                if(offset<=timeOffset&&offset>=0f){
                    SetLine(offset);
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

        void SetLine (float time) {
            Vector2 size = new Vector2 (LINE_WIDTH, 0f);
            size.y = lineHeight * (time / timeOffset);
            VRef.lineRenderer.size = size;
        }
        public override void OnSpawn ( ) {
            base.OnSpawn ( );
            VRef.lineRenderer.enabled = true;
            VRef.bgLineRenderer.enabled = true;
            VRef.lineAnimator.SetTrigger ("FadeIn");
            if (Note.Controller.SlidePos.ContainsKey (Note.Info.id)) {
                //calculate the degree from this note to prev note
                Vector2 tmp = Note.Controller.SlidePos [Note.Info.id] - Note.transform.position;
                tmp.Normalize ( );
                float degree = Mathf.Atan2 (tmp.y, tmp.x) * Mathf.Rad2Deg;
                Vector3 oEuler = new Vector3 (0f, 0f, degree);
                oEuler.z = degree - 90f;
                Note.transform.rotation = Quaternion.Euler (oEuler);
                timeOffset = Note.Info.endTime-Note.Controller.SlideEndTime [Note.Info.id];
                //set the slide line position
                Vector2 tp = Note.Controller.SlidePos [Note.Info.id] - Note.transform.position;
                Vector2 size = new Vector2 (LINE_WIDTH, Mathf.Ceil (tp.magnitude));
                VRef.lineRenderer.size = size;
                VRef.bgLineRenderer.size = size;
                lineHeight = VRef.bgLineRenderer.size.y;
            }
        }
    }
}
