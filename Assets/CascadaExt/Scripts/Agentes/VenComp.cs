using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// La Venus no se mueve. Cuando una polilla se acerca, decide una sola vez si la "casa" (la come).
/// Además intenta reproducirse siempre que pueda.
/// </summary>
[RequireComponent(typeof(Venus), typeof(Sensor), typeof(Reproducer))]
public class VenusBrain : MonoBehaviour
{
    Venus venus;
    Sensor sensor;
    Reproducer repro;
    SimulacionConfig cfg;
    int mothMask;
    float nextThink;

    readonly List<Polilla> nearby = new List<Polilla>();
    readonly List<Polilla> toRemove = new List<Polilla>();
    readonly Dictionary<Polilla, bool> decisions = new Dictionary<Polilla, bool>(); // polilla -> ¿la como?

    void Awake()
    {
        venus = GetComponent<Venus>();
        sensor = GetComponent<Sensor>();
        repro = GetComponent<Reproducer>();
        mothMask = LayerMask.GetMask("Polillas");
    }

    void Start()
    {
        cfg = GameManager.Instance.Config;
        sensor.Radius = cfg.venusSenseRadius;
        nextThink = Time.time + Random.value * cfg.thinkInterval;
    }

    void Update()
    {
        if (cfg == null || venus.IsDead || Time.time < nextThink) return;
        nextThink = Time.time + cfg.thinkInterval;

        HandleMoths();
        TryReproduce();
    }

    void HandleMoths()
    {
        sensor.GetAll(nearby, mothMask);

        // Olvida decisiones de polillas que ya se fueron o murieron
        toRemove.Clear();
        foreach (var kv in decisions)
            if (kv.Key == null || !nearby.Contains(kv.Key)) toRemove.Add(kv.Key);
        foreach (var m in toRemove) decisions.Remove(m);

        foreach (Polilla moth in nearby)
        {
            // Decide una única vez por cada polilla que pasa cerca
            if (!decisions.ContainsKey(moth))
                decisions[moth] = Random.value < cfg.venusCatchChance;

            if (!decisions[moth]) continue;

            Collider mc = moth.GetComponentInChildren<Collider>();
            Vector3 edge = mc != null ? mc.ClosestPoint(transform.position) : moth.transform.position;
            float d = Vector3.Distance(transform.position, edge);

            if (d <= cfg.venusEatRadius)
            {
                moth.GetEaten();
                venus.ChangeLife(cfg.venusMothGain - cfg.venusEatCost); // +5 y -2 en una sola operación
                decisions.Remove(moth);
                Debug.Log("Se comio a "+ moth.name);
                break; // máximo una polilla por ciclo
            }
        }
    }

    void TryReproduce()
    {
        if (!repro.IsAvailable) return;
        if (venus.Life <= cfg.venusReproduceThreshold) return;
        if (GameManager.Instance.GetPopulation(Species.venus) >= cfg.venusMaxPopulation) return;

        GameObject child = repro.SpawnChild(cfg.venusPrefab, cfg.venusChildLife);
        if (child == null) return; // no hubo hueco en el NavMesh

        venus.ChangeLife(cfg.venusReproduceGain);
        repro.StartCooldown(cfg.venusReproduceCooldown);
    }
}