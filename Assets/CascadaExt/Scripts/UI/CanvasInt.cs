using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasInt : MonoBehaviour
{
    [Header("Flores")]
    [SerializeField] TMP_InputField tiempoespera;
    [SerializeField] FlorSpawner florspawner;

    [Header("Venus: probabilidad de capturar")]
    [SerializeField] Slider proatraparpolilla;
    [SerializeField] TMP_Text mostrarprobabilidad;   // opcional: muestra el valor junto al slider

    [Header("Conejo: daño del mordisco")]
    [SerializeField] TMP_InputField ataqueconejo;

    SimulacionConfig cfg;

    void Start()
    {
        cfg = GameManager.Instance.Config;

        // --- Slider valores ---
        proatraparpolilla.minValue = 0f;
        proatraparpolilla.maxValue = 1f;
        proatraparpolilla.wholeNumbers = false;
        proatraparpolilla.SetValueWithoutNotify(cfg.venusCatchChance); // muestra el valor actual sin disparar el evento
        VisualizarProbabilidad(cfg.venusCatchChance);
        proatraparpolilla.onValueChanged.AddListener(CamProbabilidadVenus);

        
        ataqueconejo.SetTextWithoutNotify(cfg.rabbitBiteDamage.ToString(CultureInfo.InvariantCulture));
        ataqueconejo.onEndEdit.AddListener(DañoConejo);

        
        tiempoespera.onEndEdit.AddListener(GuardarTexto);

        GameManager.Instance.StartSimulation(40); // valor por defecto
    }

    void OnDestroy()
    {
        tiempoespera.onEndEdit.RemoveListener(GuardarTexto);
        ataqueconejo.onEndEdit.RemoveListener(DañoConejo);
        proatraparpolilla.onValueChanged.RemoveListener(CamProbabilidadVenus);
    }

    
    void GuardarTexto(string texto)
    {
        if (!DeStringaNum(texto, out float intervalo) || intervalo <= 0f)
        {
            Debug.LogWarning("Introduce un número mayor que 0 (puedes usar decimales).");
            return;
        }

        florspawner.SetRespawnInterval(intervalo);
        Debug.Log("Intervalo de flores: " + intervalo);
    }

    
    void CamProbabilidadVenus(float valor)
    {
        cfg.venusCatchChance = valor;
        VisualizarProbabilidad(valor);
    }

    void VisualizarProbabilidad(float valor)
    {
        if (mostrarprobabilidad != null)
            mostrarprobabilidad.text = Mathf.RoundToInt(valor * 100f) + "%";
    }

    
    void DañoConejo(string texto)
    {
        if (!DeStringaNum(texto, out float dano) || dano <= 0f)
        {
            Debug.LogWarning("El daño del mordisco debe ser un número mayor que 0.");
            // Restaura el valor válido anterior en el campo
            ataqueconejo.SetTextWithoutNotify(cfg.rabbitBiteDamage.ToString(CultureInfo.InvariantCulture));
            return;
        }

        cfg.rabbitBiteDamage = dano;
        Debug.Log("Daño del mordisco del conejo: " + dano);
    }

    static bool DeStringaNum(string texto, out float valor)
    {
        texto = texto.Replace(',', '.');
        return float.TryParse(texto, NumberStyles.Float, CultureInfo.InvariantCulture, out valor);
    }

    public void Reinicio()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(escenaActual);
    }
}
