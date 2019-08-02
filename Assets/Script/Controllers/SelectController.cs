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
    [SerializeField] Text bestScore;
    [SerializeField] Texture2D errorTex;
    [SerializeField] Button backBtn;
    [SerializeField] List<int> cAs = new List<int> ( );
    new AudioSource audio;
    bool bBackClicked = false;

    void Awake ( ) {
        audio = GetComponent<AudioSource> ( );
    }

    // Start is called before the first frame update
    void Start ( ) {
        backBtn.onClick.AddListener (OnBackClicked);
        AllSheet.AddRange (SourceLoader.LoadAllSheets ( ));
        //獲取最佳分數
        NoR2252Application.ScoreBoard = SourceLoader.LoadScoreBoard ( );
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
        if (cAs [2] != -1) {
            NoR2252Application.CurrentSheet = AllSheet [cAs [2]];
            if (!bBackClicked)
                SceneManager.LoadSceneAsync ("Game");
        }
    }

    void AnimFin ( ) {
        GetNext ( );
        SetUI ( );
    }
    void SetUI ( ) {
        if (cAs [2] == -1) {
            title.text = "Can't get the song";
            author.text = "Error No2252X00";
        }
        else {
            BestScorePair bestPair = null;
            title.text = AllSheet [cAs [2]].name;
            author.text = AllSheet [cAs [2]].author;
            bestPair = NoR2252Application.ScoreBoard.Find (AllSheet [cAs [2]].name);
            if (bestPair != null) {
                bestScore.text = "Best Score\n"+bestPair.Score.ToString ( );
            }
            else {
                NoR2252Application.ScoreBoard.Add (AllSheet [cAs [2]].name);
                bestScore.text = "Best Score \n0";
            }

        }
        for (int i = 0; i < sprites.Count; i++) {
            Texture2D tex = errorTex;
            if (cAs [i] != -1) tex = AllSheet [cAs [i]].cover;
            sprites [i].texture = tex;
        }
    }

    void GetNext ( ) {
        int next = cAs [0];
        if (next + 1 >= AllSheet.Count) next = 0;
        else next++;
        cAs [0] = next;
        for (int i = 1; i < sprites.Count; i++) {
            if (next + i >= AllSheet.Count) {
                int offset = next + i - AllSheet.Count;
                if (offset >= AllSheet.Count) cAs [i] = -1;
                else cAs [i] = 0 + offset;
            }
            else {
                cAs [i] = next + i;
            }

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
