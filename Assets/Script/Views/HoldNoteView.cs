using NoR2252.Models;

using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class HoldNoteView : NoteView {
        public HoldNoteView (GameNote note, SpriteRenderer renderer, Animation animation) : base (note,renderer,animation) {
            color = NoR2252Data.Instance.NoteColor [(int) ENoteType.HOLD];
        }

    }
}
