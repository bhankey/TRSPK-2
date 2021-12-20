using System.Collections.Generic;
using System.IO;

namespace LucasKanade
{
    public static class Registry
    {
        private static readonly Dictionary<string, object> rDictionary = new()
        {
            {"convolution", true},
            {"valid_conv", false},
            {"convolution_on_all_image", true}
        };

        public static object Get(string key)
        {
            return rDictionary[key];
        }
        
    }
}