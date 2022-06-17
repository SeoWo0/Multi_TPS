using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    //총을 맞았을때
    void TakeDamage(int damage);

    void Die();
}
