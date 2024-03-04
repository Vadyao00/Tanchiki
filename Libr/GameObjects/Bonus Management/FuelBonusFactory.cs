namespace Libr
{
    public class FuelBonusFactory : BonusFactory
    {
        public FuelBonusFactory(Player player) : base(player) { }
        public override Bonus CreateBonus()
        {
            return new FuelBonus(player);
        }
    }
}