namespace Libr
{
    /// <summary>
    /// Конркетный декоратор для увеличения скорость движения.
    /// </summary>
    /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
    public class SpeedBonusDecorator(Tank player) : BonusDecorator(player)
    {
        /// <summary>
        /// Значение скорости движения для увеличения.
        /// </summary>
        private readonly float speedEffect = 0.15f;
        /// <summary>
        /// Стандратное значение скорости передвижения.
        /// </summary>
        private readonly float speedNormal = 0.35f;
        /// <summary>
        /// Метод, увеличивающий скорость движения.
        /// </summary>
        public override void ActivateBonus()
        {
            if(_player.Speed <= speedNormal)
                _player.Speed += speedEffect;
        }
        /// <summary>
        /// Метод, возвращающий скорость движения к стандартному значению.
        /// </summary>
        public override void DeactivateBonus()
        {
            if(_player.Speed > speedNormal)
                _player.Speed -= speedEffect;
        }
    }
}
