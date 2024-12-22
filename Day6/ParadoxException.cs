using System.Runtime.Serialization;

namespace Day6;

public class ParadoxException : Exception
{
    public ParadoxException()
    {
    }

    protected ParadoxException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ParadoxException(string? message) : base(message)
    {
    }

    public ParadoxException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}