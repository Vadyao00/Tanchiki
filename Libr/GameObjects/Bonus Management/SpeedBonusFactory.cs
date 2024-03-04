namespace Libr
{
    public class SpeedBonusFactory : BonusFactory
    {
        public SpeedBonusFactory(Player player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new SpeedBonusDecorator(player);
        }
    }
}