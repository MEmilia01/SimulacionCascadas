using UnityEngine;

public class SimulationStarter : MonoBehaviour
{
    [SerializeField] int flowers = 15;

    void Start() => GameManager.Instance.StartSimulation(flowers);
}
