namespace Libr
{
    public class SpeedBonusFactory : BonusFactory
    {
        public SpeedBonusFactory(Player player) : base(player) { }
        public override Bonus CreateBonus()
        {
            return new SpeedBonus(player);
        }
    }
}