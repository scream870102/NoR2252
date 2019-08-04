using System.Collections;
using System.Collections.Generic;
using System.IO;

using NoR2252.Models;

using UnityEngine;
using UnityEngine.Video;

namespace NoR2252.Utils {
    public static class SourceLoader {
        #region ASSET_BUNDLE
        public static Sheet LoadSheetFromBundle (TextAsset textAsset) {
            string content = textAsset.text;
            Sheet s = JsonUtility.FromJson<Sheet> (content);
            return s;
        }
        public static List<AssetBundle> LoadAllAssetBundle ( ) {
            List<AssetBundle> bundles = new List<AssetBundle> ( );
            bundles.AddRange (AssetBundle.GetAllLoadedAssetBundles ( ));
            string path = Application.persistentDataPath + "/Bundle";
            CheckDirectory (path);
            string [ ] allfile = Directory.GetFiles (path);
            foreach (string file in allfile) {
                bool bLoaded = false;
                //沒有副檔名是我們要讀取的file
                if (!Path.HasExtension (file)) {
                    foreach (AssetBundle item in bundles) {
                        if (Path.GetFileNameWithoutExtension (file) == item.name) {
                            bLoaded = true;
                            break;
                        }
                    }
                    if (!bLoaded) {
                        AssetBundle SheetBundle = AssetBundle.LoadFromFile (Path.Combine (path, Path.GetFileNameWithoutExtension (file)));
                        if (SheetBundle == null) {
                            Debug.Log ("Failed to load AssetBundle!");
                            return null;
                        }
                        else
                            bundles.Add (SheetBundle);
                    }
                }
            }
            return bundles;
        }

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
        #region CREATE_SHEET
        public static void CreateSheet (Sheet sheet) {
            string path = Application.persistentDataPath + "/Sheet/" + sheet.name + ".json";
            FileStream fs = new FileStream (path, FileMode.Create);
            string fileContext = JsonUtility.ToJson (sheet);
            StreamWriter file = new StreamWriter (fs);
            file.Write (fileContext);
            file.Close ( );
        }
        #endregion
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
        #region Utils
        //check directory if exist
        public static void CheckDirectory (string path) {
            // Check if directory exists, if not create it
            if (!Directory.Exists (path)) {
                Directory.CreateDirectory (path);
            }
        }
        #endregion
        #region OPTION
        public static Option LoadOption ( ) {
            string path = Application.persistentDataPath + "/Option";
            SourceLoader.CheckDirectory (path);
            path = Application.persistentDataPath + "/Option/option.json";
            //option no
            if (!File.Exists (path)) {
                FileStream fs = new FileStream (path, FileMode.Create);
                string fileContext = JsonUtility.ToJson (new Option ( ));
                Debug.Log ("Try to create" + fileContext);
                StreamWriter file = new StreamWriter (fs);
                file.Write (fileContext);
                file.Close ( );
            }
            return JsonUtility.FromJson<Option> (File.ReadAllText (path));

        }

        public static void SaveOption ( ) {
            string path = Application.persistentDataPath + "/Option/option.json";
            if (!File.Exists (path)) {
                Debug.Log ("None Option Exist");
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
        public static BestScore LoadScoreBoard ( ) {
            string path = Application.persistentDataPath + "/Option";
            SourceLoader.CheckDirectory (path);
            path = Application.persistentDataPath + "/Option/scoreBoard.json";
            //option no
            if (!File.Exists (path)) {
                FileStream fs = new FileStream (path, FileMode.Create);
                string fileContext = JsonUtility.ToJson (new BestScore ( ));
                Debug.Log ("Try to create" + fileContext);
                StreamWriter file = new StreamWriter (fs);
                file.Write (fileContext);
                file.Close ( );
            }
            return JsonUtility.FromJson<BestScore> (File.ReadAllText (path));
        }

        public static void SaveScoreBoard ( ) {
            string path = Application.persistentDataPath + "/Option/scoreBoard.json";
            if (!File.Exists (path)) {
                Debug.Log ("None scoreBoard Exist");
                return;
            }
            FileStream fs = new FileStream (path, FileMode.Create);
            string fileContext = JsonUtility.ToJson (NoR2252Application.ScoreBoard);
            StreamWriter file = new StreamWriter (fs);
            file.Write (fileContext);
            file.Close ( );
        }
        #endregion

    }
}
