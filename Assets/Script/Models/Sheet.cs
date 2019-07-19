using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Video;
namespace NoR2252.Models {
    [System.Serializable]
    public class SheetNote {
        public int type;
        public int id;
        public int nextId;
        public float startTime;
        public float endTime;
        public Vector3 pos;
        public Vector3 endPos;
        public float Duration { get { return endTime - startTime; } }
    }

    [System.Serializable]
    public class SheetPage {
        public float startTime;
        public float endTime;
        public float Duration { get { return endTime - startTime; } }
        public List<SheetNote> notes;
    }

    [System.Serializable]
    public class Sheet {
        public string name;
        public string author;
        public VideoClip music;
        public Texture2D cover;
        public float bpm;
        public List<SheetPage> pages;
        public double musicOffset;
        public double size;
    }

}
