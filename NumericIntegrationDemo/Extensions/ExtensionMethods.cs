using System;

namespace NumericIntegrationDemo.Extensions
{
    public static class ExtensionMethods
    {
        public static TResult Maybe<TInput, TResult>(this TInput obj, Func<TInput, TResult> evaluator)
        where TInput : class
        where TResult: class
        {
            if (obj is null) return null;
            return evaluator?.Invoke(obj);
        }
    }
}