using Eccentric;
using NG= NoR2252.Models.Graphics;
using NoR2252.Models;
using EU = Eccentric.Utils;
using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class SlideHeadNoteView : NoteView {
        public SlideHeadNoteView (GameNote note, NG.RefObject refs) : base (note, refs) {


        }
        // bool bClockWise;
        // Color ptcC;
        // readonly float inRotVelo = 300f;
        // readonly float inZoomOutVelo = 0.95f;
        // readonly float outZoomOutVelo = 0.75f;
        // readonly float clearTime = .2f;
        // public SlideHeadNoteView (GameNote note, SpriteRenderer renderer, Animation animation, SpriteRenderer secondRend, LineRenderer lineRenderer, ParticleSystem particle, Transform tf, Transform secondTf, Transform lineTf):
        //     base (note, renderer, animation, secondRend, lineRenderer, particle, tf, secondTf, lineTf) {
        //         color = NoR2252Data.Instance.NoteColor [(int) ENoteType.SLIDE_HEAD];
        //         ptcC = NoR2252Data.Instance.SlideHeadPtcColor;
        //         bClockWise = Math.RandomBool ( );
        //         ParticleSystem.MainModule ptcMain = particle.main;
        //         ptcMain.startColor = ptcC;
        //         tf.rotation = Quaternion.identity;
        //         tf.localScale = new Vector3 (NoR2252Application.Size, NoR2252Application.Size, 1f);

        //     }
        // public override void Render ( ) {
        //     if (IsRendering) {
        //         // note inside
        //         Vector3 insideRot = tf.rotation.eulerAngles;
        //         if (bClockWise) insideRot.z += inRotVelo * Time.deltaTime;
        //         else insideRot.z -= inRotVelo * Time.deltaTime;
        //         tf.rotation = Quaternion.Euler (insideRot);
        //         //about color
        //         if (Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.GOOD] <= NoR2252Application.VideoTime) {
        //             Color c = color;
        //             Vector3 hsvC = EU.Color.RGB2HSV (c);
        //             hsvC.y += Time.deltaTime * 5f;
        //             c = EU.Color.HSV2RGB (hsvC);
        //             renderer.color = c;
        //         }
        //         else if (Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS] <= NoR2252Application.VideoTime) {
        //             renderer.color = color;
        //         }
        //         else {
        //             Color c = color;
        //             c.a = c.a / 2f;
        //             renderer.color = c;
        //         }
        //         if (bClearing) {
        //             Vector3 tmpScale = secondTf.localScale * inZoomOutVelo;
        //             secondTf.localScale = tmpScale;
        //             Vector3 tpScale = tf.localScale * outZoomOutVelo;
        //             tf.localScale = tpScale;

        //             if (timer.IsFinished)
        //                 OnCleared ( );
        //         }
        //     }

        // }

        // public override void OnClear (ENoteGrade grade) {
        //     if (grade != ENoteGrade.UNKNOWN && !bClearing) {
        //         bClearing = true;
        //         timer.Reset (clearTime);
        //         if (grade == ENoteGrade.PERFECT || grade == ENoteGrade.GREAT) {
        //             particle.Play ( );
        //         }
        //         resultText = ResultTextController.Instance.SetResult (grade, Note.transform.position);
        //     }
        // }
        // public override void OnSpawn ( ) {
        //     base.OnSpawn ( );
        // }
        // protected override void OnCleared ( ) {
        //     ResultTextController.Instance.Recycle (resultText);
        //     resultText = null;
        //     base.OnCleared ( );
        // }

    }
}
