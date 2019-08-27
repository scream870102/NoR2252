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
    [SerializeField] Button OptionBtn;
    [SerializeField] Button CheckBtn;
    [SerializeField] Button BackBtn;
    [SerializeField] LeanTouch touch;
    [SerializeField] Text ProgressText;
    [Header ("Volume")]
    [SerializeField] Slider VolumeSlider;
    [SerializeField] Text VolumeText;
    [Header ("Offset")]
    [SerializeField] Slider OffsetSlider;
    [SerializeField] Text OffsetText;
    [Header ("Opacity")]
    [SerializeField] Slider OpacitySlider;
    [SerializeField] Text OpacityText;
    bool bOptionClicked = false;
    bool bBundleDownFined = false;
    Option options = null;
    List<string> bundles = new List<string> ( );
    void Awake ( ) {
        //subscribe ui event
        bOptionClicked = false;
        bBundleDownFined = false;
        touch.enabled = false;
        OptionBtn.onClick.AddListener (OnOptionClicked);
        CheckBtn.onClick.AddListener (OnCheckClicked);
        BackBtn.onClick.AddListener (OnBackClicked);
        VolumeSlider.onValueChanged.AddListener (OnVolumeValueChanged);
        OffsetSlider.onValueChanged.AddListener (OnOffsetValueChanged);
        OpacitySlider.onValueChanged.AddListener (OnOpacityValueChanged);
    }
    async void Start ( ) {
        NoR2252Application.Option = SourceLoader.LoadOption ( );
        OptionMenu.SetActive (false);
        ProgressText.text = "Updating SheetList ...";
        await SourceLoader.LoadBundleList ( ).ContinueWith (
            (task) => {
                bundles.AddRange (task.Result);
            }
        );
        ProgressText.text = "Downloading All Sheets";
        await SourceLoader.DownloadAllBundles (bundles).ContinueWith (
            (task) => {
                bBundleDownFined = true;
            }
        );
        ProgressText.text = "Already Downloaded All Sheets";
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
        OpacitySlider.value = options.Opacity;
        VolumeText.text = options.Volume.ToString ( );
        OffsetText.text = options.Offset.ToString ( );
        OpacityText.text = options.Opacity.ToString ( );
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
    void OnOpacityValueChanged (float value) {
        this.options.Opacity = value;
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
        if (!bOptionClicked && bBundleDownFined) {
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
