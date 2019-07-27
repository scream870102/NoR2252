using Eccentric;
using NG = NoR2252.Models.Graphics;
using NoR2252.Models;
using EU = Eccentric.Utils;
using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class HoldNoteView : NoteView {
        public HoldNoteView (GameNote note, NG.RefObject refs) : base (note, refs) {

        }
        // Color lineColor;
        // Color ptcC;
        // readonly float clearTime = .2f;
        // readonly float inZoomOutVelo = 0.95f;
        // readonly float outZoomOutVelo = 0.75f;
        // Camera main;
        // public HoldNoteView (GameNote note, SpriteRenderer renderer, Animation animation, SpriteRenderer secondRend, LineRenderer lineRenderer, ParticleSystem particle, Transform tf, Transform secondTf, Transform lineTf):
        //     base (note, renderer, animation, secondRend, lineRenderer, particle, tf, secondTf, lineTf) {
        //         color = NoR2252Data.Instance.NoteColor [(int) ENoteType.HOLD];
        //         lineColor = NoR2252Data.Instance.LineColor;
        //         ptcC = NoR2252Data.Instance.HoldPtcColor;
        //         ParticleSystem.MainModule ptcMain = particle.main;
        //         ptcMain.startColor = ptcC;
        //         main = Camera.main;
        //     }
        // public override void Render ( ) {
        //     if (IsRendering) {
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
        //         //lineRenderer.colorGradient.alphaKeys [1].alpha = (Note.Info.endTime - NoR2252Application.VideoTime) / (Note.Info.Duration);
        //         //Debug.Log ("before" + (Note.Info.endTime - NoR2252Application.RawVideoTime) / (Note.Info.Duration + NoR2252Application.PreLoad));
        //         GradientAlphaKey [ ] aKs = lineRenderer.colorGradient.alphaKeys;
        //         GradientColorKey [ ] cKs = lineRenderer.colorGradient.colorKeys;
        //         Debug.Log (aKs [2].alpha + " " + aKs [2].time);
        //         foreach (GradientAlphaKey a in aKs) {
        //             //Debug.Log (a.alpha + " " + a.time);
        //         }
        //         aKs [1].time = Mathf.Clamp01 (Math.InverseProbability ((Note.Info.endTime - NoR2252Application.RawVideoTime) / (Note.Info.Duration + NoR2252Application.PreLoad)));
        //         aKs [2].time = aKs [1].time + .01f;

        //         //lineRenderer.colorGradient.SetKeys (cKs, aKs);

        //         //lineRenderer.colorGradient.alphaKeys [1].time = (Note.Info.endTime - NoR2252Application.RawVideoTime) / (Note.Info.Duration + NoR2252Application.PreLoad));
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
        //     Color c = lineColor;
        //     c.a = 0f;
        //     lineRenderer.startColor = lineColor;
        //     lineRenderer.endColor = c;
        //     Vector3 tmp = main.ScreenToViewportPoint (Note.Info.pos);
        //     Vector3 start = tmp;
        //     start.y = 0f;
        //     Vector3 end = tmp;
        //     end.y = 1f;
        //     Vector3 sPos = main.ViewportToWorldPoint (start);
        //     sPos.z = 0f;
        //     Vector3 ePos = main.ViewportToWorldPoint (end);
        //     ePos.z = 0f;
        //     lineRenderer.SetPosition (0, sPos);
        //     lineRenderer.SetPosition (1, ePos);
        //     lineRenderer.enabled = true;

        // }
        // protected override void OnCleared ( ) {
        //     lineRenderer.enabled = false;
        //     ResultTextController.Instance.Recycle (resultText);
        //     resultText = null;
        //     base.OnCleared ( );
        // }

    }
}
