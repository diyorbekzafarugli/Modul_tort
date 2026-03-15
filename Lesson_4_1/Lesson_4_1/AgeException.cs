namespace Lesson_4_1;

public class AgeException : Exception
{
    public AgeException(string massage) : base(massage)
    {
        
    }
    public AgeException()
    {
        
    }

    public AgeException(Exception? exception) : base()
    {
        
    }



}
