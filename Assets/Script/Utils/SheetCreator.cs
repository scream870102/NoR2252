using System.Collections.Generic;
using System.Text.RegularExpressions;

using NoR2252.Models;

using UnityEditor;

using UnityEngine;
#if (UNITY_EDITOR) 
namespace NoR2252.Utils {
    /// <summary>this is a easy component to create a sheet with aegisub</summary>
    public class SheetCreator : MonoBehaviour {
        [SerializeField] NoR2252.Models.Sheet t;
        [SerializeField] [Multiline] string NoteString;
        [SerializeField] TextAsset SheetToLoad;
        public void Create ( ) {
            SourceLoader.CreateSheet (t);
            Debug.Log (t.name + " save successfully");
        }
        public void ConverString ( ) {

            //time and style section
            string pattern = @"\d:(?<stm>\d+):(?<sts>\d+.\d+),\d:(?<etm>\d+):(?<ets>\d+.\d+),(?<style>\w+-?\w*)";
            Regex regex = new Regex (pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches (NoteString);
            float slideHT = 0f;
            List<SheetNote> notes = new List<SheetNote> ( );
            int index = 0;
            foreach (Match match in matches) {
                GroupCollection g = match.Groups;
                float sTime = float.Parse (g ["stm"].Value) * 60f + float.Parse (g ["sts"].Value);
                float eTime = float.Parse (g ["etm"].Value) * 60f + float.Parse (g ["ets"].Value);
                int type = 0;
                int nextId = 0;
                if (g ["style"].Value == "TAP") {
                    sTime = (sTime + eTime) / 2f;
                    eTime = sTime;
                    type = (int) ENoteType.TAP;
                }
                else if (g ["style"].Value == "HOLD") {
                    type = (int) ENoteType.HOLD;
                }
                else if (g ["style"].Value == "FLICK") {
                    sTime = (sTime + eTime) / 2f;
                    eTime = sTime;
                    type = (int) ENoteType.FLICK;
                }
                else if (g ["style"].Value == "SLIDE-HEAD") {
                    sTime = (sTime + eTime) / 2f;
                    eTime = sTime;
                    slideHT = sTime;
                    type = (int) ENoteType.SLIDE_HEAD;
                    nextId = index + 1;
                }
                else if (g ["style"].Value == "SLIDE-CHILD") {
                    sTime = slideHT;
                    type = (int) ENoteType.SLIDE_CHILD;
                    nextId = index + 1;
                }
                notes.Add (new SheetNote (sTime, eTime, type, index, nextId));
                index++;
            }

            //pos section
            string pattern1 = @"\\pos\((?<x>\d+),(?<y>\d+)\)";
            Regex regex1 = new Regex (pattern1, RegexOptions.IgnoreCase);
            MatchCollection matches1 = regex1.Matches (NoteString);
            index = 0;
            foreach (Match match in matches1) {
                GroupCollection g = match.Groups;
                notes [index].pos = new Vector3 (float.Parse (g ["x"].Value), float.Parse (g ["y"].Value), 0f);
                notes [index].endPos = notes [index].pos;
                index++;
            }
            for (index = 0; index < notes.Count; index++) {
                if (notes [index].type == (int) ENoteType.SLIDE_CHILD && index + 1 < notes.Count && notes [index + 1].type != (int) ENoteType.SLIDE_CHILD)
                    notes [index].nextId = 0;
                if (notes [index].type == (int) ENoteType.SLIDE_CHILD && index + 1 >= notes.Count)
                    notes [index].nextId = 0;
            }
            t.notes = notes;
            Debug.Log ("Convert finished");
        }
        public void LoadSheet ( ) {
            t = SourceLoader.LoadSheetToInspector (SheetToLoad);

            Debug.Log ("Load successful");
        }

        [CustomEditor (typeof (SheetCreator))]
        public class SheetCreatorEditor : Editor {
            public override void OnInspectorGUI ( ) {
                DrawDefaultInspector ( );
                SheetCreator myScript = (SheetCreator) target;
                if (GUILayout.Button ("Load Sheet")) {
                    myScript.LoadSheet ( );
                }
                if (GUILayout.Button ("Convert string to notes")) {
                    myScript.ConverString ( );
                }
                if (GUILayout.Button ("Create Sheet")) {
                    myScript.Create ( );
                }
            }
        }
    }
}
#endif
