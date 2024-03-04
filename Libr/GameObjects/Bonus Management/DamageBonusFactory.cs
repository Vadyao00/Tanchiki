namespace Libr
{
    public class DamageBonusFactory : BonusFactory
    {
        public DamageBonusFactory(Player player) : base(player) { }
        public override Bonus CreateBonus()
        {
            return new DamageBonus(player);
        }
    }
}
