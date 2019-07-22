using Eccentric.Collections;

using NoR2252.Models;
using NoR2252.View.Note;

using UnityEngine;
namespace NoR2252.Models {
    [RequireComponent (typeof (Collider2D))]
    public class GameNote : MonoBehaviour, IObjectPoolAble {
        //-------Ref
        new SpriteRenderer renderer;
        new Animation animation;
        LineRenderer lineRenderer;
        //-------Field
        [SerializeField] SheetNote info;
        //define how this note Update and react with touch
        AGameNoteStrategy strategy = null;
        //define how this note view according to its type
        NoteView view;
        //define if this note is already set a sheetNote
        bool bUsing = false;
        Collider2D col = null;
        GameController controller;
        bool bRendering = false;
        //-------Property
        public SheetNote Info { get { return info; } }
        public ObjectPool Pool { get; set; }
        public NoteView View { get { return view; } }
        public GameController Controller { get { return controller; } }
        public bool IsUsing { get { return bUsing; } }
        public bool IsRendering { get { return bRendering; } set { bRendering = value; } }
        void Awake ( ) {
            col = GetComponent<Collider2D> ( );
            controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ( );
            animation = GetComponent<Animation> ( );
            renderer = GetComponent<SpriteRenderer> ( );
            lineRenderer = transform.GetComponentInChildren<LineRenderer> ( );
        }
        public void Recycle ( ) {
            Pool.RecycleObject (this);
            bUsing = false;
        }
        public void Init<T> (T data) {
            this.info = data as SheetNote;
            bUsing = true;
            //according to the type choose different strategy and view
            switch (info.type) {
                case (int) ENoteType.TAP:
                    view = new TapNoteView (this, renderer, animation);
                    strategy = new BasicStrategy (this);
                    break;
                case (int) ENoteType.FLICK:
                    view = new FlickNoteView (this, renderer, animation);
                    strategy = new BasicStrategy (this);
                    break;
                case (int) ENoteType.HOLD:
                    view = new HoldNoteView (this, renderer, animation);
                    strategy = new HoldStrategy (this);
                    break;
                case (int) ENoteType.SLIDE_HEAD:
                    view = new SlideHeadNoteView (this, renderer, animation);
                    strategy = new BasicStrategy (this);
                    break;
                case (int) ENoteType.SLIDE_CHILD:
                    view = new SlideChildNoteView (this, renderer, animation, lineRenderer);
                    strategy = new SlideChildStrategy (this);
                    break;
            }
            view.SetNote (this);
        }

        public ENoteGrade Tick ( ) {
            if (strategy != null)
                return strategy.OnTick (Controller.PreLoad);
            else
                return ENoteGrade.UNKNOWN;
        }

        /// <summary>if this note being touch it will return the result of this touch</summary>
        /// <param name="action">which kind of touch action on this note</param>
        /// <param name="fingerId">the fingerIndex of this touch</param>
        public ENoteGrade OnTouch (EFingerAction action, int fingerId) {
            if (strategy != null)
                return strategy.OnTouch (action, fingerId);
            else
                return ENoteGrade.UNKNOWN;
        }

        /// <summary>if this note being touch</summary>
        /// <param name="fingerPos">the touch point worldCoordinate</param>
        public bool IsCollide (Vector2 fingerPos) {
            return col.OverlapPoint (fingerPos);
        }
    }
    public abstract class AGameNoteStrategy {
        //save the ref for GameNote
        protected GameNote Note;
        public AGameNoteStrategy (GameNote parent) {
            this.Note = parent;
        }
        /// <summary>define the action update on every frame</summary>
        public abstract ENoteGrade OnTick (float preLoad);
        /// <summary>define the action when this note being touch</summary>
        public abstract ENoteGrade OnTouch (EFingerAction action, int fingerId);
        /// <summary>define the action after got the grade</summary>
        protected void GetResult (ENoteGrade grade) {
            Note.View.OnClear (grade);
            Note.Recycle ( );
            Note.IsRendering = false;
        }
        /// <summary>return the grade due to the time offset</summary>
        protected ENoteGrade GetGrade (float timeOffset) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            if (timeOffset < NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.PERFECT])
                grade = ENoteGrade.PERFECT;
            else if (timeOffset < NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.GREAT])
                grade = ENoteGrade.GREAT;
            else if (timeOffset < NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.GOOD])
                grade = ENoteGrade.GOOD;
            else if (timeOffset < NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.BAD])
                grade = ENoteGrade.BAD;
            else if (timeOffset < NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS])
                grade = ENoteGrade.MISS;
            return grade;
        }

    }

    //define the action for TAP FLICK SLIDE-HEAD
    class BasicStrategy : AGameNoteStrategy {
        public BasicStrategy (GameNote parent) : base (parent) { }
        public override ENoteGrade OnTick (float preLoad) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            //decide to show according to the video time and note start time
            if (!Note.IsRendering && Note.Info.startTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS] - preLoad <= NoR2252Application.VideoTime) {
                Note.IsRendering = true;
                Note.View.OnSpawn ( );
            }
            Note.View.Render ( );
            //if video time is over the judge time recycle self and set result to miss
            if (NoR2252Application.VideoTime >= Note.Info.endTime + NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]) {
                grade = ENoteGrade.MISS;
                Debug.Log ("沒按" + Note.Info.endTime + "  " + NoR2252Application.VideoTime + NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]);
                GetResult (grade);
            }
            return grade;
        }

        public override ENoteGrade OnTouch (EFingerAction action, int fingerId) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            //if current video time is in the judge time range or not
            if (NoR2252Application.VideoTime <= Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]) {
                Debug.Log ("判定不能");
                return grade;
            }
            float offset = Mathf.Abs (NoR2252Application.VideoTime - Note.Info.endTime);
            grade = GetGrade (offset);
            Debug.Log (NoR2252Application.VideoTime + " " + Note.Info.endTime + "判定結果為" + grade.ToString ( ));
            GetResult (grade);
            return grade;
        }
    }
    //define the action for HOLD
    class HoldStrategy : AGameNoteStrategy {
        bool bHolding;
        public HoldStrategy (GameNote parent) : base (parent) {
            bHolding = false;
        }
        public override ENoteGrade OnTick (float preLoad) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            if (!Note.IsRendering && Note.Info.startTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS] - preLoad <= NoR2252Application.VideoTime) {
                Note.IsRendering = true;
                Note.View.OnSpawn ( );
            }
            Note.View.Render ( );

            //if not being touch and time is over the startTime then recycle self and set grade to miss
            if (!bHolding && NoR2252Application.VideoTime >= Note.Info.startTime + NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]) {
                grade = ENoteGrade.MISS;
                Debug.Log ("HOLD沒按" + Note.Info.startTime + "  " + NoR2252Application.VideoTime + NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]);
                GetResult (grade);
            }
            //if player keep hold this note until note end time recycle it and plus a perfect point by call PlusPoint
            if (NoR2252Application.VideoTime >= Note.Info.endTime) {
                grade = bHolding?ENoteGrade.PERFECT : ENoteGrade.MISS;
                Debug.Log ("HOLD回收" + Note.Info.endTime + "  " + NoR2252Application.VideoTime + grade.ToString ( ));
                Debug.Log (bHolding + " " + grade);
                GetResult (grade);
                Note.Controller.PlusPoint (grade);
            }
            return grade;
        }

        public override ENoteGrade OnTouch (EFingerAction action, int fingerId) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            float offset = Mathf.Abs (NoR2252Application.VideoTime - Note.Info.endTime);
            grade = GetGrade (offset);

            //if action is down check its grade
            if (action == EFingerAction.DOWN) {
                offset = Mathf.Abs (NoR2252Application.VideoTime - Note.Info.startTime);
                grade = GetGrade (offset);
                //if grade is miss or bad just return the result and recycle self
                if (grade == ENoteGrade.BAD || grade == ENoteGrade.MISS) {
                    Debug.Log (grade);
                    GetResult (grade);
                }
                //if grade not equal to UNKNOWN MISS BAD then start holding this note
                else if (grade != ENoteGrade.UNKNOWN) {
                    Debug.Log ("開始長按" + NoR2252Application.VideoTime + " " + Note.Info.startTime + grade.ToString ( ));
                    bHolding = true;
                }
            }

            //if player release finger before note end time
            //give it a grade
            else if (action == EFingerAction.UP) {
                bHolding = false;
                offset = Mathf.Abs (NoR2252Application.VideoTime - Note.Info.endTime);
                grade = GetGrade (offset);
                Debug.Log ("鬆開長按" + NoR2252Application.VideoTime + " " + Note.Info.endTime + grade.ToString ( ));
            }
            //if player keep holding return UNKNOWN
            else {
                if (bHolding)
                    grade = ENoteGrade.UNKNOWN;
            }
            return grade;
        }
    }

    class SlideChildStrategy : BasicStrategy {
        public SlideChildStrategy (GameNote parent) : base (parent) { }
        public override ENoteGrade OnTouch (EFingerAction action, int fingerId) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            if (NoR2252Application.VideoTime <= Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]) {
                Debug.Log ("判定不能");
                return grade;
            }
            float offset = Mathf.Abs (NoR2252Application.VideoTime - Note.Info.endTime);
            grade = GetGrade (offset);
            //if fingerID exist check if the same finger with pre note of slide
            if (Note.Controller.SlideFinger.ContainsKey (fingerId)) {
                if (Note.Controller.SlideFinger [fingerId] != Note.Info.id) {
                    Debug.Log (fingerId + " " + Note.Info.endTime + " " + NoR2252Application.VideoTime + "不同手指 下去");
                    grade = ENoteGrade.MISS;
                }
            }
            //if its a new finger got miss grade
            else grade = ENoteGrade.MISS;
            GetResult (grade);
            return grade;
        }
    }
}
