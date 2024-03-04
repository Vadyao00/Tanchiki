namespace Libr
{
    public class ShellBonusFactory : BonusFactory
    {
        public ShellBonusFactory(Player player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new ShellBonusDecorator(player);
        }
    }
}