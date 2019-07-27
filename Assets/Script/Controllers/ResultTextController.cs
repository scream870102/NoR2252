using System.Collections.Generic;

using NoR2252.Models;

using UnityEngine;
using UnityEngine.UI;
public class ResultTextController : Eccentric.Utils.TSingletonMonoBehavior<ResultTextController> {
    [SerializeField] List<GameObject> resultObjects;
    [SerializeField] List<TextAndAnim> texts = new List<TextAndAnim> ( );
    Camera main;
    private void Start ( ) {
        main = Camera.main;
        foreach (GameObject t in resultObjects) {
            TextAndAnim ta = new TextAndAnim (t.GetComponent<Text> ( ), t.GetComponent<Animation> ( ));
            texts.Add (ta);
            ta.IsActive = false;
        }
    }
    public TextAndAnim SetResult (ENoteGrade grade, Vector2 pos) {

        //find not using
        foreach (TextAndAnim t in texts) {
            if (!t.IsActive) {
                t.text.text = grade.ToString ( );
                Vector2 p = main.WorldToScreenPoint (pos);
                t.text.rectTransform.anchoredPosition = p;
                t.IsActive = true;
                t.animation.Play ( );
                return t;
            }
            break;
        }
        Debug.Log ("can't get result text");
        return null;
    }

    public void Recycle (TextAndAnim o) {
        if (o != null) {
            o.IsActive = false;
        }
    }

    [System.Serializable]
    public class TextAndAnim {
        public TextAndAnim (Text text, Animation anim) {
            this.text = text;
            this.animation = anim;
        }
        public Text text;
        public Animation animation;
        bool bActive = false;
        public bool IsActive {
            get {
                return bActive;
            }
            set {
                this.text.enabled = value;
                this.animation.enabled = value;
                bActive = value;
            }
        }
    }
}
