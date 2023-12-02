using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    protected UIDocument UIDocument;

    [SerializeField]
    protected AudioClip UISfx;


    public UIDocument GetUIDocument()
    {
        return UIDocument;
    }

    protected virtual void Start()
    {
        EnableSfx();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        UIDocument = GetComponent<UIDocument>();
    }

    /**
     * Enables UI SFX
     */
    private void EnableSfx()
    {
        var buttons = UIDocument.rootVisualElement.Query<Button>();

        var sliders = UIDocument.rootVisualElement.Query<Slider>();

        var dropdownFields = UIDocument.rootVisualElement.Query<DropdownField>();

        var toggles = UIDocument.rootVisualElement.Query<Toggle>();


        buttons.ForEach(button =>
        {
            button.RegisterCallback<ClickEvent>(ev =>
            {
                AudioManager.Instance.PlaySfx(UISfx);
            });

            button.RegisterCallback<MouseEnterEvent>(ev =>
            {
                AudioManager.Instance.PlaySfx(UISfx);
            });
        });

        sliders.ForEach(slider =>
        {
            slider.RegisterValueChangedCallback(ev =>
            {
                AudioManager.Instance.PlaySfx(UISfx);
            });
        });

        dropdownFields.ForEach(dropdownField =>
        {
            dropdownField.RegisterValueChangedCallback(ev =>
            {
                AudioManager.Instance.PlaySfx(UISfx);
            });
        });

        toggles.ForEach(toggle =>
        {
            toggle.RegisterValueChangedCallback(ev =>
            {
                AudioManager.Instance.PlaySfx(UISfx);
            });
        });
    }
}
