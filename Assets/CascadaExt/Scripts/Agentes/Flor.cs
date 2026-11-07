using UnityEngine;

public class Flower : MonoBehaviour
{
    bool consumed;

    public bool TryConsume()
    {
        if (consumed) return false;
        consumed = true;
        Destroy(this.gameObject);
        Debug.Log("la flor comida es: " + this.gameObject.name);
        return true;
    }
}