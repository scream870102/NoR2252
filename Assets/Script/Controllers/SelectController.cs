using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
    [SerializeField] Text downText;
    [SerializeField] Texture2D errorTex;
    [SerializeField] Button backBtn;
    [SerializeField] List<RawImage> sprites;
    [SerializeField] new AnimationEventHandler animation;
    //store all the sheet from assetBundle
    [SerializeField] AnimationClip NextAnim;
    [SerializeField] AnimationClip PrevAnim;
    List<GameSheet> AllSheet = new List<GameSheet> ( );
    readonly float MIN_START_UP_PER = 0.05f;
    readonly int MID_SPRITE_INDEX = 2;
    //store the sheet index for sprite should display
    List<int> cAs = new List<int> ( );
    new AudioSource audio;
    bool bBackClicked = false;
    bool bVideoExist = false;
    Vector2 startPos = new Vector2 ( );
    Vector2 currentPos = new Vector2 ( );
    int fingerId = -1;
    int direction = 0;
    Vector2 screenSize = new Vector2 ( );
    bool bSelecting = false;

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
        screenSize = new Vector2 (Screen.width, Screen.height);
        //set all the texture for sprite
        //if sheet is not exist set the value = -1
        for (int i = 0; i < sprites.Count; i++) {
            cAs.Add (i);
            if (i < AllSheet.Count)
                sprites [i].texture = AllSheet [i].cover;
            else
                cAs [i] = -1;
        }
        GetNext ( );
        SetUI ( );
        direction = 0;
        fingerId = -1;
    }

    //when player tap the screen and the middle song value not equal to -4
    //set the current sheet to middle one and load game scene
    void Tap (LeanFinger finger) {
        audio.Play ( );
        if (cAs [MID_SPRITE_INDEX] != -1) {
            NoR2252Application.CurrentSheet = AllSheet [cAs [MID_SPRITE_INDEX]];
            if (!bVideoExist) {
                DownloadVideo (Path.GetFileName (AllSheet [cAs [MID_SPRITE_INDEX]].music));
            }
            else if (!bBackClicked)
                SceneManager.LoadSceneAsync ("Game");
        }
    }
    void Down (LeanFinger finger) {
        startPos = finger.ScreenPosition;
        currentPos = finger.ScreenPosition;
        fingerId = finger.Index;
        audio.Play ( );
    }
    void Set (LeanFinger finger) {
        animation.Animation.Stop ( );
        if (finger.Index == fingerId) {
            currentPos = finger.ScreenPosition;
            Vector2 offset = currentPos - startPos;
            direction = offset.x > 0f?1: -1;
            float percentage = Mathf.Abs (offset.x) / screenSize.x;
            if (percentage <= MIN_START_UP_PER) return;
            bSelecting = true;
            foreach (AnimationState state in animation.Animation) {
                if (direction == 1)
                    animation.Animation.clip = NextAnim;
                else
                    animation.Animation.clip = PrevAnim;
                state.time = state.length * percentage;
            }
            animation.Animation.Play ( );

        }

    }
    void Up (LeanFinger finger) {
        if (bSelecting)
            animation.Animation.Play ( );
        bSelecting = false;
    }

    //if fade animation fin get the next sheet
    //and update all the ui on the scene
    void AnimFin ( ) {
        if (direction == 1)
            GetNext ( );
        else
            GetPrev ( );

        SetUI ( );
    }

    // if the value is equal to -1
    //then set title and cover to error
    void SetUI ( ) {
        if (cAs [MID_SPRITE_INDEX] == -1) {
            title.text = "Can't get the song";
            author.text = "Error No2252X00";
        }
        else {
            CheckVideoExist ( );
            //try to find the bestscore pair for the sheet in the middle
            ScorePair bestPair = null;
            title.text = AllSheet [cAs [MID_SPRITE_INDEX]].name;
            author.text = AllSheet [cAs [MID_SPRITE_INDEX]].author;
            bestPair = NoR2252Application.ScoreBoard.Find (AllSheet [cAs [MID_SPRITE_INDEX]].name);
            if (bestPair != null)
                bestScore.text = "Best Score\n" + bestPair.Score.ToString ( );
            else {
                NoR2252Application.ScoreBoard.Add (AllSheet [cAs [MID_SPRITE_INDEX]].name);
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
    void GetPrev ( ) {
        int prev = cAs [MID_SPRITE_INDEX];
        if (prev + 1 >= AllSheet.Count) prev = 0;
        else prev++;
        cAs [MID_SPRITE_INDEX] = prev;
        CheckRange (prev);
    }
    void GetNext ( ) {
        int next = cAs [MID_SPRITE_INDEX];
        if (next - 1 < 0) next = AllSheet.Count - 1;
        else next--;
        cAs [MID_SPRITE_INDEX] = next;
        CheckRange (next);
    }

    void CheckRange (int middle) {
        for (int i = 0; i < sprites.Count; i++) {
            if (i == MID_SPRITE_INDEX) continue;
            cAs [i] = middle + MID_SPRITE_INDEX - i;
            if (i < MID_SPRITE_INDEX) {
                if (middle + MID_SPRITE_INDEX - i >= AllSheet.Count) {
                    int offset = middle + MID_SPRITE_INDEX - i - AllSheet.Count;
                    cAs [i] = offset;
                }
            }
            else if (i > MID_SPRITE_INDEX) {
                if (middle + MID_SPRITE_INDEX - i < 0) {
                    int offset = AllSheet.Count - Mathf.Abs (middle + MID_SPRITE_INDEX - i);
                    cAs [i] = offset;
                }
            }
            if (cAs [i] < 0 || cAs [i] >= AllSheet.Count) cAs [i] = -1;
        }
    }

    void OnBackClicked ( ) {
        bBackClicked = true;
        SceneManager.LoadScene ("Start");
    }
    void OnDisable ( ) {
        LeanTouch.OnFingerTap -= Tap;
        LeanTouch.OnFingerDown -= Down;
        LeanTouch.OnFingerSet -= Set;
        LeanTouch.OnFingerUp -= Up;
    }
    void OnEnable ( ) {
        LeanTouch.OnFingerTap += Tap;
        LeanTouch.OnFingerDown += Down;
        LeanTouch.OnFingerSet += Set;
        LeanTouch.OnFingerUp += Up;
    }
    void CheckVideoExist ( ) {
        string path = Application.persistentDataPath + "/Bundle/" + Path.GetFileName (AllSheet [cAs [MID_SPRITE_INDEX]].music);
        if (!File.Exists (path)) {
            bVideoExist = false;
            downText.text = "Music not exist tap to download";
        }
        else {
            bVideoExist = true;
            downText.text = "Music exist";
        }
    }
    async void DownloadVideo (string fileName) {
        string netPath = FirebaseConst.VideoFolderPath + "/" + fileName;
        Debug.Log ("Downding" + netPath);
        Task downTask = SourceLoader.DownloadVideo (netPath, fileName);
        if (!downTask.IsCompleted) {
            downText.text = "Downloading ...";
            await Task.Delay (1000 / 30);
        }
        //await SourceLoader.DownloadVideo (netPath, fileName);
        Debug.Log ("Already download" + fileName);
        if (AllSheet [cAs [MID_SPRITE_INDEX]].music == fileName) {
            downText.text = "Music exist";
            bVideoExist = true;
        }
    }
}
