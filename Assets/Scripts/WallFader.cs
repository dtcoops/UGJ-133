using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class WallFader : MonoBehaviour
{
    public Transform cat;
    public LayerMask wallLayer;

    readonly HashSet<Renderer> hitNow = new HashSet<Renderer>();
    readonly HashSet<Renderer> hidden = new HashSet<Renderer>();
    readonly List<Renderer> toRestore = new List<Renderer>();
    readonly RaycastHit[] hits = new RaycastHit[32];

    void LateUpdate()
    {
        hitNow.Clear();

        Vector3 origin = transform.position;
        Vector3 target = cat.position + Vector3.up * 0.5f;
        Vector3 dir = target - origin;

        int count = Physics.RaycastNonAlloc(origin, dir.normalized, hits, dir.magnitude, wallLayer);
        for (int i = 0; i < count; i++)
        {
            foreach (Renderer r in hits[i].collider.GetComponentsInChildren<Renderer>())
                hitNow.Add(r);
        }

        foreach (Renderer r in hitNow)
        {
            if (hidden.Add(r))
                r.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }

        toRestore.Clear();
        foreach (Renderer r in hidden)
        {
            if (r == null || !hitNow.Contains(r))
                toRestore.Add(r);
        }

        foreach (Renderer r in toRestore)
        {
            if (r != null) r.shadowCastingMode = ShadowCastingMode.On;
            hidden.Remove(r);
        }
    }
}