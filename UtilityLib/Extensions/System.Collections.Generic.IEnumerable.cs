// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityLib {
    public static partial class IEnumerableExtensions {
        public static IEnumerable<List<TSource>> Chunk<TSource>(this IEnumerable<TSource> Source, int Size) {
            Source.ThrowIfNull("Chunk.Source");
            if (Size < 1) throw new ArgumentOutOfRangeException("Chunk.Size");
		    List<TSource> chunk = new List<TSource>();
		    foreach (TSource item in Source) {
               chunk.Add(item);
			    if (chunk.Count == Size) {
				    yield return chunk;
				    chunk = new List<TSource>();
			    }
		    }
		    if (chunk.Count > 0) yield return chunk;
        }
        public static object[,] ToArray2D<T>(this IEnumerable<T> Rows, params Func<T, object>[] ColumnLambdas) {
            var array = new object[Rows.Count(), ColumnLambdas.Count()];
            var lineCounter = 0;
            Rows.ForEach(line => { for (var i = 0; i < ColumnLambdas.Length; i++) array[lineCounter, i] = ColumnLambdas[i](line); 
                                   lineCounter++; });
            return array;
        }
        // ForEach()
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> Source, Action<TSource> ForEachAction) {
            Source.ThrowIfNull("ForEach.Source");
            ForEachAction.ThrowIfNull("ForEachAction");
            foreach (TSource element in Source) ForEachAction(element);
            return Source; 
        }
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> Source, Action<TSource, int> ForEachAction) { // 2nd arg is zero-based index
            Source.ThrowIfNull("ForEach.Source");
            ForEachAction.ThrowIfNull("ForEachAction");
            int idx = 0;
            foreach (TSource element in Source) ForEachAction(element, idx++);
            return Source; 
        }
        // ToHashSet() - one function for each of HashSet's possible constructors.
        public static HashSet<TElement> ToHashSet<TSource, TElement>(this IEnumerable<TSource> Source, Func<TSource, TElement> ElementSelector, IEqualityComparer<TElement> comparer) { // creates HashSet from IEnumerable given selector and comparer
            Source.ThrowIfNull("ToHashSet.Source");
            ElementSelector.ThrowIfNull("ToHashSet.ElementSelector");
            return new HashSet<TElement>(Source.Select(ElementSelector), comparer); 
        }
        public static HashSet<TElement> ToHashSet<TSource, TElement>(this IEnumerable<TSource> Source, Func<TSource, TElement> ElementSelector) {
            return Source.ToHashSet<TSource, TElement>(ElementSelector, null);
        }
        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> Source, IEqualityComparer<TSource> comparer) {
            return Source.ToHashSet<TSource, TSource>(item => item, comparer);
        }
        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> Source) {
            return Source.ToHashSet<TSource, TSource>(item => item, null); // use null for default comparer
        }
        // SortedSet()
        public static SortedSet<TElement> ToSortedSet<TSource, TElement>(this IEnumerable<TSource> Source, Func<TSource, TElement> ElementSelector) { 
            Source.ThrowIfNull("ToSortedSet.Source");
            ElementSelector.ThrowIfNull("ToSortedSet.ElementSelector");
            return new SortedSet<TElement>(Source.Select(ElementSelector));
        }
        public static SortedSet<TSource> ToSortedSet<TSource>(this IEnumerable<TSource> Source) {
            return Source.ToSortedSet<TSource, TSource>(item => item); // use null for default comparer
        }
        // None()
        public static bool None<TSource>(this IEnumerable<TSource> Source, Func<TSource, bool> predicate) {
            return !Source.Any(predicate);
        }
        public static bool None<TSource>(this IEnumerable<TSource> Source) {
            return !Source.Any();
        }
        // IIf()
        public static IEnumerable<TResult> IIf<TSource, TResult>(this IEnumerable<TSource> Source, Func<TSource, bool> fTest, Func<TSource, TResult> fTrue, Func<TSource, TResult> fFalse) {
            Source.ThrowIfNull("IIf.Source");
            fTest.ThrowIfNull("IIf.fTest");
            return Source.Select(x => fTest(x) ? fTrue(x) : fFalse(x));
        }
        public static IEnumerable<TResult> IIf<TSource, TResult>(this IEnumerable<TSource> Source, Func<TSource, bool> fTest, Func<TSource, TResult> fTrue) {
            Source.ThrowIfNull("IIf.Source");
            fTest.ThrowIfNull("IIf.fTest");
            foreach (TSource item in Source) {
                if (fTest(item)) yield return fTrue(item);
            }
        }
        public static IEnumerable<TResult> IIf<TSource, TResult>(this IEnumerable<TSource> Source, Func<TSource, bool> fTest, TResult ResultTrue, TResult ResultFalse) {
            Source.ThrowIfNull("IIf.Source");
            fTest.ThrowIfNull("IIf.fTest");
            return Source.Select(x => fTest(x) ? ResultTrue : ResultFalse);
        }
        public static IEnumerable<TResult> IIf<TSource, TResult>(this IEnumerable<TSource> Source, Func<TSource, bool> fTest, TResult ResultTrue) {
            Source.ThrowIfNull("IIf.Source");
            fTest.ThrowIfNull("IIf.fTest");
            foreach (TSource item in Source) {
                if (fTest(item)) yield return ResultTrue;
            }
        }
        // To Equatables
        public static EquatableList<TSource> ToEquatableList<TSource>(this IEnumerable<TSource> source) {
            return new EquatableList<TSource>(source);
        }
        public static EquatableHashSet<TSource> ToEquatableHashSet<TSource>(this IEnumerable<TSource> source) {
            return new EquatableHashSet<TSource>(source);
        }
    }
}
