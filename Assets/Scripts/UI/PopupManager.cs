using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [SerializeField]
    private VisualTreeAsset tutorialVisuals;


    /**
     * Shows and prepares the Options Overlay
     */
    public void ShowOptions(Action closeCallback)
    {
        VisualElement optionsOverlay = optionsVisuals.Instantiate();

        CreatePopupHolder();
        AddVisuals(optionsOverlay);

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


        sfxVolumeSlider.Q("unity-drag-container").RegisterCallback<ClickEvent>(ev =>
        {
            UIManager.Instance.PlayUISfx();
        });
        sfxVolumeSlider.Q("unity-drag-container").RegisterCallback<MouseLeaveEvent>(ev =>
        {
            /*
             * Just an optimization. We only wanna play the sound if the mouse Leaves the Slider and he was currently
             * sliding with the left mouse button down.
             */
            if (ev.pressedButtons == 1)
            {
                UIManager.Instance.PlayUISfx();
            }
        });


        backButton.RegisterCallback<ClickEvent>((ev) =>
        {
            ClosePopup();
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


    /**
     * Shows and prepares the Death Overlay
     */
    public void ShowDeathOverlay(string finalTime)
    {
        VisualElement deathOverlay = deathOverlayVisuals.Instantiate();

        /*
         * We delay the showing of the Death Overlay popup so the Death
         * Animations of the Spaceship are still visible to the user
         */
        CreatePopupHolder(1f);
        StartCoroutine(PauseGame(1f));

        AddVisuals(deathOverlay);

        Button restartButton = deathOverlay.Q<Button>("Restart");
        Button mainMenuButton = deathOverlay.Q<Button>("MainMenu");


        Label finalTimeLabel = deathOverlay.Q<Label>("FinalTime");
        finalTimeLabel.text = finalTime;

        restartButton.RegisterCallback<ClickEvent>(ev =>
        {
            ResumeGame();
            RestartScene();
        });

        mainMenuButton.RegisterCallback<ClickEvent>(ev =>
        {
            ResumeGame();
            SceneManager.LoadScene("MainMenu");
        });
    }


    /**
     * Shows and prepares the Pause Menu Overlay
     */
    public void ShowPauseMenu(Action closeCallback)
    {
        VisualElement pauseMenu = pauseMenuVisuals.Instantiate();

        PauseGame();

        CreatePopupHolder();
        AddVisuals(pauseMenu);


        Button restartButton = pauseMenu.Q<Button>("Restart");
        Button optionsButton = pauseMenu.Q<Button>("Options");
        Button tutorialButton = pauseMenu.Q<Button>("Tutorial");
        Button mainMenuButton = pauseMenu.Q<Button>("MainMenu");
        Button continueButton = pauseMenu.Q<Button>("Continue");


        Action OnClose = () =>
        {
            ShowPauseMenu(closeCallback);
        };

        restartButton.RegisterCallback<ClickEvent>(ev =>
        {
            ResumeGame();
            RestartScene();
        });
        optionsButton.RegisterCallback<ClickEvent>(ev =>
        {
            ClosePopup();

            ShowOptions(OnClose);
        });
        tutorialButton.RegisterCallback<ClickEvent>(ev =>
        {
            ClosePopup();
            ShowTutorial(OnClose);
        });

        mainMenuButton.RegisterCallback<ClickEvent>(ev =>
        {
            ClosePopup();
            ResumeGame();
            SceneManager.LoadScene("MainMenu");
        });

        continueButton.RegisterCallback<ClickEvent>(ev =>
        {
            ClosePopup();
            ResumeGame();
            closeCallback();
        });
    }

    /**
     * Shows and prepares the Tutorial Overlay
     */
    public void ShowTutorial(Action closeCallback)
    {
        VisualElement tutorialMenu = tutorialVisuals.Instantiate();

        CreatePopupHolder();
        AddVisuals(tutorialMenu);

        Button backButton = tutorialMenu.Q<Button>("Back");
        backButton.RegisterCallback<ClickEvent>(ev =>
        {
            ClosePopup();
            closeCallback();
        });
    }

    private VisualElement GetPopupHolder()
    {
        return UIDocument.rootVisualElement.Q("PopupHolder");
    }

    private VisualElement CreatePopupHolder(float fadeInDelay = 0.1f)
    {
        VisualElement popupHolder = new VisualElement();
        popupHolder.name = "PopupHolder";
        popupHolder.AddToClassList("popupHolder");

        UIDocument.rootVisualElement.Add(popupHolder);

        StartCoroutine(FadeInPopupHolder(fadeInDelay));

        return popupHolder;
    }

    /**
     * Fades in the Popup after specified delay
     */
    private IEnumerator FadeInPopupHolder(float fadeInDelay = 0.1f)
    {
        yield return new WaitForSecondsRealtime(fadeInDelay);

        VisualElement popupHolder = GetPopupHolder();

        // Will trigger the transition
        popupHolder.AddToClassList("popupHolder-fadeIn");

        yield return null;
    }

    /**
     * Adds our "visuals" to the Popup Holder
     */
    private void AddVisuals(VisualElement content)
    {
        VisualElement popupHolder = GetPopupHolder();

        popupHolder.Add(content);
        UIManager.Instance.EnableSfx(content);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /**
     * Used for the Options Popup: Prepares and sets the available
     * Screen Resolutions for the Resolutions Dropdown
     */
    private void PrepareResolutionDropdown()
    {
        VisualElement popupHolder = GetPopupHolder();

        Resolution[] availableResolutions = Screen.resolutions;
        Resolution currentResolution = Screen.currentResolution;

        DropdownField resolutionDropdown = popupHolder.Q<DropdownField>("Resolution");
        resolutionDropdown.value = FormatResolution(currentResolution);

        resolutionDropdown.RegisterValueChangedCallback((ev) =>
        {
            string[] splittedOption = ev.newValue.Split('x');

            int width = int.Parse(splittedOption[0]);
            int height = int.Parse(splittedOption[1]);

            Screen.SetResolution(width, height, Screen.fullScreenMode);
        });

        resolutionDropdown.choices.Clear();
        foreach (Resolution availableResolution in availableResolutions)
        {
            string choice = FormatResolution(availableResolution);
            resolutionDropdown.choices.Add(choice);
        }
    }

    private string FormatResolution(Resolution resolution)
    {
        return $"{resolution.width} x {resolution.height}";
    }

    private void ClosePopup()
    {
        VisualElement popupHolder = GetPopupHolder();
        UIDocument.rootVisualElement.Remove(popupHolder);

        UIManager.Instance.SetFocus(UIDocument.rootVisualElement);
    }

    /// <summary>
    /// Pauses the Game instantly
    /// </summary>
    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// IEnumerator: Pauses the Game after provided delay
    /// </summary>
    /// <param name="delay">delay in seconds</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator PauseGame(float delay = 0f)
    {
        yield return new WaitForSecondsRealtime(delay);
        PauseGame();
    }

    /**
     * Resumes the Game instantly
     */
    private void ResumeGame()
    {
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
