using UnityEngine;

/// <summary>
/// FSM de la polilla: Wander -> GoToFlower -> (Mating) ... y Flee si ve morir a otra polilla.
/// </summary>
[RequireComponent(typeof(Polilla), typeof(AgentMov), typeof(Sensor))]
[RequireComponent(typeof(Reproducer))]
public class PolComp : MonoBehaviour
{
    enum State { Wander, GoToFlower, Flee, Mating }

    State state = State.Wander;
    Polilla moth;
    AgentMov mover;
    Sensor sensor;
    Reproducer repro;
    SimulacionConfig cfg;

    int flowerMask, venusMask;
    Flower targetFlower;
    Vector3 fearOrigin;
    float fearUntil, nextThink;

    void Awake()
    {
        moth = GetComponent<Polilla>();
        mover = GetComponent<AgentMov>();
        sensor = GetComponent<Sensor>();
        repro = GetComponent<Reproducer>();
        flowerMask = LayerMask.GetMask("Flores");
        venusMask = LayerMask.GetMask("Venus");
    }

    void OnEnable() => SimulEvent.OnMothEaten += HandleMothEaten;
    void OnDisable() => SimulEvent.OnMothEaten -= HandleMothEaten;

    void Start()
    {
        cfg = GameManager.Instance.Config;
        sensor.Radius = cfg.mothVisionRadius;
        nextThink = Time.time + Random.value * cfg.thinkInterval; // desincroniza agentes
    }

    void Update()
    {
        if (cfg == null || moth.IsDead || Time.time < nextThink) return;
        nextThink = Time.time + cfg.thinkInterval;

        switch (state)
        {
            case State.Wander: UpdateWander(); break;
            case State.GoToFlower: UpdateGoToFlower(); break;
            case State.Flee: UpdateFlee(); break;
                // Mating: espera a que termine el Reproducer
        }
    }

    // ---------- Estados ----------
    void UpdateWander()
    {
        if (TryReproduce()) return;

        Flower flower = sensor.GetClosest<Flower>(flowerMask);
        if (flower != null)
        {
            targetFlower = flower;
            EnterState(State.GoToFlower);
            return;
        }

        if (mover.HasArrived) mover.Wander(cfg.wanderRadius);
    }

    void UpdateGoToFlower()
    {
        if (targetFlower == null) { EnterState(State.Wander); return; }
        if (TryReproduce()) return;

        float d = Vector3.Distance(transform.position, targetFlower.transform.position);
        if (d <= cfg.mothEatDistance)
        {
            if (targetFlower.TryConsume()) moth.ChangeLife(cfg.mothFlowerGain);
            targetFlower = null;
            EnterState(State.Wander);
        }
        else if (mover.HasArrived) // llegó al final del camino sin alcanzarla
        {
            targetFlower = null;
            EnterState(State.Wander);
        }
    }

    void UpdateFlee()
    {
        if (Time.time >= fearUntil) { EnterState(State.Wander); return; }
        if (mover.HasArrived) Flee();
    }

    // ---------- Acciones ----------
    void EnterState(State s)
    {
        state = s;
        switch (s)
        {
            case State.Wander: mover.Wander(cfg.wanderRadius); break;
            case State.GoToFlower: mover.MoveTo(targetFlower.transform.position); break;
            case State.Flee: Flee(); break;
        }
    }

    /// <summary>Huye en dirección contraria a la Venus o flor más cercana (o al lugar del suceso).</summary>
    void Flee()
    {
        Vector3 source = fearOrigin;
        float best = float.MaxValue;

        Venus v = sensor.GetClosest<Venus>(venusMask);
        if (v != null) { best = (v.transform.position - transform.position).sqrMagnitude; source = v.transform.position; }

        Flower f = sensor.GetClosest<Flower>(flowerMask);
        if (f != null && (f.transform.position - transform.position).sqrMagnitude < best)
            source = f.transform.position;

        mover.FleeFrom(source, cfg.mothFleeDistance);
    }

    bool TryReproduce()
    {
        if (!repro.IsAvailable) return false;
        if (GameManager.Instance.GetPopulation(Species.polilla) >= cfg.mothMaxPopulation) return false;

        bool lifeOk = cfg.mothReproduceWhenAbove
            ? moth.Life > cfg.mothReproduceThreshold
            : moth.Life < cfg.mothReproduceThreshold;
        if (!lifeOk || moth.Life <= cfg.mothReproduceCost) return false;

        moth.ChangeLife(-cfg.mothReproduceCost);
        targetFlower = null;
        state = State.Mating;
        repro.BeginMating(cfg.mothMateWait, OnMatingFinished);
        return true;
    }

    void OnMatingFinished()
    {
        repro.SpawnChild(cfg.polillaPrb, cfg.mothChildLife);
        repro.StartCooldown(cfg.mothReproduceCooldown);
        EnterState(State.Wander);
    }

    // ---------- Evento: otra polilla ha sido comida ----------
    void HandleMothEaten(Vector3 position)
    {
        if (cfg == null || moth.IsDead || state == State.Mating) return;
        if (!sensor.CanSee(position)) return; // solo reacciona si lo ve

        fearOrigin = position;
        fearUntil = Time.time + cfg.mothFearDuration;
        EnterState(State.Flee);
    }
}

