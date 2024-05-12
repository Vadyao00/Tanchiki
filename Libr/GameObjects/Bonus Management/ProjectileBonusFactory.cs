namespace Libr
{
    /// <summary>
    /// Конкретная фабрика для добавления снарядов.
    /// </summary>
    public class ProjectileBonusFactory : BonusFactory
    {
        /// <summary>
        /// Конструктор, передающий в базовый класс игрока.
        /// </summary>
        /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
        public ProjectileBonusFactory(Tank player) : base(player) { }
        /// <summary>
        ///  Метод, создающий декоратор для увелиния количества снарядов.
        /// </summary>
        /// <returns>Возвращает созданный декоратор, применяемый к игроку.</returns>
        public override BonusDecorator CreateBonus()
        {
            return new ProjectileBonusDecorator(player);
        }
    }
}