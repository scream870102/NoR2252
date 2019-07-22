using NoR2252.Models;

using UnityEngine;
namespace NoR2252.View.Note {
    [RequireComponent (typeof (Animation))]
    [RequireComponent (typeof (SpriteRenderer))]
    [System.Serializable]
    public class NoteView {
        public GameNote Note;
        protected SpriteRenderer renderer;
        protected Animation animation;
        protected Color color;
        bool bRendering = false;
        Vector3 initScale = new Vector3 ( );
        public NoteView (GameNote note) {
            this.Note = note;
            renderer = note.gameObject.GetComponent<SpriteRenderer> ( );
            renderer.enabled = false;
            animation = note.gameObject.GetComponent<Animation> ( );
            initScale = note.transform.localScale;
        }
        public virtual void SetNote (GameNote note) {
            this.Note = note;
        }
        public virtual void OnSpawn ( ) {
            renderer.color = color;
            renderer.enabled = true;
            bRendering = true;
        }
        public virtual void OnClear (ENoteGrade grade) {
            if (grade == ENoteGrade.UNKNOWN) return;
            renderer.enabled = false;
            bRendering = false;
            Note.transform.localScale = initScale;
        }
        public virtual void Render ( ) {
            if (bRendering) {
                Vector3 newScale = new Vector3 (Note.transform.localScale.x - Time.deltaTime * .01f, Note.transform.localScale.y - Time.deltaTime * .01f, initScale.z);
                Note.transform.localScale = newScale;
                if (Note.Info.endTime - GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.MISS] <= NoR2252Application.VideoTime)
                    renderer.color = Color.green;
                if (Note.Info.endTime - GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.GOOD] <= NoR2252Application.VideoTime) {
                    renderer.color = Color.white;
                }
                if (Note.Info.endTime + GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.GOOD] <= NoR2252Application.VideoTime) {
                    renderer.color = Color.red;
                }
            }
        }

    }
}
