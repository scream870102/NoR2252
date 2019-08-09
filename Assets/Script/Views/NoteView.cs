using EU = Eccentric.Utils;

using NoR2252.Models;
using NG = NoR2252.Models.Graphics;
using UnityEngine;
namespace NoR2252.View.Note {
    [RequireComponent (typeof (Animation))]
    [RequireComponent (typeof (SpriteRenderer))]
    [System.Serializable]
    public class NoteView {
        protected readonly float CLEAR_TIME = 0.5f;
        public GameNote Note;
        bool bRendering = false;
        protected Vector3 initScale = new Vector3 ( );
        protected bool bClearing = false;
        public bool IsRendering { get { return bRendering; } }
        protected EU.CountdownTimer timer = new EU.CountdownTimer ( );
        protected ResultTextController.TextAndAnim resultText = null;
        protected NoteViewRef VRef;
        public NoteView (GameNote note, NoteViewRef VRef) {
            this.Note = note;
            initScale = new Vector3 (NoR2252Application.Size, NoR2252Application.Size, 1f);
            this.VRef = VRef;
            this.VRef.animator.runtimeAnimatorController=this.VRef.animController;
        }
        public void SetNote (GameNote note) {
            this.Note = note;
        }
        public virtual void OnSpawn ( ) {
            VRef.renderer.enabled = true;
            VRef.animator.enabled=true;
            bRendering = true;
            bClearing = false;
        }
        public virtual void OnClear (ENoteGrade grade) {
            if (grade != ENoteGrade.UNKNOWN && !bClearing) {
                timer.Reset (CLEAR_TIME);
                resultText = Note.ResultTextController.SetResult (grade, Note.transform.position);
            }
        }

        protected virtual void OnCleared ( ) {
            Note.ResultTextController.Recycle (resultText);
            resultText = null;
            bRendering = false;
            bClearing = false;
            Note.transform.localScale = initScale;
            VRef.SetDisable ( );
            Note.Recycle ( );
        }
        public virtual void Render ( ) { }

    }

    [System.Serializable]
    public class NoteViewRef {
        public RuntimeAnimatorController animController;
        public Animator animator;
        public SpriteRenderer renderer;
        public SpriteRenderer lineRenderer;
        public SpriteRenderer bgLineRenderer;
        public Animator lineAnimator;
        public void SetDisable ( ) {
            renderer.transform.rotation=Quaternion.identity;
            lineRenderer.transform.rotation=Quaternion.identity;
            bgLineRenderer.transform.rotation=Quaternion.identity;
            lineRenderer.enabled = false;
            bgLineRenderer.enabled = false;
            renderer.enabled = false;
            animator.enabled=false;
        }
    }
}
