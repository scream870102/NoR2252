using System.Collections.Generic;

using Eccentric.Collections;
using EU = Eccentric.Utils;
using Eccentric.Utils;

using Lean.Touch;

using NoR2252.Models;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
public class GameController : MonoBehaviour {
    //-----------UI REF
    [SerializeField] Text title;
    [SerializeField] Text author;
    [SerializeField] RawImage cover;
    [SerializeField] Text scoreText;
    [SerializeField] Text comboText;
    [SerializeField] VideoPlayer video;
    [SerializeField] Slider progressBar;
    [SerializeField] Image progressBarUpper;
    [SerializeField] float colorTransVelocity;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Text pauseTitle;
    [SerializeField] Text pauseAuthor;
    //---------Animation REF
    //use this to get the animation on canvas
    [SerializeField] AnimationEventHandler uiAnimHandler;
    //ref for uiFadeClip when this clip play finished start the game
    [SerializeField] AnimationClip uiFadeClip;
    [SerializeField] Button pauseBtn;
    [SerializeField] Button menuBtn;
    [SerializeField] Button retryBtn;
    [SerializeField] Button continueBtn;
    [SerializeField] LeanTouch touch;
    //----------Ref
    new AudioSource audio;
    ResultTextController resultTextController;
    new Camera camera;
    //---------Property
    /// <summary>save the fingerIndex when there is a finger touch the slide key=fingerID value=the next id of this slide should touch</summary>
    public Dictionary<int, int> SlideFinger { get { return slideFinger; } }
    /// <summary>save the prePos of slideNote key=next slide-child id value = current slideNote position</summary>
    public Dictionary<int, Vector3> SlidePos { get { return slidePos; } }
    /// <summary>return the preload before note Start time for this sheet</summary>
    public float PreLoad { get { return currentSheet.notePreload; } }
    public ResultTextController ResultTextController { get { return resultTextController; } }
    //---------field
    [SerializeField] ObjectPool notePool;
    //save all the gameNote on the scene
    List<GameNote> gameNotes = new List<GameNote> ( );
    //save the note information should be set into gameNote
    Queue<SheetNote> queueNotes = new Queue<SheetNote> ( );
    GameSheet currentSheet = null;
    bool bComboing;
    int combo;
    int score;
    Dictionary<int, int> slideFinger = new Dictionary<int, int> ( );
    Dictionary<int, Vector3> slidePos = new Dictionary<int, Vector3> ( );
    float clipLength;
    bool bPausing = false;
    void Awake ( ) {
        //Init some value
        video.clip = null;
        bComboing = false;
        bPausing = false;
        combo = 0;
        score = 0;
        audio = GetComponent<AudioSource> ( );
        resultTextController = GetComponent<ResultTextController> ( );
        camera = Camera.main;
        pauseMenu.SetActive (false);
        touch.enabled = true;
        //subscribe the animation evenet on canvas which define the scene animation
        uiAnimHandler.OnAnimationFinClip += OnAnimationFinished;
        pauseBtn.onClick.AddListener (OnPauseClicked);
        retryBtn.onClick.AddListener (OnRetryClicked);
        continueBtn.onClick.AddListener (OnContinueClicked);
        menuBtn.onClick.AddListener (OnMenuClicked);
        //init for objectpooling
        List<IObjectPoolAble> tmp = new List<IObjectPoolAble> (notePool.Init ( ));
        foreach (IObjectPoolAble item in tmp) gameNotes.Add (item as GameNote);
    }
    void Start ( ) {
        //Load the sheet 
        currentSheet = NoR2252Application.CurrentSheet;
        InitRecord ( );
        foreach (SheetNote note in currentSheet.notes) queueNotes.Enqueue (note);
        video.clip = currentSheet.music;
        title.text = currentSheet.name;
        author.text = currentSheet.author;
        cover.texture = currentSheet.cover;
        pauseTitle.text = currentSheet.name;
        pauseAuthor.text = currentSheet.author;
        NoR2252Application.PreLoad = currentSheet.notePreload;
        NoR2252Application.Size = currentSheet.size;
        clipLength = (float) video.clip.length;
        for (int i = 0; i < video.audioTrackCount; i++) {
            video.SetDirectAudioVolume ((ushort) i, NoR2252Application.Option.Volume);
        }
        //Set all the noteInfo to objectPooling
        SetNoteToObjectPool ( );
        //play the fade animation
        uiAnimHandler.Animation.Play (uiFadeClip.name);

    }
    void InitRecord ( ) {
        NoR2252Application.TotalCombo = 0;
        NoR2252Application.MaxCombo = 0;
        NoR2252Application.Score = 0;
        for (int i = 0; i < NoR2252Application.NoteGrade.Length; i++) {
            NoR2252Application.NoteGrade [i] = 0;
        }
        NoR2252Application.TotalScore = 0;
    }
    void Update ( ) {
        //Keep update the video time
        NoR2252Application.VideoTime = (float) video.time + NoR2252Application.Option.Offset;
        NoR2252Application.RawVideoTime = (float) video.time;
        //Keep adding sheetNote to gameNote if objectPooling is available
        SetNoteToObjectPool ( );
        //Update all the game note and ui elements
        UpdateAllGameNote ( );
        UpdateUI ( );
        GameEnd ( );
    }

    //FingerUp
    //Only react with the gameNote which type is HOLD
    void FingerUp (LeanFinger finger) {
        Vector2 pos = Camera.main.ScreenToWorldPoint (finger.ScreenPosition);
        foreach (GameNote note in gameNotes) {
            if (note.IsRendering && note.Info.type == (int) ENoteType.HOLD && note.IsCollide (pos)) {
                ENoteGrade grade = note.OnTouch (EFingerAction.UP, finger.Index);
                CountScore (grade);
                audio.Play ( );
            }
        }
    }
    //FingerSet
    //react with the gameNote which type is HOLD or SLIDE-CHILD
    void FingerSet (LeanFinger finger) {
        Vector2 pos = Camera.main.ScreenToWorldPoint (finger.ScreenPosition);
        foreach (GameNote note in gameNotes) {
            if (note.IsRendering && (note.Info.type == (int) ENoteType.SLIDE_CHILD || note.Info.type == (int) ENoteType.HOLD) && note.IsCollide (pos)) {
                ENoteGrade grade = note.OnTouch (EFingerAction.SET, finger.Index);
                CountScore (grade);
                //if type is SLIDE-CHILD update the fingerIndex information 
                if (note.Info.type == (int) ENoteType.SLIDE_CHILD) {
                    if ((int) grade <= (int) ENoteGrade.GOOD) {
                        audio.Play ( );
                        slideFinger [finger.Index] = note.Info.nextId;
                    }
                    else if ((int) grade > (int) ENoteGrade.GOOD && grade != ENoteGrade.UNKNOWN) {
                        audio.Play ( );
                        slideFinger [finger.Index] = 0;
                    }

                }
            }
        }
    }

    //FingerDown
    //react with the gameNote which type is SLIDE-HEAD or HOLD
    void FingerDown (LeanFinger finger) {
        audio.Play ( );
        Vector2 pos = Camera.main.ScreenToWorldPoint (finger.ScreenPosition);
        foreach (GameNote note in gameNotes) {
            if (note.IsRendering && (note.Info.type == (int) ENoteType.SLIDE_HEAD || note.Info.type == (int) ENoteType.HOLD) && note.IsCollide (pos)) {
                ENoteGrade grade = note.OnTouch (EFingerAction.DOWN, finger.Index);
                if (grade != ENoteGrade.UNKNOWN) audio.Play ( );
                //if note == hold and got bad or miss at first
                if (note.Info.type == (int) ENoteType.HOLD && grade > ENoteGrade.GOOD)
                    CountScore (grade, true);
                else
                    CountScore (grade);
                //if successful add fingerID and nextID 
                //if nextID equals to zero means got failed on SLIDE-HEAD
                if (note.Info.type == (int) ENoteType.SLIDE_HEAD) {
                    if ((int) grade <= (int) ENoteGrade.GOOD)
                        if (!slideFinger.ContainsKey (finger.Index))
                            slideFinger.Add (finger.Index, note.Info.nextId);
                        else
                            slideFinger [finger.Index] = note.Info.nextId;
                    else if ((int) grade > (int) ENoteGrade.GOOD && grade != ENoteGrade.UNKNOWN)
                        if (!slideFinger.ContainsKey (finger.Index))
                            slideFinger.Add (finger.Index, 0);
                        else
                            slideFinger [finger.Index] = 0;
                }
            }
        }
    }

    //FingerSwipe
    //React with FLICK
    void FingerSwipe (LeanFinger finger) {
        Vector2 pos = Camera.main.ScreenToWorldPoint (finger.StartScreenPosition);
        foreach (GameNote note in gameNotes) {
            if (note.IsRendering && note.Info.type == (int) ENoteType.FLICK && note.IsCollide (pos)) {
                CountScore (note.OnTouch (EFingerAction.SWIPE, finger.Index));
            }
        }
    }

    //FingerTap
    //React with tap
    void FingerTap (LeanFinger finger) {
        Vector2 pos = Camera.main.ScreenToWorldPoint (finger.ScreenPosition);
        foreach (GameNote note in gameNotes) {
            if (note.IsRendering && note.Info.type == (int) ENoteType.TAP && note.IsCollide (pos)) {
                CountScore (note.OnTouch (EFingerAction.TAP, finger.Index));
            }
        }
    }

    //Animation Event Callback
    void OnAnimationFinished (AnimationClip anim) {
        if (anim.name == uiFadeClip.name) {
            UILoadFinished ( );
        }
    }

    //if animation already finished start to play video
    void UILoadFinished ( ) {
        video.Play ( );
    }

    //keep check if notePool is availabe
    //if true set a sheetNote to gameNote
    void SetNoteToObjectPool ( ) {
        if (notePool.IsAvailable && queueNotes.Count != 0) {
            GameNote note = notePool.GetPooledObject<SheetNote> (queueNotes.Dequeue ( )) as GameNote;
            //參考解析度為1920*1080
            Vector3 toViewPort = note.Info.pos;
            toViewPort.x = toViewPort.x / NoR2252Application.CurrentSheet.screenSize.x;
            toViewPort.y = toViewPort.y / NoR2252Application.CurrentSheet.screenSize.y;
            toViewPort.z = 0f;
            Vector3 tmp = camera.ViewportToWorldPoint (toViewPort);
            // #region SafeArea
            // tmp.x = tmp.x * NoR2252Data.Instance.SafeArea.x;
            // tmp.y = tmp.y * NoR2252Data.Instance.SafeArea.y;
            // #endregion
            tmp.z = 0f;
            note.transform.position = tmp;
            //if current note is slideNote save next id and its position to slidePos
            if (note.Info.type == (int) ENoteType.SLIDE_HEAD || note.Info.type == (int) ENoteType.SLIDE_CHILD) {
                if (note.Info.nextId != 0)
                    slidePos.Add (note.Info.nextId, tmp);
            }
            //處理最大基數的問題
            NoR2252Application.TotalCombo += 1;
            NoR2252Application.TotalScore += NoR2252Data.Instance.Points [(int) ENoteGrade.PERFECT];
            if (note.Info.type == (int) ENoteType.HOLD) {
                NoR2252Application.TotalCombo += 1;
                NoR2252Application.TotalScore += NoR2252Data.Instance.Points [(int) ENoteGrade.PERFECT];
            }
        }
    }

    void UpdateAllGameNote ( ) {
        foreach (GameNote note in gameNotes) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            //if the note is over its life time will return MISS
            if (note.IsUsing) grade = note.Tick ( );
            if (grade == ENoteGrade.MISS && note.Info.type != (int) ENoteType.HOLD) CountScore (grade);
            else if (grade == ENoteGrade.MISS && note.Info.type == (int) ENoteType.HOLD) CountScore (grade, true);
        }
    }

    void CountScore (ENoteGrade grade, bool bHoldingHeadMiss = false) {
        if (grade != ENoteGrade.UNKNOWN) {
            NoR2252Application.NoteGrade [(int) grade]++;
            if (bHoldingHeadMiss) NoR2252Application.NoteGrade [(int) grade]++;
            score += NoR2252Data.Instance.Points [(int) grade];
            combo++;
        }
        if (grade == ENoteGrade.BAD || grade == ENoteGrade.MISS) {
            bComboing = false;
            combo = 0;
        }
        if (combo > NoR2252Application.MaxCombo)
            NoR2252Application.MaxCombo = combo;

    }
    /// <summary>Call this method to get more point</summary>
    /// <remarks>its for hold finished</remarks>
    public void PlusPoint (ENoteGrade grade) {
        CountScore (grade);
    }
    void GameEnd ( ) {
        if (video.time + 1f >= video.clip.length) {
            NoR2252Application.Score = score;
            SceneManager.LoadScene ("Result");
        }
    }
    void UpdateUI ( ) {
        scoreText.text = score.ToString ( );
        comboText.text = "Combo " + combo.ToString ( );
        progressBar.value = NoR2252Application.RawVideoTime / clipLength;
        Vector3 hsv = EU.Color.RGB2HSV (progressBarUpper.color);
        hsv.x = Mathf.PingPong (Time.time * colorTransVelocity, 1f);
        progressBarUpper.color = EU.Color.HSV2RGB (hsv);
    }
    void OnPauseClicked ( ) {
        bPausing = true;
        pauseMenu.SetActive (true);
        video.Pause ( );
        touch.enabled = false;
    }

    void OnRetryClicked ( ) {
        audio.Play ( );
        SceneManager.LoadScene ("Game");
    }
    void OnContinueClicked ( ) {
        bPausing = false;
        pauseMenu.SetActive (false);
        video.Play ( );
        touch.enabled = true;
        audio.Play ( );
    }
    void OnMenuClicked ( ) {
        audio.Play ( );
        SceneManager.LoadSceneAsync ("Start");
    }
    void OnEnable ( ) {
        LeanTouch.OnFingerUp += FingerUp;
        LeanTouch.OnFingerSet += FingerSet;
        LeanTouch.OnFingerDown += FingerDown;
        LeanTouch.OnFingerSwipe += FingerSwipe;
        LeanTouch.OnFingerTap += FingerTap;
    }
    void OnDisable ( ) {
        LeanTouch.OnFingerUp -= FingerUp;
        LeanTouch.OnFingerSet -= FingerSet;
        LeanTouch.OnFingerDown -= FingerDown;
        LeanTouch.OnFingerSwipe -= FingerSwipe;
        LeanTouch.OnFingerTap -= FingerTap;
    }

}
