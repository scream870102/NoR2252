using NoR2252.Models;
using EU = Eccentric.Utils;
using Eccentric;
using NG = NoR2252.Models.Graphics;
using NoR2252.View.Action;

using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class FlickNoteView : NoteView {
        Vector3 initOutlineScale;
        PingPongScale ppScale;
        readonly float IN_ZOOM_OUT_VELO = 0.95f;
        readonly float OUT_ZOOM_OUT_VELO = 0.75f;
        readonly float OUTLINE_TRANS_VELO = 1.5f;
        readonly float BIGGEST_OUTLINE_SCALE = .35f;
        public FlickNoteView (GameNote note) : base (note) {

        }
    }
}
