using NoR2252.Models;

using UnityEngine;
public static class NoR2252Application {
    static Sheet currentSheet = null;
    static float videoTime = 0f;
    public static Sheet CurrentSheet { get { return currentSheet; } set { currentSheet = value; } }
    public static float VideoTime { get { return videoTime; } set { videoTime = value; } }
}
