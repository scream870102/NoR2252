using System.Collections;
using System.Collections.Generic;
using System.IO;

using NoR2252.Models;

using UnityEngine;

namespace NoR2252.Utils {
    public static class SourceLoader {
        public static Sheet LoadSheet (TextAsset textAsset) {
            string content = textAsset.text;
            Sheet sheet = JsonUtility.FromJson<Sheet> (content);
            return sheet;
        }
        
        public static void CreateSheet (Sheet sheet) {
            string path = Application.persistentDataPath + "/Sheet/" + sheet.name + ".json";
            FileStream fs = new FileStream (path, FileMode.Create);
            string fileContext = JsonUtility.ToJson (sheet);
            StreamWriter file = new StreamWriter (fs);
            file.Write (fileContext);
            file.Close ( );
        }
    }
}
