using Eccentric.Collections;

using NoR2252.View.Note;

using UnityEngine;
namespace NoR2252.Models {
    [RequireComponent (typeof (Collider2D))]
    public class GameNote : MonoBehaviour, IObjectPoolAble {
        //-------Ref
        [SerializeField] NoteViewRef ViewRef;
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
        bool bFindRef = false;

        //-------Property
        public AGameNoteStrategy Strategy { get { return strategy; } }
        public SheetNote Info { get { return info; } }
        public ObjectPool Pool { get; set; }
        public NoteView View { get { return view; } }
        public GameController Controller { get { return controller; } }
        public ResultTextController ResultTextController { get { return controller.ResultTextController; } }
        public bool IsUsing { get { return bUsing; } }
        public bool IsRendering { get { return bRendering; } set { bRendering = value; } }
        public RuntimeAnimatorController [ ] animControllers = new AnimatorOverrideController [System.Enum.GetNames (typeof (ENoteType)).Length];

        /// <summary>when this note should be recycle call this method</summary>
        public void Recycle ( ) {
            IsRendering = false;
            bUsing = false;
            Pool.RecycleObject (this);
        }

        /// <summary>Init this gamenote due to its type</summary>
        /// <param name="data">the sheetNote information</param>
        public void Init<T> (T data) {
            this.info = data as SheetNote;
            bUsing = true;
            if (!bFindRef) {
                FindRef ( );
            }
            //according to the type choose different strategy and view
            switch (info.type) {
                case (int) ENoteType.TAP:
                    ViewRef.animController = animControllers [(int) ENoteType.TAP];
                    view = new TapNoteView (this, ViewRef);
                    strategy = new BasicStrategy (this);
                    break;
                case (int) ENoteType.FLICK:
                    ViewRef.animController = animControllers [(int) ENoteType.FLICK];
                    view = new FlickNoteView (this, ViewRef);
                    strategy = new BasicStrategy (this);
                    break;
                case (int) ENoteType.HOLD:
                    ViewRef.animController = animControllers [(int) ENoteType.HOLD];
                    view = new HoldNoteView (this, ViewRef);
                    strategy = new HoldStrategy (this);
                    break;
                case (int) ENoteType.SLIDE_HEAD:
                    ViewRef.animController = animControllers [(int) ENoteType.SLIDE_HEAD];
                    view = new SlideHeadNoteView (this, ViewRef);
                    strategy = new BasicStrategy (this);
                    break;
                case (int) ENoteType.SLIDE_CHILD:
                    ViewRef.animController = animControllers [(int) ENoteType.SLIDE_CHILD];
                    view = new SlideChildNoteView (this, ViewRef);
                    strategy = new SlideChildStrategy (this);
                    break;
            }
            view.SetNote (this);
        }

        //Keep update this note if strategy not equal to null
        public ENoteGrade Tick ( ) {
            if (strategy != null)
                return strategy.OnTick ( );
            else
                return ENoteGrade.UNKNOWN;
        }

        /// <summary>if this note being touch it will return the result of this touch</summary>
        /// <param name="action">which kind of touch action on this note</param>
        /// <param name="fingerId">the fingerIndex of this touch</param>
        public ENoteGrade OnTouch (EFingerAction action, int fingerId) {
            if (strategy != null) {
                ENoteGrade result = strategy.OnTouch (action, fingerId);
                return result;
            }
            else
                return ENoteGrade.UNKNOWN;
        }

        /// <summary>if this note being touch</summary>
        /// <param name="fingerPos">the touch point worldCoordinate</param>
        public bool IsCollide (Vector2 fingerPos) {
            return col.OverlapPoint (fingerPos);
        }
        void FindRef ( ) {
            bFindRef = true;
            col = GetComponent<Collider2D> ( );
            controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ( );
        }

    }
    public abstract class AGameNoteStrategy {
        //save the ref for GameNote
        protected GameNote Note;
        protected bool bOnRecycle = false;
        protected bool bGetResult = false;
        public AGameNoteStrategy (GameNote parent) {
            this.Note = parent;
        }
        /// <summary>define the action update on every frame</summary>
        public abstract ENoteGrade OnTick ( );
        /// <summary>define the action when this note being touch</summary>
        public abstract ENoteGrade OnTouch (EFingerAction action, int fingerId);
        /// <summary>define the action after got the grade</summary>
        protected void GetResult (ENoteGrade grade) {
            bGetResult = true;
            Note.View.OnClear (grade);
        }
        /// <summary>return the grade due to the time offset</summary>
        /// <param name="factor">multiply the factor to TimeGrade if factor less than one the judge is stricker else looser</param>
        protected ENoteGrade GetGrade (float timeOffset, float factor = 1f) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            if (timeOffset < NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.PERFECT] * factor)
                grade = ENoteGrade.PERFECT;
            else if (timeOffset < NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.GREAT] * factor)
                grade = ENoteGrade.GREAT;
            else if (timeOffset < NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.GOOD] * factor)
                grade = ENoteGrade.GOOD;
            else if (timeOffset < NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.BAD] * factor)
                grade = ENoteGrade.BAD;
            else if (timeOffset < NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS] * factor)
                grade = ENoteGrade.MISS;
            return grade;
        }

    }

    //define the action for TAP FLICK SLIDE-HEAD
    public class BasicStrategy : AGameNoteStrategy {
        public BasicStrategy (GameNote parent) : base (parent) { }
        public override ENoteGrade OnTick ( ) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            //decide to show according to the video time and note start time
            if (!Note.IsRendering && Note.Info.startTime - NoR2252Application.PreLoad <= NoR2252Application.VideoTime) {
                Note.IsRendering = true;
                Note.View.OnSpawn ( );
            }
            if(Note.IsRendering)
                Note.View.Render ( );
            //if video time is over the judge time recycle self and set result to miss
            if (!bGetResult && NoR2252Application.VideoTime >= Note.Info.endTime + NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]) {
                grade = ENoteGrade.MISS;
                GetResult (grade);
            }
            return grade;
        }

        public override ENoteGrade OnTouch (EFingerAction action, int fingerId) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            //if current video time is in the judge time range or not
            if (NoR2252Application.VideoTime <= Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]) {
                return grade;
            }
            float offset = Mathf.Abs (NoR2252Application.VideoTime - Note.Info.endTime);
            grade = GetGrade (offset);
            GetResult (grade);
            return grade;
        }
    }
    //define the action for HOLD
    public class HoldStrategy : AGameNoteStrategy {
        bool bHolding;
        bool bJudging;
        public bool IsHolding { get { return bHolding; } }
        public HoldStrategy (GameNote parent) : base (parent) {
            bHolding = false;
            bJudging = false;
        }

        public override ENoteGrade OnTick ( ) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            if (!Note.IsRendering && Note.Info.startTime - NoR2252Application.PreLoad <= NoR2252Application.VideoTime) {
                Note.IsRendering = true;
                Note.View.OnSpawn ( );
            }
            Note.View.Render ( );

            //if not being touch and time is over the startTime then recycle self and set grade to miss
            if (!bHolding && NoR2252Application.VideoTime >= Note.Info.startTime + NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS] && !bGetResult) {
                grade = ENoteGrade.MISS;
                GetResult (grade);
            }
            //if player keep hold this note until note end time recycle it and plus a perfect point by call PlusPoint
            if (!bGetResult && NoR2252Application.VideoTime >= Note.Info.endTime) {
                grade = bHolding?ENoteGrade.PERFECT : ENoteGrade.MISS;
                Note.Controller.PlusPoint (grade);
                GetResult (grade);
            }
            return grade;
        }

        public override ENoteGrade OnTouch (EFingerAction action, int fingerId) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            float offset = 0f;

            //if action is down check its grade amd bJuding equals to false
            //bJudging is to prevent the sameNote being judge several times
            if (action == EFingerAction.DOWN && !bJudging) {
                offset = Mathf.Abs (NoR2252Application.VideoTime - Note.Info.startTime);
                grade = GetGrade (offset);
                //if grade is miss or bad just return the result and recycle self
                if (grade == ENoteGrade.BAD || grade == ENoteGrade.MISS) {
                    bJudging = true;
                    GetResult (grade);
                }
                //if grade not equal to UNKNOWN MISS BAD then start holding this note
                else if (grade != ENoteGrade.UNKNOWN) {
                    bHolding = true;
                    bJudging = true;
                }
            }

            //if player release finger before note end time
            //give it a grade
            //if this note has bJuding about press down action and not get the result take the grade of it
            else if (action == EFingerAction.UP && bJudging && bHolding && !bGetResult) {
                bGetResult = true;
                bHolding = false;
                offset = Mathf.Abs (NoR2252Application.VideoTime - Note.Info.endTime);
                grade = GetGrade (offset);
                if (grade == ENoteGrade.UNKNOWN) grade = ENoteGrade.MISS;
                GetResult (grade);
            }
            //if player keep holding return UNKNOWN
            else if (action == EFingerAction.SET) {
                if (!bGetResult && bHolding)
                    grade = ENoteGrade.UNKNOWN;
            }
            return grade;
        }
    }

    public class SlideChildStrategy : BasicStrategy {
        public SlideChildStrategy (GameNote parent) : base (parent) { }
        public override ENoteGrade OnTouch (EFingerAction action, int fingerId) {
            if (bGetResult) return ENoteGrade.UNKNOWN;
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            if (NoR2252Application.VideoTime <= Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]) {
                return grade;
            }
            float offset = Mathf.Abs (NoR2252Application.VideoTime - Note.Info.endTime);
            //give the grade a factor make it much easier
            grade = GetGrade (offset, NoR2252Data.Instance.SlideChildGradeFactor);
            //if fingerID exist check if the same finger with pre note of slide
            if (Note.Controller.SlideFinger.ContainsKey (fingerId)) {
                if (Note.Controller.SlideFinger [fingerId] != Note.Info.id) {
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
