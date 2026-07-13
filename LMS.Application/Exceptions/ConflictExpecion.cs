namespace LMS.Application.Exceptions;
public sealed class ConflictExpecion:AppException
{
    public ConflictExpecion(string message):base(message,409)
    {
    }
}