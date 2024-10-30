using UnityEngine;

public class PlayerData : EntityData
{
    [Header("Stat")]
    public float maxStamina;
    public float runningStaminaDecay = 1f;

    [Header("Util")]
    public float jumpPower;
    public float jumpStamina = 30;
    public float mouseSensitivity;
}

