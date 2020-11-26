using System;

public class InvalidPowerUpException : Exception
{
    public InvalidPowerUpException()
    {
    }

    public InvalidPowerUpException(string message) 
        : base(message)
    {
    }

    public InvalidPowerUpException(string message, Exception inner)
        : base(message, inner)
    {
    }
}