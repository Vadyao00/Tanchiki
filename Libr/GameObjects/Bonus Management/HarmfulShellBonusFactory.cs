namespace Libr
{
    public class HarmfulShellBonusFactory : BonusFactory
    {
        public HarmfulShellBonusFactory(Player player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new HarmfulShellBonusDecorator(player);
        }
    }
}
