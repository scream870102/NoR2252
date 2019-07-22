using NoR2252.Models;

using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class SlideChildNoteView : NoteView {
        LineRenderer lineRenderer;
        public SlideChildNoteView (GameNote note, SpriteRenderer renderer, Animation animation, LineRenderer lineRenderer) : base (note, renderer, animation) {
            color = NoR2252Data.Instance.NoteColor [(int) ENoteType.SLIDE_CHILD];
            this.lineRenderer = lineRenderer;
            lineRenderer.enabled = false;
        }

        public override void OnSpawn ( ) {
            base.OnSpawn ( );
            if (Note.Controller.SlidePos.ContainsKey (Note.Info.id)) {
                lineRenderer.SetPosition (0, Note.transform.position);
                lineRenderer.SetPosition (1, Note.Controller.SlidePos [Note.Info.id]);
            }
            lineRenderer.enabled = true;

        }

        public override void OnClear (ENoteGrade grade) {
            base.OnClear (grade);
            lineRenderer.enabled = false;
        }

    }
}
