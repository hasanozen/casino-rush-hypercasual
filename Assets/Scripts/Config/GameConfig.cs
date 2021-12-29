using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    public static class GameConfig
    {
        public static int CHARACTER_DEFAULT_HEALTH = 3;
        public static float CHARACTER_FORWARD_SPEED = 5f;
        public static float CHARACTER_SWIPE_SPEED = 10f;

        public static int[] CHIP_VALUES = new[] { 5, 10, 15, 20 };

        public static Dictionary<string, int> CHIP_FEATURES = new Dictionary<string, int>
        {
            [NameFields.MATERIAL_CHIP_RED] = CHIP_VALUES[0],
            [NameFields.MATERIAL_CHIP_BLUE] = CHIP_VALUES[1],
            [NameFields.MATERIAL_CHIP_GREEN] = CHIP_VALUES[2],
            [NameFields.MATERIAL_CHIP_BLACK] = CHIP_VALUES[3]
        };

        public static int DEFAULT_CHIP_POOL_SIZE = 100;
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
    }
}
