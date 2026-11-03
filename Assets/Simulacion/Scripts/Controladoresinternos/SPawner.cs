using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPawner : MonoBehaviour
{
    [Header("Prefabs y transform necesarios")]
    [SerializeField] GameObject flores;
    [SerializeField] private Transform contenedorobj;

    [Header("Rango de las apariciones (X, Y, Z)")]
    [SerializeField] Vector3 minRange = new Vector3(-5f, 0f, -3f);
    [SerializeField] Vector3 maxRange = new Vector3(5f, 0f, 3f);

    [Header("Configuración")]
    [SerializeField] float tiempoEntreApariciones = 4f;
    public int numinicial;
    public int numjug;

    [Header("Holder de los objetos")]
    public List<GameObject> comidas = new List<GameObject>();

    void Start()
    {
        if (contenedorobj == null)
            contenedorobj = transform;

        AparicionInicial(numinicial);
    }




    public void AparicionInicial(int cantinicial)
    {
        for (int i = 0; i < cantinicial; i++)
        {

            Vector3 posicion = new Vector3(
                Random.Range(minRange.x, maxRange.x),
                Random.Range(minRange.y, maxRange.y),
                Random.Range(minRange.z, maxRange.z)
            );

            GameObject nuevoObjeto = Instantiate(flores, posicion, Quaternion.identity, contenedorobj);
            comidas.Add(nuevoObjeto);
        }
    }

    IEnumerator Aparicioncom(int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            yield return new WaitForSeconds(tiempoEntreApariciones);

            Vector3 posicion = new Vector3(
                Random.Range(minRange.x, maxRange.x),
                Random.Range(minRange.y, maxRange.y),
                Random.Range(minRange.z, maxRange.z)
            );

            GameObject nuevoObjeto = Instantiate(flores, posicion, Quaternion.identity, contenedorobj);
            comidas.Add(nuevoObjeto);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 centro = (minRange + maxRange) / 2f;
        Vector3 tamaño = maxRange - minRange;
        Gizmos.DrawWireCube(centro, tamaño);
    }
}