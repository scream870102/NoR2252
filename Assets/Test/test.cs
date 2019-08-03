using System.Collections;
using System.Collections.Generic;
using System.IO;

using Eccentric;

using NoR2252.Models;
using NoR2252.Utils;

using UnityEditor;

using UnityEngine;
using UnityEngine.Video;
#if (UNITY_EDITOR) 
public class test : MonoBehaviour {
    public VideoPlayer player;
    public List<AssetBundle> bundles = new List<AssetBundle> ( );
    public List<GameSheet> sheets = new List<GameSheet> ( );
    void LoadAllAssetBundle ( ) {
        bundles.AddRange (SourceLoader.LoadAllAssetBundle ( ));
        // string path = Application.persistentDataPath + "/Bundle";
        // string [ ] allfile = Directory.GetFiles (path);
        // foreach (string file in allfile) {
        //     //沒有副檔名是我們要讀取的file
        //     if (!Path.HasExtension (file)) {
        //         AssetBundle SheetBundle = AssetBundle.LoadFromFile (Path.Combine (path, Path.GetFileNameWithoutExtension (file)));
        //         if (SheetBundle == null) {
        //             Debug.Log ("Failed to load AssetBundle!");
        //             return;
        //         }
        //         TextAsset sheetFile = SheetBundle.LoadAsset<TextAsset> (Path.GetFileNameWithoutExtension (file));
        //         Sheet sheet = SourceLoader.LoadSheetFromBundle (sheetFile);
        //         if (sheet != null) {
        //             s = sheet;
        //             Texture2D tex = SheetBundle.LoadAsset<Texture2D> (s.cover);
        //             texture = tex;
        //             VideoClip clip = SheetBundle.LoadAsset<VideoClip> (s.music);
        //             video = clip;
        //             Debug.Log("Finish");

        //         }

        //     }
        // }
    }
    void LoadAllSheetFromBundles ( ) {
        foreach (AssetBundle bundle in bundles) {
            sheets.Add (SourceLoader.ConvertBundleToGameSheet (bundle));
        }
    }
    void UnLoadAllBundle ( ) {
        AssetBundle.UnloadAllAssetBundles (true);
        bundles.Clear ( );
        sheets.Clear ( );
    }

    void PlayVideo ( ) {
        player.Play ( );
    }

    [CustomEditor (typeof (test))]
    public class testEditor : Editor {
        public override void OnInspectorGUI ( ) {
            DrawDefaultInspector ( );
            test myScript = (test) target;
            if (GUILayout.Button ("Load AssetBundle")) {
                myScript.LoadAllAssetBundle ( );
            }
            if (GUILayout.Button ("Load All Sheets")) {
                myScript.LoadAllSheetFromBundles ( );
            }
            if (GUILayout.Button ("Unload All AssetBundle")) {
                myScript.UnLoadAllBundle ( );
            }
            if (GUILayout.Button ("Play Vidoe")) {
                myScript.PlayVideo ( );
            }

        }
    }
}
#endif
