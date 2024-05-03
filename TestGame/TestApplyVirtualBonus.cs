using Libr.GameObjects.Bonuses;
using Libr;

namespace TestGame
{
    [TestClass]
    public class TestApplyVirtualBonus
    {
        [TestMethod]
        public void TestSuccessfulApplyBonusWhileMoveLeft()
        {
            bool expectedIsUsed = true;

            List<Wall> walls = [new Wall(0.1f, 0.2f, 0.1f)];
            List<VirtualBonus> virtualBonuses = [new VirtualBonus(-0.06f, 0.74f)];
            Movement movement = Movement.Left;
            RandomBonusFactory randomBonusFactory = new RandomBonusFactory();
            Libr.Timer timer = new Libr.Timer();
            float speedKoef = 0.1f;
            Tank firstPlayer = new Tank(1);
            Tank secondPlayer = new Tank(2);

            firstPlayer.Move(movement, walls, virtualBonuses, secondPlayer, randomBonusFactory, timer, speedKoef);

            bool isUsed = virtualBonuses[0].IsUsed;


            Assert.AreEqual(expectedIsUsed, isUsed);
        }

        [TestMethod]
        public void TestSuccessfulApplyBonusWhileMoveBottom()
        {
            bool expectedIsUsed = true;

            List<Wall> walls = [new Wall(0.1f, 0.2f, 0.1f)];
            List<VirtualBonus> virtualBonuses = [new VirtualBonus(-0.04f, 0.66f)];
            Movement movement = Movement.Bottom;
            RandomBonusFactory randomBonusFactory = new RandomBonusFactory();
            Libr.Timer timer = new Libr.Timer();
            float speedKoef = 0.1f;
            Tank firstPlayer = new Tank(1);
            Tank secondPlayer = new Tank(2);

            firstPlayer.Move(movement, walls, virtualBonuses, secondPlayer, randomBonusFactory, timer, speedKoef);

            bool isUsed = virtualBonuses[0].IsUsed;


            Assert.AreEqual(expectedIsUsed, isUsed);
        }

        [TestMethod]
        public void TestNotSuccessfulApplyBonusWhileMoveLeft()
        {
            bool expectedIsUsed = false;

            List<Wall> walls = [new Wall(0.1f, 0.2f, 0.1f)];
            List<VirtualBonus> virtualBonuses = [new VirtualBonus(0.8f, 0.74f)];
            Movement movement = Movement.Left;
            RandomBonusFactory randomBonusFactory = new RandomBonusFactory();
            Libr.Timer timer = new Libr.Timer();
            float speedKoef = 0.1f;
            Tank firstPlayer = new Tank(1);
            Tank secondPlayer = new Tank(2);

            firstPlayer.Move(movement, walls, virtualBonuses, secondPlayer, randomBonusFactory, timer, speedKoef);

            bool isUsed = virtualBonuses[0].IsUsed;


            Assert.AreEqual(expectedIsUsed, isUsed);
        }

        [TestMethod]
        public void TestNotSuccessfulApplyBonusWhileMoveBottom()
        {
            bool expectedIsUsed = false;

            List<Wall> walls = [new Wall(0.1f, 0.2f, 0.1f)];
            List<VirtualBonus> virtualBonuses = [new VirtualBonus(-0.04f, -0.66f)];
            Movement movement = Movement.Bottom;
            RandomBonusFactory randomBonusFactory = new RandomBonusFactory();
            Libr.Timer timer = new Libr.Timer();
            float speedKoef = 0.1f;
            Tank firstPlayer = new Tank(1);
            Tank secondPlayer = new Tank(2);

            firstPlayer.Move(movement, walls, virtualBonuses, secondPlayer, randomBonusFactory, timer, speedKoef);

            bool isUsed = virtualBonuses[0].IsUsed;


            Assert.AreEqual(expectedIsUsed, isUsed);
        }
    }
}
