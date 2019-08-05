using System.Collections.Generic;

using NoR2252.Models;

using UnityEngine;
using UnityEngine.UI;
public class ResultTextController : MonoBehaviour {
    //how many result objects have drag from inspector
    [SerializeField] List<GameObject> resultObjects;
    List<TextAndAnim> texts = new List<TextAndAnim> ( );
    [SerializeField] CanvasScaler scaler;
    Camera main;

    private void Start ( ) {
        main = Camera.main;
        //change the scaler ref resolution to current device
        scaler.referenceResolution = new Vector2 (Screen.width, Screen.height);
        // get all then animation and text component due to resultObjects
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
#if (UNITY_EDITOR) 
        Debug.Log ("can't get result text");
#endif
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
