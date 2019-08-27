using NoR2252.Models;
using NoR2252.Utils;

using UnityEngine;
public class GameManager : Eccentric.Utils.TSingletonMonoBehavior<GameManager> {
#if (UNITY_EDITOR) 
    [SerializeField] bool IsTest = false;
    [SerializeField] TextAsset sheet;
    [SerializeField] Texture2D tex;
    private void Start ( ) {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync ( ).ContinueWith (task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else {
                UnityEngine.Debug.LogError (System.String.Format (
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        if (IsTest) {
            NoR2252Application.Option = new Option (0f, 0.5f, 0.3f);
            GameSheet s = SourceLoader.LoadSheet (sheet, tex);
            NoR2252Application.CurrentSheet = s;
        }
    }
#endif

}
