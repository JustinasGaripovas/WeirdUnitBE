using System;

public class EnemyNotFoundException : Exception
{
    public EnemyNotFoundException()
    {
    }

    public EnemyNotFoundException(string message) 
        : base(message)
    {
    }

    public EnemyNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}