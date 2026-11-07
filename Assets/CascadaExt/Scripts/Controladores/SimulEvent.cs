using System;
using UnityEngine;

public enum Species { polilla, venus, conejo }


public static class SimulEvent
{
    public static event Action<Species> Nacido;
    public static event Action<Species, Vector3> Muerteser;
    public static event Action<Vector3> OnMothEaten; // posición donde ocurrió

    public static void RaiseBorn(Species s) => Nacido?.Invoke(s);
    public static void RaiseDied(Species s, Vector3 pos) => Muerteser?.Invoke(s, pos);
    public static void RaiseMothEaten(Vector3 pos) => OnMothEaten?.Invoke(pos);

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetAll()
    {
        Nacido = null;
        Muerteser = null;
        OnMothEaten = null;
    }
}