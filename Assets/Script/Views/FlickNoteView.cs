using NoR2252.Models;

namespace NoR2252.View.Note {
    [System.Serializable]
    public class FlickNoteView : NoteView {
        public FlickNoteView (GameNote note) : base (note) {
            color = GameManager.Instance.Data.NoteColor [(int) ENoteType.FLICK];
        }

    }
}
