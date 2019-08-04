using NoR2252.Models;
using NoR2252.Utils;

using UnityEngine;
public class GameManager : Eccentric.Utils.TSingletonMonoBehavior<GameManager> {
#if (UNITY_EDITOR) 
    // [SerializeField] TextAsset Sheet;
    // [SerializeField] bool IsTest;
    // GameSheet SheetToPlay;
    // void Start ( ) {
    //     if (IsTest) {
    //         SheetToPlay = SourceLoader.LoadSheetFromBundle (Sheet);
    //         NoR2252Application.CurrentSheet = SheetToPlay;
    //     }
    // }
#endif
}
