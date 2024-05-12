namespace Libr
{
    /// <summary>
    /// Конркетный декоратор для уменьшения количества снарядов.
    /// </summary>
    /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
    public class HarmfulProjectileBonusDecorator(Tank player) : BonusDecorator(player)
    {
        /// <summary>
        /// Количество снарядов для уменьшения.
        /// </summary>
        private readonly int Shells = 15;

        /// <summary>
        /// Метод, уменьшающий количество снарядов.
        /// </summary>
        public override void ActivateBonus()
        {
            if (_player.NumProjectiles <= Shells)
                _player.NumProjectiles = 0;
            else _player.NumProjectiles -= Shells;
        }

        public override void DeactivateBonus() { }
    }
}