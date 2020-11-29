using System;

public static class ConsoleLogger
{
    public static void LogToConsole(string stringMessage)
    {
        Console.WriteLine(stringMessage);
    }
    
    public static void LogToConsole(int intMessage)
    {
        Console.WriteLine(intMessage);
    }
}