public class Venus : Creature
{
    public override Species Species => Species.venus;
    public override float MaxLife => Config.venusMaxLife;

    public void Bite(float damage) => ChangeLife(-damage);
}