using GeoChatter.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GeoChatter.Core.Extensions
{
    /// <summary>
    /// Custom extensions for EF
    /// </summary>
    public static partial class EFExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="navigationPropertyPaths"></param>
        /// <returns></returns>
        public static IQueryable<T> Include<T>(this IQueryable<T> source, IEnumerable<string> navigationPropertyPaths)
            where T : class
        {
            return navigationPropertyPaths.Aggregate(source, (query, path) => query.Include(path));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="clrEntityType"></param>
        /// <param name="maxDepth"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<string> GetIncludePaths([NotNull] this DbContext context, Type clrEntityType, int maxDepth = int.MaxValue)
        {
            if (maxDepth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxDepth));
            }

            IEntityType entityType = context.Model.FindEntityType(clrEntityType);
            HashSet<INavigation> includedNavigations = new();
            Stack<IEnumerator<INavigation>> stack = new();
            while (true)
            {
                List<INavigation> entityNavigations = new();
                if (stack.Count <= maxDepth)
                {
                    foreach (INavigation navigation in entityType.GetNavigations())
                    {
                        if (includedNavigations.Add(navigation))
                        {
                            entityNavigations.Add(navigation);
                        }
                    }
                }
                if (entityNavigations.Count == 0)
                {
                    if (stack.Count > 0)
                    {
                        yield return string.Join(".", stack.Reverse().Select(e => e.Current.Name));
                    }
                }
                else
                {
                    foreach (INavigation navigation in entityNavigations)
                    {
                        INavigation inverseNavigation = navigation.Inverse;
                        if (inverseNavigation != null)
                        {
                            includedNavigations.Add(inverseNavigation);
                        }
                    }
                    stack.Push(entityNavigations.GetEnumerator());
                }
                while (stack.Count > 0 && !stack.Peek().MoveNext())
                {
                    stack.Pop();
                }

                if (stack.Count == 0)
                {
                    break;
                }

                entityType = stack.Peek().Current.TargetEntityType;
            }
        }

    }
}