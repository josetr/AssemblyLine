// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine.ViewModels;

public static class ListExtensions
{
    public static void Replace<T>(this List<T> list, T obj, Func<T, int> keySelector)
    {
        var key = keySelector(obj);

        for (var i = 0; i < list.Count; ++i)
        {
            if (keySelector(list[i]) == key)
            {
                list[i] = obj;
                return;
            }
        }
    }
}
