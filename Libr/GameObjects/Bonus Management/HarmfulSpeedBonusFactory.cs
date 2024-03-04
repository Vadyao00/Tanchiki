namespace Libr
{
    public class HarmfulSpeedBonusFactory : BonusFactory
    {
        public HarmfulSpeedBonusFactory(Player player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new HarmfulSpeedBonusDecorator(player);
        }
    }
}
