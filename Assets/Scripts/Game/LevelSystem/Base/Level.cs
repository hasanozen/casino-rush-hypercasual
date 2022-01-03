using UnityEngine;

namespace Game.LevelSystem.Base
{
    public enum LevelLength
    {
        SHORT = 8,
        MEDIUM = 11,
        LONG = 15
    }

    public enum LevelDifficulty
    {
        EASY = 1,
        MEDIUM = 3,
        HARD = 5
    }

    public class Level
    {
        public int LevelIndex;
        public LevelLength LevelLength;
        public LevelDifficulty LevelDifficulty;

        public Level(int levelIndex, LevelLength levelLength, LevelDifficulty levelDifficulty)
        {
            LevelIndex = levelIndex;
            LevelLength = levelLength;
            LevelDifficulty = levelDifficulty;
        }
    }
}