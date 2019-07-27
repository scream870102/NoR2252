using Eccentric;
using EU = Eccentric.Utils;
using NoR2252.Models;
using NG = NoR2252.Models.Graphics;
using NoR2252.View.Action;

using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class TapNoteView : NoteView {
        Vector3 initOutlineScale;
        Vector3 initScale;
        RotationAction rotAction;
        PingPongScale ppScale;
        readonly float CLEAR_TIME = .17f;
        readonly float IN_ZOOM_OUT_VELO = 0.95f;
        readonly float OUT_ZOOM_OUT_VELO = 0.75f;
        readonly float IN_ROT_VELO = 300f;
        readonly float BIGGEST_OUTLINE_SCALE = 0.05f;
        readonly float OUTLINE_TRANS_VELO = 0.1f;

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
            initOutlineScale = refs.OutLineTf.localScale;
            initScale = new Vector3 (NoR2252Application.Size, NoR2252Application.Size, 1f);
            //init action
            rotAction = new RotationAction (Math.RandomBool ( ), IN_ROT_VELO, ref refs.MainTf);
            ppScale = new PingPongScale (OUTLINE_TRANS_VELO, BIGGEST_OUTLINE_SCALE, ref refs.OutLineTf);
        }
        public override void Render ( ) {
            if (IsRendering) {
                // note inside
                rotAction.Tick ( );
                //note outside
                ppScale.Tick ( );
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
                    refS.Main.color = EU.Color.SetAlphaHalf (cAs.MainC);
                    refS.OutLine.color = EU.Color.SetAlphaHalf (cAs.OutLineC);
                    refS.MainTf.localScale = initScale * Math.InverseProbability ((Note.Info.startTime - NoR2252Application.VideoTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]) / (NoR2252Application.PreLoad - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]));
                }
                if (bClearing) {
                    Vector3 tmpScale = refS.OutLineTf.localScale * IN_ZOOM_OUT_VELO;
                    refS.OutLineTf.localScale = tmpScale;
                    Vector3 tpScale = refS.MainTf.localScale * OUT_ZOOM_OUT_VELO;
                    refS.MainTf.localScale = tpScale;

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
