using NoR2252.Models;

using UnityEngine;
public static class NoR2252Application {
    static Sheet currentSheet = null;
    static double videoTime = 0.0;
    public static Sheet CurrentSheet { get { return currentSheet; } set { currentSheet = value; } }
    public static double VideoTime { get { return videoTime; } set { videoTime = value; } }
}
