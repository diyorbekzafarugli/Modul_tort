namespace test;

internal class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine(MaxSumAfterPartitioning([1, 15, 7, 9, 2, 5, 10],3));
        var res = NumberOfSubstrings1("abcabc");
        Console.WriteLine(res);
    }
    public static int MaxSumAfterPartitioning(int[] arr, int k)
    {
        //[1,4,1,5,7,3,6,1,9,9,3], k = 4 , res = 83;
        var n = arr.Length;
        int[] memory = new int[n];

        int temp = 0;

        for (int i = 0; i < n; i++)
        {
            int max = 0;

            for (int j = 1; j <= k && j <= i + 1; j++)
            {
                max = Math.Max(max, arr[i - j + 1]);

                var prev = 0;
                if (i - j >= 0) prev = memory[i - j];

                temp = (max * j) + prev;

                memory[i] = Math.Max(memory[i],temp);
            }
        }
        return memory[n - 1];
    }


    public int NumberOfSubstrings1(string s)
    {
        int indexA = -1, indexB = -1, indexC = -1;

        int sum = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == 'a') indexA = i;
            else if (s[i] == 'b') indexB = i;
            else if (s[i] == 'c') indexC = i;

            int lastIndex = Math.Min(indexA, Math.Min(indexB, indexC));

            if (lastIndex != -1)
            {
                sum += (lastIndex + 1);
            }
        }

        return sum;
    }
    public int NumberOfSubstrings(string s)
    {
        var abc = "abc";
        var slow = 0;
        var fast = 3;
        var res = 0;

        while(slow < s.Length && fast < s.Length - 3)
        {
            var str = s.Substring(slow, fast).Split("");
            Array.Sort(str);
            var temp = str.ToString();

            if (str.StartsWith(abc)) res++;

            if (fast == s.Length - 3)
            {
                slow++;
                fast = 3;
            }
            else fast++;
        }
        return res;
    }
    public int NumberOfSubstrings(string s)
    {
        var strs = new List<string>();

        for (int i = 0; i < s.Length; i++)
        {
            for (int j = i + 3; j <= s.Length; j++)
            {
                var temp = s.Substring(i, j - i);

                strs.Add(temp);
            }
        }

        var res = 0;


        foreach (string item in strs)
        {
            if (item.Contains('a') && item.Contains('b') && item.Contains('c'))
            {
                res++;
            }
        }

        return res;
    }
}
