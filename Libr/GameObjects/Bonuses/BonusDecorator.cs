namespace Libr
{
    /// <summary>
    /// Абстрактный класс декоратора.
    /// </summary>
    /// <param name="player">Объект класса <see cref="Tank"/>, представляющий игрока, который будет изменяться.</param>
    public abstract class BonusDecorator(Tank player) : Tank, IBonus
    {
        /// <summary>
        /// Объект класса Tank, представляющий игрока, который будет изменяться.
        /// </summary>
        public Tank _player = player;
        /// <summary>
        /// Время активации бонуса.
        /// </summary>
        protected TimeSpan startTime;
        /// <summary>
        /// Длительность бонуса.
        /// </summary>
        protected TimeSpan duration = TimeSpan.FromMilliseconds(10000);
        /// <summary>
        /// Абстрактный метод, активирующий бонус.
        /// </summary>
        public abstract void ActivateBonus();
        /// <summary>
        /// Абстрактный метод, деактивирующий бонус.
        /// </summary>
        public abstract void DeactivateBonus();
        /// <summary>
        /// Метод, сохраняющий время начала действия бонуса.
        /// </summary>
        public void StartDurationTracking()
        {
            startTime = TimeSpan.FromMilliseconds(Environment.TickCount);
        }
        /// <summary>
        /// Метод, проверяющий, не вышло ли время действия бонуса.
        /// </summary>
        /// <returns>Возвращает true, если время бонуса не вышло, в противном случае возвращает false.</returns>
        public bool IsExpired()
        {
            TimeSpan currentTime = TimeSpan.FromMilliseconds(Environment.TickCount);
            TimeSpan elapsedTime = currentTime - startTime;
            return elapsedTime >= duration;
        }
    }
}
