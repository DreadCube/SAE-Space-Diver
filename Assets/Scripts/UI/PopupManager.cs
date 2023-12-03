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

    [SerializeField]
    private VisualTreeAsset pauseMenuVisuals;

    public void ShowOptions(Action closeCallback)
    {
        VisualElement optionsOverlay = optionsVisuals.Instantiate();

        VisualElement popupHolder = ClearPopupHolder();
        popupHolder.Add(optionsOverlay);


        UIManager.Instance.EnableSfx(popupHolder);



        Slider musicVolumeSlider = optionsOverlay.Q<Slider>("MusicVolume");
        Slider sfxVolumeSlider = optionsOverlay.Q<Slider>("SFXVolume");
        Toggle fullscreenToggle = optionsOverlay.Q<Toggle>("FullscreenToggle");

        Button backButton = optionsOverlay.Q<Button>("Back");

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

    public void ShowDeathOverlay(string finalTime)
    {
        VisualElement popupHolder = ClearPopupHolder();

        VisualElement deathOverlay = deathOverlayVisuals.Instantiate();

        popupHolder.Add(deathOverlay);


        UIManager.Instance.EnableSfx(popupHolder);


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
            RestartScene();
        });
    }

    public void ShowPauseMenu()
    {
        VisualElement popupHolder = ClearPopupHolder();

        VisualElement pauseMenu = pauseMenuVisuals.Instantiate();
        popupHolder.Add(pauseMenu);

        Time.timeScale = 0;

        UIManager.Instance.EnableSfx(popupHolder);


        Button restartButton = pauseMenu.Q<Button>("Restart");
        Button optionsButton = pauseMenu.Q<Button>("Options");
        Button mainMenuButton = pauseMenu.Q<Button>("MainMenu");
        Button continueButton = pauseMenu.Q<Button>("Continue");


        restartButton.RegisterCallback<ClickEvent>(ev => RestartScene());
        optionsButton.RegisterCallback<ClickEvent>(ev => ShowOptions(() =>
        {
            ShowPauseMenu();
        }));
        mainMenuButton.RegisterCallback<ClickEvent>(ev =>
        {
            ClosePopup();
            SceneManager.LoadScene("MainMenu");
        });

        continueButton.RegisterCallback<ClickEvent>(ev =>
        {
            Time.timeScale = 1;
            ClosePopup();
        });

    }

    private VisualElement GetPopupHolder()
    {
        VisualElement popupHolder = UIDocument.rootVisualElement.Q("PopupHolder");

        if (popupHolder == null)
        {
            popupHolder = new VisualElement();
            popupHolder.name = "PopupHolder";
            popupHolder.AddToClassList("popupHolder");

            UIDocument.rootVisualElement.Add(popupHolder);
        }

        return popupHolder;
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PrepareResolutionDropdown()
    {
        VisualElement popupHolder = GetPopupHolder();

        Resolution[] availableResolutions = Screen.resolutions;
        Resolution currentResolution = Screen.currentResolution;

        DropdownField resolutionDropdown = popupHolder.Q<DropdownField>("Resolution");
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

    private VisualElement ClearPopupHolder()
    {
        VisualElement popupHolder = GetPopupHolder();
        popupHolder.Clear();

        return popupHolder;
    }

    private void ClosePopup()
    {
        VisualElement popupHolder = GetPopupHolder();
        UIDocument.rootVisualElement.Remove(popupHolder);

        Time.timeScale = 1;
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
