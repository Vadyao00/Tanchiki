using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    interface IBonus
    {
        void ActivateBonus(Player player);
        void DeactivateBonus(Player player);
    }
}
