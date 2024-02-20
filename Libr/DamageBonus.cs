using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libr
{
    public class DamageBonus : Bonus
    {
        private readonly float normalDamage = 20.0f;
        private readonly float effectDamage = 40.0f;
        public DamageBonus() : base() { }

        public override void ActivateBonus(Player player)
        {
            player.Damage = effectDamage;
        }

        public override void DeactivateBonus(Player player)
        {
            player.Damage = normalDamage;
        }
    }
}
