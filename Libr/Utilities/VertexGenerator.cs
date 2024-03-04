using Libr.GameObjects.Bonuses;

namespace Libr.Utilities
{
    public class VertexGenerator
    {
        private static float[] bonusVertexArray = [];
        public static float[] GetPlayerVertexArray(Player player)
        {
            switch (player.Direction)
            {
                case Movement.Bottom:
                    return
                    [
                          player.X, player.Y + player.Size, 0.0f, 1.0f, 0.0f,
                            player.X, player.Y, 0.0f, 1.0f, 1.0f,
                            player.X + player.Size, player.Y, 0.0f, 0.0f, 1.0f,
                            player.X + player.Size, player.Y, 0.0f, 0.0f, 1.0f,
                            player.X + player.Size, player.Y + player.Size, 0.0f, 0.0f, 0.0f,
                            player.X, player.Y + player.Size, 0.0f, 1.0f, 0.0f
                    ];
                case Movement.Left:
                    return
                    [
                          player.X,player.Y + player.Size, 0.0f,1.0f,1.0f,
                             player.X,player.Y, 0.0f,0.0f, 1.0f,
                            player.X + player.Size,player.Y,0.0f,0.0f,0.0f,
                            player.X + player.Size,player.Y,0.0f,0.0f,0.0f,
                            player.X + player.Size,player.Y + player.Size,0.0f,1.0f,0.0f,
                            player.X,player.Y + player.Size,0.0f,1.0f,1.0f
                    ];
                case Movement.Right:
                    return
                    [
                        player.X, player.Y + player.Size, 0.0f, 1.0f, 0.0f,
                         player.X, player.Y, 0.0f, 0.0f, 0.0f,
                         player.X + player.Size, player.Y, 0.0f, 0.0f, 1.0f,
                         player.X + player.Size, player.Y, 0.0f, 0.0f, 1.0f,
                         player.X + player.Size, player.Y + player.Size, 0.0f, 1.0f, 1.0f,
                         player.X, player.Y + player.Size, 0.0f, 1.0f, 0.0f
                    ];
                case Movement.Top:
                    return
                    [
                        player.X, player.Y + player.Size, 0.0f, 0.0f, 1.0f,
                        player.X, player.Y, 0.0f, 0.0f, 0.0f,
                        player.X + player.Size, player.Y, 0.0f, 1.0f, 0.0f,
                        player.X + player.Size, player.Y, 0.0f, 1.0f, 0.0f,
                        player.X + player.Size, player.Y + player.Size, 0.0f, 1.0f, 1.0f,
                        player.X, player.Y + player.Size, 0.0f, 0.0f, 1.0f
                    ];
                default:
                    return
                    [
                        player.X, player.Y + player.Size, 0.0f, 1.0f, 1.0f,
                        player.X, player.Y, 0.0f, 0.0f, 1.0f,
                        player.X + player.Size, player.Y, 0.0f, 0.0f, 0.0f,
                        player.X + player.Size, player.Y, 0.0f, 0.0f, 0.0f,
                        player.X + player.Size, player.Y + player.Size, 0.0f, 1.0f, 0.0f,
                        player.X, player.Y + player.Size, 0.0f, 1.0f, 1.0f
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
        public static float[] GetShellVertexArray(Player player)
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
        public static float[] GetReloadLineVertexArray(Player player)
        {
            float xStart = player.X, xEnd = player.X + player.Size, yLine = player.Y + player.Size + 0.02f;
            return [xStart, xEnd, yLine];
        }
    }
}
