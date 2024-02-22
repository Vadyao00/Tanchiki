using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public abstract class Bonus(Player player) : IBonus
    {
        protected Player _player = player;

        public abstract void ActivateBonus();

        public abstract void DeactivateBonus();

    }
}
