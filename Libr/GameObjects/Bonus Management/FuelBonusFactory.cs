namespace Libr
{
    public class FuelBonusFactory : BonusFactory
    {
        public FuelBonusFactory(Tank player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new FuelBonusDecorator(player);
        }
    }
}