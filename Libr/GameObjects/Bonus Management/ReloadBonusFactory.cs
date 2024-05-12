namespace Libr
{
    /// <summary>
    /// Конкретная фабрика для уменьшения времени перезарядки.
    /// </summary>
    public class ReloadBonusFactory : BonusFactory
    {
        /// <summary>
        /// Конструктор, передающий в базовый класс игрока.
        /// </summary>
        /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
        public ReloadBonusFactory(Tank player) : base(player) { }
        /// <summary>
        ///  Метод, создающий декоратор для уменьшения скорости перезарядки.
        /// </summary>
        /// <returns>Возвращает созданный декоратор, применяемый к игроку.</returns>
        public override BonusDecorator CreateBonus()
        {
            return new ReloadBonusDecorator(player);
        }
    }
}