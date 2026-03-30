namespace LeetCode75_StudyPlan;

public static class HelperExtensions
{
    public static bool IsVowel(this char ch)
    {
        return ch is 'A' or 'E' or 'I' or 'O' or 'U' or
                     'a' or 'e' or 'i' or 'o' or 'u';
    }
}