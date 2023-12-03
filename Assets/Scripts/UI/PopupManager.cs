using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.SceneManagement;

/**
 * The PopupManager is responsible for UI Layouts that wil be injected in to the
 * current defined UIDocument. These UI Layouts act as popups and are capable to use them
 * in different scenes. As example: Options.
 */
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [SerializeField]
    private UIDocument UIDocument;

    [SerializeField]
    private VisualTreeAsset optionsVisuals;

    [SerializeField]
    private VisualTreeAsset deathOverlayVisuals;

    public void ShowOptions(Action closeCallback)
    {
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

    // TODO: Move out to Popup.
    public void ShowDeathOverlay(string finalTime)
    {
        VisualElement popupHolder = GetPopupHolder();
        popupHolder.Clear();

        VisualElement deathOverlay = deathOverlayVisuals.Instantiate();

        popupHolder.Add(deathOverlay);


        deathOverlay.RegisterCallback<KeyDownEvent>(ev =>
        {
            if (ev.keyCode == KeyCode.R)
            {
                RestartScene();
            }
        });

        deathOverlay.style.display = DisplayStyle.Flex;
        deathOverlay.focusable = true;
        deathOverlay.Focus();

        Button restartButton = deathOverlay.Q<Button>("Restart");

        Label finalTimeLabel = deathOverlay.Q<Label>("FinalTime");
        finalTimeLabel.text = finalTime;

        restartButton.RegisterCallback<ClickEvent>(ev =>
        {
            UIManager.Instance.PlayUISfx();
            RestartScene();
        });


        /*Button quitButton = deathOverlay.Q<Button>("Quit");


        quitButton.RegisterCallback<ClickEvent>(ev =>
        {
            UIManager.Instance.PlayUISfx();
            Application.Quit();
        });*/
    }

    private VisualElement GetPopupHolder()
    {
        Debug.Log("A");
        VisualElement popupHolder = UIDocument.rootVisualElement.Q("PopupHolder");

        if (popupHolder == null)
        {
            popupHolder = new VisualElement();
            popupHolder.name = "PopupHolder";
            popupHolder.AddToClassList("popupHolder");

            UIDocument.rootVisualElement.Add(popupHolder);
        }

        Debug.Log("B");

        return popupHolder;
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
