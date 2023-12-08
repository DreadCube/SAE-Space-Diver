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

    private bool isPopupOpen = false;


    public void ShowOptions(Action closeCallback)
    {
        if (isPopupOpen)
        {
            return;
        }
        isPopupOpen = true;
        VisualElement optionsOverlay = optionsVisuals.Instantiate();

        CreatePopupHolder();
        AddContent(optionsOverlay);


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

    public void ShowDeathOverlay(string finalTime)
    {
        if (isPopupOpen)
        {
            return;
        }
        isPopupOpen = true;

        VisualElement deathOverlay = deathOverlayVisuals.Instantiate();

        CreatePopupHolder(1);
        AddContent(deathOverlay);


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
        Button mainMenuButton = deathOverlay.Q<Button>("MainMenu");


        Label finalTimeLabel = deathOverlay.Q<Label>("FinalTime");
        finalTimeLabel.text = finalTime;

        restartButton.RegisterCallback<ClickEvent>(ev =>
        {
            RestartScene();
        });

        mainMenuButton.RegisterCallback<ClickEvent>(ev =>
        {
            SceneManager.LoadScene("MainMenu");
        });
    }

    public void ShowPauseMenu()
    {
        if (isPopupOpen)
        {
            return;
        }
        isPopupOpen = true;

        VisualElement pauseMenu = pauseMenuVisuals.Instantiate();

        CreatePopupHolder();
        AddContent(pauseMenu);


        Button restartButton = pauseMenu.Q<Button>("Restart");
        Button optionsButton = pauseMenu.Q<Button>("Options");
        Button tutorialButton = pauseMenu.Q<Button>("Tutorial");
        Button mainMenuButton = pauseMenu.Q<Button>("MainMenu");
        Button continueButton = pauseMenu.Q<Button>("Continue");


        Action OnClose = () =>
        {
            ShowPauseMenu();
        };


        restartButton.RegisterCallback<ClickEvent>(ev => RestartScene());
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
            SceneManager.LoadScene("MainMenu");
        });

        continueButton.RegisterCallback<ClickEvent>(ev =>
        {
            ClosePopup();
        });
    }

    public void ShowTutorial(Action closeCallback)
    {
        if (isPopupOpen)
        {
            return;
        }
        isPopupOpen = true;

        VisualElement tutorialMenu = tutorialVisuals.Instantiate();

        CreatePopupHolder();
        AddContent(tutorialMenu);

        Button backButton = tutorialMenu.Q<Button>("Back");
        backButton.RegisterCallback<ClickEvent>(ev =>
        {
            ClosePopup();
            closeCallback();
        });

    }


    private VisualElement CreatePopupHolder(float fadeInDelay = 0.1f)
    {
        Time.timeScale = 1;

        VisualElement popupHolder = UIDocument.rootVisualElement.Q("PopupHolder");

        if (popupHolder == null)
        {
            popupHolder = new VisualElement();
            popupHolder.name = "PopupHolder";
            popupHolder.AddToClassList("popupHolder");

            UIDocument.rootVisualElement.Add(popupHolder);

            InvokeRepeating("FadeInPopupHolder", fadeInDelay, 0f);
        }


        return popupHolder;
    }

    private void FadeInPopupHolder()
    {
        VisualElement popupHolder = UIDocument.rootVisualElement.Q("PopupHolder");
        popupHolder.AddToClassList("popupHolder-fadeIn");
        CancelInvoke("FadeInPopupHolder");

        Time.timeScale = 0;
    }


    private void AddContent(VisualElement content)
    {
        VisualElement popupHolder = UIDocument.rootVisualElement.Q("PopupHolder");
        popupHolder.Add(content);
        UIManager.Instance.EnableSfx(content);
    }


    private void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PrepareResolutionDropdown()
    {
        VisualElement popupHolder = UIDocument.rootVisualElement.Q("PopupHolder");

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

    private void ClosePopup()
    {
        VisualElement popupHolder = UIDocument.rootVisualElement.Q("PopupHolder");
        UIDocument.rootVisualElement.Remove(popupHolder);
        Time.timeScale = 1;
        isPopupOpen = false;
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
