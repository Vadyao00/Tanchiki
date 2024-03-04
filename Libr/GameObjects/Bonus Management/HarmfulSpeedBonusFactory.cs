namespace Libr
{
    public class HarmfulSpeedBonusFactory : BonusFactory
    {
        public HarmfulSpeedBonusFactory(Player player) : base(player) { }
        public override Bonus CreateBonus()
        {
            return new HarmfulSpeedBonus(player);
        }
    }
}
