using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Lógica común de reproducción: cooldown, estado "apareándose" (con pausa del agente)
/// y creación de la cría. Los cerebros deciden CUÁNDO; esto decide CÓMO.
/// </summary>
public class Reproducer : MonoBehaviour
{
    float cooldownEnd;
    AgentMov mover; // puede ser null (Venus)

    public bool IsMating { get; private set; }
    public bool IsOnCooldown => Time.time < cooldownEnd;
    /// <summary>Libre para reproducirse: ni apareándose ni en cooldown.</summary>
    public bool IsAvailable => !IsMating && !IsOnCooldown;

    void Awake() => mover = GetComponent<AgentMov>();

    public void StartCooldown(float seconds) => cooldownEnd = Time.time + seconds;

    /// <summary>
    /// Entra en estado de apareamiento durante 'duration' segundos.
    /// Si tiene AgentMover, se detiene y se reanuda al terminar.
    /// </summary>
    public void BeginMating(float duration, Action onFinished = null)
    {
        if (IsMating) return;
        StartCoroutine(MatingRoutine(duration, onFinished));
    }

    IEnumerator MatingRoutine(float duration, Action onFinished)
    {
        IsMating = true;
        if (mover != null) mover.Stop();

        yield return new WaitForSeconds(duration);

        if (mover != null) mover.Resume();
        IsMating = false;
        onFinished?.Invoke();
    }

    /// <summary>
    /// Crea una cría cerca. childLife &lt; 0 = vida máxima de la especie.
    /// </summary>
    public GameObject SpawnChild(GameObject prefab, float childLife = -1f)
    {
        var gm = GameManager.Instance;
        GameObject child = gm.SpawnNear(prefab, transform.position, gm.Config.spawnradio);
        if (child == null) return null;

        if (childLife >= 0f && child.TryGetComponent(out Creature creature))
            creature.SetLife(childLife);

        return child;
    }
}