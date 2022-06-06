using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AccountManager.Utils.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static ObservableCollection<TSource> ToObservableCollection<TSource>(this IEnumerable<TSource> source)
        {
            return new ObservableCollection<TSource>(source);
        }

        public static ObservableCollection<TResult> ToObservableCollection<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return new ObservableCollection<TResult>(source.Select(selector));
        }
    }
}