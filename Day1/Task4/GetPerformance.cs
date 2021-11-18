using System.Collections;

namespace DefaultNamespace
{
    public class GetPerformance: IPerformance
    { 
        private static void check<TContainer>(TContainer c) 
            where TContainer : IList, new()
        {
            for (var i = 0; i < IPerformance.CountOfIteration; ++i)
            {
                _ = c[i];
            }
        }
        
        public void Check<TContainer, TArg>(TContainer c, TArg arg) 
            where TContainer : IList, new()
        {
            check(c);
        }
    }
}