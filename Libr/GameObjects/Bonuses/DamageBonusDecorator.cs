namespace Libr
{
    public class DamageBonusDecorator(Tank player) : BonusDecorator(player)
    {
        private readonly float normalDamage = 20.0f;
        private readonly float effectDamage = 40.0f;

        public override void ActivateBonus()
        {
            _player.Damage = effectDamage;
        }

        public override void DeactivateBonus()
        {
            _player.Damage = normalDamage;
        }
    }
}
