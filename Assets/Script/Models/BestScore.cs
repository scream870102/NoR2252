using System.Collections.Generic;

using NoR2252.Utils;

using UnityEngine;
using UnityEngine.Video;
namespace NoR2252.Models {
    [System.Serializable]
    public class ScoreBoard {
        public List<ScorePair> bestScores = new List<ScorePair> ( );
        public ScorePair Find (string title) {
            foreach (ScorePair item in bestScores)
                if (item.Title == title) return item;
            return null;
        }
        /// <summary>Call this method to add a new record to the score board</summary>
        public void Add (string title, int score = 0) {
            bestScores.Add (new ScorePair (title, score));
        }

        /// <summary>call this method to update the score</summary>
        public void Modify (string title, int score) {
            ScorePair tmp = this.Find (title);
            if (tmp.Score < score) {
                tmp.Score = score;
                SourceLoader.SaveScoreBoard ( );
            }
        }
    }

    [System.Serializable]
    public class ScorePair {
        public ScorePair (string title, int score) {
            this.Title = title;
            this.Score = score;
        }
        public string Title;
        public int Score;

    }
}
