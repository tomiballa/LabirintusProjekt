using System;
using System.Collections;
using System.Numerics;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;

namespace Labirintus
{
	public class MapManager
	{
        private char[,] matrix; // map matrix
        private bool[,] discMatrix; // discovered areas
        private int[,][] moveMatrix; // valid moves matrix

        private int width;
        private int height;
        private string mapName = "";

        Labirintus.Player player;
        Labirintus.LanguageManager languageManager;

        int hasError = 0;

        int infoDiff = 4;

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public char[,] getMatrix()
        {
            return matrix;
        }

        public bool[,] getDiscMatrix()
        {
            return discMatrix;
        }

        public int[,][] getMoveMatrix()
        {
            return moveMatrix;
        }

        public void setAreaDiscovered(int i, int j)
        {
            discMatrix[i, j] = true;
        }

        public MapManager(Labirintus.Player p, Labirintus.LanguageManager lm, string inp)
		{
            languageManager = lm;
            player = p;

            if (inp.EndsWith(".SAV"))
            {
                loadMap(inp);
                return;
            }

            mapName = inp;

            initGame();

            int[] sp = getStartingPos();
            p.setPos(sp[0], sp[1]);
            setAreaDiscovered(sp[1], sp[0]);
        }

        public void initGame()
        {
            var text = File.ReadAllText(mapName);
            string[] minta = text.Split("\n");

            width = minta[0].Length;
            height = minta.Length;

            matrix = new char[height, width];
            discMatrix = new bool[height, width];
            moveMatrix = new int[height, width][];

            for (int i = 0; i < minta.Length; i++)
            {
                for (int j = 0; j < minta[i].Length; j++)
                {
                    char ch = char.Parse(minta[i].Substring(j, 1));
                    matrix[i, j] = ch;
                    moveMatrix[i, j] = getValidMoves(ch);
                }
            }
        }

        public void loadMap(string inp)
        {
            var text = File.ReadAllText(inp);
            SavedMap smap = JsonConvert.DeserializeObject<SavedMap>(text);

            if(smap == null)
            {
                Console.WriteLine("Hibás map.");
                return;
            }

            mapName = smap.mapName;
            player.setPos(smap.posX, smap.posY);
            player.setMoveCount(smap.moveCount);
            initGame();
            discMatrix = smap.discMatrix;
        }

        public void saveMap()
        {
            SavedMap savedMap = new SavedMap
            {
                mapName = mapName,
                posX = player.getX(),
                posY = player.getY(),
                moveCount = player.getMoveCount(),
                discMatrix = discMatrix,
            };

            string json = JsonConvert.SerializeObject(savedMap, Formatting.Indented);
            // {

            File.WriteAllText(mapName + ".SAV", json);
        }

        public int getTreasureCount()
        {
            int count = 0;
            foreach(char c in matrix)
            {
                if (c == '█') count++;
            }
            return count;
        }

        public int getDiscTreasureCount()
        {
            int count = 0;
            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i,j] == '█' && discMatrix[i,j]) count++;
                }
            }
            return count;
        }

        int[] getValidMoves(char ch)
        {
            // '╬','═','╦','╩','║','╣','╠','╗','╝','╚', '╔'
            switch (ch)
            {
                case '═':
                    return new int[] { 2, 4};
                case '╬':
                    return new int[] { 1, 2, 3, 4 };
                case '╦':
                    return new int[] { 2, 3, 4 };
                case '╩':
                    return new int[] { 1, 2, 4 };
                case '║':
                    return new int[] { 1, 3 };
                case '╣':
                    return new int[] { 1, 3, 4 };
                case '╠':
                    return new int[] { 1, 2, 3 };
                case '╗':
                    return new int[] { 3, 4 };
                case '╝':
                    return new int[] { 1, 4 };
                case '╚':
                    return new int[] { 1, 2 };
                case '╔':
                    return new int[] { 2, 3 };
                default:
                    return new int[] { };
            }
        }

        public int[] getStartingPos()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    int[] retVal = new int[] { j, i };
                    if (i == 0 && moveMatrix[i, j].Contains(1)) return retVal;
                    if (j == width - 1 && moveMatrix[i, j].Contains(2)) return retVal;
                    if (i == height - 1 && moveMatrix[i, j].Contains(3)) return retVal;
                    if (j == 0 && moveMatrix[i, j].Contains(4)) return retVal;
                }
            }

            throw new Exception("[MAP ERROR] There is no starting point.");
        }

        void drawThisCharacter(int i, int j)
        {
            char ch = matrix[i, j];
            char final = ch == '.' ? ' ' : ch;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            if (ch == '█') Console.ForegroundColor = ConsoleColor.Green;

            if (player.getX() == j && player.getY() == i) Console.BackgroundColor = ConsoleColor.DarkCyan;

            Console.SetCursorPosition(j + 1, i + 1);
            Console.Write(final);
        }

        public void writeMap()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (discMatrix[i, j]) drawThisCharacter(i, j);
                }
            }
            Console.ForegroundColor = ConsoleColor.Black;
        }

        string getValidMoveNames()
        {
            string[] validNames = {"north", "east", "south", "west"};
            ArrayList validMoves = new ArrayList();
            foreach (int i in moveMatrix[player.getY(), player.getX()])
            {
                validMoves.Add(languageManager.parseText(validNames[i - 1]));
            }
            return string.Join(",", (string[])validMoves.ToArray(typeof(string)));
        }

        public void showMessageAtLine(int line, string msg)
        {
            Console.SetCursorPosition(1, line);
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(1, currentLineCursor);
            Console.Write(msg);
        }

        public void writeInfo()
        {
            string[] wantWrite = new string[] { $"{languageManager.parseText("map_name")} {mapName}, {languageManager.parseText("map_size")}: {width} {languageManager.parseText("row")} x {height} {languageManager.parseText("col")}", $"{languageManager.parseText("move_count")} {player.getMoveCount()}, {languageManager.parseText("disc_rooms")} {getDiscTreasureCount()}", $"{languageManager.parseText("valid_moves")} {getValidMoveNames()}" };

            Console.ResetColor();
            for(int i = 0; i < wantWrite.Length; i++)
            {
                showMessageAtLine(height + infoDiff + i, wantWrite[i]);
            }

            if(hasError > 1)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                showMessageAtLine(height + infoDiff - 2, "");
                hasError = 0;
            } else if(hasError == 1)
            {
                hasError++;
            }
        }

        public void showError(string msg)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            showMessageAtLine(height + infoDiff - 2, languageManager.parseText(msg));
            Console.BackgroundColor = ConsoleColor.Black;
            hasError++;
        }

        public void showSuccess(string msg)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            showMessageAtLine(height + infoDiff - 2, languageManager.parseText(msg));
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}

