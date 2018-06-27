using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace JNogueira.Bufunfa.Infraestrutura.Dados
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Ordena uma coleção a partir do nome da propriedade do seu tipo
        /// </summary>
        public static IOrderedQueryable<T> OrderByProperty<T>(this IQueryable<T> entities, string propertyName, string sortDirection)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
                return null;

            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            return sortDirection == "ASC"
                ? entities.OrderBy(e => propertyInfo.GetValue(e, null))
                : entities.OrderByDescending(e => propertyInfo.GetValue(e, null));
        }
    }
}
