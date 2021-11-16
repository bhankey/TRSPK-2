using System.Collections;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public interface IPerformance
    {
        public static int CountOfIteration { get; } = 10000000;

        public void Check<TContainer, TArg>(TContainer c, TArg e)
            where TContainer : IList, new();
    }
}