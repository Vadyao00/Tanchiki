namespace Libr
{
    public class ReloadBonusFactory : BonusFactory
    {
        public ReloadBonusFactory(Player player) : base(player) { }
        public override Bonus CreateBonus()
        {
            return new ReloadBonus(player);
        }
    }
}