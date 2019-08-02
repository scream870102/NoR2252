using System.Collections;
using System.Collections.Generic;

using NoR2252.Models;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ResultController : MonoBehaviour {
    [SerializeField] Text title;
    [SerializeField] Text author;
    [SerializeField] Text result;
    [SerializeField] Text perfect;
    [SerializeField] Text great;
    [SerializeField] Text good;
    [SerializeField] Text bad;
    [SerializeField] Text miss;
    [SerializeField] Text score;
    [SerializeField] Text combo;
    [SerializeField] Button nextBtn;
    [SerializeField] Button retryBtn;
    void Awake ( ) {
        nextBtn.onClick.AddListener (OnNextBtnClicked);
        retryBtn.onClick.AddListener (OnRetryBtnClicked);
    }
    void Start ( ) {
        title.text = NoR2252Application.CurrentSheet.name;
        author.text = NoR2252Application.CurrentSheet.author;
        result.text = GetResult ((float) NoR2252Application.Score / NoR2252Application.TotalScore);
        perfect.text = NoR2252Application.NoteGrade [(int) ENoteGrade.PERFECT].ToString ( );
        great.text = NoR2252Application.NoteGrade [(int) ENoteGrade.GREAT].ToString ( );
        good.text = NoR2252Application.NoteGrade [(int) ENoteGrade.GOOD].ToString ( );
        bad.text = NoR2252Application.NoteGrade [(int) ENoteGrade.BAD].ToString ( );
        miss.text = NoR2252Application.NoteGrade [(int) ENoteGrade.MISS].ToString ( );
        score.text = NoR2252Application.Score.ToString ( );
        combo.text = NoR2252Application.MaxCombo + " / " + NoR2252Application.TotalCombo;
        //Try to get the best score
        NoR2252Application.ScoreBoard.Modify (NoR2252Application.CurrentSheet.name, NoR2252Application.Score);

    }

    // Update is called once per frame
    void Update ( ) {

    }
    void OnNextBtnClicked ( ) {
        SceneManager.LoadScene ("Select");
    }
    void OnRetryBtnClicked ( ) {
        SceneManager.LoadScene ("Game");
    }

    string GetResult (float result) {
        EGrade grade;
        if (result >= 0.9f)
            grade = EGrade.S;
        else if (result >= 0.8f)
            grade = EGrade.A;
        else if (result >= 0.7f)
            grade = EGrade.B;
        else if (result >= 0.6f)
            grade = EGrade.C;
        else
            grade = EGrade.F;
        return grade.ToString ( );

    }
}
