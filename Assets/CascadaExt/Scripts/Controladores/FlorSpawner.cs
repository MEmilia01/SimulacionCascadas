using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlorSpawner : MonoBehaviour
{
    [SerializeField] SimulacionConfig config;
    [SerializeField] Transform holderflores; // para ordenar la jerarquía

    readonly List<GameObject> flores = new List<GameObject>();
    public int numflores;
    public float respawnInterval;
    Coroutine masflores;
    

    public int ActiveFlowers
    {
        get { flores.RemoveAll(f => f == null); return flores.Count; }
    }

    public void Spawn(int count)
    {
        numflores = Mathf.Max(0, count);
        for (int i = 0; i < numflores; i++) SpawnOne();

        if (masflores != null) StopCoroutine(masflores);
        if (config.respawnFlowers) masflores = StartCoroutine(RespawnLoop());
    }

    public void SetRespawnInterval(float seconds) => respawnInterval = Mathf.Max(0.05f, seconds);


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
        flores.Add(Instantiate(config.florPrb, pos, Quaternion.identity, holderflores));
    }

    public void ClearAll()
    {
        foreach (var f in flores) if (f != null) Destroy(f);
        flores.Clear();
    }
}