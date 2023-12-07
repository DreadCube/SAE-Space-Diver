using UnityEngine.SceneManagement;

/**
 *  THe TutorialMenuManager is the UIManager for the Tutorial Scene.
 */
public class TutorialMenuManager : UIManager
{
    protected override void Start()
    {
        PopupManager.Instance.ShowTutorial(() =>
        {
            SceneManager.LoadScene("MainMenu");

        });

        base.Start();
    }
}
