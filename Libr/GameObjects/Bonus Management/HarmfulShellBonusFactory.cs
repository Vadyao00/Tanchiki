namespace Libr
{
    public class HarmfulShellBonusFactory : BonusFactory
    {
        public HarmfulShellBonusFactory(Player player) : base(player) { }
        public override Bonus CreateBonus()
        {
            return new HarmfulShellBonus(player);
        }
    }
}
