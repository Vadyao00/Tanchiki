namespace Libr
{
    /// <summary>
    /// Класс таймера.
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// Коллекция активных бонусов.
        /// </summary>
        public List<BonusDecorator> ActiveBonuses;

        /// <summary>
        /// Конструктор класса, инициализирующий коллекцию бонусов.
        /// </summary>
        public Timer()
        {
            ActiveBonuses = [];
        }

        /// <summary>
        /// Метод, добавляющий в коллекцию активынх бонусов новый бонус.
        /// </summary>
        /// <param name="bonus">Бонус, который добавляется.</param>
        /// <param name="player">Игрок, к которому применяется бонус.</param>
        public void AddBonus(BonusDecorator bonus,Tank player)
        {
            List<BonusDecorator> bonusesToRemoving = [];
            foreach (var activeBonus in ActiveBonuses)
                if ((activeBonus.GetType() == bonus.GetType()) && activeBonus._player.Equals(player))
                {
                    activeBonus.DeactivateBonus();
                    bonusesToRemoving.Add(activeBonus);
                }
            foreach (var myBonus in bonusesToRemoving)
            {
                ActiveBonuses.Remove(myBonus);
            }
            ActiveBonuses.Add(bonus);
            bonus.ActivateBonus();
            bonus.StartDurationTracking();
        }

        /// <summary>
        /// Метод, деактиварующий и удаляющий неактивные бонусы.
        /// </summary>
        public void Update()
        {
            List<BonusDecorator> bonusesToRemoving = [];
            foreach (var bonus in ActiveBonuses)
            {
                if(bonus.IsExpired())
                {
                    bonus.DeactivateBonus();
                    bonusesToRemoving.Add(bonus);
                }
            }
            foreach (var bonus in bonusesToRemoving)
            {
                ActiveBonuses.Remove(bonus);
            }
        }
    }
}
