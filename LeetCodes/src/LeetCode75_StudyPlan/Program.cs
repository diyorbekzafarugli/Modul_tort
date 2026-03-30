using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace LeetCode75_StudyPlan;

internal class Program
{
     static void Main(string[] args)
    {
        //var str1 = "ABABAB";
        //var str2 = "AB";
        //var res = GcdOfStrings(str1, str2);
        //Console.WriteLine(res);

        var res = ProductExceptSelf1([1, 2, 3, 4]);

        foreach (var item in res)
        {
            Console.WriteLine(item);
        }
    }
    public string MergeAlternately(string word1, string word2)
    {
        int i = 0;
        var resultSb = new StringBuilder();

        while (i < word1.Length || i < word2.Length)
        {
            if (i < word1.Length) resultSb.Append(word1[i]);
            if (i < word2.Length) resultSb.Append(word2[i]);
            i++;
        }
        return resultSb.ToString();
    }
    public static string GcdOfStrings(string str1, string str2)
    {
        if (str1 + str2 != str2 + str1) return "";

        int gcd = GcdHelper(str1.Length, str2.Length);
        return str1.Substring(0, gcd);
    }
    private static int GcdHelper(int a, int b)
    {
        return b == 0 ? a : GcdHelper(b, a % b);
    }

    public IList<bool> KidsWithCandies(int[] candies, int extraCandies)
    {
        var res = new List<bool>(candies.Length);
        int max = candies.Max() - extraCandies;

        for (int i = 0; i < candies.Length; i++)
            res.Add(candies[i] >= max);

        return res;
    }
    public bool CanPlaceFlowers(int[] flowerbed, int n)
    {
        for (int i = 0; i < flowerbed.Length; i++)
        {
            if (flowerbed[i] == 0)
            {
                var prev = i == 0 || flowerbed[i - 1] == 0;
                var next = i == flowerbed.Length - 1 || flowerbed[i + 1] == 0;

                if (prev && next)
                {
                    flowerbed[i] = 1;
                    n--;
                    if (n == 0) return true;
                }
            }
        }
        return n <= 0;
    }
    public string ReverseVowels(string s)
    {
        char[] res = [.. s];
        int left = 0;
        int right = s.Length - 1;

        while (left < right)
        {
            var leftIsVowel = res[left].IsVowel();
            var rightIsVowel = res[right].IsVowel();

            if (leftIsVowel && rightIsVowel)
            {
                (res[left], res[right]) = (res[right], res[left]);
                left++;
                right--;
            }
            else if (leftIsVowel) right--;
            else left++;
        }

        return new string(res);
    }
    public string ReverseWords1(string s)
    {
        var result = new List<string>();
        var slow = 0;

        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == ' ')
            {
                if (slow != i)
                {
                    result.Add(s.Substring(slow, i - slow));
                }
                slow = i + 1;
            }
        }

        if (slow < s.Length)
        {
            result.Add(s.Substring(slow, s.Length - slow));
        }

        result.Reverse();

        return string.Join(" ", result);
    }
    public string ReverseWords(string s)
    {
        string[] words = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        Array.Reverse(words);

        return string.Join(" ", words);
    }
    public static int[] ProductExceptSelf1(int[] nums)
    {
        var prefix = new int[nums.Length];
        var sufix = new int[nums.Length];
        var res = new int[nums.Length];
        var pre = 1;
        var suf = 1;

        prefix[0] = 1;
        for (int i = 1; i < nums.Length; i++)
        {
            prefix[i] = prefix[i - 1] * nums[i - 1];
        }

        sufix[nums.Length - 1] = 1;
        for (int i = nums.Length - 2; i >= 0; i--)
        {
            sufix[i] = sufix[i + 1] * nums[i + 1];
        }

        for (int i = 0; i < res.Length; i++)
        {
            res[i] = prefix[i] * sufix[i];
        }
        return res;
    }
    public int[] ProductExceptSelf(int[] nums)
    {
        int n = nums.Length;
        var res = new int[n];

        int pre = 1;
        for (int i = 0; i < n; i++)
        {
            res[i] = pre;
            pre *= nums[i];
        }

        int suf = 1;
        for (int i = n - 1; i >= 0; i--)
        {
            res[i] *= suf;
            suf *= nums[i];
        }

        return res;
    }
    public bool IncreasingTriplet(int[] nums)
    {
        int s = int.MaxValue;
        int m = int.MaxValue;

        foreach (var num in nums)
        {
            if (num <= s) s = num;
            else if (num <= m) m = num;
            else return true;
        }
        return false;
    }
    public int Compress(char[] chars)
    {
        int write = 0;
        int read = 0;

        while (read < chars.Length)
        {
            char currentChar = chars[read];
            int count = 0;

            while (read < chars.Length && chars[read] == currentChar)
            {
                read++;
                count++;
            }

            chars[write] = currentChar;
            write++;

            if (count > 1)
            {
                foreach (char c in count.ToString())
                {
                    chars[write] = c;
                    write++;
                }
            }
        }

        return write;
    }
    public void MoveZeroes(int[] nums)
    {
        int fast = 0;
        int slow = 0;

        while(fast < nums.Length)
        {
            if (nums[fast] != 0)
            {
                (nums[fast], nums[slow]) = (nums[slow], nums[fast]);
                fast++;
                slow++;
            }
            else fast++;
        }
    }
    public bool IsSubsequence(string s, string t)
    {
        int pS = 0;
        int pT = 0;
        
        while(pS < s.Length && pT < t.Length)
        {
            if (s[pS] == t[pT]) pS++;

            pT++;
        }
        return pS == s.Length;
    }

    public int MaxArea(int[] height)
    {
        int l = 0;
        int r = height.Length - 1;
        int res = 0;

        while(l < r)
        {
            int minHeight = height[l] < height[r] ? height[l] : height[r];
            int temp = (r - l) * minHeight;

            if (temp > res) res = temp;

            if (height[l] > height[r]) r--;
            else l++;
        }
        return res;
    }
    public int MaxOperations1(int[] nums, int k)
    {
        Array.Sort(nums);
        int l = 0;
        int r = nums.Length - 1;
        int count = 0;

        while(l < r)
        {
            if (nums[l] == k - nums[r])
            {
                count++;
                l++;
                r--;
            }
            else if (nums[l] < k - nums[r]) l++;
            else r--;
        }

        return count;
    }
    public int MaxOperations(int[] nums, int k)
    {
        var map = new Dictionary<int, int>();
        int count = 0;

        foreach (int num in nums)
        {
            int target = k - num;

            if (map.ContainsKey(target) && map[target] > 0)
            {
                count++;
                map[target]--;
            }
            else
            {
                if (!map.ContainsKey(num)) map[num] = 0;
                map[num]++;
            }
        }

        return count;
    }
    public double FindMaxAverage(int[] nums, int k)
    {

    }
}
