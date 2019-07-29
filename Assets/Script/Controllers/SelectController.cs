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
    Dictionary<int, int> spriteAndCover = new Dictionary<int, int> ( );
    new AudioSource audio;

    void Awake ( ) {
        Eccentric.Utils.AndroidAskRuntimePermission permission = new AndroidAskRuntimePermission ( );
        audio = GetComponent<AudioSource> ( );

    }

    // Start is called before the first frame update
    void Start ( ) {
        AllSheet.AddRange (SourceLoader.LoadAllSheets ( ));
        string path = Application.persistentDataPath + "/Sheet/bug.json";
        FileStream fs = new FileStream (path, FileMode.Create);
        string fileContext = title.text + "  " + AllSheet [0].name + " " + AllSheet.Count;
        StreamWriter file = new StreamWriter (fs);
        file.Write (fileContext);
        file.Close ( );
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
        //animation.Animation.Play ( );
        audio.Play ( );
        GetNext ( );
        SetUI ( );
        title.text = "Het I change the title";
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
