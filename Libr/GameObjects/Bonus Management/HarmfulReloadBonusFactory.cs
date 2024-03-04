namespace Libr
{
    public class HarmfulReloadBonusFactory : BonusFactory
    {
        public HarmfulReloadBonusFactory(Player player) : base(player) { }
        public override Bonus CreateBonus()
        {
            return new HarmfulReloadBonus(player);
        }
    }
}