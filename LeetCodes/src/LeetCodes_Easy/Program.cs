using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace LeetCodes_Easy;


internal class Program
{
    /**Input:     nums = [0,1,2,2,3,0,4,2], val = 2;
     * Output: 5, nums = [0,1,4,0,3,_,_,_]
     * Input: haystack = "sadbutsad", needle = "sad"
     * Output: 0
     
     */
    public int ClimbStairs(int n)
    {
        if (n == 0) return 0;
        return n + ClimbStairs(n - 1);
    }
    public int MySqrt(int x)
    {
        if (x < 2) return x;

        int left = 1;
        int rigth = x / 2;

        while(left <= rigth)
        {
            long mid = (left + rigth) / 2;
            if (mid * mid == x) return (int)mid;
            if (mid * mid < x) left = (int)mid + 1;
            if (mid * mid > x) rigth = (int)mid - 1;
        }
        return rigth;
    }
    public string AddBinary(string a, string b)
    {
        int i = a.Length - 1;
        int j = b.Length - 1;
        int carry = 0;
        var result = new StringBuilder();

        while (i >= 0 || j >= 0 || carry > 0)
        {
            int sum = carry;
            if (i >= 0) sum += a[i--] - '0';
            if (j >= 0) sum += b[j--] - '0';

            carry = sum / 2;
            result.Insert(0, sum % 2);
        }

        return result.ToString();
    }
    public int[] PlusOne(int[] digits)
    {
        for (int i = digits.Length - 1; i >= 0; i--)
        {
            if (digits[i] != 9)
            {
                digits[i]++;
                return digits;
            }

            digits[i] = 0;
        }

        var result = new int[digits.Length + 1];
        result[0] = 1;
        return result;
    }

    public int LengthOfLastWord(string str)
    {
        int i = str.Length - 1;
        int counter = 0;

        while (i >= 0 && str[i] == ' ') --i;

        while (i >= 0 && str[i] != ' ')
        {
            ++counter;
            --i;
        }
        return counter;
    }
    public int LengthOfLastWord1(string s)
    {
        if (s.Length == 0) return 0;

        s = s.Trim();
        return s.Substring(s.LastIndexOf(" ")).Length;
    }

    public int SearchInsert(int[] nums, int target)
    {
        /*
         * Input: nums = [1,3,5,6], target = 7
         * Output: 4
         */
        int left = 0, right = nums.Length - 1;
        int mid = (left + right) / 2;

        while (left <= right)
        {
            mid = (left + right) / 2;

            if (nums[mid] == target)
                return mid;

            if (nums[mid] < target)
                left = mid + 1;

            if (nums[mid] > target)
                right = mid - 1;
        }

        return left;
    }
    public int StrStr1(string haystack, string needle)
    {
        if (haystack.StartsWith(needle)) return 0;

        for (int i = 0; i < haystack.Length - needle.Length; i++)
        {
            int j = 0;

            while (j < needle.Length && haystack[i + j] == needle[j])
            {
                j++;
            }

            if (j == needle.Length)
            {
                return i;
            }
        }

        return -1;
    }
    public int StrStr(string haystack, string needle)
    {
        if (needle.Length == 0) return 0;
        for (int i = 0; i <= haystack.Length - needle.Length; i++)
        {
            if (haystack.Substring(i, needle.Length) == needle)
            {
                return i;
            }
        }

        return -1;
    }
    public static int RemoveElement(int[] nums, int val)
    {
        if (nums.Length == 0) return 0;

        var i = 0;
        var n = nums.Length;

        while (i < n)
        {
            if (nums[i] == val)
            {
                nums[i] = nums[n - 1];
                --n;
            }
            else
            {
                ++i;
            }
        }

        return n;
    }


    public static int RemoveDuplicates(int[] nums)
    {
        if (nums.Length == 0) return 0;

        var i = 0;
        for (int j = 1; j < nums.Length; j++)
        {
            if (nums[i] != nums[j])
            {
                nums[++i] = nums[j];
            }
        }
        return i + 1;
    }
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
        int[] nums = [1, 1, 2, 2, 2, 3, 3, 4, 4, 5];
        int k = RemoveDuplicates(nums);
        Console.WriteLine(k);
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
