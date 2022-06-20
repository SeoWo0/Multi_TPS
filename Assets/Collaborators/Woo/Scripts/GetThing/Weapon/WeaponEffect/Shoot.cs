using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shoot : Item
{

    [PunRPC]
    public override void Use()
    {
        // TODO : 쏘기
        //Attack();

        PhotonNetwork.Destroy(gameObject);
    }

    public void Attack()
    {
        PlayerMove shootingPlayer = GetComponentInParent<PlayerMove>();

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;

            Physics.Raycast(shootingPlayer.transform.position, transform.forward, out hit, LayerMask.GetMask("Player"));

            PlayerMove player = hit.collider.GetComponent<PlayerMove>();

            player.Hp -= 1;

            if (player.Hp <= 0)
            {
                player.Die();
            }
        }
    }
}
