﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Pun;

public class Shield : Item
{
    public override void Use()
    {
        // TODO : 플레이어 체력 1 증가
        GameManager.Instance.player.Hp++;

        if (GameManager.Instance.player.Hp > 2)
        {
            GameManager.Instance.player.Hp = 2;
        }
    }
}