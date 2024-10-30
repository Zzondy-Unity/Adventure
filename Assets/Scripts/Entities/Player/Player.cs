using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Player : MonoBehaviour
{
    internal PlayerController controller;
    internal PlayerCondition condition;
    internal PlayerData playerData;
    internal ItemSO itemData;
    internal Equipment equip;
    internal Rigidbody rb;

    public Action addItem;

    private void Awake()
    {
        CharacterManager.Instance.player = this;

        controller = GetComponent<PlayerController>();
        playerData = GetComponent<PlayerData>();
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
        rb = GetComponent<Rigidbody>();
    }
}
