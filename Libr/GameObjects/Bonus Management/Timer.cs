using System.Diagnostics;
namespace Libr
{
    public class Timer
    {
        public List<Bonus> ActiveBonuses;
        public Stopwatch Stopwatch;

        public Timer()
        {
            Stopwatch = new Stopwatch();
            ActiveBonuses = [];
        }

        public void Start()
        {
            Stopwatch.Start();
        }

        public void Stop()
        {
            Stopwatch.Stop();
        }

        public void AddBonus(Bonus bonus,Player player)
        {
            List<Bonus> bonusesToRemoving = [];
            foreach (var activeBonus in ActiveBonuses)
                if((activeBonus.GetType() == bonus.GetType())&& activeBonus._player.Equals(player))
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
            List<Bonus> bonusesToRemoving = [];
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
