using UnityEngine;

[CreateAssetMenu(fileName = "SimulacionConfig", menuName = "Scriptable Objects/SimulacionConfig")]
public class SimulacionConfig : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject florPrb;
    public GameObject polillaPrb;
    public GameObject venusPrb;
    public GameObject conejoPrb;

    [Header("Población inicial")]
    public int numIniP = 5;
    public int numIniV = 3;
    public int numIniC = 2;

    [Header("Mundo")]
    public float spawnradio = 20f;       // radio alrededor del centro del mapa
    public float aparicioncrias = 1.5f; // dónde aparecen las crías

    [Header("Polilla")]
    public float Pvidamax = 10f;
    public float Pmovcost = 1f;          // vida perdida por tick de movimiento
    public float mothMoveTickInterval = 1f;  // cada cuántos segundos de movimiento
    public float Penergiaganada = 4f;
    public float Pratiodereproduccion = 8f; // revisa tu regla: ¿< o > ?
    public bool Ponreproduccion = true;
    public float Prepcost = 2f;
    public float Pvision = 8f;

    [Header("Venus")]
    public float Vvidamax = 20f;
    public float Vmovcost = 2f;
    public float Venergiaganada = 5f;
    public float venusReproduceThreshold = 5f; // se reproduce si vida > umbral
    public float Vrepcost = 1f;
    public float venusReproduceCooldown = 5f;
    public float Vvision = 3f;

    [Header("Conejo")]
    public float Cvidamax = 10f;
    public float Cmovcost = 2f;
    public float rabbitMoveTickInterval = 1f;
    public float Cenergiaganada = 5f;
    public float Cratiodereproduccion = 4f; // si vida < umbral -> come
    public float rabbitMateWait = 2f;
    public float Cvision = 10f;

    [Header("Flores")]
    public bool masflores = true;
    public float Ftimeaparicion = 3f;
}
