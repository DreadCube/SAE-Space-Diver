
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
