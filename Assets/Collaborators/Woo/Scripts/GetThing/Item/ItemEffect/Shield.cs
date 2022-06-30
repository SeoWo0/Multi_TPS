using Photon.Pun;
using UnityEngine;
using Managers;

public class Shield : Item
{
    public override void Use()
    {
        // TODO : 플레이어 체력 1 증가
        // GameManager.Instance.player.Hp++;
        //
        // if (GameManager.Instance.player.Hp > 2)
        // {
        //     GameManager.Instance.player.Hp = 2;
        // }
        if (GameManager.Instance.player.OnShield) return;
        
        GameManager.Instance.player.ToggleShield(true);
    }
}