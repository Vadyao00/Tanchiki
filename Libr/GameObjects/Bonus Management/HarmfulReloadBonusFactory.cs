namespace Libr
{
    public class HarmfulReloadBonusFactory : BonusFactory
    {
        public HarmfulReloadBonusFactory(Tank player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new HarmfulReloadBonusDecorator(player);
        }
    }
}