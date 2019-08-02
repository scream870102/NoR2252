using System.Collections;
using System.Collections.Generic;
using System.IO;

using Eccentric;

using NoR2252.Models;

using UnityEditor;

using UnityEngine;
#if (UNITY_EDITOR) 
public class test : MonoBehaviour {
    public BestScore pair;
    void Start ( ) { }
    void Update ( ) { }
    void Best ( ) {
        string path = Application.persistentDataPath + "/Option/bestscore.json";
        FileStream fs = new FileStream (path, FileMode.Create);
        string fileContext = JsonUtility.ToJson (pair);
        StreamWriter file = new StreamWriter (fs);
        file.Write (fileContext);
        file.Close ( );
    }

    [CustomEditor (typeof (test))]
    public class testEditor : Editor {
        public override void OnInspectorGUI ( ) {
            DrawDefaultInspector ( );
            test myScript = (test) target;
            if (GUILayout.Button ("Create Bestscore")) {
                myScript.Best ( );
            }
        }
    }
}
#endif
