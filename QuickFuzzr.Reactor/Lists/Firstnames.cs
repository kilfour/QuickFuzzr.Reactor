﻿namespace QuickFuzzr.Reactor.Lists;

public static partial class DataLists
{
    public static string[] FirstNames
    {
        get
        {
            return MaleFirstNames.Union(FemaleFirstNames).ToArray();
        }
    }
}
