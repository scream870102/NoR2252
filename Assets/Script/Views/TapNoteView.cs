using Eccentric;
using EU = Eccentric.Utils;
using NoR2252.Models;
using NG = NoR2252.Models.Graphics;
using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class TapNoteView : NoteView {
        bool bClockWise;
        Vector3 initOutlineScale;
        Vector3 initScale;
        readonly float clearTime = .2f;
        readonly float inZoomOutVelo = 0.95f;
        readonly float outZoomOutVelo = 0.75f;
        readonly float inRotVelo = 300f;
        readonly float biggestOutlineScale = 0.05f;
        readonly float outLineTransVelo = 0.1f;

        public TapNoteView (GameNote note, NG.RefObject refs) : base (note, refs) {
            //set color and sprite
            cAs = NoR2252Data.Instance.ColorsAndSprites [(int) ENoteType.TAP];
            ParticleSystem.MainModule ptcMain = refs.Ptc.main;
            ptcMain.startColor = cAs.PtcC;
            refs.Main.sprite = cAs.MainS;
            refs.Main.color = cAs.MainC;
            refs.OutLine.sprite = cAs.OutlineS;
            refs.OutLine.color = cAs.OutLineC;
            //set other var
            bClockWise = Math.RandomBool ( );
            initOutlineScale = refs.OutLineTf.localScale;
            initScale = new Vector3 (NoR2252Application.Size, NoR2252Application.Size, 1f);

        }
        public override void Render ( ) {
            if (IsRendering) {
                // note inside
                Vector3 insideRot = refS.MainTf.rotation.eulerAngles;
                if (bClockWise) insideRot.z += inRotVelo * Time.deltaTime;
                else insideRot.z -= inRotVelo * Time.deltaTime;
                refS.MainTf.rotation = Quaternion.Euler (insideRot);
                //note outside
                Vector3 outsideScale = initOutlineScale;
                outsideScale.x = Mathf.PingPong (Time.time * outLineTransVelo, biggestOutlineScale) + initOutlineScale.x;
                outsideScale.y = Mathf.PingPong (Time.time * outLineTransVelo, biggestOutlineScale) + initOutlineScale.y;
                refS.OutLineTf.localScale = outsideScale;
                //about color
                if (Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.GOOD] <= NoR2252Application.VideoTime) {
                    Color c = cAs.MainC;
                    Vector3 hsvC = EU.Color.RGB2HSV (c);
                    hsvC.y += Time.deltaTime * 5f;
                    c = EU.Color.HSV2RGB (hsvC);
                    refS.Main.color = c;
                }
                else if (Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS] <= NoR2252Application.VideoTime) {
                    refS.Main.color = cAs.MainC;
                    refS.OutLine.color = cAs.OutLineC;
                    refS.MainTf.localScale = initScale;
                }
                else {
                    Color c = cAs.MainC;
                    c.a = c.a / 2f;
                    refS.Main.color = c;
                    Color co = cAs.OutLineC;
                    co.a = co.a / 2f;
                    refS.OutLine.color = co;
                    refS.MainTf.localScale = initScale * Math.InverseProbability ((Note.Info.startTime - NoR2252Application.VideoTime) / (NoR2252Application.PreLoad + NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]));
                }
                if (bClearing) {
                    Vector3 tmpScale = refS.OutLineTf.localScale * inZoomOutVelo;
                    refS.OutLineTf.localScale = tmpScale;
                    Vector3 tpScale = refS.MainTf.localScale * outZoomOutVelo;
                    refS.MainTf.localScale = tpScale;

                    if (timer.IsFinished)
                        OnCleared ( );
                }
            }

        }

        public override void OnClear (ENoteGrade grade) {
            if (grade != ENoteGrade.UNKNOWN && !bClearing) {
                bClearing = true;
                timer.Reset (clearTime);
                if (grade == ENoteGrade.PERFECT || grade == ENoteGrade.GREAT) {
                    refS.Ptc.Play ( );
                }
                resultText = ResultTextController.Instance.SetResult (grade, Note.transform.position);
            }
        }
        public override void OnSpawn ( ) {
            base.OnSpawn ( );
            refS.Main.enabled = true;
            refS.OutLine.enabled = true;
        }
        protected override void OnCleared ( ) {
            refS.OutLineTf.localScale = initOutlineScale;
            base.OnCleared ( );
        }

    }
}
