namespace Libr
{
    public class DamageBonusFactory : BonusFactory
    {
        public DamageBonusFactory(Player player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new DamageBonusDecorator(player);
        }
    }
}
