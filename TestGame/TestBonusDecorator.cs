using Libr;

namespace TestGame
{
    [TestClass]
    public class TestBonusDecorator
    {
        [TestMethod]
        public void TestApplyDamageDecorator()
        {
            float expectedValue = 40.0f;

            Libr.Timer timer = new Libr.Timer();
            Tank firstPlayer = new Tank(1);
            
            timer.AddBonus(new DamageBonusDecorator(firstPlayer),firstPlayer);
            
            float currentValue = firstPlayer.Damage;

            Assert.AreEqual(expectedValue, currentValue);
        }

        [TestMethod]
        public void TestApplyReloadDecorator()
        {
            double expectedValue = 0.2;

            Libr.Timer timer = new Libr.Timer();
            Tank firstPlayer = new Tank(1);

            timer.AddBonus(new ReloadBonusDecorator(firstPlayer), firstPlayer);

            double currentValue = firstPlayer.TimeReload;

            Assert.AreEqual(expectedValue, currentValue);
        }


        [TestMethod]
        public void TestApplyHarmfulReloadDecorator()
        {
            double expectedValue = 0.8;

            Libr.Timer timer = new Libr.Timer();
            Tank firstPlayer = new Tank(1);

            timer.AddBonus(new HarmfulReloadBonusDecorator(firstPlayer), firstPlayer);

            double currentValue = firstPlayer.TimeReload;

            Assert.AreEqual(expectedValue, currentValue);
        }

        [TestMethod]
        public void TestApplyHarmfulSpeedDecorator()
        {
            float expectedValue = 0.2f;

            Libr.Timer timer = new Libr.Timer();
            Tank firstPlayer = new Tank(1);

            timer.AddBonus(new HarmfulSpeedBonusDecorator(firstPlayer), firstPlayer);

            float currentValue = firstPlayer.Speed;

            Assert.AreEqual(expectedValue, currentValue,0.01);
        }

        [TestMethod]
        public void TestApplySpeedDecorator()
        {
            float expectedValue = 0.5f;

            Libr.Timer timer = new Libr.Timer();
            Tank firstPlayer = new Tank(1);

            timer.AddBonus(new SpeedBonusDecorator(firstPlayer), firstPlayer);

            float currentValue = firstPlayer.Speed;

            Assert.AreEqual(expectedValue, currentValue);
        }

        [TestMethod]
        public void TestApplyHarmfulProjectileDecorator()
        {
            int expectedValue = 85;

            Libr.Timer timer = new Libr.Timer();
            Tank firstPlayer = new Tank(1);

            timer.AddBonus(new HarmfulProjectileBonusDecorator(firstPlayer), firstPlayer);

            int currentValue = firstPlayer.NumProjectiles;

            Assert.AreEqual(expectedValue, currentValue);
        }
    }
}
