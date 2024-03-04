﻿namespace Libr
{
    public class ReloadBonusFactory : BonusFactory
    {
        public ReloadBonusFactory(Player player) : base(player) { }
        public override BonusDecorator CreateBonus()
        {
            return new ReloadBonusDecorator(player);
        }
    }
}