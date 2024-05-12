namespace Libr
{
    /// <summary>
    /// Конркетный декоратор для увеличения урона.
    /// </summary>
    /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
    public class DamageBonusDecorator(Tank player) : BonusDecorator(player)
    {
        /// <summary>
        /// Стандартное значение урона.
        /// </summary>
        private readonly float normalDamage = 20.0f;
        /// <summary>
        /// Увеличенное значение урона.
        /// </summary>
        private readonly float effectDamage = 40.0f;
        /// <summary>
        /// Метод, применяющий эффект к игроку.
        /// </summary>
        public override void ActivateBonus()
        {
            _player.Damage = effectDamage;
        }
        /// <summary>
        /// Метод, возвращающий стандратные характеристики.
        /// </summary>
        public override void DeactivateBonus()
        {
            _player.Damage = normalDamage;
        }
    }
}
