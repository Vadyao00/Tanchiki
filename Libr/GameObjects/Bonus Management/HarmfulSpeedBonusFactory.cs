namespace Libr
{
    /// <summary>
    /// Конкретная фабрика для уменьшения скорости передвижения.
    /// </summary>
    public class HarmfulSpeedBonusFactory : BonusFactory
    {
        /// <summary>
        /// Конструктор, передающий в базовый класс игрока.
        /// </summary>
        /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
        public HarmfulSpeedBonusFactory(Tank player) : base(player) { }
        /// <summary>
        ///  Метод, создающий декоратор для уменьшения скорости движения.
        /// </summary>
        /// <returns>Возвращает созданный декоратор, применяемый к игроку.</returns>
        public override BonusDecorator CreateBonus()
        {
            return new HarmfulSpeedBonusDecorator(player);
        }
    }
}
