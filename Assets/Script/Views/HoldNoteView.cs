using Eccentric;
using NG = NoR2252.Models.Graphics;
using NoR2252.Models;
using NoR2252.View.Action;
using EU = Eccentric.Utils;
using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class HoldNoteView : NoteView {
        readonly float LINE_HEIGHT = 5f;
        readonly float ROT_VELO = 500f;
        RotationAction rot;
        public HoldNoteView (GameNote note) : base (note) {
            //set color and sprite

        }


    }
}
