using NoR2252.Models;

namespace NoR2252.View.Note {
    [System.Serializable]
    public class SlideHeadNoteView : NoteView {
        public SlideHeadNoteView (GameNote note) : base (note) {
            color = GameManager.Instance.Data.NoteColor [(int) ENoteType.SLIDE_HEAD];
        }

    }
}
