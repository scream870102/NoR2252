using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Video;
namespace NoR2252.Models {
    [System.Serializable]
    public class Option {
        public float Offset;
        public float Volume;
        public Option (float Offset = 0f, float Volume = 0.5f) {
            this.Offset = Offset;
            this.Volume = Volume;
        }
        public Option (Option copyOne) {
            this.Offset = copyOne.Offset;
            this.Volume = copyOne.Volume;
        }
    }
}
