using UnityEditor;

using UnityEngine;
#if (UNITY_EDITOR) 
namespace Eccentric.Utils {
    public class GetInstanceID : MonoBehaviour {
        [SerializeField] Object _object;
        public void GetID ( ) {
            Debug.Log (_object.GetInstanceID ( ));
        }
    }

    [CustomEditor (typeof (GetInstanceID))]
    public class GetInStanceIDEditor : Editor {
        public override void OnInspectorGUI ( ) {
            DrawDefaultInspector ( );

            GetInstanceID myScript = (GetInstanceID) target;
            if (GUILayout.Button ("GetID")) {
                myScript.GetID ( );
            }
        }
    }

}
#endif
