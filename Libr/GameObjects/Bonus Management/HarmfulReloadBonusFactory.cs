namespace Libr
{
    public class HarmfulReloadBonusFactory : BonusFactory
    {
        public HarmfulReloadBonusFactory(Player player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new HarmfulReloadBonusDecorator(player);
        }
    }
}