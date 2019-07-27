using NoR2252.Models;

using UnityEngine;
public static class NoR2252Application {
    static GameSheet currentSheet = null;
    static float videoTime = 0f;
    static float rawVideoTime = 0f;
    static float offset = -0.2f;
    public static GameSheet CurrentSheet { get { return currentSheet; } set { currentSheet = value; } }
    public static float VideoTime { get { return videoTime; } set { videoTime = value; } }
    public static float RawVideoTime { get { return rawVideoTime; } set { rawVideoTime = value; } }
    public static float Offset { get { return offset; } set { offset = value; } }
    public static float PreLoad { get; set; }
    public static float Size { get; set; }

}
