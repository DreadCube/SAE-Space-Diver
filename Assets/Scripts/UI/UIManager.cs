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
    private AudioClip UISfx;

    public static UIManager GetInstance()
    {
        return Instance;
    }

    public UIDocument GetUIDocument()
    {
        return UIDocument;
    }

    /**
     * Enables UI SFX on specified root element
     * Will go down the hierarchy and enables Event Listeners
     * for Buttons / DropdownFields and Toggles
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

    /**
     * Disables UI SFX on specified root element
     * Will go down the hierarchy and disable Event Listeners
     * for Buttons / DropdownFields and Toggles
     */
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

    public void PlayUISfx()
    {
        AudioManager.Instance.PlaySfx(UISfx);
    }

    protected virtual void Start()
    {
        EnableSfx(UIDocument.rootVisualElement);
        SetFocus(UIDocument.rootVisualElement);
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

    /// <summary>
    /// Plays UI Sfx on a click event
    /// </summary>
    /// <param name="_">Event</param>
    private void PlaySfx(ClickEvent _)
    {
        PlayUISfx();
    }

    /// <summary>
    /// Plays UI Sfx on a Mouse Enter event
    /// </summary>
    /// <param name="_">Event</param>
    private void PlaySfx(MouseEnterEvent _)
    {
        PlayUISfx();
    }

    /// <summary>
    /// Plays UI Sfx on a string Change Event (Slider)
    /// </summary>
    /// <param name="_">Event</param>
    private void PlaySfx(ChangeEvent<string> _)
    {
        PlayUISfx();
    }

    /// <summary>
    /// Plays UI Sfx on a bool Change Event (Checkbox)
    /// </summary>
    /// <param name="_">Event</param>
    private void PlaySfx(ChangeEvent<bool> _)
    {
        PlayUISfx();
    }
}
