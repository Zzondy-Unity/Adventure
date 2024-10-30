using UnityEngine;

public enum ButtonType
{
    Fire,
    Up,
    Down
}

[CreateAssetMenu( fileName = "ButtonSO", menuName = "DefualtButtonSO", order = 1)]
public class ButtonSO : ScriptableObject
{
    [Header("Info")]
    public string buttonName;
    public string buttonEffet;
    public ButtonType buttonType;
}