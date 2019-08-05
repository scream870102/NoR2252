using System.Collections.Generic;

using Eccentric.Utils;

using Lean.Touch;

using NoR2252.Models;
using NoR2252.Utils;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SelectController : MonoBehaviour {
    //UI REF
    [SerializeField] Text title;
    [SerializeField] Text author;
    [SerializeField] Text bestScore;
    [SerializeField] Texture2D errorTex;
    [SerializeField] Button backBtn;
    [SerializeField] List<RawImage> sprites;
    [SerializeField] new AnimationEventHandler animation;
    //store all the sheet from assetBundle
    [SerializeField] List<GameSheet> AllSheet = new List<GameSheet> ( );
    //store the sheet index for sprite should display
    List<int> cAs = new List<int> ( );
    new AudioSource audio;
    bool bBackClicked = false;

    void Awake ( ) {
        audio = GetComponent<AudioSource> ( );
    }

    void Start ( ) {
        //get all the sheet and add listener for back button onClick
        backBtn.onClick.AddListener (OnBackClicked);
        animation.OnAnimationFinVoid += AnimFin;
        AllSheet.AddRange (SourceLoader.LoadAllSheets ( ));
        //get the best point from scoreboard
        NoR2252Application.ScoreBoard = SourceLoader.LoadScoreBoard ( );
        //set all the texture for sprite
        //if sheet is not exist set the value = -1
        for (int i = 0; i < sprites.Count; i++) {
            cAs.Add (i);
            if (i < AllSheet.Count)
                sprites [i].texture = AllSheet [i].cover;
            else
                cAs [i] = -1;
        }
        SetUI ( );
    }

    //when player swipe on the screen try to get the next song
    void Swipe (LeanFinger finger) {
        animation.Animation.Play ( );
        audio.Play ( );
    }

    //when player tap the screen and the middle song value not equal to -4
    //set the current sheet to middle one and load game scene
    void Tap (LeanFinger finger) {
        audio.Play ( );
        if (cAs [2] != -1) {
            NoR2252Application.CurrentSheet = AllSheet [cAs [2]];
            if (!bBackClicked)
                SceneManager.LoadSceneAsync ("Game");
        }
    }

    //if fade animation fin get the next sheet
    //and update all the ui on the scene
    void AnimFin ( ) {
        GetNext ( );
        SetUI ( );
    }

    // if the value is equal to -1
    //then set title and cover to error
    void SetUI ( ) {
        if (cAs [2] == -1) {
            title.text = "Can't get the song";
            author.text = "Error No2252X00";
        }
        else {
            //try to find the bestscore pair for the sheet in the middle
            BestScorePair bestPair = null;
            title.text = AllSheet [cAs [2]].name;
            author.text = AllSheet [cAs [2]].author;
            bestPair = NoR2252Application.ScoreBoard.Find (AllSheet [cAs [2]].name);
            if (bestPair != null)
                bestScore.text = "Best Score\n" + bestPair.Score.ToString ( );
            else {
                NoR2252Application.ScoreBoard.Add (AllSheet [cAs [2]].name);
                bestScore.text = "Best Score \n0";
            }
        }
        //set all the cover for sprite 
        for (int i = 0; i < sprites.Count; i++) {
            Texture2D tex = errorTex;
            if (cAs [i] != -1) tex = AllSheet [cAs [i]].cover;
            sprites [i].texture = tex;
        }
    }

    //calculate the index to find all the sprite value
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
