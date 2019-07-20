using System.Collections.Generic;

using Eccentric.Collections;
using Eccentric.Utils;

using Lean.Touch;

using NoR2252.Models;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class GameController : MonoBehaviour {
    ObjectPool tapNotes;
    ObjectPool flickNotes;
    ObjectPool holdNotes;
    ObjectPool dragNotes;
    #region TEST 
    [SerializeField] ObjectPool testNotes;
    IObjectPoolAble test = null;
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
    void Awake ( ) {
        //初始化變數
        video.clip = null;
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
        NoR2252Application.VideoTime = video.time;
        //持續加入新的SheetNote給GameNotes
        SetNoteToObjectPool ( );
        //更新所有GameNotes
        UpdateAllGameNote ( );
    }
    //根據手指觸碰的位置請note判定
    //FingerUp
    void FingerUp (LeanFinger finger) {
        Debug.Log ("up");

    }
    //FingerSet
    void FingerSet (LeanFinger finger) {
        Debug.Log ("set");
    }
    //FingerDown
    void FingerDown (LeanFinger finger) {
        Debug.Log ("down");
        Vector2 pos = Camera.main.ScreenToWorldPoint (finger.ScreenPosition);
        foreach (GameNote note in gameNotes) {
            if (note.IsCollide (pos))
                note.Touch (pos);
        }
    }
    //FingerSwipe
    void FingerSwipe (LeanFinger finger) {
        Debug.Log ("swipe");
        video.Play ( );
    }
    //FingerTap
    void FingerTap (LeanFinger finger) {
        Debug.Log ("tap");
        video.Pause ( );
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
            testNotes.GetPooledObject<SheetNote> (queueNotes.Dequeue ( ));
        }
    }

    void UpdateAllGameNote ( ) {
        foreach (GameNote note in gameNotes) {
            if (note.IsUsing) note.Update ( );
        }
    }
}
