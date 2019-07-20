using Eccentric.Collections;

using NoR2252.View.Note;

using UnityEngine;
namespace NoR2252.Models {
    [RequireComponent (typeof (Collider2D))]
    public class GameNote : MonoBehaviour, IObjectPoolAble {
        [SerializeField] SheetNote info;
        public ObjectPool Pool { get; set; }
        protected NoteView view;
        bool bUsing = false;
        public bool IsUsing { get { return bUsing; } protected set { bUsing = value; } }
        protected Collider2D col = null;
        void Awake ( ) {
            col = GetComponent<Collider2D> ( );
        }
        public void Recycle ( ) {
            Debug.Log ("Being Recycle");
            Pool.RecycleObject (this);
            bUsing = false;
        }
        public virtual void Init<T> (T data) {
            this.info = data as SheetNote;
            bUsing = true;
            view = new NoteView (this);
            view.SetNote (this);
        }

        public virtual void Update ( ) { }

        public virtual bool IsCollide (Vector2 fingerPos) {
            return col.OverlapPoint (fingerPos);

        }
        public virtual void Touch (Vector2 fingerPos) {
            Debug.Log ("GetTouched");
            Recycle ( );
        }

    }
}
