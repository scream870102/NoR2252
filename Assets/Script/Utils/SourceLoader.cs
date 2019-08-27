using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Firebase.Storage;

using NoR2252.Models;

using UnityEngine;

namespace NoR2252.Utils {
    public static class SourceLoader {
        #region FIREBASE
        /// <summary>Call this method to load bundleList from firebase</summary>
        /// <example><code>
        /// await SourceLoader.LoadBundleList ( ).ContinueWith (
        ///         (task) => {
        ///             Debug.Log ("LoadBundleListfinish");
        ///             bundles.AddRange (task.Result);
        ///         }
        /// );
        /// SourceLoader.DownloadAllBundles (bundles);
        ///</code></example>
        public async static Task<List<string>> LoadBundleList ( ) {
            List<string> bundleList = new List<string> ( );
            FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
            StorageReference reference = storage.GetReferenceFromUrl (FirebaseConst.SheetListPath);
            //check local path
            string path = Application.persistentDataPath + "/Option";
            if (!Directory.Exists (path)) Directory.CreateDirectory (path);
            path = Application.persistentDataPath + "/Option/SheetList.json";
            //download json file
            Task operation = reference.GetFileAsync (path);
            while (!operation.IsCompleted) {
                await Task.Delay (1);
            }
            //load file from local
            BundleList textFile = JsonUtility.FromJson<BundleList> (File.ReadAllText (path));
            bundleList.AddRange (textFile.bundles);
            return bundleList;
        }
        /// <summary>Call this method to download all bundle from firebase</summary>
        /// <param name="bundleList">list of bundle should load</param>
        public async static Task DownloadAllBundles (List<string> bundleList) {
            List<string> netBundles = bundleList;
            List<string> localBundles = new List<string> ( );
            string path = Application.persistentDataPath + "/Bundle";
            if (!Directory.Exists (path)) Directory.CreateDirectory (path);
            string [ ] allfile = Directory.GetFiles (path);
            //Find all the bundle already downloaded in the local storage
            foreach (string file in allfile) {
                //if the file doesn't have extension will be the assetbundle file
                if (!Path.HasExtension (file)) {
                    localBundles.Add (file);
                }
            }
            //delete the bundle already downloaded from bundleList
            foreach (string bundle in localBundles) {
                int index;
                if ((index = netBundles.FindIndex (s => s == bundle)) != -1) {
                    netBundles.RemoveAt (index);
                }
            }
            //Download other netbundles to local
            FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
            List<Task> downTasks = new List<Task> ( );
            //check local path
            path = Application.persistentDataPath + "/Bundle";
            foreach (string bundle in netBundles) {
                StorageReference reference = storage.GetReferenceFromUrl (FirebaseConst.SheetFolderPath + "/" + bundle);
                path = Application.persistentDataPath + "/Bundle/" + bundle;
                downTasks.Add (reference.GetFileAsync (path));
            }
            await Task.WhenAll (downTasks.ToArray ( ));

        }
        public async static Task DownloadVideo(string netPath,string fileName){
            string localPath=Application.persistentDataPath+"/Bundle/"+fileName;
            FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
            StorageReference reference = storage.GetReferenceFromUrl (netPath);
            await reference.GetFileAsync (localPath);
            Debug.Log("download fin at"+localPath );

        }
        #endregion
        #region ASSET_BUNDLE
        /// <summary>convert the textasset to Sheet</summary>
        public static Sheet LoadSheetFromBundle (TextAsset textAsset) {
            string content = textAsset.text;
            Sheet s = JsonUtility.FromJson<Sheet> (content);
            return s;
        }

        /// <summary>Call this to load all the assetbundle in Bundle folder</summary>
        public static List<AssetBundle> LoadAllAssetBundle ( ) {
            List<AssetBundle> bundles = new List<AssetBundle> ( );
            bundles.AddRange (AssetBundle.GetAllLoadedAssetBundles ( ));
            string path = Application.persistentDataPath + "/Bundle";
            CheckDirectory (path);
            string [ ] allfile = Directory.GetFiles (path);
            foreach (string file in allfile) {
                bool bLoaded = false;
                //if the file doesn't have extension will be the assetbundle file
                if (!Path.HasExtension (file)) {
                    //make sure the bundle is already loaded or not
                    foreach (AssetBundle item in bundles) {
                        if (Path.GetFileNameWithoutExtension (file) == item.name) {
                            bLoaded = true;
                            break;
                        }
                    }
                    if (!bLoaded) {
                        AssetBundle SheetBundle = AssetBundle.LoadFromFile (Path.Combine (path, Path.GetFileNameWithoutExtension (file)));
                        if (SheetBundle == null) {
                            return null;
                        }
                        else
                            bundles.Add (SheetBundle);
                    }
                }
            }
            return bundles;
        }

        /// <summary>get the gamesheet from assetBundle also set the texture and video url</summary>
        public static GameSheet ConvertBundleToGameSheet (AssetBundle bundle) {
            TextAsset sheetFile = bundle.LoadAsset<TextAsset> (bundle.name);
            Sheet s = SourceLoader.LoadSheetFromBundle (sheetFile);
            GameSheet gameSheet = new GameSheet (s.name, s.author, s.bpm, s.notes, s.musicOffset, s.size, s.notePreload, s.screenSize);
            if (s != null) {
                Texture2D tex = bundle.LoadAsset<Texture2D> (s.cover);
                gameSheet.music = "file://" + Application.persistentDataPath + "/Bundle/" + s.music + ".mp4";
                gameSheet.cover = tex;
            }
            return gameSheet;
        }

        /// <summary>call this method to load all the gamesheet in bundle folder</summary>
        public static List<GameSheet> LoadAllSheets ( ) {
            List<AssetBundle> bundles = new List<AssetBundle> ( );
            List<GameSheet> sheets = new List<GameSheet> ( );
            bundles.AddRange (LoadAllAssetBundle ( ));
            foreach (AssetBundle bundle in bundles) {
                sheets.Add (ConvertBundleToGameSheet (bundle));
            }
            return sheets;
        }
        #endregion
#if (UNITY_EDITOR) 
        #region CREATE_SHEET
        /// <summary>create the sheet to the Sheet folder</summary>
        public static void CreateSheet (Sheet sheet) {
            string path = Application.persistentDataPath + "/Sheet/" + sheet.name + ".json";
            FileStream fs = new FileStream (path, FileMode.Create);
            string fileContext = JsonUtility.ToJson (sheet);
            StreamWriter file = new StreamWriter (fs);
            file.Write (fileContext);
            file.Close ( );
        }
        #endregion
#endif
        #region Utils

        /// <summary>check directory exist if not create it</summary>
        public static void CheckDirectory (string path) {
            // Check if directory exists, if not create it
            if (!Directory.Exists (path)) {
                Directory.CreateDirectory (path);
            }
        }
        #endregion
        #region OPTION

        /// <summary>load the option file from option folder</summary>
        public static Option LoadOption ( ) {
            string path = Application.persistentDataPath + "/Option";
            SourceLoader.CheckDirectory (path);
            path = Application.persistentDataPath + "/Option/option.json";
            //option no
            if (!File.Exists (path)) {
                FileStream fs = new FileStream (path, FileMode.Create);
                string fileContext = JsonUtility.ToJson (new Option ( ));
                StreamWriter file = new StreamWriter (fs);
                file.Write (fileContext);
                file.Close ( );
            }
            return JsonUtility.FromJson<Option> (File.ReadAllText (path));

        }

        /// <summary>save the option file to option folder if there is an old version will override it</summary>
        public static void SaveOption ( ) {
            string path = Application.persistentDataPath + "/Option/option.json";
            if (!File.Exists (path)) {
                return;
            }

            FileStream fs = new FileStream (path, FileMode.Create);
            string fileContext = JsonUtility.ToJson (NoR2252Application.Option);
            StreamWriter file = new StreamWriter (fs);
            file.Write (fileContext);
            file.Close ( );
        }
        #endregion
        #region SCORE_BOARD

        /// <summary>load the scoreboard from option folder</summary>
        /// <remarks>if the file is not exist will create a new one</remarks>
        public static ScoreBoard LoadScoreBoard ( ) {
            string path = Application.persistentDataPath + "/Option";
            SourceLoader.CheckDirectory (path);
            path = Application.persistentDataPath + "/Option/scoreBoard.json";
            //option no
            if (!File.Exists (path)) {
                FileStream fs = new FileStream (path, FileMode.Create);
                string fileContext = JsonUtility.ToJson (new ScoreBoard ( ));
                StreamWriter file = new StreamWriter (fs);
                file.Write (fileContext);
                file.Close ( );
            }
            return JsonUtility.FromJson<ScoreBoard> (File.ReadAllText (path));
        }

        /// <summary>save the scoreboard to option folder</summary>
        /// <remarks>if there is an old version will override it</remarks>
        public static void SaveScoreBoard ( ) {
            string path = Application.persistentDataPath + "/Option/scoreBoard.json";
            if (!File.Exists (path)) {
                return;
            }
            FileStream fs = new FileStream (path, FileMode.Create);
            string fileContext = JsonUtility.ToJson (NoR2252Application.ScoreBoard);
            StreamWriter file = new StreamWriter (fs);
            file.Write (fileContext);
            file.Close ( );
        }
        #endregion
#if (UNITY_EDITOR) 
        #region FOR_EDITOR_TEST
        public static GameSheet LoadSheet (TextAsset sheetFile, Texture2D cover) {
            Sheet s = SourceLoader.LoadSheetFromBundle (sheetFile);
            GameSheet gameSheet = new GameSheet (s.name, s.author, s.bpm, s.notes, s.musicOffset, s.size, s.notePreload, s.screenSize);
            if (s != null) {
                gameSheet.music = "file://" + Application.persistentDataPath + "/Editor/" + s.music + ".mp4";
                gameSheet.cover = cover;
            }
            return gameSheet;
        }
        #endregion
#endif
        #region OLD_VER
        //     public static void CreateSheet (Sheet sheet) {
        // sheet.music = "MusicVideo/" + sheet.music;
        // sheet.cover = "Cover/" + sheet.cover;
        // string path = Application.persistentDataPath + "/Sheet/" + sheet.name + ".json";
        // FileStream fs = new FileStream (path, FileMode.Create);
        // string fileContext = JsonUtility.ToJson (sheet);
        // StreamWriter file = new StreamWriter (fs);
        // file.Write (fileContext);
        // file.Close ( );
        //}
        // public static GameSheet LoadSheet (TextAsset textAsset) {
        //     string content = textAsset.text;
        //     Sheet s = JsonUtility.FromJson<Sheet> (content);
        //     GameSheet sheet = new GameSheet (s.name, s.author, s.bpm, s.notes, s.musicOffset, s.size, s.notePreload, s.screenSize);
        //     //sheet.music = Resources.Load<VideoClip> (s.music);
        //     sheet.cover = Resources.Load<Texture2D> (s.cover);
        //     return sheet;
        // }

        // public static GameSheet LoadSheet (string content) {
        //     Sheet s = JsonUtility.FromJson<Sheet> (content);
        //     GameSheet sheet = new GameSheet (s.name, s.author, s.bpm, s.notes, s.musicOffset, s.size, s.notePreload, s.screenSize);
        //     //sheet.music = Resources.Load<VideoClip> (s.music);
        //     sheet.cover = Resources.Load<Texture2D> (s.cover);
        //     return sheet;
        // }

        // public static Sheet LoadSheetToInspector (TextAsset asset) {
        //     string content = asset.text;
        //     Sheet s = JsonUtility.FromJson<Sheet> (content);
        //     string [ ] music = s.music.Split ('/');
        //     string [ ] cover = s.cover.Split ('/');
        //     s.music = music [1];
        //     s.cover = cover [1];
        //     return s;
        // }
        // public static List<GameSheet> LoadAllSheets ( ) {
        //     List<GameSheet> allSheets = new List<GameSheet> ( );
        //     string path = Application.persistentDataPath + "/Sheet";
        //     SourceLoader.CheckDirectory (path);
        //     // Check Save Path
        //     foreach (string fileFullPath in Directory.GetFiles (path)) {
        //         allSheets.Add (LoadSheet (ConvertFileToString (fileFullPath)));
        //     }
        //     return allSheets;
        // }
        // public static string ConvertFileToString (string path) {
        //     if (File.Exists (path)) return File.ReadAllText (path);
        //     else return " ";
        // }
        #endregion

    }
}
