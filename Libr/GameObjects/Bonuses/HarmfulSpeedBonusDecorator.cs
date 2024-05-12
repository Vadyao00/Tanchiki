namespace Libr
{
    /// <summary>
    /// Конркетный декоратор для увеличения скорости движения.
    /// </summary>
    /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
    public class HarmfulSpeedBonusDecorator(Tank player) : BonusDecorator(player)
    {
        /// <summary>
        /// Значение скорости движения для уменьшения.
        /// </summary>
        private readonly float speedEffect = 0.15f;
        /// <summary>
        /// Стандартная скорость движения.
        /// </summary>
        private readonly float speedNormal = 0.35f;
        /// <summary>
        /// Метод, уменьшающий скорость.
        /// </summary>
        public override void ActivateBonus()
        {
            if(_player.Speed >= speedNormal)
                _player.Speed -= speedEffect;
        }
        /// <summary>
        /// Метод, возвращающий скорость к стандартному значению.
        /// </summary>
        public override void DeactivateBonus()
        {
            if(_player.Speed < speedNormal)
                _player.Speed += speedEffect;
        }
    }
}
