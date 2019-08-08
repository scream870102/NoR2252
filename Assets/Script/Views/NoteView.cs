using EU = Eccentric.Utils;

using NoR2252.Models;
using NG = NoR2252.Models.Graphics;
using UnityEngine;
namespace NoR2252.View.Note {
    [RequireComponent (typeof (Animation))]
    [RequireComponent (typeof (SpriteRenderer))]
    [System.Serializable]
    public class NoteView {
        protected readonly float CLEAR_TIME = .17f;
        public GameNote Note;
        bool bRendering = false;
        protected Vector3 initScale = new Vector3 ( );
        protected bool bClearing = false;
        public bool IsRendering { get { return bRendering; } }
        protected EU.CountdownTimer timer = new EU.CountdownTimer ( );
        protected ResultTextController.TextAndAnim resultText = null;
        public NoteView (GameNote note) {
            this.Note = note;
            initScale = new Vector3 (NoR2252Application.Size, NoR2252Application.Size, 1f);
        }
        public void SetNote (GameNote note) {
            this.Note = note;
        }
        public virtual void OnSpawn ( ) {
            bRendering = true;
            bClearing = false;
        }
        public virtual void OnClear (ENoteGrade grade) {
            if (grade != ENoteGrade.UNKNOWN && !bClearing) {
                resultText = Note.ResultTextController.SetResult (grade, Note.transform.position);
            }
        }

        protected virtual void OnCleared ( ) {
            Note.ResultTextController.Recycle (resultText);
            resultText = null;
            bRendering = false;
            bClearing = false;
            Note.transform.localScale = initScale;
            Note.transform.position = Vector3.zero;
            Note.Recycle ( );
        }
        public virtual void Render ( ) { }

    }
}
