using Libr.GameObjects.Bonuses;

namespace Libr.Utilities
{
    public class VertexGenerator
    {
        private static float[] backgroundVertArray = [
             -1.0f, 1.0f, 0.0f, 0.0f,1.0f,
             -1.0f, -1.0f, 0.0f, 0.0f,0.0f,
             1.0f, -1.0f, 0.0f, 1.0f,0.0f,
             1.0f, -1.0f, 0.0f, 1.0f,0.0f,
             1.0f, 1.0f, 0.0f, 1.0f,1.0f,
             -1.0f, 1.0f, 0.0f, 0.0f,1.0f
            ];

        private static float[] bonusVertexArray = [];
        public static float[] GetPlayerVertexArray(Tank player)
        {
            switch (player.Direction)
            {
                case Movement.Bottom:
                    return
                    [
                        player.X + player.Size/2, player.Y + player.Size/2,0.0f, 0.5f,0.5f,
                        player.X, player.Y + player.Size, 0.0f, 1.0f, 0.0f,
                        player.X, player.Y + player.Size*0.25f, 0.0f, 1.0f, 0.75f,
                        player.X + player.Size*0.42f, player.Y + player.Size*0.25f, 0.0f, 0.56f, 0.75f,
                        player.X + player.Size*0.42f, player.Y, 0.0f, 0.56f, 1.0f,
                        player.X + player.Size*0.58f, player.Y, 0.0f, 0.44f, 1.0f,
                        player.X + player.Size*0.58f, player.Y + player.Size*0.25f, 0.0f, 0.44f, 0.75f,
                        player.X + player.Size, player.Y + player.Size*0.25f, 0.0f, 0.0f, 0.75f,
                        player.X + player.Size, player.Y + player.Size, 0.0f, 0.0f, 0.0f,
                        player.X, player.Y + player.Size, 0.0f, 1.0f, 0.0f,
                        player.X + player.Size/2, player.Y + player.Size/2,0.0f, 0.5f,0.5f
                    ];
                case Movement.Left:
                    return
                    [
                        player.X + player.Size/2, player.Y + player.Size/2,0.0f, 0.5f,0.5f,
                        player.X + player.Size, player.Y + player.Size, 0.0f, 1.0f, 0.0f,
                        player.X + player.Size*0.25f, player.Y + player.Size, 0.0f, 1.0f, 0.75f,
                        player.X + player.Size*0.25f, player.Y + player.Size*0.58f, 0.0f, 0.56f, 0.75f,
                        player.X, player.Y + player.Size*0.58f, 0.0f, 0.56f, 1.0f,
                        player.X, player.Y + player.Size*0.42f, 0.0f, 0.44f, 1.0f,
                        player.X + player.Size*0.25f, player.Y + player.Size*0.42f, 0.0f, 0.44f, 0.75f,
                        player.X + player.Size*0.25f, player.Y, 0.0f, 0.0f, 0.75f,
                        player.X + player.Size, player.Y, 0.0f, 0.0f, 0.0f,
                        player.X + player.Size, player.Y + player.Size, 0.0f, 1.0f, 0.0f,
                        player.X + player.Size/2, player.Y + player.Size/2,0.0f, 0.5f,0.5f
                    ];
                case Movement.Right:
                    return
                    [
                        player.X + player.Size/2, player.Y + player.Size/2,0.0f, 0.5f,0.5f,
                        player.X, player.Y + player.Size, 0.0f, 0.0f, 0.0f,
                        player.X, player.Y, 0.0f, 1.0f, 0.0f,
                        player.X + player.Size*0.75f, player.Y, 0.0f, 1.0f, 0.75f,
                        player.X + player.Size*0.75f, player.Y + player.Size*0.42f, 0.0f, 0.56f, 0.75f,
                        player.X + player.Size, player.Y + player.Size*0.42f, 0.0f, 0.56f, 1.0f,
                        player.X + player.Size, player.Y + player.Size*0.58f, 0.0f, 0.44f, 1.0f,
                        player.X + player.Size*0.75f, player.Y + player.Size*0.58f, 0.0f, 0.44f, 0.75f,
                        player.X + player.Size*0.75f, player.Y + player.Size, 0.0f, 0.0f, 0.75f,
                        player.X, player.Y + player.Size, 0.0f, 0.0f, 0.0f,
                        player.X + player.Size/2, player.Y + player.Size/2,0.0f, 0.5f,0.5f
                    ];
                case Movement.Top:
                    return
                    [
                        player.X + player.Size/2, player.Y + player.Size/2,0.0f, 0.5f,0.5f,
                        player.X, player.Y + player.Size*0.75f, 0.0f, 0.0f, 0.75f,
                        player.X, player.Y, 0.0f, 0.0f, 0.0f,
                        player.X + player.Size, player.Y, 0.0f, 1.0f, 0.0f,
                        player.X + player.Size, player.Y + player.Size*0.75f, 0.0f, 1.0f, 0.75f,
                        player.X + player.Size * 0.58f, player.Y + player.Size*0.75f, 0.0f, 0.56f, 0.75f,
                        player.X + player.Size * 0.58f, player.Y + player.Size, 0.0f, 0.56f, 1.0f,
                        player.X + player.Size * 0.42f, player.Y + player.Size, 0.0f, 0.44f, 1.0f,
                        player.X + player.Size * 0.42f, player.Y + player.Size*0.75f, 0.0f, 0.44f, 0.75f,
                        player.X, player.Y + player.Size*0.75f, 0.0f, 0.0f, 0.75f,
                        player.X + player.Size/2, player.Y + player.Size/2,0.0f, 0.5f,0.5f
                    ];
                default:
                    return
                    [
                        player.X + player.Size/2, player.Y + player.Size/2,0.0f, 0.5f,0.5f,
                        player.X, player.Y + player.Size*0.75f, 0.0f, 0.0f, 0.75f,
                        player.X, player.Y, 0.0f, 0.0f, 0.0f,
                        player.X + player.Size, player.Y, 0.0f, 1.0f, 0.0f,
                        player.X + player.Size, player.Y + player.Size*0.75f, 0.0f, 1.0f, 0.75f,
                        player.X + player.Size * 0.58f, player.Y + player.Size*0.75f, 0.0f, 0.56f, 0.75f,
                        player.X + player.Size * 0.58f, player.Y + player.Size, 0.0f, 0.56f, 1.0f,
                        player.X + player.Size * 0.42f, player.Y + player.Size, 0.0f, 0.44f, 1.0f,
                        player.X + player.Size * 0.42f, player.Y + player.Size*0.75f, 0.0f, 0.44f, 0.75f,
                        player.X, player.Y + player.Size*0.75f, 0.0f, 0.0f, 0.75f,
                        player.X + player.Size/2, player.Y + player.Size/2,0.0f, 0.5f,0.5f
                    ];
            }
        }
        public static float[] GetWallsVertexArray(List<Wall> listWalls)
        {
            List<float> result = [];

            foreach (Wall cell in listWalls)
            {
                float[] cellVertColorArr = [
                    cell.X, cell.Y + cell.Size, 0.0f, 1.0f,1.0f,
                    cell.X, cell.Y, 0.0f, 0.0f,1.0f,
                    cell.X + cell.Size, cell.Y, 0.0f, 0.0f,0.0f,
                    cell.X + cell.Size, cell.Y, 0.0f, 0.0f,0.0f,
                    cell.X + cell.Size, cell.Y + cell.Size, 0.0f, 1.0f,0.0f,
                    cell.X, cell.Y + cell.Size, 0.0f, 1.0f,1.0f
                    ];
                foreach (float vertColor in cellVertColorArr)
                    result.Add(vertColor);
            }

            return result.ToArray();
        }
        public static float[] GetBonusVertexArray(List<VirtualBonus> virtualBonusesList)
        {
            bonusVertexArray = [];
            foreach (VirtualBonus bonus in virtualBonusesList)
            {
                bonusVertexArray = bonusVertexArray.Concat(bonus.GetVertexArray()).ToArray();
            }
            return bonusVertexArray;
        }
        public static float[] GetShellVertexArray(Tank player)
        {
            float x = 0, y = 0;
            switch (player.Direction)
            {
                case Movement.Left:
                    x = player.X;
                    y = player.Y + player.Size / 2;
                    break;
                case Movement.Top:
                    x = player.X + player.Size / 2;
                    y = player.Y + player.Size;
                    break;
                case Movement.Right:
                    x = player.X + player.Size;
                    y = player.Y + player.Size / 2;
                    break;
                case Movement.Bottom:
                    x = player.X + player.Size / 2;
                    y = player.Y;
                    break;
            }
            return [x, y];
        }

        public static float[] GetReloadLineVertexArray(Tank player)
        {
            float xStart = player.X, xEnd = player.X + player.Size, yLine = player.Y + player.Size + 0.02f;
            return [xStart, xEnd, yLine];
        }

        public static float[] GetBackgroundVertexArray() => backgroundVertArray;
        public static float[] GetSpeedFirstVertexArray() => [-1.0f, 0.75f, 0.0f, 0.0f, 0.5f, -0.975f, 0.725f, 0.0f, 0.5f, 0.0f, -0.95f, 0.75f, 0.0f, 1.0f, 0.5f, -0.975f, 0.775f, 0.0f, 0.5f, 1.0f, -1.0f, 0.75f, 0.0f, 0.0f, 0.5f];
        public static float[] GetSpeedSecondVertexArray() => [0.8f, 0.75f, 0.0f, 0.0f, 0.5f, 0.825f, 0.725f, 0.0f, 0.5f, 0.0f, 0.85f, 0.75f, 0.0f, 1.0f, 0.5f, 0.825f, 0.775f, 0.0f, 0.5f, 1.0f, 0.8f, 0.75f, 0.0f, 0.0f, 0.5f];
        public static float[] GetDamageFirstVertexArray() => [-0.94f, 0.75f, 0.0f, 0.0f, 0.5f, -0.915f, 0.725f, 0.0f, 0.5f, 0.0f, -0.89f, 0.75f, 0.0f, 1.0f, 0.5f, -0.915f, 0.775f, 0.0f, 0.5f, 1.0f, -0.94f, 0.75f, 0.0f, 0.0f, 0.5f];
        public static float[] GetDamageSecondVertexArray() => [0.86f, 0.75f, 0.0f, 0.0f, 0.5f, 0.885f, 0.725f, 0.0f, 0.5f, 0.0f, 0.91f, 0.75f, 0.0f, 1.0f, 0.5f, 0.885f, 0.775f, 0.0f, 0.5f, 1.0f, 0.86f, 0.75f, 0.0f, 0.0f, 0.5f];
        public static float[] GetReloadFirstVertexArray() => [-0.88f, 0.75f, 0.0f, 0.0f, 0.5f, -0.855f, 0.725f, 0.0f, 0.5f, 0.0f, -0.83f, 0.75f, 0.0f, 1.0f, 0.5f, -0.855f, 0.775f, 0.0f, 0.5f, 1.0f, -0.88f, 0.75f, 0.0f, 0.0f, 0.5f];
        public static float[] GetReloadSecondVertexArray() => [0.92f, 0.75f, 0.0f, 0.0f, 0.5f, 0.945f, 0.725f, 0.0f, 0.5f, 0.0f, 0.97f, 0.75f, 0.0f, 1.0f, 0.5f, 0.945f, 0.775f, 0.0f, 0.5f, 1.0f, 0.92f, 0.75f, 0.0f, 0.0f, 0.5f];
    }
}
