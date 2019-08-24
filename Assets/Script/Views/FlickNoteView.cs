using NoR2252.Models;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class FlickNoteView : TapNoteView {
        public FlickNoteView (GameNote note, NoteViewRef Ref):
            base (note, Ref) {

            }
    }
}
