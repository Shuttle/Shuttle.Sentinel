using System;
using System.Collections.Generic;
using System.Linq;
using Shuttle.Core.Contract;

namespace Shuttle.Sentinel
{
    public static class EnumerableExtensions
    {
        public static T Get<T>(this IEnumerable<T> enumerable)
        {
            Guard.AgainstNull(enumerable, nameof(enumerable));

            var result = enumerable.FirstOrDefault();

            if (result == null)
            {
                throw new ApplicationException(string.Format(Resources.GetException, typeof(T).Name));
            }

            return result;
        }
    }
}