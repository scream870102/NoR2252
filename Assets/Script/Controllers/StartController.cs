using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartController : MonoBehaviour {
    [SerializeField] Button StartBtn;
    [SerializeField] Button OptionBtn;
    [SerializeField] Button AboutBtn;
    void Start ( ) {
        StartBtn.onClick.AddListener (OnStartClicked);
        OptionBtn.onClick.AddListener (OnOptionClicked);
        AboutBtn.onClick.AddListener (OnAboutClicked);
    }
    void OnStartClicked ( ) {
        SceneManager.LoadScene ("Select");
    }
    void OnOptionClicked ( ) {
        Debug.Log ("Option Clicked");
    }
    void OnAboutClicked ( ) {
        Application.Quit ( );
    }
}
