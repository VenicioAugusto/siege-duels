using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowClosestGizmos : MonoBehaviour
{
    public Vector3 location;

    public void OnDrawGizmos()
    {
        var collider = GetComponent<Collider>();

        if (!collider)
        {
            return; // nothing to do without a collider
        }

        Vector3 closestPoint = collider.ClosestPoint(location);

        Gizmos.DrawSphere(location, 0.1f);
        Gizmos.DrawWireSphere(closestPoint, 0.1f);
    }
}
