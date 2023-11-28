namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Generic;

    public static class ExceptionExtensions
    {
        public static IList<Exception> GetInnerExceptions(this Exception ex, int deepLevel = 10)
        {
            var result = new List<Exception>(deepLevel) { ex };

            var exception = ex;
            for (int i = 0; i < deepLevel; i++)
            {
                var innerException = exception.InnerException;
                if (innerException == null)
                {
                    break;
                }
                exception = innerException;
                result.Add(innerException);
            }

            return result;
        }
    }
}