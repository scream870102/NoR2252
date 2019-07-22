using NoR2252.Models;
using NoR2252.Utils;

using UnityEngine;
public class GameManager : Eccentric.Utils.TSingletonMonoBehavior<GameManager> {
    [SerializeField] TextAsset Sheet;
    Sheet SheetToPlay;
    void Start ( ) {
        SheetToPlay = SourceLoader.LoadSheet (Sheet);
        NoR2252Application.CurrentSheet = SheetToPlay;
    }
}
