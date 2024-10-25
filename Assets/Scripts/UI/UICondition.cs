using UnityEngine;

public class UICondition : MonoBehaviour
{
    public ConditionBar HP;
    public ConditionBar MP;
    public ConditionBar Stamina;

    private void Start()
    {
        CharacterManager.Instance.player.condition.uiCondition = this;
        HP.maxValue = CharacterManager.Instance.player.playerData.maxHealth;
        MP.maxValue = CharacterManager.Instance.player.playerData.maxMP;
        Stamina.maxValue = CharacterManager.Instance.player.playerData.maxStamina;
    }
}