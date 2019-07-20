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
        public NoteView (GameNote note) {
            this.Note = note;
            renderer = note.gameObject.GetComponent<SpriteRenderer> ( );
            animation = note.gameObject.GetComponent<Animation> ( );
        }
        public virtual void SetNote (GameNote note) {
            this.Note = note;
        }
        public virtual void OnSpawn ( ) { }
        public virtual void OnClear (ENoteGrade grade) { }
        public virtual void Render ( ) { }

    }
}
