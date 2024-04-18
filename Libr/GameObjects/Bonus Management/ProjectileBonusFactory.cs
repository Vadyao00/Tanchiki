namespace Libr
{
    public class ProjectileBonusFactory : BonusFactory
    {
        public ProjectileBonusFactory(Player player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new ProjectileBonusDecorator(player);
        }
    }
}