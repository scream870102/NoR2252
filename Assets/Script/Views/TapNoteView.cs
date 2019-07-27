using Eccentric;
using EU = Eccentric.Utils;
using NoR2252.Models;

using UnityEngine;
namespace NoR2252.View.Note {
    [System.Serializable]
    public class TapNoteView : NoteView {
        bool bClockWise;
        bool bResetTimer = false;
        Vector3 initOutlineScale;
        Vector3 initScale;
        Color outLineC;
        Color ptcC;
        readonly float clearTime = .2f;
        readonly float inZoomOutVelo = 0.95f;
        readonly float outZoomOutVelo = 0.75f;
        readonly float inRotVelo = 300f;
        readonly float biggestOutlineScale = 0.05f;
        readonly float outLineTransVelo = 0.1f;

        public TapNoteView (GameNote note, SpriteRenderer renderer, Animation animation, SpriteRenderer secondRend, LineRenderer lineRenderer, ParticleSystem particle, Transform tf, Transform secondTf, Transform lineTf):
            base (note, renderer, animation, secondRend, lineRenderer, particle, tf, secondTf, lineTf) {
                color = NoR2252Data.Instance.NoteColor [(int) ENoteType.TAP];
                outLineC = NoR2252Data.Instance.TapOutLineColor;
                ptcC = NoR2252Data.Instance.TapPtcColor;
                bClockWise = Math.RandomBool ( );
                ParticleSystem.MainModule ptcMain = particle.main;
                ptcMain.startColor = ptcC;
                initOutlineScale = secondTf.localScale;
                initScale = new Vector3 (NoR2252Application.Size, NoR2252Application.Size, 1f);
                secondRend.sprite = NoR2252Data.Instance.NoteOutLineSprite[(int)ENoteType.TAP];

            }
        public override void Render ( ) {
            if (IsRendering) {
                // note inside
                Vector3 insideRot = tf.rotation.eulerAngles;
                if (bClockWise) insideRot.z += inRotVelo * Time.deltaTime;
                else insideRot.z -= inRotVelo * Time.deltaTime;
                tf.rotation = Quaternion.Euler (insideRot);
                //note outside
                Vector3 outsideScale = initOutlineScale;
                outsideScale.x = Mathf.PingPong (Time.time * outLineTransVelo, biggestOutlineScale) + initOutlineScale.x;
                outsideScale.y = Mathf.PingPong (Time.time * outLineTransVelo, biggestOutlineScale) + initOutlineScale.y;
                secondTf.localScale = outsideScale;
                //about color
                if (Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.GOOD] <= NoR2252Application.VideoTime) {
                    Color c = color;
                    Vector3 hsvC = EU.Color.RGB2HSV (c);
                    hsvC.y += Time.deltaTime * 5f;
                    c = EU.Color.HSV2RGB (hsvC);
                    renderer.color = c;
                }
                else if (Note.Info.endTime - NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS] <= NoR2252Application.VideoTime) {
                    renderer.color = color;
                    secondRenderer.color = outLineC;
                    tf.localScale = initScale;
                }
                else {
                    Color c = color;
                    c.a = c.a / 2f;
                    renderer.color = c;
                    Color co = outLineC;
                    co.a = co.a / 2f;
                    secondRenderer.color = co;
                    tf.localScale = initScale * Math.InverseProbability ((Note.Info.startTime - NoR2252Application.VideoTime) / (NoR2252Application.PreLoad + NoR2252Data.Instance.TimeGrade [(int) ENoteGrade.MISS]));
                }
                if (bClearing) {
                    Vector3 tmpScale = secondTf.localScale * inZoomOutVelo;
                    secondTf.localScale = tmpScale;
                    Vector3 tpScale = tf.localScale * outZoomOutVelo;
                    tf.localScale = tpScale;

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
                    particle.Play ( );
                }
                resultText = ResultTextController.Instance.SetResult (grade, Note.transform.position);
            }   
        }
        public override void OnSpawn ( ) {
            base.OnSpawn ( );
            secondRenderer.enabled = true;
            secondRenderer.color = outLineC;
        }
        protected override void OnCleared ( ) {
            secondTf.localScale = initOutlineScale;
            secondRenderer.enabled = false;
            ResultTextController.Instance.Recycle (resultText);
            resultText = null;
            base.OnCleared ( );
        }

    }
}
