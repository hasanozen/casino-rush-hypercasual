using System.Collections.Generic;
using Game.LevelSystem.Base;
using JetBrains.Annotations;
using UnityEngine;

namespace Config
{
    public static class GameConfig
    {
        public static int CHARACTER_DEFAULT_HEALTH = 3;
        public static float CHARACTER_FORWARD_SPEED = 5f;
        public static float CHARACTER_SWIPE_SPEED = 10f;

        public static readonly int PLATFORM_CHIP_VALUE_MAX_INDEX = 4;
        public static readonly int[] CHIP_VALUES = new[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50};
        [CanBeNull] public static readonly int[] PLATFORM_CHIP_VALUES = new[] { 5, 10, 15, 20 };
        public static Dictionary<string, int> CHIP_FEATURES = new Dictionary<string, int>
        {
            [NameFields.MATERIAL_CHIP_RED] = CHIP_VALUES[0],
            [NameFields.MATERIAL_CHIP_BLUE] = CHIP_VALUES[1],
            [NameFields.MATERIAL_CHIP_GREEN] = CHIP_VALUES[2],
            [NameFields.MATERIAL_CHIP_PURPLE] = CHIP_VALUES[3],
            [NameFields.MATERIAL_CHIP_PINK] = CHIP_VALUES[4],
            [NameFields.MATERIAL_CHIP_YELLOW] = CHIP_VALUES[5],
            [NameFields.MATERIAL_CHIP_GRAY] = CHIP_VALUES[6],
            [NameFields.MATERIAL_CHIP_BROWN] = CHIP_VALUES[7],
            [NameFields.MATERIAL_CHIP_WHITE] = CHIP_VALUES[8],
            [NameFields.MATERIAL_CHIP_BLACK] = CHIP_VALUES[9]
            
            // [NameFields.MATERIAL_CHIP_RED] = CHIP_VALUES[0],
            // [NameFields.MATERIAL_CHIP_BLUE] = CHIP_VALUES[1],
            // [NameFields.MATERIAL_CHIP_GREEN] = CHIP_VALUES[2],
            // [NameFields.MATERIAL_CHIP_BLACK] = CHIP_VALUES[3],
            // [NameFields.MATERIAL_CHIP_PINK] = CHIP_VALUES[4],
            // [NameFields.MATERIAL_CHIP_PURPLE] = CHIP_VALUES[5],
            // [NameFields.MATERIAL_CHIP_GRAY] = CHIP_VALUES[6],
            // [NameFields.MATERIAL_CHIP_BROWN] = CHIP_VALUES[7],
            // [NameFields.MATERIAL_CHIP_WHITE] = CHIP_VALUES[8],
            // [NameFields.MATERIAL_CHIP_YELLOW] = CHIP_VALUES[9]
        };

        public static int DEFAULT_CHIP_POOL_SIZE = 50;
        public static int DEFAULT_MINI_CHIP_POOL_SIZE = 300;

        public static int CHIP_STACK_MAX_Z = 3;

        #region Gate Config

        public static Color DEFAULT_GATE_POSITIVE_COLOR = new Color32(62, 157, 249, 128);
        public static Color DEFAULT_GATE_NEGATIVE_COLOR = new Color32(255, 67, 57, 128);

        public static int MAX_MULTIPLICATION_NUMBER = 5;
        public static int MAX_DIVISION_NUMBER = 5;
        public static int MAX_ADDITION_NUMBER = 10;
        public static int MAX_SUBTRACTION_NUMBER = 5;

        #endregion
        
        public static LevelLength GetLevelLength(int levelIndex)
        {
            if (levelIndex < 5)
                return LevelLength.SHORT;
            if (levelIndex < 10)
                return LevelLength.MEDIUM;

            return LevelLength.LONG;
        }
        
        public static LevelDifficulty GetLevelDifficulty(int levelIndex)
        {
            if (levelIndex < 5)
                return LevelDifficulty.EASY;
            if (levelIndex < 10)
                return LevelDifficulty.MEDIUM;

            return LevelDifficulty.HARD;
        }

        public static LevelGameType GetEndGame()
        {
            LevelGameType levelGameType = (LevelGameType)Random.Range(0, 1);

            return levelGameType;
        }
    }
}
