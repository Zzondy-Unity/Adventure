using UnityEngine;

public interface IInteractable
{
    public string GetInteractNamePrompt();
    public string GetInteractDescriptionPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemData;

    public string GetInteractNamePrompt()
    {
        string str = $"{itemData.itemName}\n";
        return str;
    }
    public string GetInteractDescriptionPrompt()
    {
        string str = $"{itemData.description}\n";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.player.itemData = itemData;
        CharacterManager.Instance.player.addItem?.Invoke();
    }
}