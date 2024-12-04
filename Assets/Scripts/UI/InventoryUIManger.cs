using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManger : MonoBehaviour
{
    public static InventoryUIManger Instance { get; private set; }

    [Header("UI References")]
    public GameObject inventoryPanle;                   // 인벤토리 패널
    public Transform itemContainer;                     // 아이템 슬롯들이 들어갈 컨테이너
    public GameObject itemSlotPrefab;                   // 아이템 슬롯 프리팹
    public Button closeButton;                          // 닫기 버튼

    private PlayerInventory playerInventory;
    private SurvivalStats survivalStats;

    private void Awake()
    {
        Instance = this;
        inventoryPanle.SetActive(false);
    }

    void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        survivalStats = FindObjectOfType<SurvivalStats>();
        closeButton.onClick.AddListener(HideUI);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryPanle.activeSelf)
            {
                HideUI();
            }
            else
            {
                ShowUI();
            }
        }
    }

    public void ShowUI()                                        // UI 창을 열었을 때
    {
        inventoryPanle.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        RefreshInventory();
    }

    public void HideUI()                                        // // UI 창을 닫았을 때
    {
        inventoryPanle.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RefreshInventory()
    {
        // 기존 아이템 슬롯들을 제거 itemContainer 하위에 있는 모든 오브젝트 제거
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }

        CreateItemSlot(ItemType.Crystal);
        CreateItemSlot(ItemType.Plant);
        CreateItemSlot(ItemType.Bush);
        CreateItemSlot(ItemType.Tree);
        CreateItemSlot(ItemType.VegetableStew);
        CreateItemSlot(ItemType.FruitSalad);
        CreateItemSlot(ItemType.RepairKit);
    }

    private void CreateItemSlot(ItemType type)
    {
        GameObject slotObj = Instantiate(itemSlotPrefab, itemContainer);
        ItemSlot slot = slotObj.GetComponent<ItemSlot>();
        slot.SetUp(type, playerInventory.GetItemCount(type));                   // 플레이어 인벤토리에서 개수를 반환한다.
    }
}
