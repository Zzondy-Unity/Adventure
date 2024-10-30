using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;

    [SerializeField] private Transform equipPos;

    private PlayerCondition condition;
    private PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    public void NewEquip(ItemSO data)
    {
        if(curEquip != null)
        {
            Debug.Log("이미 장착된 아이템이 존재합니다.");
            return;
        }
        else
        {
            curEquip = Instantiate(data.equipPrefab, equipPos).GetComponent<Equip>();
        }
    }

    public void UnEquip()
    {
        if(curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }
}