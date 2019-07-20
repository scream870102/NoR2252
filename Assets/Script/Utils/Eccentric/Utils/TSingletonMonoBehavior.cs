using UnityEngine;
namespace Eccentric.Utils {
    /// <summary>derived this class then you will got a monobehavior implement singleton</summary>
    public class TSingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour {
        static T instance = null;

        public static T Instance {
            get { return instance ?? (instance = FindObjectOfType (typeof (T)) as T); }
            set { instance = value; }
        }

        protected virtual void Awake ( ) {
            if (instance == null) instance = this as T;
            if (instance == this) DontDestroyOnLoad (this);
            else Destroy (this);
        }

        protected virtual void OnDestroy ( ) {
            instance = null;
        }
    }
}
