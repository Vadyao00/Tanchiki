namespace Libr
{
    public class RandomBonusFactory
    {
        private int randomBonus;

        public BonusDecorator CreateBonus(Tank player)
        {
            Random random = new();
            randomBonus = random.Next(1, 9);
            return randomBonus switch
            {
                1 => new SpeedBonusFactory(player).CreateBonus(),
                2 => new FuelBonusFactory(player).CreateBonus(),
                3 => new DamageBonusFactory(player).CreateBonus(),
                4 => new ReloadBonusFactory(player).CreateBonus(),
                5 => new ProjectileBonusFactory(player).CreateBonus(),
                6 => new HarmfulSpeedBonusFactory(player).CreateBonus(),
                7 => new HarmfulProjectileBonusFactory(player).CreateBonus(),
                8 => new HarmfulReloadBonusFactory(player).CreateBonus(),
                _ => new SpeedBonusFactory(player).CreateBonus(),
            };
        }
    }
}
