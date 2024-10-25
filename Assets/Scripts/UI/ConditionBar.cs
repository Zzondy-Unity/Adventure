using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionBar : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float passiveValue;
    public float startValue;
    [SerializeField] private Image uiBar;

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    private float GetPercentage()
    {
        return curValue / maxValue; 
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(0, curValue - value);
    }
}
