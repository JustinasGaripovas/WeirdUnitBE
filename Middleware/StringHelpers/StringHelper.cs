using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class StringHelper
{
    public static IEnumerable<string> SplitStringByCamelCase(string source)
    {
        string regexPattern = String.Empty;
        regexPattern += new UpperCaseRegex().GetExpression();
        regexPattern += new LowerCaseRegex().GetExpression();
        regexPattern += new RepeatedRegex().GetExpression();
        
        var matches = Regex.Matches(source, regexPattern);
        foreach (Match match in matches)
        {
            yield return match.Value;
        }
    }
}