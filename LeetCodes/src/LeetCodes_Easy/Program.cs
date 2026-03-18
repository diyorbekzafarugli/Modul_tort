namespace LeetCodes_Easy;


internal class Program
{
    public ListNode MergeTwoLists(ListNode list1, ListNode list2)
    {
        var node = new ListNode();
        if (list1.value != 0 && list2.value == 0)
            return list1;
        else if (list1.value == 0 && list2.value != 0)
            return list2;

        while (true)
        {
            if (list1.value < list2.value)
            {
                node.value = list1.value;
                list1 = list1.next;
            }
            else
            {
                node.value = list2.value;
                list2 = list2.next;
            }
        }
    }
    public static bool IsValid1(string str)
    {
        Stack<char> chars = new((str.Length + 1) / 2);

        foreach (var ch in str)
        {
            switch (ch)
            {
                case '(': chars.Push(')'); break;
                case '[': chars.Push(']'); break;
                case '{': chars.Push('}'); break;
                default:
                    if (chars.Count == 0 || ch != chars.Pop())
                        return false;
                    break;
            }
        }

        return chars.Count == 0;
    }
    public static bool IsValid(string s)
    {
        Stack<char> chars = [];

        for (int i = 0; i < s.Length; i++)
        {
            var ch = s[i];
            if (ch == '(' || ch == '[' || ch == '{')
            {
                chars.Push(ch);
            }

            if (chars.Count > 0)
            {
                var c = chars.Peek();
                if (ch == ')' && c == '(' ||
                   ch == ']' && c == '[' ||
                   ch == '}' && c == '{')
                {
                    chars.Pop();
                }
            }
        }

        return chars.Count == 0;
    }
    public static int RomanToInt(string s)
    {
        int res = 0;

        for (int i = 0; i < s.Length; i++)
        {
            int curr = Val(s[i]);
            int next = i + 1 < s.Length ? Val(s[i + 1]) : 0;

            res += curr < next ? -curr : curr;
        }

        return res;
    }

    private static int Val(char c) => c switch
    {
        'I' => 1,
        'V' => 5,
        'X' => 10,
        'L' => 50,
        'C' => 100,
        'D' => 500,
        'M' => 1000,
        _ => 0
    };
    public static string LongestCommonPrefix(string[] strs)
    {
        if (strs.Length == 0) return string.Empty;
        var prefix = strs[0];

        for (int i = 1; i < strs.Length; i++)
        {
            while (!strs[i].StartsWith(prefix))
            {
                prefix = prefix.Substring(0, prefix.Length - 1);
                if (prefix == string.Empty) return string.Empty;
            }
        }

        return prefix;
    }
    static int RomanToInt1(string str)
    {
        Dictionary<char, int> keyValues = new Dictionary<char, int>()
        {
            ['I'] = 1,
            ['V'] = 5,
            ['X'] = 10,
            ['L'] = 50,
            ['C'] = 100,
            ['D'] = 500,
            ['M'] = 1000
        };

        int res = 0;
        for (int i = 0; i < str.Length; i++)
        {
            var currency = keyValues[str[i]];
            var next = (i + 1 < str.Length) ? keyValues[str[i + 1]] : 0;

            if (currency < next)
                res -= currency;
            else
                res += currency;
        }

        return res;
    }
    static bool IsPalindrome(int x)
    {
        if (x < 0)
            return false;

        int origin = x;
        int res = 0;
        while (x > 0)
        {
            res = res * 10 + x % 10;
            x /= 10;
        }
        return res == origin;
    }
    static void Main(string[] args)
    {
        
        //string[] strs = ["flower", "flow", "flight"];
        //string str = LongestCommonPrefix(strs);
        //Console.WriteLine(str);
        //Console.WriteLine(IsPalindrome(-221));
        //Console.WriteLine("Hello, World!");

        //int[] nums = { 45, 5, 23, 232, 32, 1 };
        //int[] res = TwoSum(nums, 6);
    }

    static int[] TwoSum(int[] nums, int target)
    {
        for (int i = 0; i < nums.Length; i++)
        {
            for (int j = i + 1; j < nums.Length; j++)
            {
                if (nums[i] + nums[j] == target)
                    return new int[] { i, j };
            }
        }

        return Array.Empty<int>();
    }

    static int[] TwoSum1(int[] nums, int target)
    {
        Dictionary<int, int> valuePairs = [];

        for (int i = 0; i < nums.Length; i++)
        {
            var complement = target - nums[i];
            if (valuePairs.TryGetValue(complement, out int j))
                return new int[] { i, j };

            valuePairs[nums[i]] = i;
        }

        return Array.Empty<int>();
    }
}
