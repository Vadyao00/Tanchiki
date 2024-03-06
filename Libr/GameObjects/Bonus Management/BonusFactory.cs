namespace Libr
{
    public abstract class BonusFactory(Player player)
    {
        protected Player player = player;
        public abstract BonusDecorator CreateBonus();
    }
}
