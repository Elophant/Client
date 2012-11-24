using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotMissing
{
    public static class CloneExt
    {
        /// <summary>
        /// Generic method for Clone
        /// </summary>
        /// <typeparam name="T">ICloneable Type</typeparam>
        /// <param name="obj">ICloneable Object</param>
        /// <returns>Clone() as T</returns>
        public static T CloneT<T>(this T obj) where T : class, ICloneable 
        {
            return obj.Clone() as T;
        }

        public static IEnumerable<T> Clone<T>(this IEnumerable<T> listToClone) where T : class, ICloneable
        {
            return listToClone.Select(item => (T)item.Clone());
        }

    }
}
