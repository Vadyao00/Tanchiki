namespace Libr
{
    public class FuelBonusFactory : BonusFactory
    {
        public FuelBonusFactory(Player player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new FuelBonusDecorator(player);
        }
    }
}