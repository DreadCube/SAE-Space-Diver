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


    public UIDocument GetUIDocument()
    {
        return UIDocument;
    }

    protected virtual void Start()
    {
        EnableSfx();
    }

    public void PlayUISfx()
    {
        AudioManager.Instance.PlaySfx(UISfx);
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

        var dropdownFields = UIDocument.rootVisualElement.Query<DropdownField>();

        var toggles = UIDocument.rootVisualElement.Query<Toggle>();


        buttons.ForEach(button =>
        {
            button.RegisterCallback<ClickEvent>(ev =>
            {
                PlayUISfx();
            });

            button.RegisterCallback<MouseEnterEvent>(ev =>
            {
                PlayUISfx();
            });
        });

        dropdownFields.ForEach(dropdownField =>
        {
            dropdownField.RegisterValueChangedCallback(ev =>
            {
                PlayUISfx();
            });
        });

        toggles.ForEach(toggle =>
        {
            toggle.RegisterValueChangedCallback(ev =>
            {
                PlayUISfx();
            });
        });
    }
}
