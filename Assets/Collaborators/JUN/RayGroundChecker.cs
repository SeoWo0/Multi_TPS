using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayGroundChecker : GroundChecker
{
    public override bool IsGrounded()
    {
        return Physics.Raycast(feetPos.position, Vector3.down, detectSize, LayerMask.GetMask("Ground"));
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(feetPos.position, feetPos.position + Vector3.down * detectSize, new Color(255, 0, 0));
    }
}
