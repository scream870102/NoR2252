using Eccentric;
using NG = NoR2252.Models.Graphics;
using NoR2252.Models;
using EU = Eccentric.Utils;
using NoR2252.View.Action;

using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class SlideChildNoteView : NoteView {
        float lineHeight;
        Vector3 initOutlineScale;
        Vector3 halfOfInitOutlineScale;
        readonly float ROT_VELO = 50f;
        RotationAction rotAction;
        public SlideChildNoteView (GameNote note, NG.RefObject refs) : base (note, refs) {
            cAs = NoR2252Data.Instance.ColorsAndSprites [(int) ENoteType.SLIDE_CHILD];
            ParticleSystem.MainModule ptcMain = refs.Ptc.main;
            ptcMain.startColor = cAs.PtcC;
            refs.OutLine.sprite = cAs.OutlineS;
            refs.OutLine.color = cAs.OutLineC;
            refs.Line.sprite = cAs.LineS;
            refs.LineBG.sprite = cAs.LineS;
            refs.Line.color = cAs.LineC;
            refs.LineBG.color = cAs.LineBGC;
            rotAction = new RotationAction (Math.RandomBool ( ), ROT_VELO, ref refs.OutLineTf);
            initOutlineScale = refs.OutLineTf.localScale;
            halfOfInitOutlineScale = initOutlineScale / 2f;

        }
        public override void Render ( ) {
            rotAction.TickZ ( );
            if (IsRendering) {
                float fOffset = Note.Info.endTime - NoR2252Application.VideoTime;
                fOffset = fOffset <= 0f?0f : fOffset;
                if (Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.GOOD] <= NoR2252Application.VideoTime) {
                    Color c = cAs.OutLineC;
                    Vector3 hsvC = EU.Color.RGB2HSV (c);
                    hsvC.y += Time.deltaTime * 5f;
                    c = EU.Color.HSV2RGB (hsvC);
                    refS.OutLine.color = c;
                    SetLine (fOffset);
                }
                else if (Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS] <= NoR2252Application.VideoTime) {
                    refS.OutLine.color = cAs.OutLineC;
                    refS.OutLineTf.localScale = initOutlineScale;
                    SetLine (fOffset);
                }
                else {
                    float tf = Math.InverseProbability ((Note.Info.endTime - NoR2252Application.VideoTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]) / (NoR2252Application.PreLoad + Note.Info.Duration - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]));
                    refS.OutLineTf.localScale = halfOfInitOutlineScale + halfOfInitOutlineScale * tf;
                }
                if (bClearing) {
                    if (timer.IsFinished)
                        OnCleared ( );
                }
            }
        }
        public override void OnClear (ENoteGrade grade) {
            if (grade != ENoteGrade.UNKNOWN && !bClearing) {
                bClearing = true;
                timer.Reset (CLEAR_TIME);
                if (grade == ENoteGrade.PERFECT || grade == ENoteGrade.GREAT) {
                    refS.Ptc.Play ( );
                }
                resultText = Note.ResultTextController.SetResult (grade, Note.transform.position);
            }
        }
        void SetLine (float fOffset) {
            fOffset = fOffset / NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS];
            refS.Line.size = new Vector2 (1f, lineHeight * fOffset);
        }
        public override void OnSpawn ( ) {
            base.OnSpawn ( );
            refS.OutLine.enabled = true;
            refS.LineBG.enabled = true;
            refS.Line.enabled = true;
            if (Note.Controller.SlidePos.ContainsKey (Note.Info.id)) {
                //calculate the degree from this note to prev note
                Vector2 tmp = Note.Controller.SlidePos [Note.Info.id] - refS.MainTf.position;
                tmp.Normalize ( );
                float degree = Mathf.Atan2 (tmp.y, tmp.x) * Mathf.Rad2Deg;
                Vector3 oEuler = new Vector3 (0f, 0f, degree);
                oEuler.z = degree - 90f;
                refS.MainTf.rotation = Quaternion.Euler (oEuler);
                //set the slide line position
                Vector2 tp = Note.Controller.SlidePos [Note.Info.id] - refS.MainTf.position;
                Vector2 size = new Vector2 (1f, Mathf.Ceil (tp.magnitude));
                refS.Line.size = size;
                refS.LineBG.size = size;
                lineHeight = refS.LineBG.size.y;
            }
        }
        protected override void OnCleared ( ) {
            refS.OutLineTf.localScale = initOutlineScale;
            refS.OutLineTf.rotation = Quaternion.identity;
            base.OnCleared ( );
        }
    }
}
