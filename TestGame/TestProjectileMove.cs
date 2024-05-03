using Libr;
using Libr.GameObjects.Projectilies;

namespace TestGame
{
    [TestClass]
    public class TestProjectileMove
    {
        [TestMethod]
        public void TestNotSuccessfulProjectileMoveBottom()
        {
            bool expectedIsMove = false;

            List<Wall> walls = [new Wall(-0.04f, 0.7f, 0.1f)];
            List<Projectile> projectilesToRemove = [];
            Projectile projectile = new Projectile(Movement.Bottom, [-0.03f,0.71f]);

            projectile.Move(walls, projectilesToRemove, new Tank(1),new Tank(2),1,0.01f);

            bool isMove = true;
            if(projectilesToRemove.Count > 0) { isMove = false;}

            Assert.AreEqual(expectedIsMove, isMove);
        }

        [TestMethod]
        public void TestSuccessfulProjectileMoveBottom()
        {
            bool expectedIsMove = true;

            List<Wall> walls = [new Wall(-0.04f, -0.7f, 0.1f)];
            List<Projectile> projectilesToRemove = [];
            Projectile projectile = new Projectile(Movement.Bottom, [-0.03f, 0.71f]);

            projectile.Move(walls, projectilesToRemove, new Tank(1), new Tank(2), 1, 0.01f);

            bool isMove = true;
            if (projectilesToRemove.Count > 0) { isMove = false; }

            Assert.AreEqual(expectedIsMove, isMove);
        }

        [TestMethod]
        public void TestNotSuccessfulProjectileMoveLeft()
        {
            bool expectedIsMove = false;

            List<Wall> walls = [new Wall(-0.04f, 0.76f, 0.1f)];
            List<Projectile> projectilesToRemove = [];
            Projectile projectile = new Projectile(Movement.Left, [-0.03f, 0.78f]);

            projectile.Move(walls, projectilesToRemove, new Tank(1), new Tank(2), 1, 0.01f);

            bool isMove = true;
            if (projectilesToRemove.Count > 0) { isMove = false; }

            Assert.AreEqual(expectedIsMove, isMove);
        }

        [TestMethod]
        public void TestSuccessfulProjectileMoveLeft()
        {
            bool expectedIsMove = true;

            List<Wall> walls = [new Wall(-0.04f, -0.76f, 0.1f)];
            List<Projectile> projectilesToRemove = [];
            Projectile projectile = new Projectile(Movement.Left, [-0.03f, 0.78f]);

            projectile.Move(walls, projectilesToRemove, new Tank(1), new Tank(2), 1, 0.01f);

            bool isMove = true;
            if (projectilesToRemove.Count > 0) { isMove = false; }

            Assert.AreEqual(expectedIsMove, isMove);
        }

        [TestMethod]
        public void TestSuccessfulProjectileCollisionWithPlayer()
        {
            bool expectedCollision = true;

            List<Wall> walls = [new Wall(-0.04f, -0.76f, 0.1f)];
            List<Projectile> projectilesToRemove = [];
            Projectile projectile = new Projectile(Movement.Top, [-0.03f, 0.75f]);

            projectile.Move(walls, projectilesToRemove, new Tank(1), new Tank(2), 2, 0.01f);

            bool isCollision = true;
            if (projectilesToRemove.Count > 0) { isCollision = false; }

            Assert.AreEqual(expectedCollision, isCollision);
        }

        [TestMethod]
        public void TestNotSuccessfulProjectileCollisionWithPlayer()
        {
            bool expectedCollision = false;

            List<Wall> walls = [new Wall(-0.04f, -0.76f, 0.1f)];
            List<Projectile> projectilesToRemove = [];
            Projectile projectile = new Projectile(Movement.Top, [-0.03f, -0.75f]);

            projectile.Move(walls, projectilesToRemove, new Tank(1), new Tank(2), 2, 0.01f);

            bool isCollision = true;
            if (projectilesToRemove.Count > 0) { isCollision = false; }

            Assert.AreEqual(expectedCollision, isCollision);
        }
    }
}
