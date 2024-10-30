using UnityEngine;

public abstract class Button : MonoBehaviour ,IInteractable
{
    [SerializeField] protected ButtonSO buttonData;

    public string GetInteractNamePrompt()
    {
        string str = $"{buttonData.buttonName}\n";
        return str;
    }
    public string GetInteractDescriptionPrompt()
    {
        string str = $"{buttonData.buttonEffet}\n";
        return str;
    }

    public virtual void OnInteract()
    {

    }
}