using NoR2252.Models;
using NoR2252.Utils;

using UnityEngine;
public class GameManager : Eccentric.Utils.TSingletonMonoBehavior<GameManager> {
#if (UNITY_EDITOR) 
    [SerializeField] bool IsTest = false;
    [SerializeField] TextAsset sheet;
    [SerializeField] Texture2D tex;
    private void Start ( ) {
        if (IsTest) {
            NoR2252Application.Option = new Option (0f, 0.5f, 0.3f);
            GameSheet s = SourceLoader.LoadSheet (sheet, tex);
            NoR2252Application.CurrentSheet = s;
        }
    }
#endif

}
