using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using static InventoryManager;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private UIDocument CoreUIDocument;

    private VisualElement inventoryRoot;
    private VisualElement inventoryItemsRoot;


    private Button amountButton;
    private Button hueButton;
    private Button shapeButton;

    private Button ascButton;
    private Button descButton;

    private string classNamePrimarySelected = "button-primary-selected";
    private string classNameInventoryItem = "inventory-item";

    [SerializeField]
    private AudioClip buttonSfx;


    /**
     * Based on the current sorted Inventory Items
     * this draws the inventory UI
     */
    public void DrawInventoryUI()
    {
        List<InventoryItem> inventoryItems = InventoryManager.Instance.GetInventoryItems();

        // Clear our Items in the UI
        inventoryItemsRoot.Clear();


        // Reset selected styles of amount / hue / shape Button (if exists)
        amountButton.RemoveFromClassList(classNamePrimarySelected);
        hueButton.RemoveFromClassList(classNamePrimarySelected);
        shapeButton.RemoveFromClassList(classNamePrimarySelected);

        // Add the selected styles to the correct button
        switch (InventoryManager.Instance.GetActiveSortType())
        {
            case SortType.Amount:
                amountButton.AddToClassList(classNamePrimarySelected);
                break;
            case SortType.Hue:
                hueButton.AddToClassList(classNamePrimarySelected);

                break;
            default:
                shapeButton.AddToClassList(classNamePrimarySelected);
                break;
        }

        // Sets selected Styles for the correct Asc / Desc Button
        if (InventoryManager.Instance.GetActiveSortDirection() == InventoryManager.SortDirection.Asc)
        {
            ascButton.AddToClassList(classNamePrimarySelected);
            descButton.RemoveFromClassList(classNamePrimarySelected);
        }
        else
        {
            descButton.AddToClassList(classNamePrimarySelected);
            ascButton.RemoveFromClassList(classNamePrimarySelected);
        }


        InventoryItem activeInventoryItem = InventoryManager.Instance.GetActiveInventoryItem();
        /**
         * For every inventoryItem we create the proper Button and apply the correct
         * backgroundImage / tintColor and item Count.
         * 
         * At the end we add the itemButton to our Inventory Items Root as child.
         */
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            InventoryItem item = inventoryItems[i];

            Button itemButton = new Button();
            itemButton.focusable = false;
            itemButton.AddToClassList(classNameInventoryItem);

            itemButton.style.backgroundImage = item.GetShape().UITexture;


            // TODO: Add Controls for switching between current active item
            Color shapeColor = item.GetShape().Color;
            itemButton.style.unityBackgroundImageTintColor = shapeColor;

            itemButton.text = item.GetAmount().ToString();

            itemButton.RegisterCallback<ClickEvent>((ev) =>
            {
                InventoryManager.Instance.SetActiveInventoryItem(item);
                DrawInventoryUI();
            });

            // Sets corresponding active Styles for the active Inventory Item
            // TODO: could also be done over className.
            if (item == activeInventoryItem)
            {
                itemButton.style.color = Color.white;
                itemButton.style.borderBottomColor = Color.white;
                itemButton.style.borderBottomWidth = 2;
            }

            inventoryItemsRoot.Add(itemButton);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        EnableInventoryUI();
        DrawInventoryUI();

        InvokeRepeating("DrawRoundTime", 0, 1f);
    }

    /**
     * EnableInventoryUI registers the needed functionality of our Inventory UI.
     * This includes:
     * 1. Reference our Buttons from the UI so we can work with them
     * 2. Register proper on click callbacks for the buttons
     * 3. Register proper KeyPress Events so we can control the Inventory UI with
     *    keypresses
     */
    private void EnableInventoryUI()
    {
        inventoryRoot = CoreUIDocument.rootVisualElement.Q<VisualElement>("Inventory");
        inventoryItemsRoot = inventoryRoot.Q<VisualElement>("InventoryItems");

        amountButton = inventoryRoot.Q<Button>("Amount");
        hueButton = inventoryRoot.Q<Button>("Hue");
        shapeButton = inventoryRoot.Q<Button>("Shape");

        ascButton = inventoryRoot.Q<Button>("Asc");
        descButton = inventoryRoot.Q<Button>("Desc");

        CoreUIDocument.rootVisualElement.focusable = true;
        CoreUIDocument.rootVisualElement.RegisterCallback<KeyDownEvent>(ev =>
        {
            switch (ev.keyCode)
            {
                case KeyCode.Alpha1:
                    ChangeSortType(SortType.Amount);
                    break;

                case KeyCode.Alpha2:
                    ChangeSortType(SortType.Hue);
                    break;
                case KeyCode.Alpha3:
                    ChangeSortType(SortType.ShapeType);
                    break;

                case KeyCode.Tab:
                    ChangeSortDirection(InventoryManager.Instance.GetInactiveSortDirection());
                    break;

                case KeyCode.Q:
                    InventoryManager.Instance.SetActiveInventoryItem(-1);
                    DrawInventoryUI();
                    AudioManager.Instance.PlaySfx(buttonSfx);

                    break;
                case KeyCode.E:
                    InventoryManager.Instance.SetActiveInventoryItem(1);
                    DrawInventoryUI();
                    AudioManager.Instance.PlaySfx(buttonSfx);

                    break;
            }
        });


        amountButton.RegisterCallback(OnClickSortType(SortType.Amount));
        hueButton.RegisterCallback(OnClickSortType(SortType.Hue));
        shapeButton.RegisterCallback(OnClickSortType(SortType.ShapeType));

        ascButton.RegisterCallback(OnClickSortDirection(InventoryManager.SortDirection.Asc));
        descButton.RegisterCallback(OnClickSortDirection(InventoryManager.SortDirection.Desc));
    }

    public void ShowDeathOverlay()
    {
        VisualElement deathOverlay = CoreUIDocument.rootVisualElement.Q<VisualElement>("DeathOverlay");

        deathOverlay.RegisterCallback<KeyDownEvent>(ev =>
        {
            if (ev.keyCode == KeyCode.R)
            {
                Restart();
            }
        });

        deathOverlay.style.display = DisplayStyle.Flex;
        deathOverlay.focusable = true;
        deathOverlay.Focus();

        Button restartButton = deathOverlay.Q<Button>("Restart");

        Label finalTime = deathOverlay.Q<Label>("FinalTime");
        finalTime.text = GetRoundTime();

        restartButton.RegisterCallback<ClickEvent>(ev =>
        {
            AudioManager.Instance.PlaySfx(buttonSfx);
            Restart();
        });
        DrawRoundTime();
        CancelInvoke("DrawRoundTime");
    }

    private EventCallback<ClickEvent> OnClickSortType(SortType sortType)
    {
        return (ev) => ChangeSortType(sortType);
    }

    private EventCallback<ClickEvent> OnClickSortDirection(InventoryManager.SortDirection sortDirection)
    {
        return (ev) => ChangeSortDirection(sortDirection);
    }

    private void ChangeSortType(SortType sortType)
    {
        AudioManager.Instance.PlaySfx(buttonSfx);
        InventoryManager.SortDirection activeSortDirection = InventoryManager.Instance.GetActiveSortDirection();
        InventoryManager.Instance.SortInventoryItems(sortType, activeSortDirection);
        DrawInventoryUI();
    }

    private void ChangeSortDirection(InventoryManager.SortDirection sortDirection)
    {
        AudioManager.Instance.PlaySfx(buttonSfx);
        SortType activeSortType = InventoryManager.Instance.GetActiveSortType();
        InventoryManager.Instance.SortInventoryItems(activeSortType, sortDirection);
        DrawInventoryUI();
    }

    private void DrawRoundTime()
    {
        Label timer = CoreUIDocument.rootVisualElement.Q<Label>("Timer");
        timer.text = GetRoundTime();
    }

    private string GetRoundTime()
    {
        int seconds = Mathf.RoundToInt(Time.timeSinceLevelLoad);

        int minutes = seconds / 60;
        int rest = seconds % 60;

        string formattedMinutes = minutes.ToString().PadLeft(2, '0');
        string formattedSeconds = rest.ToString().PadLeft(2, '0');

        return $"{formattedMinutes}:{formattedSeconds}";
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

