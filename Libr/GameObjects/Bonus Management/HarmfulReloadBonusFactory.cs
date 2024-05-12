namespace Libr
{
    /// <summary>
    /// Конкретная фабрика для увеличения скорости перезарядки.
    /// </summary>
    public class HarmfulReloadBonusFactory : BonusFactory
    {
        /// <summary>
        /// Конструктор, передающий в базовый класс игрока.
        /// </summary>
        /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
        public HarmfulReloadBonusFactory(Tank player) : base(player) { }
        /// <summary>
        ///  Метод, создающий декоратор для увеличения скорости перезарядки.
        /// </summary>
        /// <returns>Возвращает созданный декоратор, применяемый к игроку.</returns>
        public override BonusDecorator CreateBonus()
        {
            return new HarmfulReloadBonusDecorator(player);
        }
    }
}