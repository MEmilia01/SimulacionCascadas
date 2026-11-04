using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawnea el número de flores que indica el jugador y, opcionalmente,
/// las repone para mantener ese número (así se controla el flujo de polillas).
/// </summary>
public class FlorSpawner : MonoBehaviour
{
    [SerializeField] SimulacionConfig config;
    [SerializeField] Transform holderflores; // opcional, para ordenar la jerarquía

    readonly List<GameObject> flores = new List<GameObject>();
    public int numflores;
    Coroutine masflores;

    public int ActiveFlowers
    {
        get { flores.RemoveAll(f => f == null); return flores.Count; }
    }

    /// <summary>Llamar desde GameManager / UI.</summary>
    public void Spawn(int count)
    {
        numflores = Mathf.Max(0, count);
        for (int i = 0; i < numflores; i++) SpawnOne();

        if (masflores != null) StopCoroutine(masflores);
        if (config.respawnFlowers) masflores = StartCoroutine(RespawnLoop());
    }

    /// <summary>Cambiar el objetivo en caliente (slider/input durante la partida).</summary>
    public void SetTarget(int count) => numflores = Mathf.Max(0, count);

    IEnumerator RespawnLoop()
    {
        var wait = new WaitForSeconds(config.flowerRespawnInterval);
        while (true)
        {
            yield return wait;
            if (ActiveFlowers < numflores) SpawnOne();
        }
    }

    void SpawnOne()
    {
        if (!NavMesh.RandomPoint(transform.position, config.spawnRadius, out Vector3 pos)) return;
        flores.Add(Instantiate(config.flowerPrefab, pos, Quaternion.identity, holderflores));
    }

    public void ClearAll()
    {
        foreach (var f in flores) if (f != null) Destroy(f);
        flores.Clear();
    }
}