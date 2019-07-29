using System.Collections.Generic;

using Eccentric.Collections;
using Eccentric.Utils;

using Lean.Touch;

using NoR2252.Models;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class GameController : MonoBehaviour {
    //-----------UI REF
    [SerializeField] Text title;
    [SerializeField] Text author;
    [SerializeField] RawImage cover;
    [SerializeField] Text scoreText;
    [SerializeField] Text comboText;
    [SerializeField] Text resultText;
    [SerializeField] VideoPlayer video;
    //---------Animation REF
    //use this to get the animation on canvas
    [SerializeField] AnimationEventHandler uiAnimHandler;
    //ref for uiFadeClip when this clip play finished start the game
    [SerializeField] AnimationClip uiFadeClip;
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
    void Awake ( ) {
        //Init some value
        video.clip = null;
        bComboing = false;
        combo = 0;
        score = 0;
        audio = GetComponent<AudioSource> ( );
        resultTextController = GetComponent<ResultTextController> ( );
        camera = Camera.main;
        //subscribe the animation evenet on canvas which define the scene animation
        uiAnimHandler.OnAnimationFinClip += OnAnimationFinished;
        //init for objectpooling
        List<IObjectPoolAble> tmp = new List<IObjectPoolAble> (notePool.Init ( ));
        foreach (IObjectPoolAble item in tmp) gameNotes.Add (item as GameNote);
    }
    void Start ( ) {
        //Load the sheet 
        currentSheet = NoR2252Application.CurrentSheet;
        foreach (SheetNote note in currentSheet.notes) queueNotes.Enqueue (note);
        video.clip = currentSheet.music;
        title.text = currentSheet.name;
        author.text = currentSheet.author;
        cover.texture = currentSheet.cover;
        NoR2252Application.PreLoad = currentSheet.notePreload;
        NoR2252Application.Size = currentSheet.size;
        //Set all the noteInfo to objectPooling
        SetNoteToObjectPool ( );
        //play the fade animation
        uiAnimHandler.Animation.Play (uiFadeClip.name);

    }
    void Update ( ) {
        //Keep update the video time
        NoR2252Application.VideoTime = (float) video.time + NoR2252Application.Offset;
        NoR2252Application.RawVideoTime = (float) video.time;
        //Keep adding sheetNote to gameNote if objectPooling is available
        SetNoteToObjectPool ( );
        //Update all the game note and ui elements
        UpdateAllGameNote ( );
        UpdateUI ( );
    }

    //FingerUp
    //Only react with the gameNote which type is HOLD
    void FingerUp (LeanFinger finger) {
        Vector2 pos = Camera.main.ScreenToWorldPoint (finger.ScreenPosition);
        foreach (GameNote note in gameNotes) {
            if (note.IsRendering && note.Info.type == (int) ENoteType.HOLD && note.IsCollide (pos)) {
                ENoteGrade grade = note.OnTouch (EFingerAction.UP, finger.Index);
                CountScore (grade);
                if (grade != ENoteGrade.UNKNOWN) audio.Play ( );
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
        Vector2 pos = Camera.main.ScreenToWorldPoint (finger.ScreenPosition);
        foreach (GameNote note in gameNotes) {
            if (note.IsRendering && (note.Info.type == (int) ENoteType.SLIDE_HEAD || note.Info.type == (int) ENoteType.HOLD) && note.IsCollide (pos)) {
                ENoteGrade grade = note.OnTouch (EFingerAction.DOWN, finger.Index);
                if (grade != ENoteGrade.UNKNOWN) audio.Play ( );
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
        audio.Play ( );
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
        audio.Play ( );
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
            tmp.z = 0f;
            note.transform.position = tmp;
            //if current note is slideNote save next id and its position to slidePos
            if (note.Info.type == (int) ENoteType.SLIDE_HEAD || note.Info.type == (int) ENoteType.SLIDE_CHILD) {
                if (note.Info.nextId != 0)
                    slidePos.Add (note.Info.nextId, tmp);
            }
        }
    }

    void UpdateAllGameNote ( ) {
        foreach (GameNote note in gameNotes) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            //if the note is over its life time will return MISS
            if (note.IsUsing) grade = note.Tick ( );
            if (grade == ENoteGrade.MISS) CountScore (grade);
        }
    }

    void CountScore (ENoteGrade grade) {
        if (grade != ENoteGrade.UNKNOWN) {
            resultText.text = grade.ToString ( );
            score += NoR2252Data.Instance.Points [(int) grade];
            combo++;
        }
        if (grade == ENoteGrade.BAD || grade == ENoteGrade.MISS) {
            bComboing = false;
            combo = 0;
        }

    }
    /// <summary>Call this method to get more point</summary>
    /// <remarks>its for hold finished</remarks>
    public void PlusPoint (ENoteGrade grade) {
        CountScore (grade);
    }
    void UpdateUI ( ) {
        scoreText.text = score.ToString ( );
        comboText.text = combo.ToString ( );
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
