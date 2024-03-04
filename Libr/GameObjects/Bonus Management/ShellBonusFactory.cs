namespace Libr
{
    public class ShellBonusFactory : BonusFactory
    {
        public ShellBonusFactory(Player player) : base(player) { }
        public override Bonus CreateBonus()
        {
            return new ShellBonus(player);
        }
    }
}