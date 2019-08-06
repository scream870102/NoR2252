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
        protected NG.RefObject refS;
        protected NG.NoteColorAndSprite cAs;
        bool bRendering = false;
        protected Vector3 initScale = new Vector3 ( );
        protected bool bClearing = false;
        public bool IsRendering { get { return bRendering; } }
        protected EU.CountdownTimer timer = new EU.CountdownTimer ( );
        protected ResultTextController.TextAndAnim resultText = null;
        public NoteView (GameNote note, NG.RefObject refs) {
            this.Note = note;
            this.refS = refs;
            initScale = new Vector3 (NoR2252Application.Size, NoR2252Application.Size, 1f);
            this.refS.MainTf.localScale = initScale;
            this.refS.IsAllEnable = false;
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
            refS.IsAllEnable = false;
            bRendering = false;
            bClearing = false;
            Note.transform.localScale = initScale;
            Note.transform.position = Vector3.zero;
            Note.transform.rotation=Quaternion.identity;
            refS.LineTf.localPosition=new Vector3(0f,0.52f,0f);
            refS.LineTf.localScale=new Vector3(0.5f,0.5f,1f);
            refS.LineTf.rotation=Quaternion.identity;
            refS.OutLineTf.localPosition=Vector3.zero;
            refS.OutLineTf.localScale=new Vector3(1.3f,1.3f,1f);
            refS.OutLineTf.rotation=Quaternion.identity;
            refS.LineBGTf.localPosition=new Vector3(0f,0.52f,0f);
            refS.LineBGTf.localScale=new Vector3(0.5f,0.5f,1f);
            refS.LineBGTf.rotation=Quaternion.identity;
            Note.Recycle ( );
        }
        public virtual void Render ( ) { }

    }
}
