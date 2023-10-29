using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public static class CollectionExtensions
{
    public static void Each<T>(this IEnumerable<T> p_collection, Action<T> p_func)
    {
        foreach (var p_var in p_collection)
        {
            p_func.Invoke(p_var);
        }
    }

    public static void Each<T>(this IEnumerable<T> p_collection, Action<T, int> p_func)
    {
        for (int i = 0; i < p_collection.Count(); i++)
        {
            p_func.Invoke(p_collection.ElementAt(i), i);
        }
    }

}