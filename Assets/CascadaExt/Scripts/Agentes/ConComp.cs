using UnityEngine;


[RequireComponent(typeof(Rabbit), typeof(AgentMov), typeof(Sensor))]
[RequireComponent(typeof(Reproducer))]
public class ConComp : MonoBehaviour
{
    enum State { Wander, HuntVenus, Mating }

    State state = State.Wander;
    Rabbit rabbit;
    AgentMov mover;
    Sensor sensor;
    Reproducer repro;
    SimulacionConfig cfg;

    int venusMask;
    Venus targetVenus;
    float nextThink;

    bool Hungry => rabbit.Life < cfg.rabbitHungerThreshold;

    void Awake()
    {
        rabbit = GetComponent<Rabbit>();
        mover = GetComponent<AgentMov>();
        sensor = GetComponent<Sensor>();
        repro = GetComponent<Reproducer>();
        venusMask = LayerMask.GetMask("Venus");
    }

    void Start()
    {
        cfg = GameManager.Instance.Config;
        sensor.Radius = cfg.rabbitVisionRadius;
        nextThink = Time.time + Random.value * cfg.thinkInterval;
    }

    void Update()
    {
        if (cfg == null || rabbit.IsDead || Time.time < nextThink) return;
        nextThink = Time.time + cfg.thinkInterval;

        switch (state)
        {
            case State.Wander: UpdateWander(); break;
            case State.HuntVenus: UpdateHunt(); break;
        }
    }

    void UpdateWander()
    {
        if (Hungry)
        {
            Venus v = sensor.GetClosest<Venus>(venusMask);
            if (v != null)
            {
                targetVenus = v;
                state = State.HuntVenus;
                mover.MoveTo(v.transform.position);
                return;
            }
        }
        else if (TryReproduce()) return;

        if (mover.HasArrived) mover.Wander(cfg.wanderRadius);
    }

    void UpdateHunt()
    {
        if (targetVenus == null || !Hungry) { GoWander(); return; }

        float d = Vector3.Distance(transform.position, targetVenus.transform.position);
        if (d <= cfg.rabbitEatRadius)
        {
            targetVenus.Bite(cfg.rabbitBiteDamage);
            rabbit.ChangeLife(cfg.rabbitVenusGain);
            GoWander();
        }
        else if (mover.HasArrived) // llegó al borde del obstáculo sin alcanzarla
        {
            GoWander();
        }
    }

    void GoWander()
    {
        targetVenus = null;
        state = State.Wander;
        mover.Wander(cfg.wanderRadius);
    }

    bool TryReproduce()
    {
        if (!repro.IsAvailable) return false;
        if (GameManager.Instance.GetPopulation(Species.conejo) >= cfg.rabbitMaxPopulation) return false;
        if (rabbit.Life <= cfg.rabbitReproduceCost) return false;

        rabbit.ChangeLife(-cfg.rabbitReproduceCost);
        state = State.Mating;
        repro.BeginMating(cfg.rabbitMateWait, OnMatingFinished);
        return true;
    }

    void OnMatingFinished()
    {
        repro.SpawnChild(cfg.conejoPrb, cfg.rabbitChildLife);
        repro.StartCooldown(cfg.rabbitReproduceCooldown);
        GoWander();
    }
}