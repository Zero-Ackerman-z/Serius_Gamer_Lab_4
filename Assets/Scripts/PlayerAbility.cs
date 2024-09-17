using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PlayerAbility
{
    protected PlayerController player;

    public PlayerAbility(PlayerController player)
    {
        this.player = player;
    }

    public abstract void Execute();
}

