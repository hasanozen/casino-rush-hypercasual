using UnityEngine;

namespace Game.LevelSystem.Base
{
    public enum LevelLength
    {
        SHORT = 11,
        MEDIUM = 14,
        LONG = 18
    }

    public enum LevelDifficulty
    {
        EASY = 1,
        MEDIUM = 3,
        HARD = 5
    }

    public enum LevelGameType
    {
        HORSE_RACE
    }

    public class Level
    {
        public int LevelIndex;
        public LevelLength LevelLength;
        public LevelDifficulty LevelDifficulty;
        public LevelGameType LevelGameType;

        public Level(int levelIndex, LevelLength levelLength, LevelDifficulty levelDifficulty, LevelGameType levelGameType)
        {
            LevelIndex = levelIndex;
            LevelLength = levelLength;
            LevelDifficulty = levelDifficulty;
            LevelGameType = levelGameType;
        }
    }
}