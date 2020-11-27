using System;

public class InvalidTowerTypeException : Exception
{
    public InvalidTowerTypeException()
    {
    }

    public InvalidTowerTypeException(string message) 
        : base(message)
    {
    }

    public InvalidTowerTypeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}