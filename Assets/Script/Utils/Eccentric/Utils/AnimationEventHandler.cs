using UnityEngine;
namespace Eccentric.Utils {
    [RequireComponent (typeof (Animation))]
    public class AnimationEventHandler : MonoBehaviour {
        new Animation animation;
        public Animation Animation { get { return animation; } }
        public event System.Action<AnimationClip> OnAnimationFinished;
        void AnimationFinished (AnimationClip anim) {
            if (OnAnimationFinished != null)
                OnAnimationFinished (anim);
        }
        void Awake ( ) {
            animation = GetComponent<Animation> ( );
        }
    }
}
