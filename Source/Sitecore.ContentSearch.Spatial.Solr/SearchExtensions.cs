using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Sitecore.ContentSearch.SearchTypes;

namespace Sitecore.ContentSearch.Spatial.Solr
{
    public static  class SearchExtensions
    {
        public static IQueryable<TSource> WithinRadius<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, double lat, double lon, int radius)
        {
            return WithinRadius(source, keySelector, lat, lon, System.Convert.ToDouble(radius));
        }

        public static IQueryable<TSource> WithinRadius<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, double lat, double lon, double radius)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            var exp = Expression.Call(null,
                                      ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource),
                                                                                                     typeof (TKey)),
                                      new Expression[5]
                                          {
                                              source.Expression,
                                              Expression.Quote(keySelector),
                                              Expression.Constant(lat,typeof(double)),
                                              Expression.Constant(lon,typeof(double)),
                                              Expression.Constant(radius,typeof(double))
                                          });
            return source.Provider.CreateQuery<TSource>(exp);
        }

        public static IQueryable<TSource> OrderByNearest<TSource>(this IQueryable<TSource> source) where  TSource : SearchResultItem
        {
            if (source == null)
                throw new ArgumentNullException("source");
            return source.OrderBy(i => i["score"]);
        }
        
        public static IQueryable<TSource> WithinBounds<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, double lowerLeftlat, double lowerLeftlon, double upperRightLat, double upperRightLon)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            var contextMethodInfo = (MethodInfo)MethodBase.GetCurrentMethod();

            MethodCallExpression methodCallExpression = Expression.Call(
                (Expression)null, contextMethodInfo.MakeGenericMethod(typeof(TSource), typeof(TKey)),
                source.Expression,
                (Expression)Expression.Quote(keySelector),
                (Expression)Expression.Constant(lowerLeftlat, typeof(double)),
                (Expression)Expression.Constant(lowerLeftlon, typeof(double)),
                (Expression)Expression.Constant(upperRightLat, typeof(double)),
                (Expression)Expression.Constant(upperRightLon, typeof(double)));

            return source.Provider.CreateQuery<TSource>(methodCallExpression);
        }
    }
}