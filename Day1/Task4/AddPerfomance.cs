using System.Collections;

namespace DefaultNamespace
{
    public class AddPerformance: IPerformance
    {
        private static void check<TContainer, TArg>(TContainer c, TArg arg) 
            where TContainer : IList, new()
        {
            for (var i = 0; i < IPerformance.CountOfIteration; ++i)
            {
                c.Add(arg);
            }
        }
        
        public void Check<TContainer, TArg>(TContainer c, TArg arg) 
            where TContainer : IList, new ()
        {
            check(c, arg);
        }
        
    }
}