namespace Libr
{
    public abstract class BonusFactory(Tank player)
    {
        protected Tank player = player;
        public abstract BonusDecorator CreateBonus();
    }
}
