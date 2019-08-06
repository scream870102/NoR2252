using Eccentric;
using NG = NoR2252.Models.Graphics;
using NoR2252.Models;
using NoR2252.View.Action;
using EU = Eccentric.Utils;
using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class HoldNoteView : NoteView {
        readonly float LINE_HEIGHT = 5f;
        readonly float ROT_VELO = 500f;
        RotationAction rot;
        public HoldNoteView (GameNote note, NG.RefObject refs) : base (note, refs) {
            //set color and sprite
            cAs = NoR2252Data.Instance.ColorsAndSprites [(int) ENoteType.HOLD];
            ParticleSystem.MainModule ptcMain = refs.Ptc.main;
            ptcMain.startColor = cAs.PtcC;
            refs.Main.sprite = cAs.MainS;
            refs.Main.color = cAs.MainC;
            refs.Line.sprite = cAs.LineS;
            refs.Line.color = cAs.LineC;
            refs.LineBG.sprite = cAs.LineS;
            refs.LineBG.color = cAs.LineBGC;
            refs.Line.size = new Vector2 (1f, 0f);
            refs.OutLine.sprite = cAs.OutlineS;
            refs.OutLine.color = cAs.OutLineC;
            //Set the line position
            Vector3 linePos = refS.LineTf.localPosition;
            Vector3 lineRot = refS.LineTf.rotation.eulerAngles;
            //if note on the top part of screen
            if (note.Info.pos.y > NoR2252Application.CurrentSheet.screenSize.y / 2f) {
                linePos.y = -Mathf.Abs (linePos.y);
                lineRot.z = 180f;
            }
            //if note on the bottom part of screen
            else {
                linePos.y = Mathf.Abs (linePos.y);
                lineRot.z = 0f;
            }
            //set the position
            refS.LineBGTf.localPosition = linePos;
            refs.LineBGTf.rotation = Quaternion.Euler (lineRot);
            refS.LineTf.localPosition = linePos;
            refs.LineTf.rotation = Quaternion.Euler (lineRot);
            rot = new RotationAction (Math.RandomBool ( ), ROT_VELO, ref refs.OutLineTf);
        }
        public override void Render ( ) {
            if (IsRendering) {
                if (Note.Info.startTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.GOOD] <= NoR2252Application.VideoTime) {
                    Color c = cAs.MainC;
                    Vector3 hsvC = EU.Color.RGB2HSV (c);
                    hsvC.y += Time.deltaTime * 5f;
                    c = EU.Color.HSV2RGB (hsvC);
                    refS.Main.color = c;
                }
                else if (Note.Info.startTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS] <= NoR2252Application.VideoTime) {
                    refS.Main.color = cAs.MainC;
                    refS.MainTf.localScale = initScale;
                }
                else {
                    refS.Main.color = EU.Color.SetAlpha01 (refS.Main.color, Math.InverseProbability ((Note.Info.startTime - NoR2252Application.VideoTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]) / (NoR2252Application.PreLoad - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS])));
                    refS.LineBG.color = EU.Color.SetAlpha01 (refS.LineBG.color, Math.InverseProbability ((Note.Info.startTime - NoR2252Application.VideoTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]) / (NoR2252Application.PreLoad - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS])));
                    refS.Line.color = EU.Color.SetAlpha01 (refS.Line.color, Math.InverseProbability ((Note.Info.startTime - NoR2252Application.VideoTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]) / (NoR2252Application.PreLoad - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS])));
                }
                if (NoR2252Application.VideoTime >= Note.Info.startTime && (Note.Strategy as HoldStrategy).IsHolding) {
                    Vector2 size = refS.Line.size;
                    size.y = LINE_HEIGHT * Math.InverseProbability ((Note.Info.endTime - NoR2252Application.VideoTime) / Note.Info.Duration);
                    refS.Line.size = size;
                    refS.Ptc.Play ( );
                    rot.TickY ( );
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
        public override void OnSpawn ( ) {
            base.OnSpawn ( );
            refS.Main.enabled = true;
            refS.Line.enabled = true;
            refS.LineBG.enabled = true;
            refS.OutLine.enabled = true;
        }
        protected override void OnCleared ( ) {
            refS.OutLineTf.rotation = Quaternion.identity;
            base.OnCleared ( );
        }

    }
}
