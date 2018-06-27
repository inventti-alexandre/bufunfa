using System;
using System.Linq;
using System.Linq.Expressions;

namespace JNogueira.Bufunfa.Infraestrutura.Dados
{
    public static class ExtendedMethods
    {
        /// <summary>
        /// Ordena não importando o tipo da propriedade utilizada para ordenação
        /// </summary>
        public static IOrderedQueryable<TSource> OrderByExtended<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, object>> keySelector, string sortDirection)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            var body = keySelector.Body;

            if (body.NodeType == ExpressionType.Convert)
            {
                body = ((UnaryExpression)body).Operand;
            }

            var keySelector2 = Expression.Lambda(body, keySelector.Parameters);
            var tkey = keySelector2.ReturnType;

            var orderbyMethod = (from x in typeof(Queryable).GetMethods()
                                 where x.Name == (sortDirection.ToUpper() == "ASC" ? "OrderBy" : "OrderByDescending")
                                 let parameters = x.GetParameters()
                                 where parameters.Length == 2
                                 let generics = x.GetGenericArguments()
                                 where generics.Length == 2
                                 where parameters[0].ParameterType == typeof(IQueryable<>).MakeGenericType(generics[0]) &&
                                       parameters[1].ParameterType == typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(generics[0], generics[1]))
                                 select x).Single();

            return
                (IOrderedQueryable<TSource>)
                source.Provider.CreateQuery<TSource>(Expression.Call(null,
                    orderbyMethod.MakeGenericMethod(typeof(TSource), tkey), new[]
                    {
                        source.Expression,
                        Expression.Quote(keySelector2)
                    }));
        }

    }
}
