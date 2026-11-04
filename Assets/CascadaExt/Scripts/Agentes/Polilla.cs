public class Polilla : Creature
{
    public override Species Species => Species.polilla;
    public override float MaxLife => Config.mothMaxLife;
    protected override float MoveCost => Config.mothMoveCost;
    protected override float MoveTickInterval => Config.mothMoveTickInterval;

    /// <summary>Llamado por la Venus. Avisa a las demás polillas antes de morir.</summary>
    public void GetEaten()
    {
        if (IsDead) return;
        SimulEvent.RaiseMothEaten(transform.position);
        Die();
    }
}
