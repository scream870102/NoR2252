using System.Collections;
using System.Collections.Generic;
using System.IO;

using NoR2252.Models;

using UnityEngine;
using UnityEngine.Video;

namespace NoR2252.Utils {
    public static class SourceLoader {
        public static GameSheet LoadSheet (TextAsset textAsset) {
            string content = textAsset.text;
            Sheet s = JsonUtility.FromJson<Sheet> (content);
            GameSheet sheet = new GameSheet (s.name, s.author, s.bpm, s.notes, s.musicOffset, s.size, s.notePreload, s.screenSize);
            sheet.music = Resources.Load<VideoClip> (s.music);
            sheet.cover = Resources.Load<Texture2D> (s.cover);
            return sheet;
        }
        public static GameSheet LoadSheet (string content) {
            Sheet s = JsonUtility.FromJson<Sheet> (content);
            GameSheet sheet = new GameSheet (s.name, s.author, s.bpm, s.notes, s.musicOffset, s.size, s.notePreload, s.screenSize);
            sheet.music = Resources.Load<VideoClip> (s.music);
            sheet.cover = Resources.Load<Texture2D> (s.cover);
            return sheet;
        }

        public static Sheet LoadSheetToInspector (TextAsset asset) {
            string content = asset.text;
            Sheet s = JsonUtility.FromJson<Sheet> (content);
            string [ ] music = s.music.Split ('/');
            string [ ] cover = s.cover.Split ('/');
            s.music = music [1];
            s.cover = cover [1];
            return s;
        }

        public static void CreateSheet (Sheet sheet) {
            sheet.music = "MusicVideo/" + sheet.music;
            sheet.cover = "Cover/" + sheet.cover;
            string path = Application.persistentDataPath + "/Sheet/" + sheet.name + ".json";
            FileStream fs = new FileStream (path, FileMode.Create);
            string fileContext = JsonUtility.ToJson (sheet);
            StreamWriter file = new StreamWriter (fs);
            file.Write (fileContext);
            file.Close ( );
        }
        
        //check directory if exist
        public static void CheckDirectory (string path) {
            // Check if directory exists, if not create it
            if (!Directory.Exists (path)) {
                Directory.CreateDirectory (path);
            }
        }
        public static List<GameSheet> LoadAllSheets ( ) {
            List<GameSheet> allSheets = new List<GameSheet> ( );
            string path = Application.persistentDataPath + "/Sheet";
            SourceLoader.CheckDirectory (path);
            // Check Save Path
            foreach (string fileFullPath in Directory.GetFiles (path)) {
                allSheets.Add (LoadSheet (ConvertFileToString (fileFullPath)));
            }
            return allSheets;
        }

        public static string ConvertFileToString (string path) {
            if (File.Exists (path)) return File.ReadAllText (path);
            else return " ";
        }
    }
}
