using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemSO curItemData;

    [SerializeField] private Inventory inventory;
    [SerializeField] private Image icon;
    public Outline outline;
    private Button slotButton;
    private TextMeshProUGUI quantityText;

    public int quantity;
    public bool equipped;
    public int index;

    private void Awake()
    {
        slotButton = GetComponent<Button>();
        outline = GetComponent<Outline>();
        quantityText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        outline.enabled = equipped;

        SlotClear();
    }

    public void SlotClear()
    {
        curItemData = null;
        quantityText.text = string.Empty;
    }

    public void Setslot()
    {
        icon.sprite = curItemData.icon;
        quantityText.text = quantity > 1? quantity.ToString() : string.Empty;

        if(outline != null)
        {
            outline.enabled = equipped;
        }
    }
}
