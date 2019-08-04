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
        Debug.Log(sheets [0].music);
        player.url = sheets [0].music;
        //Debug.Log(player.url+player.clip.length+player.clip.name);
        if (player.isPrepared) {
            Debug.Log ("ready");
            player.Play ( );
        }
        else {
            Debug.Log ("Not ready");
            player.Prepare ( );
            player.prepareCompleted+=Prepared;
        }
    }
    void Prepared(UnityEngine.Video.VideoPlayer vPlayer) {
        Debug.Log("End reached!");
        vPlayer.Play();
    }

    void StopVideo ( ) {
        player.Stop ( );
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
            if (GUILayout.Button ("Play Video")) {
                myScript.PlayVideo ( );
            }
            if (GUILayout.Button ("Stop Video")) {
                myScript.StopVideo ( );
            }

        }
    }
}
#endif
