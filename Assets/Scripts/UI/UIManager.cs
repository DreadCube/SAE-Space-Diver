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
