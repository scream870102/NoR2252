using Eccentric;
using NG = NoR2252.Models.Graphics;
using NoR2252.Models;
using EU = Eccentric.Utils;
using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class SlideChildNoteView : NoteView {
        public SlideChildNoteView (GameNote note, NG.RefObject refs) : base (note, refs) {

        }
        // AnimationClip clip;
        // Color lineColor;
        // Color ptcC;
        // Vector3 initScale;
        // readonly float inZoomOutVelo = 0.95f;
        // readonly float outZoomOutVelo = 0.75f;
        // readonly float clearTime = .2f;
        // public SlideChildNoteView (GameNote note, SpriteRenderer renderer, Animation animation, SpriteRenderer secondRend, LineRenderer lineRenderer, ParticleSystem particle, Transform tf, Transform secondTf, Transform lineTf):
        //     base (note, renderer, animation, secondRend, lineRenderer, particle, tf, secondTf, lineTf) {
        //         color = NoR2252Data.Instance.NoteColor [(int) ENoteType.SLIDE_CHILD];
        //         lineColor = NoR2252Data.Instance.LineColor;
        //         ptcC = NoR2252Data.Instance.TapPtcColor;
        //         ParticleSystem.MainModule ptcMain = particle.main;
        //         ptcMain.startColor = ptcC;
        //         tf.rotation = Quaternion.identity;
        //         initScale = new Vector3 (NoR2252Application.Size, NoR2252Application.Size, 1f);

        //     }

        // public override void OnSpawn ( ) {
        //     base.OnSpawn ( );
        //     if (Note.Controller.SlidePos.ContainsKey (Note.Info.id)) {
        //         lineRenderer.SetPosition (0, Note.transform.position);
        //         lineRenderer.SetPosition (1, Note.Controller.SlidePos [Note.Info.id]);
        //         Vector2 tmp = Note.Controller.SlidePos [Note.Info.id] - Note.transform.position;
        //         tmp.Normalize ( );
        //         float degree = Mathf.Atan2 (tmp.y, tmp.x) * Mathf.Rad2Deg;
        //         Vector3 oEuler = Note.transform.rotation.eulerAngles;
        //         oEuler.z = degree - 90f;
        //         Note.transform.rotation = Quaternion.Euler (oEuler);
        //     }
        //     lineRenderer.startColor = lineColor;
        //     lineRenderer.endColor = lineColor;
        //     lineRenderer.enabled = true;
        //     tf.localScale = Vector3.zero;

        // }
        // public override void Render ( ) {
        //     if (IsRendering) {
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
        //             lineRenderer.startColor = lineColor;
        //             lineRenderer.endColor = lineColor;
        //             tf.localScale = initScale;
        //         }
        //         else {
        //             Color c = color;
        //             c.a = c.a / 2f;
        //             renderer.color = c;
        //             Color cL = color;
        //             cL.a = cL.a / 2f;
        //             lineRenderer.endColor = cL;
        //             lineRenderer.startColor = cL;
        //             tf.localScale = initScale * Math.InverseProbability ((Note.Info.endTime - NoR2252Application.VideoTime) / (NoR2252Application.PreLoad + NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]));
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
        // protected override void OnCleared ( ) {
        //     lineRenderer.enabled = false;
        //     ResultTextController.Instance.Recycle (resultText);
        //     resultText = null;
        //     base.OnCleared ( );
        // }

    }
}
