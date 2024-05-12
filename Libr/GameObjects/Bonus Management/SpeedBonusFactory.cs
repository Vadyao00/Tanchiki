namespace Libr
{
    /// <summary>
    /// Конкретная фабрика для увеличения скорости движения.
    /// </summary>
    public class SpeedBonusFactory : BonusFactory
    {
        /// <summary>
        /// Конструктор, передающий в базовый класс игрока.
        /// </summary>
        /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
        public SpeedBonusFactory(Tank player) : base(player) { }
        /// <summary>
        ///  Метод, создающий декоратор для увеличения скорости движения.
        /// </summary>
        /// <returns>Возвращает созданный декоратор, применяемый к игроку.</returns>
        public override BonusDecorator CreateBonus()
        {
            return new SpeedBonusDecorator(player);
        }
    }
}