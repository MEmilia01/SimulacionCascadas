using UnityEngine;

/// <summary>
/// Todos los parámetros de balance en un solo asset.
/// Crear con: Click derecho en Assets/Config > Create > Simulation > Config
/// </summary>
[CreateAssetMenu(fileName = "SimulationConfig", menuName = "Simulation/Config")]
public class SimulacionConfig : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject flowerPrefab;
    public GameObject mothPrefab;
    public GameObject venusPrefab;
    public GameObject rabbitPrefab;

    [Header("Población inicial")]
    public int initialMoths = 5;
    public int initialVenus = 3;
    public int initialRabbits = 2;

    [Header("Mundo")]
    public float spawnRadius = 20f;
    public float childSpawnRadius = 1.5f;
    public float venusChildSpawnRadius = 3f;
    public float wanderRadius = 8f;        // NUEVO: radio al vagar
    public float thinkInterval = 0.2f;     // NUEVO: cada cuánto "piensan" los cerebros

    [Header("Polilla")]
    public float mothMaxLife = 10f;
    public float mothMoveCost = 1f;
    public float mothMoveTickInterval = 1f;
    public float mothFlowerGain = 4f;
    public float mothReproduceThreshold = 8f;
    public bool mothReproduceWhenAbove = true;
    public float mothReproduceCost = 2f;
    public float mothVisionRadius = 8f;
    public float mothEatDistance = 1.2f;         // NUEVO
    public float mothFearDuration = 5f;          // NUEVO: segundos huyendo tras ver una muerte
    public float mothFleeDistance = 6f;          // NUEVO
    public float mothChildLife = 5f;             // NUEVO
    public float mothMateWait = 1f;              // NUEVO
    public float mothReproduceCooldown = 5f;     // NUEVO
    public int mothMaxPopulation = 60;         // NUEVO: tope para evitar explosiones

    [Header("Venus")]
    public float venusMaxLife = 20f;
    public float venusEatCost = 2f;
    public float venusMothGain = 5f;
    public float venusReproduceThreshold = 5f;
    public float venusReproduceGain = 1f;
    public float venusReproduceCooldown = 5f;
    public float venusSenseRadius = 3f;
    public float venusEatRadius = 1.5f;          // NUEVO (<= senseRadius)
    [Range(0f, 1f)] public float venusCatchChance = 0.7f; // NUEVO: prob. de "casar" con una polilla que pasa
    public float venusChildLife = 10f;           // NUEVO
    public int venusMaxPopulation = 30;        // NUEVO

    [Header("Conejo")]
    public float rabbitMaxLife = 10f;
    public float rabbitMoveCost = 2f;
    public float rabbitMoveTickInterval = 1f;
    public float rabbitVenusGain = 5f;
    public float rabbitHungerThreshold = 4f;
    public float rabbitMateWait = 2f;
    public float rabbitVisionRadius = 10f;
    public float rabbitBiteDamage = 5f;          // NUEVO: daño que hace a la Venus
    public float rabbitEatRadius = 2f;           // NUEVO (>= radio del obstáculo + stoppingDistance)
    public float rabbitReproduceCost = 3f;       // NUEVO
    public float rabbitReproduceCooldown = 6f;   // NUEVO
    public float rabbitChildLife = 5f;           // NUEVO
    public int rabbitMaxPopulation = 30;       // NUEVO

    [Header("Flores")]
    public bool respawnFlowers = true;
    public float flowerRespawnInterval = 3f;
}