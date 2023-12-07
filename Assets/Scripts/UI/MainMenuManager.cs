
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuManager : UIManager
{
    [SerializeField]
    private VisualTreeAsset mainMenuVisuals;

    protected override void Start()
    {
        UIDocument.rootVisualElement.Q("Layout").Add(mainMenuVisuals.Instantiate());

        Button startGameButton = UIDocument.rootVisualElement.Q<Button>("StartGame");
        Button optionsButton = UIDocument.rootVisualElement.Q<Button>("Options");
        Button tutorialButton = UIDocument.rootVisualElement.Q<Button>("Tutorial");

        Button quitGameButton = UIDocument.rootVisualElement.Q<Button>("QuitGame");

        startGameButton.RegisterCallback<ClickEvent>((ev) =>
        {
            SceneManager.LoadScene("GameLoop");
        });

        optionsButton.RegisterCallback<ClickEvent>((ev) =>
        {
            SceneManager.LoadScene("OptionsMenu");
        });

        tutorialButton.RegisterCallback<ClickEvent>((ev) =>
        {
            SceneManager.LoadScene("TutorialMenu");
        });


        quitGameButton.RegisterCallback<ClickEvent>((ev) =>
        {
            Application.Quit();
        });

        base.Start();
    }
}
