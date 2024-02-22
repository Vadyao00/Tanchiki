using System.Diagnostics;

namespace Libr.GameObjects.Bonus_Management
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

        public void AddBonus(Bonus bonus)
        {
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
