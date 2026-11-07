using UnityEngine;

[CreateAssetMenu(fileName = "SimulationConfig", menuName = "Simulation/Config")]
public class SimulacionConfig : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject florPrb;
    public GameObject polillaPrb;
    public GameObject venusPrb;
    public GameObject conejoPrb;

    [Header("Población inicial")]
    public int polillasinicio = 5;
    public int venusinicio = 3;
    public int conejosinicio = 2;

    [Header("Mundo")]
    public float spawnRadius = 20f;
    public float childSpawnRadius = 1.5f;
    public float venusChildSpawnRadius = 3f;
    public float wanderRadius = 8f;        
    public float thinkInterval = 0.2f;     

    [Header("Polilla")]
    public float mothMaxLife = 10f;
    public float mothMoveCost = 1f;
    public float mothMoveTickInterval = 1f;
    public float mothFlowerGain = 4f;
    public float mothReproduceThreshold = 8f;
    public bool mothReproduceWhenAbove = true;
    public float mothReproduceCost = 2f;
    public float mothVisionRadius = 8f;
    public float mothEatDistance = 1.2f;       
    public float mothFearDuration = 5f;        
    public float mothFleeDistance = 6f;        
    public float mothChildLife = 5f;           
    public float mothMateWait = 1f;            
    public float mothReproduceCooldown = 5f;   
    public int mothMaxPopulation = 60;         

    [Header("Venus")]
    public float venusMaxLife = 20f;
    public float venusEatCost = 2f;
    public float venusMothGain = 5f;
    public float venusReproduceThreshold = 5f;
    public float venusReproduceCost = 6f;
    public float venusGracePeriod = 40f;
    public float venusReproduceGain = 1f; //este se va a quitar
    public float venusReproduceCooldown = 5f;
    public float venusSenseRadius = 3f;
    public float venusEatRadius = 1.5f;          
    [Range(0f, 1f)] public float venusCatchChance = 0.7f; 
    public float venusChildLife = 10f;         
    public int venusMaxPopulation = 30;
    public float venusUpkeepCost = 0.4f;      
    public float venusUpkeepInterval = 1f;    

    [Header("Conejo")]
    public float rabbitMaxLife = 10f;
    public float rabbitMoveCost = 2f;
    public float rabbitMoveTickInterval = 1f;
    public float rabbitVenusGain = 5f;
    public float rabbitHungerThreshold = 4f;
    public float rabbitMateWait = 2f;
    public float rabbitVisionRadius = 10f;
    public float rabbitBiteDamage = 5f;        
    public float rabbitEatRadius = 2f;         
    public float rabbitReproduceCost = 3f;     
    public float rabbitReproduceCooldown = 6f; 
    public float rabbitChildLife = 5f;         
    public int rabbitMaxPopulation = 30;       

    [Header("Flores")]
    public bool respawnFlowers = true;
    public float flowerRespawnInterval = 3f;
}