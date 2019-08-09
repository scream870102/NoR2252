using NoR2252.Models;
using EU = Eccentric.Utils;
using Eccentric;
using NG = NoR2252.Models.Graphics;
using NoR2252.View.Action;

using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class FlickNoteView : TapNoteView {
        public FlickNoteView (GameNote note, NoteViewRef Ref):
            base (note, Ref) {

            }
    }
}
