using UnityEngine;
using UnityEngine.UIElements;

/**
 * The UIManager is the main Manager for UI behaviours.
 * 
 * As example we Register proper SoundEffects for buttons / dropdown fields / toggles
 * on start.
 */
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    protected UIDocument UIDocument;

    [SerializeField]
    protected AudioClip UISfx;


    public static UIManager GetInstance()
    {
        return Instance;
    }

    public UIDocument GetUIDocument()
    {
        return UIDocument;
    }

    protected virtual void Start()
    {
        EnableSfx(UIDocument.rootVisualElement);
        SetFocus(UIDocument.rootVisualElement);
    }

    public void PlayUISfx()
    {
        AudioManager.Instance.PlaySfx(UISfx);
    }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        UIDocument = GetComponent<UIDocument>();
    }

    private void PlaySfx(ClickEvent _)
    {
        PlayUISfx();
    }

    private void PlaySfx(MouseEnterEvent _)
    {
        PlayUISfx();
    }

    private void PlaySfx(ChangeEvent<string> _)
    {
        PlayUISfx();
    }

    private void PlaySfx(ChangeEvent<bool> _)
    {
        PlayUISfx();
    }


    /**
     * Enables UI SFX
     */
    public void EnableSfx(VisualElement root)
    {
        var buttons = root.Query<Button>();

        var dropdownFields = root.Query<DropdownField>();

        var toggles = root.Query<Toggle>();


        buttons.ForEach(button =>
        {
            button.RegisterCallback<ClickEvent>(PlaySfx);
            button.RegisterCallback<MouseEnterEvent>(PlaySfx);
        });

        dropdownFields.ForEach(dropdownField =>
        {
            dropdownField.RegisterValueChangedCallback(PlaySfx);
        });

        toggles.ForEach(toggle =>
        {
            toggle.RegisterValueChangedCallback(PlaySfx);
        });
    }

    public void DisableSfx(VisualElement root)
    {
        var buttons = root.Query<Button>();

        var dropdownFields = root.Query<DropdownField>();

        var toggles = root.Query<Toggle>();


        buttons.ForEach(button =>
        {
            button.UnregisterCallback<ClickEvent>(PlaySfx);
            button.UnregisterCallback<MouseEnterEvent>(PlaySfx);
        });

        dropdownFields.ForEach(dropdownField =>
        {
            dropdownField.UnregisterValueChangedCallback(PlaySfx);
        });

        toggles.ForEach(toggle =>
        {
            toggle.UnregisterValueChangedCallback(PlaySfx);
        });
    }

    /**
     * Make the provided visualElement focusable and set the focus to it
     */
    public void SetFocus(VisualElement element)
    {
        element.focusable = true;
        element.Focus();
    }
}
