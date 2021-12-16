using System.Collections.Generic;
using System.IO;

namespace LucasKanade
{
    public static class Registry
    {
        private static readonly Dictionary<string, object> rDictionary = new()
        {
            { "gausin_filter", 1}
        };

        public static object Get(string key)
        {
            return rDictionary[key];
        }
        
    }
}