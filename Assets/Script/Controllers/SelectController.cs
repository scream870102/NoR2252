using System.Collections;
using System.Collections.Generic;

using Eccentric.Utils;

using Lean.Touch;

using NoR2252.Models;
using NoR2252.Utils;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SelectController : MonoBehaviour {
    [SerializeField] List<GameSheet> AllSheet = new List<GameSheet> ( );
    [SerializeField] List<RawImage> sprites;
    [SerializeField] new AnimationEventHandler animation;
    [SerializeField] Text title;
    [SerializeField] Text author;
    Dictionary<int, int> spriteAndCover = new Dictionary<int, int> ( );
    new AudioSource audio;

    void Awake ( ) {
        GetPermission ( );
        audio = GetComponent<AudioSource> ( );

    }
    void GetPermission ( ) {
        AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission ("android.permission.ACCESS_FINE_LOCATION");
        if (result == AndroidRuntimePermissions.Permission.Granted)
            Debug.Log ("We have permission to access external storage!");
        else
            Debug.Log ("Permission state: " + result);
    }

    // Start is called before the first frame update
    void Start ( ) {
        AllSheet.AddRange (SourceLoader.LoadAllSheets ( ));
        animation.OnAnimationFinVoid += AnimFin;
        for (int i = 0; i < sprites.Count; i++) {
            spriteAndCover.Add (i, i);
            if (AllSheet [i] != null) {
                sprites [i].texture = AllSheet [i].cover;
            }
        }
        SetUI ( );
    }
    void Swipe (LeanFinger finger) {
        animation.Animation.Play ( );
        audio.Play ( );
    }
    void Tap (LeanFinger finger) {
        audio.Play ( );
        NoR2252Application.CurrentSheet = AllSheet [spriteAndCover [0]];
        SceneManager.LoadSceneAsync ("Game");
    }

    void AnimFin ( ) {
        GetNext ( );
        SetUI ( );
    }
    void SetUI ( ) {
        title.text = AllSheet [spriteAndCover [0]].name;
        author.text = AllSheet [spriteAndCover [0]].author;
        for (int i = 0; i < sprites.Count; i++)
            sprites [i].texture = AllSheet [spriteAndCover [i]].cover;
    }

    void GetNext ( ) {
        for (int i = 0; i < sprites.Count; i++) {
            if (spriteAndCover [i] - 1 > -1)
                spriteAndCover [i] -= 1;
            else
                spriteAndCover [i] = AllSheet.Count - 1;
        }
    }
    void OnDisable ( ) {
        LeanTouch.OnFingerTap -= Tap;
        LeanTouch.OnFingerSwipe -= Swipe;
    }
    void OnEnable ( ) {
        LeanTouch.OnFingerSwipe += Swipe;
        LeanTouch.OnFingerTap += Tap;
    }

}
