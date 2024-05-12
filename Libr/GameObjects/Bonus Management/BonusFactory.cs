namespace Libr
{
    /// <summary>
    /// Абстрактный класс фабрики бонусов.
    /// </summary>
    /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
    public abstract class BonusFactory(Tank player)
    {
        /// <summary>
        /// Поле, содержащеее объект класса <see cref="Tank"/>.
        /// </summary>
        protected Tank player = player;
        /// <summary>
        /// Абстрактный метод, создающий бонус.
        /// </summary>
        /// <returns>Возвращает конкретный декоратор, применяемый к игроку.</returns>
        public abstract BonusDecorator CreateBonus();
    }
}
