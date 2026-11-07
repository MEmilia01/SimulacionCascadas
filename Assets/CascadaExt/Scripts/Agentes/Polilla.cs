public class Polilla : Creature
{
    public override Species Species => Species.polilla;
    public override float MaxLife => Config.mothMaxLife;
    protected override float MoveCost => Config.mothMoveCost;
    protected override float MoveTickInterval => Config.mothMoveTickInterval;

    public void GetEaten()
    {
        if (IsDead) return;
        SimulEvent.RaiseMothEaten(transform.position);
        Die();
    }
}
