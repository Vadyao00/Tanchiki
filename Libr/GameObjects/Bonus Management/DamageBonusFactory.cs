namespace Libr
{
    public class DamageBonusFactory : BonusFactory
    {
        public DamageBonusFactory(Tank player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new DamageBonusDecorator(player);
        }
    }
}
