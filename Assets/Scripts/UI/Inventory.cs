using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private Slot[] slots;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI conditionText;
    [SerializeField] private TextMeshProUGUI conditionValue;

    [Header("Buttons")]
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject unequipButton;
    [SerializeField] private GameObject dropButton;

    [Header("DropPosition")]
    [SerializeField] private Transform dropPosition;

    private PlayerCondition condition;
    private PlayerController controller;

    private ItemSO ItemData;
    private int selectedIndex;



    private void Start()
    {
        condition = CharacterManager.Instance.player.condition;
        controller = CharacterManager.Instance.player.controller;
        OffDescription();
        InventoryPanel.SetActive(false);

        controller.ToggleInventory += ToggleInventory;
        CharacterManager.Instance.player.addItem += AddItem;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].index = i;
        }
    }

    private void OnDisable()
    {
        OffDescription();
    }

    private void ToggleInventory()
    {
        InventoryPanel.SetActive(!InventoryPanel.activeSelf);
    }

    private void UpdateSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].curItemData != null)
            {
                slots[i].Setslot();
            }
            else
            {
                slots[i].SlotClear();
            }
        }
    }

    public void AddItem()
    {
        ItemData = CharacterManager.Instance.player.itemData;

        if (ItemData.canStack)
        {
            Slot slot = GetItemStack(ItemData);
            if(slot != null)
            {
                slot.quantity++;
                UpdateSlots();
                CharacterManager.Instance.player.itemData = null;
                return;
            }
        }
        Slot emptySlot = FindEmptySlot();
        if(emptySlot != null)
        {
            emptySlot.curItemData = ItemData;
            emptySlot.quantity = 1;
            UpdateSlots();
            CharacterManager.Instance.player.itemData = null;
            return;
        }

        ThrowItem(ItemData);
        CharacterManager.Instance.player.itemData = null;
    }

    private Slot GetItemStack(ItemSO item)
    {
        for(int i = 0;i < slots.Length; i++)
        {
            if (slots[i].curItemData == item && slots[i].quantity < item.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    private Slot FindEmptySlot()
    {
        return slots.FirstOrDefault(slot => slot.curItemData == null);
    }

    private void OffDescription()
    {
        itemName.text = string.Empty;
        itemDescription.text = string.Empty;
        conditionText.text = string.Empty;
        conditionValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    private void OpenDescription(ItemSO data)
    {
        itemName.text = data.itemName;
        itemDescription.text = data.description;

        conditionText.text = string.Empty;
        conditionValue.text = string.Empty;

        if(data.itemType == ItemType.Consumable)
        {
            for (int i = 0; i < data.consumables.Length; i++)
            {
                conditionText.text += $"{data.consumables[i].ConsumableType}\n";
                conditionValue.text += $"{data.consumables[i].value}\n";
            }
        }
    }

    private void ThrowItem(ItemSO item)
    {
        Instantiate(item.prefab, dropPosition.position, Quaternion.identity);
    }

    //TODO :: 아이템슬롯 선택했을 때
    public void OnItemSlotClick(int index)
    {
        selectedIndex = index;
        ItemData = slots[index].curItemData;

        OffDescription();
        if (slots[index].curItemData == null)
        {
            return;
        }
        else
        {
            OpenDescription(slots[index].curItemData);
            switch (slots[index].curItemData.itemType)
            {
                case ItemType.Consumable:
                    useButton.SetActive(true);
                    break;
                case ItemType.Equipable:
                    if (slots[index].equipped)
                    {
                        unequipButton.SetActive(true);
                        break;
                    }
                    else
                        equipButton.SetActive(true);
                    break;
                case ItemType.Resource:
                case ItemType.Openable:
                    break;
            }
            dropButton.SetActive(true);
        }
    }

    private void RemoveSelectedItem()
    {
        slots[selectedIndex].quantity--;
        if(slots[selectedIndex].quantity <= 0)
        {
            slots[selectedIndex].quantity = 0;
            slots[selectedIndex].curItemData = null;
        }
        UpdateSlots();
    }

    //TODO :: 사용, 장착, 해제, 버리기 버튼 선택했을 때
    public void OnUseButton()
    {
        if(ItemData.itemType == ItemType.Consumable)
        {
            for (int i = 0; i < ItemData.consumables.Length; i++)
            {
                switch (ItemData.consumables[i].ConsumableType)
                {
                    case ConsumableType.HP:
                        condition.Heal(ItemData.consumables[i].value);
                        break;
                    case ConsumableType.MP:
                        condition.GetMana(ItemData.consumables[i].value);
                        break;
                    case ConsumableType.Stamina:
                        condition.GetStamina(ItemData.consumables[i].value);
                        break;
                }
            }
        }
        RemoveSelectedItem();
        OnItemSlotClick(selectedIndex);
    }

    public void OnEquipButton()
    {
        if (CharacterManager.Instance.player.equip.curEquip != null)
        {
            return;
        }
        else
        {
            slots[selectedIndex].outline.enabled = true;
            slots[selectedIndex].equipped = true;
            CharacterManager.Instance.player.equip.NewEquip(slots[selectedIndex].curItemData);
            UpdateSlots();
        }
        OnItemSlotClick(selectedIndex);
    }

    public void OnUnequipButton()
    {
        slots[selectedIndex].outline.enabled = false;
        slots[selectedIndex].equipped = false;
        CharacterManager.Instance.player.equip.UnEquip();
        OnItemSlotClick(selectedIndex);
        UpdateSlots();
    }

    public void OnDropButton()
    {
        if (slots[selectedIndex].equipped == true) return;
        ThrowItem(ItemData);
        UpdateSlots();
        RemoveSelectedItem();
    }
}
