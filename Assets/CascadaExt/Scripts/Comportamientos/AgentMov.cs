using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMov : MonoBehaviour
{
    public NavMeshAgent Agent { get; private set; }

    void Awake() => Agent = GetComponent<NavMeshAgent>();

    public bool IsMoving =>
        Agent.enabled && Agent.isOnNavMesh && !Agent.isStopped &&
        (Agent.pathPending || Agent.remainingDistance > Agent.stoppingDistance);

    public bool HasArrived =>
        Agent.enabled && Agent.isOnNavMesh && !Agent.pathPending &&
        Agent.remainingDistance <= Agent.stoppingDistance + 0.05f;

    public void SetSpeed(float speed) => Agent.speed = speed;

    public bool MoveTo(Vector3 target)
    {
        if (!Agent.isOnNavMesh) return false;
        if (!NavMesh.TrySample(target, out Vector3 valid, 2f, Agent.areaMask)) return false;

        Agent.isStopped = false;
        return Agent.SetDestination(valid);
    }

    public bool Wander(float radius)
    {
        if (NavMesh.RandomPoint(transform.position, radius, out Vector3 point))
            return MoveTo(point);
        return false;
    }

    public bool FleeFrom(Vector3 threat, float distance)
    {
        Vector3 away = transform.position - threat;
        away.y = 0f;
        if (away.sqrMagnitude < 0.01f) away = Random.insideUnitSphere;
        away.y = 0f;
        away.Normalize();

        float[] angles = { 0f, 45f, -45f, 90f, -90f, 135f, -135f };
        foreach (float a in angles)
        {
            Vector3 dir = Quaternion.Euler(0f, a, 0f) * away;
            if (NavMesh.TrySample(transform.position + dir * distance, out _, 3f, Agent.areaMask))
                return MoveTo(transform.position + dir * distance);
        }
        return false;
    }

    public void Stop()
    {
        if (!Agent.isOnNavMesh) return;
        Agent.isStopped = true;
        Agent.velocity = Vector3.zero;
    }

    public void Resume()
    {
        if (Agent.isOnNavMesh) Agent.isStopped = false;
    }
}