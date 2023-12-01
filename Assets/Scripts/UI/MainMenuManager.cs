
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private UIDocument UIDocument;

    private void Awake()
    {
        UIDocument = GetComponent<UIDocument>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Button startGameButton = UIDocument.rootVisualElement.Q<Button>("StartGame");
        Button optionsButton = UIDocument.rootVisualElement.Q<Button>("StartGame");
        Button quitGameButton = UIDocument.rootVisualElement.Q<Button>("StartGame");

        startGameButton.RegisterCallback<ClickEvent>((ev) =>
        {
            SceneManager.LoadScene("Main");
        });

        optionsButton.RegisterCallback<ClickEvent>((ev) =>
        {
            SceneManager.LoadScene("Options");
        });


        quitGameButton.RegisterCallback<ClickEvent>((ev) =>
        {
            Application.Quit();
        });

    }
}
