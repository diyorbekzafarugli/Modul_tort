namespace test;

internal class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine(MaxSumAfterPartitioning([1, 15, 7, 9, 2, 5, 10],3));
        //var res = NumberOfSubstrings1("abcabc");
        //Console.WriteLine(res);
        string str = "abcaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        var res = NumberOfSubstrings(str);
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

                memory[i] = Math.Max(memory[i], temp);
            }
        }
        return memory[n - 1];
    }


    public int NumberOfSubstrings1(string s)
    {
        // str = "abcaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        int indexA = 0, indexB = 0, indexC = 0;

        int sum = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == 'a') indexA = i;
            else if (s[i] == 'b') indexB = i;
            else if (s[i] == 'c') indexC = i;

            int lastIndex = Math.Min(indexA, Math.Min(indexB, indexC));

            if (lastIndex != 0)
            {
                sum += (lastIndex + 1);
            }
        }

        return sum;
    }
    public static int NumberOfSubstrings(string s)
    {
        // str = "abcaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        int res = 0;
        int n = s.Length;
        int left = 0;
        int[] abc = new int[3];

        for (int i = 0; i < n; i++) 
        {
            abc[s[i] - 'a']++;//har charni olib sanadim

            while (abc[0] > 0 && abc[1] > 0 && abc[2] > 0)
            {
                abc[s[left] - 'a']--;//shu yerda left qaysi index da tursa o'shani o'sha char miqdori kamaytirdim
                left++;    //chap pointerni bitta surib keyingi index dagi charga o'tdim
            }

            res += left;// leftda gi pointer qaysi indexga kelgan bo'lsa o'shancha bor 
        }               //abcaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa shuni oladigan bo'lsak boshidan sanasak ham
                         // left kelib qolgan joydan bita qo'shi sanasak ham bitta dan chiqadi ikkalasi
                         // leftni miqdori
        return res;
    }
    public int NumberOfSubstrings2(string s)
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

    public static int NumberOfSubstrings12(string s)
    {
        /*
         * abcddddddddddddddddddddddddddddddddddd
         * dddddddddddddddddddddddddddddddddddddddddddd
         * ddddddddddddddddddddddddddddddddddddddddddddd
         */
        var res = 0;
        for (var i = 0; i < s.Length - 3; i++)
        {
            for (var j = i + 2; j <= s.Length; j++)
            {
                // i bilan j oralig'idagi charlarni ko'ramiz. Copy qilmasdan.
                var hasA = false;
                var hasB = false;
                var hasC = false;
                // Endi aynan shu oraliqda stringni charlarini ko'rib chiqamiz.

                for (var k = i; k <= j; k++)
                {
                    if (s[k] == 'a') // Agar char 'a' bo'lsa 
                        hasA = true;  // 'a' topildi deymiz
                    else if (s[k] == 'b') // agar char 'b' bo'lsa
                        hasB = true; // 'b' topildi deymiz
                    else if (s[k] == 'c') // agar char 'c' bo'lsa
                        hasC = true; // 'c' topildi deymiz

                    if (hasA && hasB && hasC)
                    {
                        // Demak biz substringni mosligiga ishonch hosil qildik.
                        res++; // Counterni oshiramiz.
                               // .. va bironta copy qilmasdan davom etamiz
                        break;
                    }
                }
            }
        }


        return res;
    }

}
