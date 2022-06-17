using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGroundChecker : GroundChecker
{
    public override bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(feetPos.position, detectSize, LayerMask.GetMask("Ground"));
        if (colliders.Length > 0)
            return true;
        else
            return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetPos.position, detectSize);
    }
}
