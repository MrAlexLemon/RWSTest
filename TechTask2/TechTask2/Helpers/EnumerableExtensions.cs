using System;
using System.Collections.Generic;
using System.Linq;

namespace TechTask2.Helpers
{
    public static class EnumerableExtensions
    {
        public static void UpdateByPredicate<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector, Func<T, T> func)
        {
            var stack = new Stack<T>(items);
            while (stack.Any())
            {
                var next = stack.Pop();
                next = func(next);

                foreach (var child in childSelector(next))
                    stack.Push(child);
            }
        }
    }
}
