using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Video;
namespace NoR2252.Models {
    [System.Serializable]
    public class Option {
        public float Offset;
        public float Volume;
        public float Opacity;
        public Option (float Offset = 0f, float Volume = 0.5f,float Opacity = 0.7f) {
            this.Offset = Offset;
            this.Volume = Volume;
            this.Opacity = Opacity;
        }
        public Option (Option copyOne) {
            this.Offset = copyOne.Offset;
            this.Volume = copyOne.Volume;
            this.Opacity=copyOne.Opacity;
        }
    }
}
