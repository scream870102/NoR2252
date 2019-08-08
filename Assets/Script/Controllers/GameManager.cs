using NoR2252.Models;
using NoR2252.Utils;

using UnityEngine;
public class GameManager : Eccentric.Utils.TSingletonMonoBehavior<GameManager> {
    [SerializeField] bool IsTest = false;
    [SerializeField] TextAsset sheet;
    public Texture2D tex;
    private void Start ( ) {
        if (IsTest) {
            NoR2252Application.Option = new Option ( );
            GameSheet s = SourceLoader.LoadSheet (sheet, tex);
            NoR2252Application.CurrentSheet = s;
        }
    }

}
