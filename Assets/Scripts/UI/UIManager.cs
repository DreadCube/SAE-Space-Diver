using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

using static InventoryManager;
using static UnityEditor.Progress;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private UIDocument UIDocument;

    private VisualElement inventoryRoot;
    private VisualElement inventoryItemsRoot;


    private Button amountButton;
    private Button hueButton;
    private Button shapeButton;

    private Button ascButton;
    private Button descButton;

    private string classNamePrimarySelected = "button-primary-selected";
    private string classNameInventoryItem = "inventory-item";


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
    }

    private void EnableInventoryUI()
    {
        inventoryRoot = UIDocument.rootVisualElement.Q<VisualElement>("Inventory");
        inventoryItemsRoot = inventoryRoot.Q<VisualElement>("InventoryItems");

        amountButton = inventoryRoot.Q<Button>("Amount");
        hueButton = inventoryRoot.Q<Button>("Hue");
        shapeButton = inventoryRoot.Q<Button>("Shape");

        ascButton = inventoryRoot.Q<Button>("Asc");
        descButton = inventoryRoot.Q<Button>("Desc");


        UIDocument.rootVisualElement.focusable = true;
        UIDocument.rootVisualElement.RegisterCallback<KeyDownEvent>(ev =>
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
            }
        });


        amountButton.RegisterCallback(OnClickSortType(SortType.Amount));
        hueButton.RegisterCallback(OnClickSortType(SortType.Hue));
        shapeButton.RegisterCallback(OnClickSortType(SortType.ShapeType));

        ascButton.RegisterCallback(OnClickSortDirection(InventoryManager.SortDirection.Asc));
        descButton.RegisterCallback(OnClickSortDirection(InventoryManager.SortDirection.Desc));
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
        InventoryManager.SortDirection activeSortDirection = InventoryManager.Instance.GetActiveSortDirection();
        InventoryManager.Instance.SortInventoryItems(sortType, activeSortDirection);
        DrawInventoryUI();
    }

    private void ChangeSortDirection(InventoryManager.SortDirection sortDirection)
    {
        SortType activeSortType = InventoryManager.Instance.GetActiveSortType();
        InventoryManager.Instance.SortInventoryItems(activeSortType, sortDirection);
        DrawInventoryUI();
    }

    private void DrawInventoryUI()
    {
        List<InventoryItem> inventoryItems = InventoryManager.Instance.GetInventoryItems();

        inventoryItemsRoot.Clear();

        amountButton.RemoveFromClassList(classNamePrimarySelected);
        hueButton.RemoveFromClassList(classNamePrimarySelected);
        shapeButton.RemoveFromClassList(classNamePrimarySelected);

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

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            InventoryItem item = inventoryItems[i];

            Button itemButton = new Button();
            itemButton.AddToClassList(classNameInventoryItem);

            itemButton.style.backgroundImage = item.GetShape().UITexture;


            // TODO: Add Controls for switching between current active item
            Color shapeColor = item.GetShape().Color;
            itemButton.style.unityBackgroundImageTintColor = shapeColor;

            itemButton.text = item.GetAmount().ToString();

            inventoryItemsRoot.Add(itemButton);
        }
    }
}

