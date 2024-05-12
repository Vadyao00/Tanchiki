namespace Libr
{
    /// <summary>
    /// Конкретная фабрика для уменьшения количества снарядов.
    /// </summary>
    public class HarmfulProjectileBonusFactory : BonusFactory
    {
        /// <summary>
        /// Конструктор, передающий в базовый класс игрока.
        /// </summary>
        /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
        public HarmfulProjectileBonusFactory(Tank player) : base(player) { }
        /// <summary>
        ///  Метод, создающий декоратор для уменьшения количества снарядов.
        /// </summary>
        /// <returns>Возвращает созданный декоратор, применяемый к игроку.</returns>
        public override BonusDecorator CreateBonus()
        {
            return new HarmfulProjectileBonusDecorator(player);
        }
    }
}
