namespace Libr
{
    public class HarmfulProjectileBonusFactory : BonusFactory
    {
        public HarmfulProjectileBonusFactory(Player player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new HarmfulProjectileBonusDecorator(player);
        }
    }
}
