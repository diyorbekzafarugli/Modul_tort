using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace LeetCode75_StudyPlan;

internal class Program
{
    static void Main(string[] args)
    {
        //var res = DecodeString("2[abc]3[cd]ef");
        //Console.WriteLine(res);
        int[] nums = [1, 4, 6, 5, 7, 3];

        ListNode head = new ListNode(nums[0]);
        ListNode curr = head;

        for (int i = 1; i < nums.Length; i++)
        {
            curr.next = new ListNode(nums[i]);
            curr = curr.next;
        }
        var res = PairSum(head);
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

        while (fast < nums.Length)
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

        while (pS < s.Length && pT < t.Length)
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

        while (l < r)
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

        while (l < r)
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
    public static double FindMaxAverage(int[] nums, int k)
    {
        double maxSumm = double.MinValue;
        int currentSumm = 0;
        int i = 0;
        int j = 0;

        while (j < nums.Length)
        {
            currentSumm += nums[j];

            if (j - i + 1 == k)
            {
                maxSumm = Math.Max(maxSumm, currentSumm);
                currentSumm -= nums[i];
                i++;
            }
            j++;
        }

        return maxSumm / k;
    }
    public static double FindMaxAverage1(int[] nums, int k)
    {
        int currentSumm = 0;

        for (int i = 0; i < k; i++)
        {
            currentSumm += nums[i];
        }

        int maxSumm = currentSumm;

        for (int i = k; i < nums.Length; i++)
        {
            currentSumm = currentSumm + nums[i] - nums[i - k];
            maxSumm = Math.Max(maxSumm, currentSumm);
        }
        return (double)maxSumm / k;
    }
    public static int MaxVowels(string s, int k)
    {
        //s = "abciiidef", k = 3
        int fast = 0;
        int slow = 0;
        int count = 0;
        int res = 0;
        while (fast < s.Length)
        {
            if (s[fast].IsVowel()) count++;
            if (fast - slow + 1 == k)
            {
                res = Math.Max(res, count);
                if (s[slow].IsVowel()) count--;
                slow++;
            }
            fast++;
        }
        return res;
    }
    public int LongestOnes(int[] nums, int k)
    {
        //nums = [0,0,1,1,0,0,1,1,1,0,1,1,0,0,0,1,1,1,1], k = 3

        int slow = 0;
        int fast = 0;
        int res = 0;

        while (fast < nums.Length)
        {
            if (nums[fast] == 0) k--;
            if (k < 0 && nums[slow++] == 0) k++;
            res = Math.Max(res, fast - slow + 1);
            fast++;
        }
        return res;
    }
    public int LongestSubarray(int[] nums)
    {
        //nums = [0,1,1,1,0,1,1,0,1]

        int slow = 0;
        int fast = 0;
        int res = 0;
        int countZero = 0;

        while (fast < nums.Length)
        {
            if (nums[fast] == 0) countZero++;
            while (countZero > 1)
                if (nums[slow++] == 0) countZero--;

            res = Math.Max(res, fast - slow);
            fast++;
        }
        return res;
    }
    public int LargestAltitude(int[] gain)
    {
        int summ = 0;
        int res = 0;

        foreach (var num in gain)
        {
            summ += num;
            res = Math.Max(res, summ);
        }
        return res;
    }
    public int PivotIndex(int[] nums)
    {
        //nums = [2,1,-1], [1,7,3,6,5,6]
        int summ = 0;
        foreach (var num in nums)
        {
            summ += num;
        }


        int res = 0;
        int med = 0;
        while (med < nums.Length)
        {
            if (res == summ - res - nums[med]) return med;
            res += nums[med++];
        }
        return -1;
    }
    public IList<IList<int>> FindDifference(int[] nums1, int[] nums2)
    {
        var set1 = new HashSet<int>(nums1);
        var set2 = new HashSet<int>(nums2);

        var res1 = set1.Except(set2).ToList();
        var res2 = set2.Except(set1).ToList();
        return [res1, res2];
    }
    public bool UniqueOccurrences(int[] arr)
    {
        Dictionary<int, int> valuePairs = [];

        foreach (var num in arr)
        {
            valuePairs.TryGetValue(num, out int count);
            valuePairs[num] = count + 1;
        }

        return valuePairs.Count == valuePairs.Values.ToHashSet().Count;
    }
    public bool CloseStrings1(string word1, string word2)
    {
        if (word1.Length != word2.Length) return false;
        Dictionary<char, int> pairs1 = [];

        foreach (var ch in word1)
        {
            pairs1.TryGetValue(ch, out int val);
            pairs1[ch] = val + 1;
        }

        Dictionary<char, int> pairs2 = [];

        foreach (var ch in word2)
        {
            pairs2.TryGetValue(ch, out int val);
            pairs2[ch] = val + 1;
        }

        HashSet<char> res1 = [.. word1];
        HashSet<char> res2 = [.. word2];
        if (!res1.SetEquals(res2)) return false;

        var freqs1 = pairs1.Values.OrderBy(x => x);
        var freqs2 = pairs2.Values.OrderBy(x => x);

        return freqs1.SequenceEqual(freqs2);
    }
    public bool CloseStrings(string word1, string word2)
    {
        if (word1.Length != word2.Length) return false;

        int[] w1 = new int[26];
        int[] w2 = new int[26];

        foreach (var ch in word1) w1[ch - 'a']++;

        foreach (var ch in word2) w2[ch - 'a']++;

        for (int i = 0; i < 26; i++)
            if ((w1[i] == 0 && w2[i] != 0) ||
                (w1[i] != 0 && w2[i] == 0))
                return false;

        Array.Sort(w1);
        Array.Sort(w2);
        return w1.SequenceEqual(w2);
    }
    public int EqualPairs1(int[][] grid)
    {
        /*
         * grid = [[3,1,2,2],
         *         [1,4,4,5],
         *         [2,4,2,2],
         *         [2,4,2,2]]
         */

        int res = 0;
        int n = grid.Length;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                int count = 0;
                for (int k = 0; k < n; k++)
                {
                    if (grid[i][k] == grid[k][j]) count++;
                    else break;
                }
                if (count == n) res++;
            }
        }

        return res;
    }
    public int EqualPairs(int[][] grid)
    {
        /*
         * grid = [[3,1,2,2],
         *         [1,4,4,5],
         *         [2,4,2,2],
         *         [2,4,2,2]]
         */

        int res = 0;
        int n = grid.Length;
        Dictionary<string, int> pairs = [];

        for (int i = 0; i < n; i++)
        {
            var rowStr = string.Join(",", grid[i]);
            if (!pairs.ContainsKey(rowStr)) pairs[rowStr] = 0;
            pairs[rowStr]++;
        }

        for (int i = 0; i < n; i++)
        {
            int[] col = new int[n];
            for (int j = 0; j < n; j++)
            {
                col[j] = grid[j][i];
            }

            var colStr = string.Join(",", col);
            if (pairs.TryGetValue(colStr, out int val))
            {
                res += val;
            }
        }
        return res;
    }
    public static string RemoveStars(string s)
    {
        Stack<char> stac = [];
        foreach (var ch in s)
        {
            if (ch != '*') stac.Push(ch);
            else stac.Pop();
        }
        var resChars = new char[stac.Count];
        for (int i = stac.Count - 1; i >= 0; i--)
        {
            resChars[i] = stac.Pop();
        }
        return new string(resChars);
    }
    public static int[] AsteroidCollision(int[] asteroids)
    {
        Stack<int> stac = [];

        foreach (var num in asteroids)
        {
            if (num > 0)
            {
                stac.Push(num);
            }
            else
            {
                bool flag = true;
                while (stac.Count != 0 && stac.Peek() > 0)
                {
                    if (stac.Peek() < -1 * num) stac.Pop();
                    else if (stac.Peek() == -1 * num)
                    {
                        flag = false;
                        stac.Pop();
                        break;
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag) stac.Push(num);
            }
        }

        var res = new int[stac.Count];
        for (int i = res.Length - 1; i >= 0; i--)
        {
            res[i] = stac.Pop();
        }
        return res;
    }
    public static string DecodeString(string s)
    {
        Stack<int> countStr = [];
        Stack<StringBuilder> repStr = [];

        var res = new StringBuilder();
        int k = 0;

        foreach (var c in s)
        {
            if (char.IsDigit(c))
            {
                k = k * 10 + (c - '0');
            }
            else if (c == '[')
            {
                countStr.Push(k);
                repStr.Push(res);

                res = new StringBuilder();
                k = 0;
            }
            else if (c == ']')
            {
                var curr = repStr.Pop();
                int count = countStr.Pop();

                for (int i = 0; i < count; i++)
                {
                    curr.Append(res);
                }

                res = curr;
            }
            else
            {
                res.Append(c);
            }
        }
        return res.ToString();
    }
    private Queue<int> queue = [];
    public int Ping(int t)
    {
        queue.Enqueue(t);

        while (queue.Count != 0 && queue.Peek() < t - 3000)
        {
            queue.Dequeue();
        }
        return queue.Count;
    }
    public string PredictPartyVictory(string senate)
    {
        int size = senate.Length;
        Queue<int> indecesRadiant = [];
        Queue<int> indecesDire = [];

        for (int i = 0; i < size; i++)
        {
            if (senate[i] == 'R') indecesRadiant.Enqueue(i);
            else indecesDire.Enqueue(i);
        }

        while (indecesRadiant.Count != 0 && indecesDire.Count != 0)
        {
            int radiantIndex = indecesRadiant.Dequeue();
            int direIndex = indecesDire.Dequeue();

            if (radiantIndex < direIndex)
                indecesRadiant.Enqueue(radiantIndex + size);
            else
                indecesDire.Enqueue(direIndex + size);
        }

        return indecesRadiant.Count == 0 ? "Dire" : "Radiant";
    }
    public ListNode DeleteMiddle(ListNode head)
    {
        if (head == null || head.next == null) return null;

        var fast = head.next.next;
        var slow = head;

        while (fast != null && fast.next != null)
        {
            fast = fast.next.next;
            slow = slow.next;
        }
        slow.next = slow.next.next;
        return head;
    }
    public ListNode OddEvenList(ListNode head)
    {
        if (head == null || head.next == null) return head;

        var even = head.next;
        var odd = head;
        var evenFirst = even;

        while (even != null && even.next != null)
        {
            odd.next = even.next;
            odd = odd.next;

            even.next = odd.next;
            even = even.next;
        }
        odd.next = evenFirst;
        return head;
    }
    public ListNode ReverseList(ListNode head)
    {
        ListNode prev = null;
        ListNode curr = head;

        while (curr != null)
        {
            ListNode temp = curr.next;

            curr.next = prev;
            prev = curr;

            curr = temp;
        }

        return prev;
    }
    public static int PairSum(ListNode head)
    {
        var curr = head;
        var counter = 0;

        while (curr != null)
        {
            counter++;
            curr = curr.next;
        }
        int n = counter / 2;
        int[] summs = new int[n];
        var temp = head;
        int i = 0;

        while (temp != null)
        {
            if (n > 0)
            {
                summs[i++] = temp.val;
                n--;

            }
            else
            {
                summs[--i] += temp.val;
            }
            temp = temp.next;
        }
        return summs.Max();
    }
    public int PairSum1(ListNode head)
    {
        ListNode fast = head;
        ListNode slow = head;

        while(fast != null)
        {
            fast = fast.next.next;
            slow = slow.next;
        }

        slow = ReverseList(slow);
        fast = head;
        var res = 0;
        while(slow != null)
        {
            res = Math.Max(res, (slow.val + fast.val));
            slow = slow.next;
            fast = fast.next;
        }
        return res;
    }
    public int MaxDepth(TreeNode root)
    {
        if (root == null) return 0;
        var left = MaxDepth(root.left);
        var right = MaxDepth(root.right);

        return 1 + Math.Max(left, right);
    }
}