namespace Libr
{
    /// <summary>
    /// Конркетный декоратор для добавления топлива.
    /// </summary>
    /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
    public class FuelBonusDecorator(Tank player) : BonusDecorator(player)
    {
        /// <summary>
        /// Количество добавляемого топлива.
        /// </summary>
        private readonly float fuel = 10.0f;
        /// <summary>
        /// Метод, добавляющий топливо.
        /// </summary>
        public override void ActivateBonus()
        {
            if (_player.Fuel <= 90.0f)
                _player.Fuel += fuel;
            else _player.Fuel = 100.0f;
        }

        public override void DeactivateBonus() { }
    }
}
