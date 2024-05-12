namespace Libr
{
    /// <summary>
    /// Конкретная фабрика для увеличенного урона.
    /// </summary>
    public class DamageBonusFactory : BonusFactory
    {
        /// <summary>
        /// Конструктор, передающий в базовый класс игрока.
        /// </summary>
        /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
        public DamageBonusFactory(Tank player) : base(player) { }
        /// <summary>
        ///  Метод, создающий декоратор для увеличения урона.
        /// </summary>
        /// <returns>Возвращает созданный декоратор, применяемый к игроку.</returns>
        public override BonusDecorator CreateBonus()
        {
            return new DamageBonusDecorator(player);
        }
    }
}
