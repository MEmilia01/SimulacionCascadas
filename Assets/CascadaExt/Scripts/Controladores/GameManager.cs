using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla el ciclo de la simulación: inicio, pausa, reinicio,
/// spawn inicial de criaturas y conteo de población por especie.
/// </summary>

[DefaultExecutionOrder(-100)]

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] SimulacionConfig config;
    [SerializeField] FlorSpawner florsp;
    [SerializeField] Transform holdercriaturas; // opcional

    public SimulacionConfig Config => config;
    public bool enEjecucion { get; private set; }
    public bool enPausa { get; private set; }

    readonly Dictionary<Species, int> poblacion = new Dictionary<Species, int>
    {
        { Species.polilla, 0 }, { Species.venus, 0 }, { Species.conejo, 0 }
    };

    /// <summary>La UI (StatsPanel) se suscribe aquí para refrescar contadores.</summary>
    public event Action poblacioncambiando;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Time.timeScale = 1f;
    }

    void OnEnable()
    {
        SimulEvent.Nacido += HandleBorn;
        SimulEvent.Muerteser += HandleDied;
    }

    void OnDisable()
    {
        SimulEvent.Nacido -= HandleBorn;
        SimulEvent.Muerteser -= HandleDied;
    }

    public int GetPopulation(Species s) => poblacion[s];

    // ---------- Ciclo ----------
    public void StartSimulation(int flowerCount)
    {
        if (enEjecucion) return;
        enEjecucion = true;

        florsp.Spawn(flowerCount);
        SpawnGroup(config.venusPrb, config.numIniV);
        SpawnGroup(config.polillaPrb, config.numIniP);
        SpawnGroup(config.conejoPrb, config.numIniC);
    }

    public void TogglePause()
    {
        enPausa = !enPausa;
        Time.timeScale = enPausa ? 0f : 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ---------- Spawn ----------
    void SpawnGroup(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
            SpawnNear(prefab, florsp.transform.position, config.spawnradio);
    }

    /// <summary>Usado también por Reproducer para instanciar crías.</summary>
    public GameObject SpawnNear(GameObject prefab, Vector3 center, float radius)
    {
        if (!NavMesh.RandomPoint(center, radius, out Vector3 pos)) return null;
        return Instantiate(prefab, pos, Quaternion.identity, holdercriaturas);
    }

    // ---------- Población ----------
    void HandleBorn(Species s) { poblacion[s]++; poblacioncambiando?.Invoke(); }
    void HandleDied(Species s, Vector3 _) { poblacion[s] = Mathf.Max(0, poblacion[s] - 1); poblacioncambiando?.Invoke(); }
}
