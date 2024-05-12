using Libr;

namespace TestGame
{
    /// <summary>
    /// Класс, тестирующий работу декоратора
    /// </summary>
    [TestClass]
    public class TestBonusDecorator
    {
        /// <summary>
        /// Метод, проверяющий применение декоратора с увеличенным уроном
        /// </summary>
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
        /// <summary>
        /// Метод, проверяющий применение декоратора с уменьшенной скоростью перезарядки
        /// </summary>
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
        /// <summary>
        /// Метод, проверяющий применение декоратора с увеличенной скоростью перезарядки
        /// </summary>

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
        /// <summary>
        /// Метод, проверяющий применение декоратора с уменьшенной скоростью движения
        /// </summary>
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
        /// <summary>
        /// Метод, проверяющий применение декоратора с увеличенной скоростью движения
        /// </summary>
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
        /// <summary>
        /// Метод, проверяющий применение декоратора с уменьшение количества снарядов
        /// </summary>
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
