using Libr.GameObjects.Bonuses;

namespace Libr.Utilities
{

    /// <summary>
    /// Класс, генерирующий массивы координат игровых объектов.
    /// </summary>
    public class VertexGenerator
    {

        /// <summary>
        /// Массив вершин для заднего фона.
        /// </summary>
        private static float[] backgroundVertArray = [
             -1.0f, 1.0f, 0.0f, 0.0f,1.0f,
             -1.0f, -1.0f, 0.0f, 0.0f,0.0f,
             1.0f, -1.0f, 0.0f, 1.0f,0.0f,
             1.0f, -1.0f, 0.0f, 1.0f,0.0f,
             1.0f, 1.0f, 0.0f, 1.0f,1.0f,
             -1.0f, 1.0f, 0.0f, 0.0f,1.0f
            ];

        /// <summary>
        /// Массив вершин для бонусов.
        /// </summary>
        private static float[] bonusVertexArray = [];

        /// <summary>
        /// Метод, возвращающий массив вершин игрока.
        /// </summary>
        /// <param name="player">Игрок, вершины которого будут возвращены.</param>
        /// <returns>Возвращает массив вершин игрока.</returns>
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

        /// <summary>
        /// Метод, вовзращающий массив вершин стен.
        /// </summary>
        /// <param name="listWalls">Коллекция стен.</param>
        /// <returns>Возвращает массив вершин стен.</returns>
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

        /// <summary>
        /// Метод, возвращающий массив вершин виртуальных бонусов.
        /// </summary>
        /// <param name="virtualBonusesList">Коллекция виртуальных бонусов.</param>
        /// <returns>Вовзращает массив вершин виртуальных бонусов.</returns>
        public static float[] GetBonusVertexArray(List<VirtualBonus> virtualBonusesList)
        {
            bonusVertexArray = [];
            foreach (VirtualBonus bonus in virtualBonusesList)
            {
                bonusVertexArray = bonusVertexArray.Concat(GetVertexArrayOfVirtualBonus(bonus)).ToArray();
            }
            return bonusVertexArray;
        }

        /// <summary>
        /// Метод, возвращающий координаты снарядов игрока.
        /// </summary>
        /// <param name="player">Игрок, координаты снарядов которого будут возвращены.</param>
        /// <returns>Возвращает массив координат снарядов игрока.</returns>
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

        /// <summary>
        /// Метод, возвращающий массив вершин виртуального бонуса.
        /// </summary>
        /// <param name="virtualBonus">Виртуальный бонус, вершины которого будут возвращены.</param>
        /// <returns>Возвращает массив вершин заданного виртуального бонуса.</returns>
        public static float[] GetVertexArrayOfVirtualBonus(VirtualBonus virtualBonus)
        {
            return
            [
             virtualBonus.X, virtualBonus.Y + virtualBonus.Size, 0.0f, 0.0f,1.0f,
             virtualBonus.X, virtualBonus.Y, 0.0f, 0.0f,0.0f,
             virtualBonus.X + virtualBonus.Size, virtualBonus.Y, 0.0f, 1.0f,0.0f,
             virtualBonus.X + virtualBonus.Size, virtualBonus.Y, 0.0f, 1.0f,0.0f,
             virtualBonus.X + virtualBonus.Size, virtualBonus.Y + virtualBonus.Size, 0.0f, 1.0f,1.0f,
             virtualBonus.X, virtualBonus.Y + virtualBonus.Size, 0.0f, 0.0f,1.0f
            ];
        }

        /// <summary>
        /// Метод, вовзращающий вершины линии перезарядки игрока.
        /// </summary>
        /// <param name="player">Игрок,вершины линии перезарядки которого будут возвращены.</param>
        /// <returns>Массив вершин линии перезарядки.</returns>
        public static float[] GetReloadLineVertexArray(Tank player)
        {
            float xStart = player.X, xEnd = player.X + player.Size, yLine = player.Y + player.Size + 0.02f;
            return [xStart, xEnd, yLine];
        }

        /// <summary>
        /// Метод, возвращающий массив вершин заднего фона.
        /// </summary>
        /// <returns>Возвращает массив вершин заднего фона.</returns>
        public static float[] GetBackgroundVertexArray() => backgroundVertArray;

        /// <summary>
        /// Метод, возвращающий массив вершин информационной иконки для скорости первого игрока.
        /// </summary>
        /// <returns>Масиив вершин информационной иконки.</returns>
        public static float[] GetSpeedFirstVertexArray() => [-1.0f, 0.75f, 0.0f, 0.0f, 0.5f, -0.975f, 0.725f, 0.0f, 0.5f, 0.0f, -0.95f, 0.75f, 0.0f, 1.0f, 0.5f, -0.975f, 0.775f, 0.0f, 0.5f, 1.0f, -1.0f, 0.75f, 0.0f, 0.0f, 0.5f];

        /// <summary>
        /// Метод, возвращающий массив вершин информационной иконки для скорости второго игрока.
        /// </summary>
        /// <returns>Масиив вершин информационной иконки.</returns>
        public static float[] GetSpeedSecondVertexArray() => [0.8f, 0.75f, 0.0f, 0.0f, 0.5f, 0.825f, 0.725f, 0.0f, 0.5f, 0.0f, 0.85f, 0.75f, 0.0f, 1.0f, 0.5f, 0.825f, 0.775f, 0.0f, 0.5f, 1.0f, 0.8f, 0.75f, 0.0f, 0.0f, 0.5f];

        /// <summary>
        /// Метод, возвращающий массив вершин информационной иконки для урона первого игрока.
        /// </summary>
        /// <returns>Масиив вершин информационной иконки.</returns>
        public static float[] GetDamageFirstVertexArray() => [-0.94f, 0.75f, 0.0f, 0.0f, 0.5f, -0.915f, 0.725f, 0.0f, 0.5f, 0.0f, -0.89f, 0.75f, 0.0f, 1.0f, 0.5f, -0.915f, 0.775f, 0.0f, 0.5f, 1.0f, -0.94f, 0.75f, 0.0f, 0.0f, 0.5f];

        /// <summary>
        /// Метод, возвращающий массив вершин информационной иконки для урона второго игрока.
        /// </summary>
        /// <returns>Масиив вершин информационной иконки.</returns>
        public static float[] GetDamageSecondVertexArray() => [0.86f, 0.75f, 0.0f, 0.0f, 0.5f, 0.885f, 0.725f, 0.0f, 0.5f, 0.0f, 0.91f, 0.75f, 0.0f, 1.0f, 0.5f, 0.885f, 0.775f, 0.0f, 0.5f, 1.0f, 0.86f, 0.75f, 0.0f, 0.0f, 0.5f];

        /// <summary>
        /// Метод, возвращающий массив вершин информационной иконки для скорости перезарядки первого игрока.
        /// </summary>
        /// <returns>Масиив вершин информационной иконки.</returns>
        public static float[] GetReloadFirstVertexArray() => [-0.88f, 0.75f, 0.0f, 0.0f, 0.5f, -0.855f, 0.725f, 0.0f, 0.5f, 0.0f, -0.83f, 0.75f, 0.0f, 1.0f, 0.5f, -0.855f, 0.775f, 0.0f, 0.5f, 1.0f, -0.88f, 0.75f, 0.0f, 0.0f, 0.5f];

        /// <summary>
        /// Метод, возвращающий массив вершин информационной иконки для скорости перезарядки второго игрока.
        /// </summary>
        /// <returns>Масиив вершин информационной иконки.</returns>
        public static float[] GetReloadSecondVertexArray() => [0.92f, 0.75f, 0.0f, 0.0f, 0.5f, 0.945f, 0.725f, 0.0f, 0.5f, 0.0f, 0.97f, 0.75f, 0.0f, 1.0f, 0.5f, 0.945f, 0.775f, 0.0f, 0.5f, 1.0f, 0.92f, 0.75f, 0.0f, 0.0f, 0.5f];
    }
}
