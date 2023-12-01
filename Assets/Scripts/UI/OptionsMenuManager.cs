
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class OptionsMenuManager : MonoBehaviour
{
    private UIDocument UIDocument;

    private void Awake()
    {
        UIDocument = GetComponent<UIDocument>();
    }

    void Start()
    {
        Slider musicVolumeSlider = UIDocument.rootVisualElement.Q<Slider>("MusicVolume");
        Slider sfxVolumeSlider = UIDocument.rootVisualElement.Q<Slider>("SFXVolume");
        EnumField resolutionEnum = UIDocument.rootVisualElement.Q<EnumField>("Resolution");
        Button backButton = UIDocument.rootVisualElement.Q<Button>("Back");

        musicVolumeSlider.RegisterValueChangedCallback(value =>
        {
            Debug.Log(value);
        });

        backButton.RegisterCallback<ClickEvent>((ev) =>
        {
            // TODO: Check if possible to previous scene
            SceneManager.LoadScene("MainMenu");
        });
    }
}
