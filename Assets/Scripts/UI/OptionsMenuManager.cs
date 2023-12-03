using UnityEngine.SceneManagement;

/**
 *  THe OptionsMenuManager is the UIManager for the Options Scene.
 */
public class OptionsMenuManager : UIManager
{
    protected override void Start()
    {
        PopupManager.Instance.ShowOptions(() =>
        {
            SceneManager.LoadScene("MainMenu");

        });

        base.Start();
    }
}
