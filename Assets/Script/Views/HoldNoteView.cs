using NoR2252.Models;

namespace NoR2252.View.Note {
    [System.Serializable]
    public class HoldNoteView : NoteView {
        public HoldNoteView (GameNote note) : base (note) {
            color = GameManager.Instance.Data.NoteColor [(int) ENoteType.HOLD];
        }

    }
}
