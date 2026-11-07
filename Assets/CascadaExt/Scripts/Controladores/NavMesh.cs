using UnityEngine;
using UnityEngine.AI;

public static class NavMesh
{
    public static bool TrySample(Vector3 position, out Vector3 result, float maxDistance = 2f, int areaMask = UnityEngine.AI.NavMesh.AllAreas)
    {
        if (UnityEngine.AI.NavMesh.SamplePosition(position, out NavMeshHit hit, maxDistance, areaMask))
        {
            result = hit.position;
            return true;
        }
        result = position;
        return false;
    }

    public static bool RandomPoint(Vector3 center, float radius, out Vector3 result, int attempts = 20)
    {
        for (int i = 0; i < attempts; i++)
        {
            Vector2 c = Random.insideUnitCircle * radius;
            Vector3 candidate = center + new Vector3(c.x, 0f, c.y);
            if (TrySample(candidate, out result, 2f)) return true;
        }
        result = center;
        return false;
    }

    public static bool IsReachable(Vector3 from, Vector3 to, int areaMask = UnityEngine.AI.NavMesh.AllAreas)
    {
        NavMeshPath path = new NavMeshPath();
        return UnityEngine.AI.NavMesh.CalculatePath(from, to, areaMask, path) && path.status == NavMeshPathStatus.PathComplete;
    }
}