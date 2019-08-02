using NoR2252.Models;

using UnityEngine;
public static class NoR2252Application {
    public static GameSheet CurrentSheet { get; set; }
    public static float VideoTime { get; set; }
    public static float RawVideoTime { get; set; }
    public static float PreLoad { get; set; }
    public static float Size { get; set; }
    public static int TotalCombo;
    public static int MaxCombo;
    public static int Score;
    public static int TotalScore;
    public static int [ ] NoteGrade = new int [5];
    public static Option Option { get; set; }
    public static BestScore ScoreBoard { get; set; }

}
