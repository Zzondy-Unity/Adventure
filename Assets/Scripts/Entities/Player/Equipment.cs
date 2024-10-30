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
            Debug.Log("�̹� ������ �������� �����մϴ�.");
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