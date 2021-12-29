using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class HEM
    {
        public static void SafeInvoke(this Action source)
        {
            if (source != null) source.Invoke();
        }

        public static void SafeInvoke<T>(this Action<T> source, T value)
        {
            if (source != null) source.Invoke(value);
        }

        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
        
        private static Random rng = new Random();  
        public static void Shuffle<T>(this IList<T> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
    }
}
