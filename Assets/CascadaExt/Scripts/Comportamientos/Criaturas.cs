using System;
using UnityEngine;

/// <summary>
/// Clase base de Polilla, Venus y Conejo: vida, muerte, coste por movimiento
/// y aviso de nacimiento/muerte al resto de la simulación.
/// </summary>
public abstract class Creature : MonoBehaviour
{
    // --- Cada especie define estos valores leyendo del SimulationConfig ---
    public abstract Species Species { get; }
    public abstract float MaxLife { get; }
    protected virtual float MoveCost => 0f;          // vida perdida por tick de movimiento
    protected virtual float MoveTickInterval => 1f;  // segundos de movimiento por tick

    public float Life { get; private set; }
    public bool IsDead { get; private set; }

    public event Action<float> OnLifeChanged; // vida actual
    public event Action<Creature> OnDied;

    protected SimulacionConfig Config => GameManager.Instance.Config;
    protected AgentMov Mover { get; private set; }  // null en la Venus (no se mueve)

    float moveTimer;

    protected virtual void Awake()
    {
        Mover = GetComponent<AgentMov>();
        Life = MaxLife;
    }

    protected virtual void Start()
    {
        SimulEvent.RaiseBorn(Species);
    }

    protected virtual void Update()
    {
        ApplyMovementCost();
    }

    void ApplyMovementCost()
    {
        if (Mover == null || MoveCost <= 0f || IsDead) return;

        if (Mover.IsMoving)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer >= MoveTickInterval)
            {
                moveTimer -= MoveTickInterval;
                ChangeLife(-MoveCost);
            }
        }
    }

    /// <summary>Suma o resta vida. Si llega a 0 la criatura muere.</summary>
    public void ChangeLife(float delta)
    {
        if (IsDead) return;
        Life = Mathf.Min(Life + delta, MaxLife);
        OnLifeChanged?.Invoke(Life);
        if (Life <= 0f) Die();
    }

    /// <summary>Fija la vida directamente (útil para las crías).</summary>
    public void SetLife(float value)
    {
        Life = Mathf.Clamp(value, 0f, MaxLife);
        OnLifeChanged?.Invoke(Life);
        if (Life <= 0f) Die();
    }

    public virtual void Die()
    {
        if (IsDead) return;
        IsDead = true;
        OnDied?.Invoke(this);
        SimulEvent.RaiseDied(Species, transform.position);
        Destroy(gameObject);
    }
}