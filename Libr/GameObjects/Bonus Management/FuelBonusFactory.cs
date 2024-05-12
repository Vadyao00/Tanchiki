namespace Libr
{
    /// <summary>
    /// Конкретная фабрика для добавления топлива.
    /// </summary>
    public class FuelBonusFactory : BonusFactory
    {
        /// <summary>
        /// Конструктор, передающий в базовый класс игрока.
        /// </summary>
        /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
        public FuelBonusFactory(Tank player) : base(player) { }
        /// <summary>
        ///  Метод, создающий декоратор для добавления топлива.
        /// </summary>
        /// <returns>Возвращает созданный декоратор, применяемый к игроку.</returns>
        public override BonusDecorator CreateBonus()
        {
            return new FuelBonusDecorator(player);
        }
    }
}