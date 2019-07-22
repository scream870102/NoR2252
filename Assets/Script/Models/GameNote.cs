using Eccentric.Collections;

using NoR2252.Models;
using NoR2252.View.Note;

using UnityEngine;
namespace NoR2252.Models {
    [RequireComponent (typeof (Collider2D))]
    public class GameNote : MonoBehaviour, IObjectPoolAble {
        [SerializeField] SheetNote info;
        public SheetNote Info { get { return info; } }
        public ObjectPool Pool { get; set; }
        protected NoteView view;
        bool bUsing = false;
        public bool IsUsing { get { return bUsing; } protected set { bUsing = value; } }
        protected Collider2D col = null;
        public bool IsRendering = false;
        bool bHolding = false;
        GameController controller;
        void Awake ( ) {
            col = GetComponent<Collider2D> ( );
            controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ( );
        }
        public void Recycle ( ) {
            Pool.RecycleObject (this);
            bUsing = false;
        }
        public virtual void Init<T> (T data) {
            this.info = data as SheetNote;
            bUsing = true;
            //根據Note種類選擇不同的view
            switch (info.type) {
                case (int) ENoteType.TAP:
                    view = new TapNoteView (this);
                    break;
                case (int) ENoteType.FLICK:
                    view = new FlickNoteView (this);
                    break;
                case (int) ENoteType.HOLD:
                    view = new HoldNoteView (this);
                    bHolding = false;
                    break;
                case (int) ENoteType.SLIDE_HEAD:
                    view = new SlideHeadNoteView (this);
                    break;
                case (int) ENoteType.SLIDE_CHILD:
                    view = new SlideChildNoteView (this);
                    break;
            }
            view.SetNote (this);
        }

        public virtual ENoteGrade Tick (float preLoad) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            //根據自己的時間決定 是否要顯示
            if (!IsRendering && info.startTime - GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.MISS] - preLoad <= NoR2252Application.VideoTime) {
                IsRendering = true;
                view.OnSpawn ( );
            }
            //開始時間點-sheet.note.preload
            view.Render ( );
            //當已經超過判定時間回傳結果
            //回收自己
            if (NoR2252Application.VideoTime >= info.endTime + GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.MISS]) {
                grade = ENoteGrade.MISS;
                Debug.Log ("沒按" + info.endTime + "  " + NoR2252Application.VideoTime + GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.MISS]);
                GetResult (grade);
            }
            //如果類型是Hold也要回收自己
            if (info.type == (int) ENoteType.HOLD && !bHolding && NoR2252Application.VideoTime >= Info.startTime + GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.MISS]) {
                grade = ENoteGrade.MISS;
                Debug.Log ("HOLD沒按" + info.startTime + "  " + NoR2252Application.VideoTime + GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.MISS]);
                GetResult (grade);
            }
            //如果類型是Hold到EndTime就先回收
            if (info.type == (int) ENoteType.HOLD && NoR2252Application.VideoTime >= Info.endTime) {
                grade = bHolding?ENoteGrade.PERFECT : ENoteGrade.MISS;
                Debug.Log ("HOLD回收" + info.endTime + "  " + NoR2252Application.VideoTime + grade.ToString ( ));
                GetResult (grade);
            }
            return grade;
        }

        public virtual bool IsCollide (Vector2 fingerPos) {
            return col.OverlapPoint (fingerPos);

        }
        public virtual ENoteGrade Touch (Vector2 fingerPos, EFingerAction action, int fingerId) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            //先看當下時間點是否可以判定
            if (info.type != (int) ENoteType.HOLD && NoR2252Application.VideoTime <= Info.endTime - GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.MISS]) {
                Debug.Log ("判定不能");
                return grade;
            }
            //根據note種類來決定反饋
            float offset = Mathf.Abs (NoR2252Application.VideoTime - Info.endTime);
            grade = GetGrade (offset);
            //只要不是長按或是子滑行 就計算結果
            if (info.type != (int) ENoteType.HOLD && info.type != (int) ENoteType.SLIDE_CHILD) {
                Debug.Log (NoR2252Application.VideoTime + " " + Info.endTime + "判定結果為" + grade.ToString ( ));
                GetResult (grade);
            }
            else if (info.type == (int) ENoteType.SLIDE_CHILD) {
                if (controller.SlideFinger.ContainsKey (fingerId)) {
                    if (controller.SlideFinger [fingerId] != info.id) {
                        Debug.Log (fingerId+" "+info.endTime+" "+NoR2252Application.VideoTime+"不同手指 下去");
                        grade = ENoteGrade.MISS;
                    }
                    else {
                        Debug.Log (fingerId + " " + info.id + " " + controller.SlideFinger[fingerId] + grade.ToString ( ));
                    }
                }
                else grade = ENoteGrade.MISS;
                if (!controller.SlideFinger.ContainsKey (fingerId))
                    Debug.Log (fingerId + " " + info.id + " " + fingerId + grade.ToString ( ));
                //如果字典中下一個id是自己 就做判定
                GetResult (grade);

            }
            //如果是Hold就看down
            else {
                if (action == EFingerAction.DOWN) {
                    offset = Mathf.Abs (NoR2252Application.VideoTime - Info.startTime);
                    grade = GetGrade (offset);
                    //如果按下去就已經miss 或 bad就結束
                    if (grade == ENoteGrade.BAD || grade == ENoteGrade.MISS) {
                        Debug.Log (grade);
                        GetResult (grade);
                    }
                    else if (grade != ENoteGrade.UNKNOWN) {
                        Debug.Log ("開始長按" + NoR2252Application.VideoTime + " " + Info.startTime + grade.ToString ( ));
                        bHolding = true;
                    }
                }
                else if (action == EFingerAction.UP) {
                    bHolding = false;
                    offset = Mathf.Abs (NoR2252Application.VideoTime - Info.endTime);
                    grade = GetGrade (offset);
                    Debug.Log ("鬆開長按" + NoR2252Application.VideoTime + " " + Info.endTime + grade.ToString ( ));
                }
                else {
                    if (bHolding)
                        grade = ENoteGrade.UNKNOWN;
                }
            }
            return grade;
        }
        void GetResult (ENoteGrade grade) {
            view.OnClear (grade);
            Recycle ( );
            IsRendering = false;
        }
        ENoteGrade GetGrade (float timeOffset) {
            ENoteGrade grade = ENoteGrade.UNKNOWN;
            if (timeOffset < GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.PERFECT])
                grade = ENoteGrade.PERFECT;
            else if (timeOffset < GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.GREAT])
                grade = ENoteGrade.GREAT;
            else if (timeOffset < GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.GOOD])
                grade = ENoteGrade.GOOD;
            else if (timeOffset < GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.BAD])
                grade = ENoteGrade.BAD;
            else if (timeOffset < GameManager.Instance.Data.Time_Grade [(int) ENoteGrade.MISS])
                grade = ENoteGrade.MISS;
            return grade;
        }

    }
}
