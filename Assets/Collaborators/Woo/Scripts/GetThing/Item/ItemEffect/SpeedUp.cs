using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : Item
{
    public override void Use()
    {
        gameObject.SetActive(false);
        GameManager.Instance.player.MoveSpeed = 15f;

        Invoke("RestoreBuff", 3f);
    }

    private void RestoreBuff()
    {
        GameManager.Instance.player.MoveSpeed = 10f;

        Destroy(gameObject);

    }
}