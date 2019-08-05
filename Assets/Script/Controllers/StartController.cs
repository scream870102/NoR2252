using System.Collections;
using System.Collections.Generic;

using Lean.Touch;

using NoR2252.Models;
using NoR2252.Utils;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartController : MonoBehaviour {
    //UI REF
    [SerializeField] GameObject OptionMenu;
    [SerializeField] Button CheckBtn;
    [SerializeField] Button BackBtn;
    [SerializeField] Slider VolumeSlider;
    [SerializeField] Slider OffsetSlider;
    [SerializeField] Button OptionBtn;
    [SerializeField] LeanTouch touch;
    [SerializeField] Text OffsetText;
    [SerializeField] Text VolumeText;
    bool bOptionClicked = false;
    Option options = null;
    void Awake ( ) {
        //subscribe ui event
        bOptionClicked = false;
        touch.enabled = false;
        OptionBtn.onClick.AddListener (OnOptionClicked);
        CheckBtn.onClick.AddListener (OnCheckClicked);
        BackBtn.onClick.AddListener (OnBackClicked);
        VolumeSlider.onValueChanged.AddListener (OnVolumeValueChanged);
        OffsetSlider.onValueChanged.AddListener (OnOffsetValueChanged);
    }
    void Start ( ) {
        NoR2252Application.Option = SourceLoader.LoadOption ( );
        OptionMenu.SetActive (false);
    }

    void OnOptionClicked ( ) {
        bOptionClicked = true;
        options = new Option (NoR2252Application.Option);
        UpdateOptionUI ( );
        OptionMenu.SetActive (true);
    }
    
    //update all the ui on the option panel
    void UpdateOptionUI ( ) {
        VolumeSlider.value = options.Volume;
        OffsetSlider.value = options.Offset;
        VolumeText.text = options.Volume.ToString ( );
        OffsetText.text = options.Offset.ToString ( );
    }
    
    void OnCheckClicked ( ) {
        NoR2252Application.Option = this.options;
        SourceLoader.SaveOption ( );
        bOptionClicked = false;
        OptionMenu.SetActive (false);
        EnableTouch ( );
    }

    void OnBackClicked ( ) {
        this.options = null;
        bOptionClicked = false;
        OptionMenu.SetActive (false);
        EnableTouch ( );
    }

    void OnOffsetValueChanged (float value) {
        this.options.Offset = value;
        UpdateOptionUI ( );
    }

    void OnVolumeValueChanged (float value) {
        this.options.Volume = value;
        UpdateOptionUI ( );
    }

    //when first enter this scene clear all the finger remain from other scene
    //then enable ths scene
    void EnableTouch ( ) {
        LeanTouch.Fingers.Clear ( );
        touch.enabled = true;
    }

    //if option panel is off then load the select scene
    void Tap (LeanFinger finger) {
        if (!bOptionClicked) {
            SceneManager.LoadScene ("Select");
        }
    }
    
    void OnEnable ( ) {
        LeanTouch.OnFingerTap += Tap;
        EnableTouch ( );

    }
    
    void OnDisable ( ) {
        LeanTouch.OnFingerTap -= Tap;
    }
}
