/// <summary>Datos de la Venus (no se mueve). El comportamiento está en VenusBrain.</summary>
public class Venus : Creature
{
    public override Species Species => Species.venus;
    public override float MaxLife => Config.venusMaxLife;

    /// <summary>Llamado por el conejo al morderla.</summary>
    public void Bite(float damage) => ChangeLife(-damage);
}