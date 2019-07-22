using System;

using UnityEngine;
namespace Eccentric.Utils {
    [RequireComponent (typeof (Animation))]
    /// <summary>this is a class can add to gameObject then when animation finished you can use animation event to call AnimationFinished</summary>
    /// <remarks>it will broadcast the animation clip it set on animation event who subscribe the event OnAnimationFinshed</remarks>
    public class AnimationEventHandler : MonoBehaviour {
        new Animation animation;
        public Animation Animation { get { return animation; } }
        /// <summary>this callback will return a animation clip</summary>
        public event System.Action<AnimationClip> OnAnimationFinClip;
        /// <summary>this callback will return a string</summary>
        public event System.Action<String> OnAnimationFinString;
        void AnimationFinished (AnimationClip anim) {
            if (OnAnimationFinClip != null)
                OnAnimationFinClip (anim);
        }

        void AnimationFinished (string value) {
            if (OnAnimationFinString != null)
                OnAnimationFinString (value);
        }

        void Awake ( ) {
            animation = GetComponent<Animation> ( );
        }
    }
}
