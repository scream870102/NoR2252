using EU = Eccentric.Utils;

using NoR2252.Models;

using UnityEngine;
namespace NoR2252.View.Note {
    [RequireComponent (typeof (Animation))]
    [RequireComponent (typeof (SpriteRenderer))]
    [System.Serializable]
    public class NoteView {
        protected float clearTime = 0.5f;
        public GameNote Note;
        protected Vector3 initScale = new Vector3 ( );
        protected bool bClearing = false;
        protected EU.CountdownTimer timer = new EU.CountdownTimer ( );
        protected ResultTextController.TextAndAnim resultText = null;
        protected NoteViewRef VRef;
        public NoteView (GameNote note, NoteViewRef VRef) {
            this.Note = note;
            initScale = new Vector3 (NoR2252Application.Size, NoR2252Application.Size, 1f);
            this.VRef = VRef;
            this.VRef.animator.runtimeAnimatorController = this.VRef.animController;
        }
        public void SetNote (GameNote note) {
            this.Note = note;
        }
        public virtual void OnSpawn ( ) {
            Note.transform.localScale = initScale;
            VRef.renderer.enabled = true;
            VRef.animator.enabled = true;
            bClearing = false;
            clearTime = NoR2252Application.PreLoad / 2f;
        }
        public virtual void OnClear (ENoteGrade grade) {
            if (grade != ENoteGrade.UNKNOWN && !bClearing) {
                timer.Reset (clearTime);
                resultText = Note.ResultTextController.SetResult (grade, Note.transform.position);
            }
        }

        protected virtual void OnCleared ( ) {
            Note.ResultTextController.Recycle (resultText);
            resultText = null;
            bClearing = false;
            VRef.SetDisable ( );
            Note.Recycle ( );
        }
        public virtual void Render ( ) { }

    }

    [System.Serializable]
    public class NoteViewRef {
        readonly Vector3 LINE_INIT_POS = new Vector3 (0f, 0.5f, 0f);
        readonly Vector2 LINE_INIT_SIZE=new Vector2(0.3f,0f);
        public RuntimeAnimatorController animController;
        public Animator animator;
        public SpriteRenderer renderer;
        public SpriteRenderer lineRenderer;
        public SpriteRenderer bgLineRenderer;
        public Animator lineAnimator;
        public void SetDisable ( ) {
            renderer.transform.rotation = Quaternion.identity;
            lineRenderer.transform.rotation = Quaternion.identity;
            bgLineRenderer.transform.rotation = Quaternion.identity;
            bgLineRenderer.transform.localPosition = LINE_INIT_POS;
            lineRenderer.transform.localPosition = LINE_INIT_POS;
            lineRenderer.size = LINE_INIT_SIZE;
            bgLineRenderer.size = LINE_INIT_SIZE;
            lineRenderer.enabled = false;
            bgLineRenderer.enabled = false;
            renderer.enabled = false;
            animator.enabled = false;
        }
    }
}
