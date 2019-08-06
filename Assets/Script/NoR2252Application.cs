using NoR2252.Models;

using UnityEngine;
public static class NoR2252Application {
    /// <summary>what the sheet should game scene play</summary>
    public static GameSheet CurrentSheet { get; set; }
    /// <summary>the video time current which plus the player offset Option</summary>
    public static float VideoTime { get; set; }
    /// <summary>the raw video time current</summary>
    public static float RawVideoTime { get; set; }
    /// <summary>how long should the note before its judge time the value is from the sheet</summary>
    public static float PreLoad { get; set; }
    /// <summary>the size of all note should transfer to due to sheet setting</summary>
    public static float Size { get; set; }
    /// <summary>the total combo of current sheet</summary>
    public static int TotalCombo;
    /// <summary>the max combo player get</summary>
    public static int MaxCombo;
    /// <summary>the score of current game</summary>
    public static int Score;
    /// <summary>the total score of current sheet</summary>
    public static int TotalScore;
    /// <summary>the counter of all kind og grade player get in this game</summary>
    public static int [ ] NoteGrade = new int [5];
    /// <summary>the option of player include offset and volume</summary>
    public static Option Option { get; set; }
    /// <summary>this record the best score player get on each song</summary>
    public static ScoreBoard ScoreBoard { get; set; }

}
