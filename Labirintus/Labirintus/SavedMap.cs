using System;
namespace Labirintus
{
	public class SavedMap
    {
        public string mapName { get; set; }
        public bool[,] discMatrix { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public int moveCount { get; set; }
    }
}

