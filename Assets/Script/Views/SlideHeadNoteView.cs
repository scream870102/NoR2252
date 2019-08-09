using Eccentric;
using NG = NoR2252.Models.Graphics;
using NoR2252.Models;
using EU = Eccentric.Utils;
using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class SlideHeadNoteView : TapNoteView {
        public SlideHeadNoteView (GameNote note, NoteViewRef VRef):
            base (note, VRef) {

            }
    }
}
