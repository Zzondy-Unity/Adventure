using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    private Player _player;

    public Player player
    {
        get { return _player; }
        set { _player = value; }
    }





}