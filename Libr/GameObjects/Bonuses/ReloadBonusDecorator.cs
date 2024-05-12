namespace Libr
{
    /// <summary>
    /// Конркетный декоратор для уменьшения скорости перезарядки.
    /// </summary>
    /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
    public class ReloadBonusDecorator(Tank player) : BonusDecorator(player)
    {
        /// <summary>
        /// Время для изменения скорости перезарядки.
        /// </summary>
        private readonly double reloadEffect = 0.3;
        /// <summary>
        /// Стандартная скорость перезарядки.
        /// </summary>
        private readonly double reloadNormal = 0.5;
        /// <summary>
        /// Метод, уменьшающий скорость перезарядки.
        /// </summary>
        public override void ActivateBonus()
        {
            if(_player.TimeReload >= reloadNormal)
                _player.TimeReload -= reloadEffect;
        }
        /// <summary>
        /// Метод, возвращающий время перезардяки к стандартному значению.
        /// </summary>
        public override void DeactivateBonus()
        {
            if(_player.TimeReload < reloadNormal)
                _player.TimeReload += reloadEffect;
        }
    }
}