using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections;

using static InventoryManager;

/*
 * The GameLoopManager is the UI Manager for the Main Game Loop Scene.
 * It handles as example the UI Inventory Logic.
 */
public class GameLoopManager : UIManager
{
    public static new GameLoopManager Instance { get; protected set; }

    private VisualElement inventoryRoot;
    private VisualElement inventoryItemsRoot;


    private Button amountButton;
    private Button hueButton;
    private Button shapeButton;

    private Button ascButton;
    private Button descButton;

    private string classNamePrimarySelected = "button-primary-selected";
    private string classNameInventoryItem = "inventory-item";
    private string classNameInventoryItemSelected = "inventory-item-selected";


    /**
     * Draws the energy in the UI.
     */
    public void DrawEnergy(int energy)
    {
        VisualElement energyWrapper = inventoryRoot.Q<VisualElement>("EnergyWrapper");

        Label energyLabel = energyWrapper.Q<Label>("Energy");
        energyLabel.text = energy.ToString();
    }

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
            itemButton.AddToClassList(classNameInventoryItem);

            itemButton.style.backgroundImage = item.GetShape().UITexture;

            Color shapeColor = item.GetShape().Color;
            itemButton.style.unityBackgroundImageTintColor = shapeColor;

            itemButton.text = item.GetAmount().ToString();

            itemButton.RegisterCallback(OnClickInventoryItem(item));

            if (item == activeInventoryItem)
            {
                itemButton.AddToClassList(classNameInventoryItemSelected);
            }

            inventoryItemsRoot.Add(itemButton);
        }
    }

    /**
     * Will be called if the player finished a round:
     * 
     * 1. Stop Drawing Round Time
     * 2. Disable Inventory UI and SFX
     * 3. Show the Death Overlay
     */
    public void HandleFinish()
    {
        string roundTime = GetRoundTime();

        StopAllCoroutines();

        ToggleUIInteractions();
        PopupManager.Instance.ShowDeathOverlay(roundTime);
    }


    protected override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        UIDocument = GetComponent<UIDocument>();
    }


    protected override void Start()
    {
        RegisterInventoryUI();
        DrawInventoryUI();

        StartCoroutine(DrawRoundTime());

        base.Start();
    }

    /**
     * ToggleUIInteractions will toggle the current UI state.
     * 
     * If its active: We gonna deactivate its interactions and SFX
     * If its inactive: We gonna reactivate its interations and SFX
     * 
     * The UI is disabled while we are in the Pause Menu.
     */
    private void ToggleUIInteractions()
    {
        if (inventoryRoot.enabledSelf)
        {
            UnregisterInventoryUI();
            inventoryRoot.SetEnabled(false);
            UIDocument.rootVisualElement.Q("PauseMenu").SetEnabled(false);
            DisableSfx(inventoryRoot);
        }
        else
        {
            RegisterInventoryUI();
            inventoryRoot.SetEnabled(true);
            UIDocument.rootVisualElement.Q("PauseMenu").SetEnabled(true);
            EnableSfx(inventoryRoot);
        }
    }

    /**
     * Handles proper User Key Down Input for the UI:
     * 
     * Escape: Show Pause Menu
     * 1 / 2 / 3: Will switch the Sort Type
     * Tab: Will switch the Sort Direction
     * Q / E: Switch between active Inventory Item
     */
    private void HandleKeyDownEvent(KeyDownEvent ev)
    {
        switch (ev.keyCode)
        {
            case KeyCode.Escape:
                ToggleUIInteractions();
                PopupManager.Instance.ShowPauseMenu(ToggleUIInteractions);
                break;

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
                UIManager.Instance.PlayUISfx();
                break;

            case KeyCode.E:
                InventoryManager.Instance.SetActiveInventoryItem(1);
                DrawInventoryUI();
                UIManager.Instance.PlayUISfx();
                break;

            default:
                break;
        }
    }

    /**
     * RegisterInventoryUI registers the needed functionality of our Inventory UI.
     * This includes:
     * 1. Reference our Buttons from the UI so we can work with them
     * 2. Register proper on click callbacks for the buttons
     * 3. Register proper KeyPress Events so we can control the Inventory UI with
     *    keypresses
     */
    private void RegisterInventoryUI()
    {
        inventoryRoot = UIDocument.rootVisualElement.Q<VisualElement>("Inventory");
        inventoryItemsRoot = inventoryRoot.Q<VisualElement>("InventoryItems");

        amountButton = inventoryRoot.Q<Button>("Amount");
        hueButton = inventoryRoot.Q<Button>("Hue");
        shapeButton = inventoryRoot.Q<Button>("Shape");

        ascButton = inventoryRoot.Q<Button>("Asc");
        descButton = inventoryRoot.Q<Button>("Desc");

        UIDocument.rootVisualElement.RegisterCallback<KeyDownEvent>(HandleKeyDownEvent);


        amountButton.RegisterCallback(OnClickSortType(SortType.Amount));
        hueButton.RegisterCallback(OnClickSortType(SortType.Hue));
        shapeButton.RegisterCallback(OnClickSortType(SortType.ShapeType));

        ascButton.RegisterCallback(OnClickSortDirection(InventoryManager.SortDirection.Asc));
        descButton.RegisterCallback(OnClickSortDirection(InventoryManager.SortDirection.Desc));
    }

    /**
     * Unregisters proper Callbacks from the Inventory UI
     */
    private void UnregisterInventoryUI()
    {
        amountButton = inventoryRoot.Q<Button>("Amount");
        hueButton = inventoryRoot.Q<Button>("Hue");
        shapeButton = inventoryRoot.Q<Button>("Shape");

        ascButton = inventoryRoot.Q<Button>("Asc");
        descButton = inventoryRoot.Q<Button>("Desc");

        UIDocument.rootVisualElement.UnregisterCallback<KeyDownEvent>(HandleKeyDownEvent);

        amountButton.UnregisterCallback(OnClickSortType(SortType.Amount));
        hueButton.UnregisterCallback(OnClickSortType(SortType.Hue));
        shapeButton.UnregisterCallback(OnClickSortType(SortType.ShapeType));

        ascButton.UnregisterCallback(OnClickSortDirection(InventoryManager.SortDirection.Asc));
        descButton.UnregisterCallback(OnClickSortDirection(InventoryManager.SortDirection.Desc));
    }

    private EventCallback<ClickEvent> OnClickInventoryItem(InventoryItem item)
    {
        return (ev) =>
        {
            PlayUISfx();

            InventoryManager.Instance.SetActiveInventoryItem(item);
            DrawInventoryUI();
        };
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
        PlayUISfx();
        InventoryManager.SortDirection activeSortDirection = InventoryManager.Instance.GetActiveSortDirection();
        InventoryManager.Instance.SortInventoryItems(sortType, activeSortDirection);
        DrawInventoryUI();
    }

    private void ChangeSortDirection(InventoryManager.SortDirection sortDirection)
    {
        PlayUISfx();
        SortType activeSortType = InventoryManager.Instance.GetActiveSortType();
        InventoryManager.Instance.SortInventoryItems(activeSortType, sortDirection);
        DrawInventoryUI();
    }

    /**
     * Calculates the round Time based on time since start of the level
     */
    private string GetRoundTime()
    {
        int seconds = Mathf.RoundToInt(Time.timeSinceLevelLoad);

        int minutes = seconds / 60;
        int rest = seconds % 60;

        string formattedMinutes = minutes.ToString().PadLeft(2, '0');
        string formattedSeconds = rest.ToString().PadLeft(2, '0');

        return $"{formattedMinutes}:{formattedSeconds}";
    }

    /**
     * Draws the roundTime every 0.5 Seconds to the UI.
     */
    private IEnumerator DrawRoundTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            Label timer = UIDocument.rootVisualElement.Q<Label>("Timer");
            timer.text = GetRoundTime();
        }
    }
}

