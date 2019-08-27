using System.IO;

using NoR2252.Models;

using UnityEditor;

using UnityEngine;
namespace NoR2252.Utils {
#if (UNITY_EDITOR) 
    /// <summary>this is a easy component to create a sheet with aegisub</summary>
    public class BundleListCreator : MonoBehaviour {
        [SerializeField]
        BundleList list = new BundleList ( );
        void CreateList ( ) {
            string folderPath = Application.persistentDataPath + "/Option";
            string path = Application.persistentDataPath + "/Option/SheetList.json";
            if (!Directory.Exists (folderPath))
                Directory.CreateDirectory (folderPath);
            FileStream fs = new FileStream (path, FileMode.Create);
            string fileContext = JsonUtility.ToJson (list);
            StreamWriter file = new StreamWriter (fs);
            file.Write (fileContext);
            file.Close ( );
        }

        [CustomEditor (typeof (BundleListCreator))]
        public class BundleListCreatorEditor : Editor {
            public override void OnInspectorGUI ( ) {
                DrawDefaultInspector ( );
                BundleListCreator myScript = (BundleListCreator) target;
                if (GUILayout.Button ("Create List"))
                    myScript.CreateList ( );
            }
        }
    }
#endif
}
