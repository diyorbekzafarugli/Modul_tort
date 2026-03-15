namespace Lesson_4_1;

public class InvalidEmailException : Exception
{
    public InvalidEmailException(string massage) : base(massage) { }

    public InvalidEmailException() { }

}
