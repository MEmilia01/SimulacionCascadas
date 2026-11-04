public class Rabbit : Creature
{
    public override Species Species => Species.conejo;
    public override float MaxLife => Config.rabbitMaxLife;
    protected override float MoveCost => Config.rabbitMoveCost;
    protected override float MoveTickInterval => Config.rabbitMoveTickInterval;
}
