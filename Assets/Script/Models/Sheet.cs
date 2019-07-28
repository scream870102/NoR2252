using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Video;
namespace NoR2252.Models {
    [System.Serializable]
    public class SheetNote {
        public SheetNote (float startTime, float endTime, int type, int id, int nextId = 0) {
            this.startTime = startTime;
            this.endTime = endTime;
            this.type = type;
            this.id = id;
            this.nextId = nextId;
        }
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
    public class GameSheet {
        public GameSheet (string name, string author, float bpm, List<SheetNote> notes, double musicOffset, float size, float notePreload, Vector2 screenSize) {
            this.name = name;
            this.author = author;
            this.bpm = bpm;
            this.notes = notes;
            this.musicOffset = musicOffset;
            this.size = size;
            this.notePreload = notePreload;
            this.screenSize = screenSize;
        }
        public string name;
        public string author;
        public VideoClip music;
        public Texture2D cover;
        public float bpm;
        public List<SheetNote> notes;
        public double musicOffset = 0f;
        public float size = 2.0f;
        public float notePreload = 2.0f;
        public Vector2 screenSize;
    }

    [System.Serializable]
    public class Sheet {
        public string name;
        public string author;
        public string music;
        public string cover;
        public float bpm;
        public List<SheetNote> notes;
        public double musicOffset = 0f;
        public float size = 2.0f;
        public float notePreload = 2.0f;
        public Vector2 screenSize;
    }

}
