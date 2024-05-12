namespace Libr
{
    /// <summary>
    /// Конркетный декоратор для увеличения количества снарядов.
    /// </summary>
    /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
    public class ProjectileBonusDecorator(Tank player) : BonusDecorator(player)
    {
        /// <summary>
        /// Количество снарядов для увеличения.
        /// </summary>
        private readonly int Shells = 30;
        /// <summary>
        /// Метод, увеличивающий количество снарядов.
        /// </summary>
        public override void ActivateBonus()
        {
            if (_player.NumProjectiles <= 70)
                _player.NumProjectiles += Shells;
            else _player.NumProjectiles = 100;
        }

        public override void DeactivateBonus() { }
    }
}

