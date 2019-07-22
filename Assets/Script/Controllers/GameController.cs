using System.Collections.Generic;

using Eccentric.Collections;
using Eccentric.Utils;

using Lean.Touch;

using NoR2252.Models;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class GameController : MonoBehaviour {
    #region TEST 
    [SerializeField] ObjectPool testNotes;
    #endregion TEST
    [SerializeField] List<GameNote> gameNotes = new List<GameNote> ( );
    Queue<SheetNote> queueNotes = new Queue<SheetNote> ( );
    Sheet currentSheet = null;
    [SerializeField] VideoPlayer video;
    [SerializeField] AnimationEventHandler uiAnimHandler;
    [SerializeField] AnimationClip uiFadeClip;
    [SerializeField] Text title;
    [SerializeField] Text author;
    [SerializeField] RawImage cover;
    [SerializeField] Text scoreText;
    [SerializeField] Text comboText;
    [SerializeField] Text resultText;
    int combo;
    [SerializeField] bool bComboing;
    int score;
    Dictionary<int, int> slideFinger = new Dictionary<int, int> ( );
    public Dictionary<int, int> SlideFinger { get { return slideFinger; } }
    void Awake ( ) {
        //初始化變數
        video.clip = null;
        bComboing = false;
        combo = 0;
        score = 0;
        //註冊Lean Touch 事件    
        LeanTouch.OnFingerUp += FingerUp;
        LeanTouch.OnFingerSet += FingerSet;
        LeanTouch.OnFingerDown += FingerDown;
        LeanTouch.OnFingerSwipe += FingerSwipe;
        LeanTouch.OnFingerTap += FingerTap;
        //尋找相關參照
        uiAnimHandler.OnAnimationFinished += OnAnimationFinished;
        //生成所有Note的物件池
        List<IObjectPoolAble> tmp = new List<IObjectPoolAble> (testNotes.Init ( ));
        foreach (IObjectPoolAble item in tmp) gameNotes.Add (item as GameNote);
    }
    void Start ( ) {
        //載入Sheet
        currentSheet = NoR2252Application.CurrentSheet;
        foreach (SheetNote note in currentSheet.notes) queueNotes.Enqueue (note);
        video.clip = currentSheet.music;
        title.text = currentSheet.name;
        author.text = currentSheet.author;
        cover.texture = currentSheet.cover;
        //根據Page載入Note到Note 並且透過物件池生成
        SetNoteToObjectPool ( );
        //播放入場動畫
        uiAnimHandler.Animation.Play (uiFadeClip.name);

    }
    void Update ( ) {
        //不停的更新Video Time
        NoR2252Application.VideoTime = (float) video.time;
        //持續加入新的SheetNote給GameNotes
        SetNoteToObjectPool ( );
        //更新所有GameNotes
        UpdateAllGameNote ( );
        UpdateUI ( );
    }
    //根據手指觸碰的位置請note判定
    //FingerUp
    void FingerUp (LeanFinger finger) {
        Vector2 pos = Camera.main.ScreenToWorldPoint (finger.ScreenPosition);
        foreach (GameNote note in gameNotes) {
            if (note.IsRendering && note.Info.type == (int) ENoteType.HOLD && note.IsCollide (pos))
                CountScore (note.Touch (pos, EFingerAction.UP, finger.Index));
        }
    }
    //FingerSet
    void FingerSet (LeanFinger finger) {
        Vector2 pos = Camera.main.ScreenToWorldPoint (finger.ScreenPosition);
        foreach (GameNote note in gameNotes) {
            if (note.IsRendering && (note.Info.type == (int) ENoteType.SLIDE_CHILD || note.Info.type == (int) ENoteType.HOLD) && note.IsCollide (pos)) {
                ENoteGrade grade = note.Touch (pos, EFingerAction.SET, finger.Index);
                CountScore (grade);
                //如果是SLIDE-CHILD
                if (note.Info.type == (int) ENoteType.SLIDE_CHILD) {
                    if ((int) grade <= (int) ENoteGrade.GOOD)
                        slideFinger [finger.Index] = note.Info.nextId;
                    else if ((int) grade > (int) ENoteGrade.GOOD && grade != ENoteGrade.UNKNOWN)
                        slideFinger [finger.Index] = 0;

                }
            }
        }
    }
    //FingerDown
    void FingerDown (LeanFinger finger) {
        Vector2 pos = Camera.main.ScreenToWorldPoint (finger.ScreenPosition);
        foreach (GameNote note in gameNotes) {
            if (note.IsRendering && (note.Info.type == (int) ENoteType.SLIDE_HEAD || note.Info.type == (int) ENoteType.HOLD) && note.IsCollide (pos)) {
                ENoteGrade grade = note.Touch (pos, EFingerAction.DOWN, finger.Index);
                CountScore (grade);
                //如果Head有成功加入finger跟下一個人的ID表示成功
                //若Finger存在ID 為零表示前面失敗
                if (note.Info.type == (int) ENoteType.SLIDE_HEAD) {
                    if ((int) grade <= (int) ENoteGrade.GOOD)
                        slideFinger.Add (finger.Index, note.Info.nextId);
                    else if ((int) grade > (int) ENoteGrade.GOOD && grade != ENoteGrade.UNKNOWN)
                        slideFinger.Add (finger.Index, 0);

                }
            }
        }
    }
    //FingerSwipe
    void FingerSwipe (LeanFinger finger) {
        Vector2 pos = Camera.main.ScreenToWorldPoint (finger.StartScreenPosition);
        foreach (GameNote note in gameNotes) {
            if (note.IsRendering && note.Info.type == (int) ENoteType.FLICK && note.IsCollide (pos)) {
                Debug.Log ("有Flick了");
                CountScore (note.Touch (pos, EFingerAction.SWIPE, finger.Index));
            }
        }
    }
    //FingerTap
    void FingerTap (LeanFinger finger) {
        Vector2 pos = Camera.main.ScreenToWorldPoint (finger.ScreenPosition);
        foreach (GameNote note in gameNotes) {
            if (note.IsRendering && note.Info.type == (int) ENoteType.TAP && note.IsCollide (pos)) {
                CountScore (note.Touch (pos, EFingerAction.TAP, finger.Index));
            }
        }
    }
    //Animation Event Callback
    void OnAnimationFinished (AnimationClip anim) {
        if (anim.name == uiFadeClip.name) {
            UILoadFinished ( );
        }
    }

    void UILoadFinished ( ) {
        video.Play ( );
    }

    void SetNoteToObjectPool ( ) {
        if (testNotes.IsAvailable && queueNotes.Count != 0) {
            GameNote note = testNotes.GetPooledObject<SheetNote> (queueNotes.Dequeue ( )) as GameNote;
            Vector3 tmp = Camera.main.ScreenToWorldPoint (note.Info.pos);
            tmp.z = 0f;
            tmp.y = -tmp.y;
            note.transform.position = tmp;
        }
    }

    void UpdateAllGameNote ( ) {
        foreach (GameNote note in gameNotes) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            //當超過時間沒有產生任何觸碰會回傳miss
            if (note.IsUsing) grade = note.Tick (currentSheet.notePreload);
            //如果miss的話
            if (grade == ENoteGrade.MISS) CountScore (grade);
        }
    }

    void CountScore (ENoteGrade grade) {
        resultText.text = grade.ToString ( );
        if (grade != ENoteGrade.UNKNOWN) {
            score += GameManager.Instance.Data.Points [(int) grade];
            combo++;
        }
        if (grade == ENoteGrade.BAD || grade == ENoteGrade.MISS) {
            bComboing = false;
            combo = 0;
        }

    }
    void UpdateUI ( ) {
        scoreText.text = score.ToString ( );
        comboText.text = combo.ToString ( );
    }
}
