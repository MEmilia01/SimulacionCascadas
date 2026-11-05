using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Lógica común de reproducción: cooldown, estado "apareándose" (con pausa del agente)
/// y creación de la cría. Los cerebros deciden CUÁNDO; esto decide CÓMO.
/// </summary>
public class Reproducer : MonoBehaviour
{
    [Header("Contacto")]
    [Tooltip("Margen extra (en unidades) sobre el tamaño del collider para considerar que 'se tocan'.")]
    [SerializeField] float contactMargin = 0.3f;

    float cooldownEnd;
    AgentMov mover;        // puede ser null (Venus)
    Collider ownCollider;
    readonly Collider[] buffer = new Collider[16];

    public bool IsMating { get; private set; }
    public bool IsOnCooldown => Time.time < cooldownEnd;
    /// <summary>Libre para reproducirse: ni apareándose ni en cooldown.</summary>
    public bool IsAvailable => !IsMating && !IsOnCooldown;

    void Awake()
    {
        mover = GetComponent<AgentMov>();
        ownCollider = GetComponentInChildren<Collider>();
    }

    public void StartCooldown(float seconds) => cooldownEnd = Time.time + seconds;

    // ---------- Contacto ----------

    /// <summary>
    /// Busca otro individuo de LA MISMA ESPECIE (misma Layer) que esté tocando a este
    /// y que esté libre para reproducirse. Devuelve null si no hay ninguno.
    /// </summary>
    public Reproducer FindTouchingPartner()
    {
        if (ownCollider == null) return null;

        Bounds b = ownCollider.bounds;
        float radius = Mathf.Max(b.extents.x, b.extents.z) + contactMargin;
        int mask = 1 << gameObject.layer; // cada especie tiene su propia Layer

        int count = Physics.OverlapSphereNonAlloc(b.center, radius, buffer, mask, QueryTriggerInteraction.Collide);
        for (int i = 0; i < count; i++)
        {
            Reproducer other = buffer[i].GetComponentInParent<Reproducer>();
            if (other == null || other == this || !other.IsAvailable) continue;
            if (other.TryGetComponent(out Creature c) && c.IsDead) continue;
            return other;
        }
        return null;
    }

    /// <summary>
    /// Pone a la pareja en pausa 'duration' segundos y le aplica el cooldown al terminar.
    /// La pareja no crea cría: solo la crea quien inició el apareamiento.
    /// </summary>
    public void HoldAsPartner(float duration, float cooldownAfter)
    {
        BeginMating(duration, () => StartCooldown(cooldownAfter));
    }

    // ---------- Apareamiento ----------

    /// <summary>
    /// Entra en estado de apareamiento durante 'duration' segundos.
    /// Si tiene AgentMov, se detiene y se reanuda al terminar.
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

        float radius = TryGetComponent(out Venus _)
        ? gm.Config.venusChildSpawnRadius
        : gm.Config.childSpawnRadius;

        GameObject child = gm.SpawnNear(prefab, transform.position, gm.Config.childSpawnRadius);
        if (child == null) return null;

        if (childLife >= 0f && child.TryGetComponent(out Creature creature))
            creature.SetLife(childLife);

        return child;
    }

    // Muestra el radio de contacto al seleccionar el objeto
    void OnDrawGizmosSelected()
    {
        Collider col = GetComponentInChildren<Collider>();
        if (col == null) return;
        Bounds b = col.bounds;
        Gizmos.color = new Color(1f, 0.2f, 0.6f, 0.5f);
        Gizmos.DrawWireSphere(b.center, Mathf.Max(b.extents.x, b.extents.z) + contactMargin);
    }
}