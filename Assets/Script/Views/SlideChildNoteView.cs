using Eccentric;
using NG = NoR2252.Models.Graphics;
using NoR2252.Models;
using EU = Eccentric.Utils;
using NoR2252.View.Action;

using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class SlideChildNoteView : NoteView {
        float lineHeight;
        Vector3 initOutlineScale;
        Vector3 halfOfInitOutlineScale;
        readonly float ROT_VELO = 50f;
        RotationAction rotAction;
        public SlideChildNoteView (GameNote note) : base (note) {


        }

    }
}
