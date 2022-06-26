using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    //총을 맞았을때
    void TakeDamage(int damage, string attackerName, int attackerNumber);

    void Die(string killerName, int killerNumber);
}
