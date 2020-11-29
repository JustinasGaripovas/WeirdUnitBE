using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class StringHelper
{
    public static IEnumerable<string> SplitStringByCamelCase(string source)
    {
        string pattern = UpperCaseChar() + LowerCaseChar()+Repeated();
        
        var matches = Regex.Matches(source, pattern);
        foreach (Match match in matches)
        {
            yield return match.Value;
        }
    }

    private static string UpperCaseChar()
    {
        return @"[A-Z]";
    }

    private static string LowerCaseChar()
    {
        return @"[a-z]";
    }

    private static string Repeated()
    {
        return @"*";
    }
}