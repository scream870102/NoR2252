using EU = Eccentric.Utils;

using NoR2252.Models;

using UnityEngine;
using UnityEngine.UI;
namespace NoR2252.View.Note {
    [RequireComponent (typeof (Animation))]
    [RequireComponent (typeof (SpriteRenderer))]
    [System.Serializable]
    public class NoteView {
        public GameNote Note;
        protected SpriteRenderer renderer;
        protected Animation animation;
        protected SpriteRenderer secondRenderer;
        protected LineRenderer lineRenderer;
        protected ParticleSystem particle;
        protected Transform tf;
        protected Transform secondTf;
        protected Transform lineTf;
        protected Color color;

        bool bRendering = false;
        Vector3 initScale = new Vector3 ( );
        protected bool bClearing = false;
        public bool IsRendering { get { return bRendering; } }
        protected EU.CountdownTimer timer = new EU.CountdownTimer ( );
        protected ResultTextController.TextAndAnim resultText = null;
        public NoteView (GameNote note, SpriteRenderer renderer, Animation animation, SpriteRenderer secondRend, LineRenderer lineRenderer, ParticleSystem particle, Transform tf, Transform secondTf, Transform lineTf) {
            this.Note = note;
            this.renderer = renderer;
            this.animation = animation;
            this.secondRenderer = secondRend;
            this.lineRenderer = lineRenderer;
            this.particle = particle;
            this.tf = tf;
            this.secondTf = secondTf;
            this.lineTf = lineTf;
            this.renderer.sprite = NoR2252Data.Instance.NoteSprite [note.Info.type];
            renderer.enabled = false;
            lineRenderer.enabled = false;
            secondRend.enabled = false;
            initScale = note.transform.localScale;
        }
        public void SetNote (GameNote note) {
            this.Note = note;
        }
        public virtual void OnSpawn ( ) {
            renderer.color = color;
            renderer.enabled = true;
            bRendering = true;
            bClearing = false;
        }
        public virtual void OnClear (ENoteGrade grade) {
        }

        protected virtual void OnCleared ( ) {
            renderer.enabled = false;
            
            bRendering = false;
            bClearing = false;
            Note.transform.localScale = initScale;
            Note.Recycle ( );
        }
        public virtual void Render ( ) {
            if (bRendering) {
                Vector3 newScale = new Vector3 (Note.transform.localScale.x - Time.deltaTime * .01f, Note.transform.localScale.y - Time.deltaTime * .01f, initScale.z);
                Note.transform.localScale = newScale;
                if (Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS] <= NoR2252Application.VideoTime)
                    renderer.color = Color.green;
                if (Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.GOOD] <= NoR2252Application.VideoTime) {
                    renderer.color = Color.white;
                }
                if (Note.Info.endTime + NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.GOOD] <= NoR2252Application.VideoTime) {
                    renderer.color = Color.red;
                }
            }
        }

    }
}
