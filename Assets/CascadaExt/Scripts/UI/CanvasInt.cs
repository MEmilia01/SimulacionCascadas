using UnityEngine;
using UnityEngine.UI;

public class CanvasInt : MonoBehaviour
{
    public InputField obtencionnumflores;

    [SerializeField] int flowers = 15;

    void Start()
    {
        GameManager.Instance.StartSimulation(flowers);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
