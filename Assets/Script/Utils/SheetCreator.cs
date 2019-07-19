using UnityEditor;

using UnityEngine;
namespace NoR2252.Utils {
    public class SheetCreator : MonoBehaviour {
        [SerializeField] NoR2252.Models.Sheet t;
        public void Create ( ) {
            NoR2252.Utils.SourceLoader.CreateSheet (t);
            Debug.Log (t.name);
        }
    }

    [CustomEditor (typeof (SheetCreator))]
    public class SheetCreatorEditor : Editor {
        public override void OnInspectorGUI ( ) {
            DrawDefaultInspector ( );

            SheetCreator myScript = (SheetCreator) target;
            if (GUILayout.Button ("Create Sheet")) {
                myScript.Create ( );
            }
        }
    }

}
