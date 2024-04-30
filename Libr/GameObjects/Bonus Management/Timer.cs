using System.Diagnostics;
namespace Libr
{
    public class Timer
    {
        public List<BonusDecorator> ActiveBonuses;

        public Timer()
        {
            ActiveBonuses = [];
        }

        public void AddBonus(BonusDecorator bonus,Player player)
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
