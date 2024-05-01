namespace Libr
{
    public class SpeedBonusFactory : BonusFactory
    {
        public SpeedBonusFactory(Tank player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new SpeedBonusDecorator(player);
        }
    }
}