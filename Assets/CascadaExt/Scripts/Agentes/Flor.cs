using UnityEngine;

/// <summary>Recurso comestible. Solo una polilla puede consumirla.</summary>
public class Flower : MonoBehaviour
{
    bool consumed;

    /// <summary>Devuelve true si este agente consiguió comerla (evita que dos polillas la coman a la vez).</summary>
    public bool TryConsume()
    {
        if (consumed) return false;
        consumed = true;
        Destroy(gameObject);
        return true;
    }
}