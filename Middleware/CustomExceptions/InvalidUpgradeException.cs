using System;

public class InvalidUpgradeException : Exception
{
    public InvalidUpgradeException()
    {
    }

    public InvalidUpgradeException(string message) 
        : base(message)
    {
    }

    public InvalidUpgradeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}