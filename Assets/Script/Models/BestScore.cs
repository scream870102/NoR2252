using System.Collections.Generic;

using NoR2252.Utils;

using UnityEngine;
using UnityEngine.Video;
namespace NoR2252.Models {
    [System.Serializable]
    public class BestScore {
        public List<BestScorePair> bestScores = new List<BestScorePair> ( );
        public BestScorePair Find (string title) {
            foreach (BestScorePair item in bestScores)
                if (item.Title == title) return item;
            return null;
        }
        public void Add (string title, int score = 0) {
            bestScores.Add (new BestScorePair (title, score));
        }
        public void Modify (string title, int score) {
            BestScorePair tmp = this.Find (title);
            if (tmp.Score < score) {
                tmp.Score = score;
                SourceLoader.SaveScoreBoard ( );
            }
        }
    }

    [System.Serializable]
    public class BestScorePair {
        public BestScorePair (string title, int score) {
            this.Title = title;
            this.Score = score;
        }
        public string Title;
        public int Score;

    }
}
