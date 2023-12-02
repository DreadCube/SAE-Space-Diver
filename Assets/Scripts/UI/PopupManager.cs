using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [SerializeField]
    private VisualTreeAsset optionsVisuals;

    public void ShowOptions(Action closeCallback)
    {
        UIDocument UIDocument = UIManager.Instance.GetUIDocument();

        UIDocument.rootVisualElement.Q("Layout").Add(optionsVisuals.Instantiate());

        Slider musicVolumeSlider = UIDocument.rootVisualElement.Q<Slider>("MusicVolume");
        Slider sfxVolumeSlider = UIDocument.rootVisualElement.Q<Slider>("SFXVolume");
        Toggle fullscreenToggle = UIDocument.rootVisualElement.Q<Toggle>("FullscreenToggle");

        Button backButton = UIDocument.rootVisualElement.Q<Button>("Back");

        musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxVolumeSlider.value = AudioManager.Instance.GetSfxVolume();


        musicVolumeSlider.RegisterValueChangedCallback(ev =>
        {
            AudioManager.Instance.SetMusicVolume(ev.newValue);
        });

        sfxVolumeSlider.RegisterValueChangedCallback(ev =>
        {
            AudioManager.Instance.SetSfxVolume(ev.newValue);
        });

        backButton.RegisterCallback<ClickEvent>((ev) =>
        {
            closeCallback();
        });


        fullscreenToggle.RegisterValueChangedCallback(ev =>
        {
            Resolution currentResolution = Screen.currentResolution;

            FullScreenMode fullScreenMode = ev.newValue ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

            Screen.SetResolution(currentResolution.width, currentResolution.height, fullScreenMode);
        });
        fullscreenToggle.value = Screen.fullScreenMode == FullScreenMode.FullScreenWindow ? true : false;

        PrepareResolutionDropdown();
    }

    private void PrepareResolutionDropdown()
    {
        UIDocument UIDocument = UIManager.Instance.GetUIDocument();

        Resolution[] availableResolutions = Screen.resolutions;
        Resolution currentResolution = Screen.currentResolution;


        DropdownField resolutionDropdown = UIDocument.rootVisualElement.Q<DropdownField>("Resolution");
        resolutionDropdown.value = $"{currentResolution.width} x {currentResolution.height}";

        resolutionDropdown.RegisterValueChangedCallback((ev) =>
        {
            string[] splittedOption = ev.newValue.Split('x');

            Screen.SetResolution(int.Parse(splittedOption[0]), int.Parse(splittedOption[1]), Screen.fullScreenMode);
        });


        resolutionDropdown.choices.Clear();
        foreach (Resolution availableResolution in availableResolutions)
        {
            string choice = $"{availableResolution.width} x {availableResolution.height}";
            resolutionDropdown.choices.Add(choice);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }
}
