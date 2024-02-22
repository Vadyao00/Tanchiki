namespace Libr
{
    public class DamageBonus(Player player) : Bonus(player)
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
