using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    public event Action poblacioncambiando;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        config = Instantiate(config);
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

    public void StartSimulation(int flowerCount)
    {
        if (enEjecucion) return;
        enEjecucion = true;

        florsp.Spawn(flowerCount);
        SpawnGroup(config.venusPrb, config.venusinicio);
        SpawnGroup(config.polillaPrb, config.polillasinicio);
        SpawnGroup(config.conejoPrb, config.conejosinicio);
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

    void SpawnGroup(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
            SpawnNear(prefab, florsp.transform.position, config.spawnRadius);
    }

    public GameObject SpawnNear(GameObject prefab, Vector3 center, float radius)
    {
        if (!NavMesh.RandomPoint(center, radius, out Vector3 pos)) return null;
        return Instantiate(prefab, pos, Quaternion.identity, holdercriaturas);
    }

    void HandleBorn(Species s) { poblacion[s]++; poblacioncambiando?.Invoke(); }
    void HandleDied(Species s, Vector3 _) { poblacion[s] = Mathf.Max(0, poblacion[s] - 1); poblacioncambiando?.Invoke(); }
}
