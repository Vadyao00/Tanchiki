using Libr;
using Libr.GameObjects.Bonuses;
namespace TestGame
{
    /// <summary>
    ///  ласс, тестрирующий столкновение игрока с игровыми объектами
    /// </summary>
    [TestClass]
    public class TestTankCollision
    {
        /// <summary>
        /// ћетод, провер€ющий успешное движение игрока вниз
        /// </summary>
        [TestMethod]
        public void TestSuccessfulMoveBottom()
        {
            bool expectedMove = true;

            List<Wall> walls = [new Wall(0.5f,0.5f,0.1f)];
            List<VirtualBonus> virtualBonuses = [];
            Movement movement = Movement.Bottom;
            RandomBonusFactory randomBonusFactory = new RandomBonusFactory();
            Libr.Timer timer = new Libr.Timer();
            float speedKoef = 0.1f;
            Tank firstPlayer = new Tank(1);
            Tank secondPlayer = new Tank(2);
            float Y = firstPlayer.Y;

            firstPlayer.Move(movement,walls,virtualBonuses,secondPlayer,randomBonusFactory,timer,speedKoef);

            bool isMove = false;
            if(firstPlayer.Y - Y != 0)
                isMove = true;

            Assert.AreEqual(expectedMove, isMove);
        }
        /// <summary>
        /// ћетод, провер€ющий успешное движение игрока влево
        /// </summary>
        [TestMethod]
        public void TestSuccessfulMoveLeft()
        {
            bool expectedMove = true;

            List<Wall> walls = [new Wall(0.5f, 0.5f, 0.1f)];
            List<VirtualBonus> virtualBonuses = [];
            Movement movement = Movement.Left;
            RandomBonusFactory randomBonusFactory = new RandomBonusFactory();
            Libr.Timer timer = new Libr.Timer();
            float speedKoef = 0.1f;
            Tank firstPlayer = new Tank(1);
            Tank secondPlayer = new Tank(2);
            float X = firstPlayer.X;

            firstPlayer.Move(movement, walls, virtualBonuses, secondPlayer, randomBonusFactory, timer, speedKoef);

            bool isMove = false;
            if (firstPlayer.X - X != 0)
                isMove = true;

            Assert.AreEqual(expectedMove, isMove);
        }
        /// <summary>
        /// ћетод, провер€ющий неуспешное движение игрока вниз
        /// </summary>
        [TestMethod]
        public void TestNotSuccessfulMoveBottom()
        {
            bool expectedMove = false;

            List<Wall> walls = [new Wall(-0.04f, 0.66f, 0.1f)];
            List<VirtualBonus> virtualBonuses = [];
            Movement movement = Movement.Bottom;
            RandomBonusFactory randomBonusFactory = new RandomBonusFactory();
            Libr.Timer timer = new Libr.Timer();
            float speedKoef = 0.1f;
            Tank firstPlayer = new Tank(1);
            Tank secondPlayer = new Tank(2);
            float Y = firstPlayer.Y;

            firstPlayer.Move(movement, walls, virtualBonuses, secondPlayer, randomBonusFactory, timer, speedKoef);

            bool isMove = false;
            if (firstPlayer.Y - Y != 0)
                isMove = true;

            Assert.AreEqual(expectedMove, isMove);
        }
        /// <summary>
        /// ћетод, провер€ющий неуспешное движение игрока влево
        /// </summary>
        [TestMethod]
        public void TestNotSuccessfulMoveLeft()
        {
            bool expectedMove = false;

            List<Wall> walls = [new Wall(-0.1f, 0.74f, 0.1f)];
            List<VirtualBonus> virtualBonuses = [];
            Movement movement = Movement.Left;
            RandomBonusFactory randomBonusFactory = new RandomBonusFactory();
            Libr.Timer timer = new Libr.Timer();
            float speedKoef = 0.1f;
            Tank firstPlayer = new Tank(1);
            Tank secondPlayer = new Tank(2);
            float X = firstPlayer.X;

            firstPlayer.Move(movement, walls, virtualBonuses, secondPlayer, randomBonusFactory, timer, speedKoef);

            bool isMove = false;
            if (firstPlayer.X - X != 0)
                isMove = true;

            Assert.AreEqual(expectedMove, isMove);
        }
    }
}