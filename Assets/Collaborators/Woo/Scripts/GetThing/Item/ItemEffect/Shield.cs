using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // PhotonNetwork.Destroy();
        Destroy(gameObject);
    }
}