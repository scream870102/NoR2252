using System.Collections.Generic;
using System.IO;

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
    [SerializeField] Texture2D errorTex;
    [SerializeField] Button backBtn;
    List<int> cAs = new List<int> ( );
    new AudioSource audio;
    bool bBackClicked = false;

    void Awake ( ) {
        audio = GetComponent<AudioSource> ( );
    }

    // Start is called before the first frame update
    void Start ( ) {
        backBtn.onClick.AddListener (OnBackClicked);
        AllSheet.AddRange (SourceLoader.LoadAllSheets ( ));
        animation.OnAnimationFinVoid += AnimFin;
        for (int i = 0; i < sprites.Count; i++) {
            cAs.Add (i);
            if (i < AllSheet.Count) {
                sprites [i].texture = AllSheet [i].cover;
            }
            else {
                cAs [i] = -1;
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
        NoR2252Application.CurrentSheet = AllSheet [cAs [0]];
        if (!bBackClicked)
            SceneManager.LoadSceneAsync ("Game");
    }

    void AnimFin ( ) {
        GetNext ( );
        SetUI ( );
    }
    void SetUI ( ) {
        if (cAs [0] == -1) {
            title.text = "Can't get the song";
            author.text = "Error No2252X00";
        }
        else {
            title.text = AllSheet [cAs [0]].name;
            author.text = AllSheet [cAs [0]].author;
        }
        for (int i = 0; i < sprites.Count; i++) {
            Texture2D tex = errorTex;
            if (cAs [i] != -1) tex = AllSheet [cAs [i]].cover;
            sprites [i].texture = tex;
        }
    }

    void GetNext ( ) {
        for (int i = 0; i < sprites.Count; i++) {
            if (cAs [i] - 1 > -1)
                cAs [i] -= 1;
            else
                cAs [i] = AllSheet.Count - 1;
        }
    }
    void OnBackClicked ( ) {
        bBackClicked = true;
        SceneManager.LoadScene ("Start");
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
