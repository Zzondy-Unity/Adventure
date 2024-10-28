using System;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;
    
    private ConditionBar HP { get { return uiCondition.HP; } }
    private ConditionBar MP { get { return uiCondition.MP; } }
    private ConditionBar Stamina {  get { return uiCondition.Stamina; } }

    public event Action OnTakeDamage;

    private void Start()
    {
        
    }


    private void Update()
    {
        HP.Add(HP.passiveValue * Time.deltaTime);
        MP.Add(MP.passiveValue * Time.deltaTime);
        Stamina.Add(Stamina.passiveValue * Time.deltaTime);
    }

    public void Heal(float amount)
    {
        HP.Add(amount);
    }
    public void TakePhysicalDamage(int damage)
    {
        HP.Subtract(damage);
        OnTakeDamage?.Invoke();
    }

    public void GetMana(float amount)
    {
        MP.Add(amount);
    }

    public void UseMana(float amount)
    {
        MP.Subtract(amount);
    }

    public bool UseStamina(float amount)
    {
        if(Stamina.curValue - amount < 0)
        {
            return false;
        }

        Stamina.Subtract(amount);
        return true;
    }

    public void GetStamina(float amount)
    {
        Stamina.Add(amount);
    }
}