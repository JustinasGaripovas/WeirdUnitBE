using System;

public class InvalidMovementException : Exception
{
    public InvalidMovementException()
    {
    }

    public InvalidMovementException(string message) 
        : base(message)
    {
    }

    public InvalidMovementException(string message, Exception inner)
        : base(message, inner)
    {
    }

}