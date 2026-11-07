using System.Collections.Generic;
using UnityEngine;


public class Sensor : MonoBehaviour
{
    [SerializeField] float radius = 8f;
    [SerializeField, Range(1f, 360f)] float viewAngle = 360f;
    [SerializeField] LayerMask defaultTargetMask;   // qué layers busca por defecto

    readonly Collider[] buffer = new Collider[64];

    public float Radius { get => radius; set => radius = value; }

    public T GetClosest<T>() where T : Component => GetClosest<T>(defaultTargetMask);

    public T GetClosest<T>(LayerMask mask) where T : Component
    {
        T best = null;
        float bestDist = float.MaxValue;
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, buffer, mask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            T candidate = buffer[i].GetComponentInParent<T>();
            if (!IsValid(candidate)) continue;

            float d = (candidate.transform.position - transform.position).sqrMagnitude;
            if (d < bestDist) { bestDist = d; best = candidate; }
        }
        return best;
    }

    /// <summary>Rellena la lista con todos los objetivos visibles de tipo T.</summary>
    public int GetAll<T>(List<T> results) where T : Component => GetAll(results, defaultTargetMask);

    public int GetAll<T>(List<T> results, LayerMask mask) where T : Component
    {
        results.Clear();
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, buffer, mask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            T candidate = buffer[i].GetComponentInParent<T>();
            if (IsValid(candidate) && !results.Contains(candidate)) results.Add(candidate);
        }
        return results.Count;
    }

    /// <summary>¿Está este punto dentro del radio y del ángulo de visión?</summary>
    public bool CanSee(Vector3 point)
    {
        Vector3 toTarget = point - transform.position;
        if (toTarget.magnitude > radius) return false;

        if (viewAngle < 360f)
        {
            Vector3 flat = new Vector3(toTarget.x, 0f, toTarget.z);
            if (Vector3.Angle(transform.forward, flat) > viewAngle * 0.5f) return false;
        }

        return true;
    }

    bool IsValid<T>(T candidate) where T : Component
    {
        if (candidate == null || candidate.gameObject == gameObject) return false;
        return CanSee(candidate.transform.position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.35f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
