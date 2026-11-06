using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasInt : MonoBehaviour
{
    public TMP_InputField obtencionnumflores;

    [SerializeField] int flowers = 15;

    void Start()
    {
        obtencionnumflores.onEndEdit.AddListener(GuardarTexto);
        //el valor por defecto es 15
        GameManager.Instance.StartSimulation(15);
    }

    void GuardarTexto(string texto)
    {
        flowers = int.Parse(texto);
        Debug.Log("Texto ingresado (TMP): " + texto);
    }
    void Update()
    {
        obtencionnumflores.onEndEdit.AddListener(GuardarTexto);
    }
}
