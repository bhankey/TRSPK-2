using System.Collections.Generic;
using System.IO;

namespace LucasKanade
{
    //В нем хранятся значения которые мы можем переключать, для изменения работы нашей программы. Для того, чтобы показать как будет работать алгоритм с модификациями и без.
    public static class Registry
    {
        private static readonly Dictionary<string, object> rDictionary = new()
        {
            {"convolution", true},
            {"valid_conv", true},
            {"convolution_on_all_image", false},
            {"convolution_coefficient", 4},
            {"interval_between_points", 15},
            {"box_size", 15},
        };

        public static object Get(string key)
        {
            return rDictionary[key];
        }
        
    }
}