using System.Text;

namespace LeetCodes_Easy;


internal class Program
{
    /**Input:     nums = [0,1,2,2,3,0,4,2], val = 2;
     * Output: 5, nums = [0,1,4,0,3,_,_,_]
     * Input: haystack = "sadbutsad", needle = "sad"
     * Output: 0
     *Input: numRows = 5
      Output: [    [1],
                  [1,1],
                 [1,2,1],
                [1,3,3,1],
               [1,4,6,4,1]
    ]
     */
    public bool HasPathSum(TreeNode root, int targetSum)
    {
        if (root == null) return false;

        targetSum = targetSum - root.val;

        if (root.left == null && root.right == null && targetSum == 0)
            return true;

        return HasPathSum(root.left, targetSum) || HasPathSum(root.right, targetSum);
    }
    public IList<int> GetRow(int rowIndex)
    {
        int[] nums = new int[rowIndex + 1];

        nums[0] = 1;

        for (int i = 1; i <= rowIndex; i++)
        {
            long res = (long)nums[i - 1] * (rowIndex - i + 1);
            nums[i] = (int)(res / i);
        }
        return nums;
    }
    public int MinDepth(TreeNode root)
    {
        if (root == null) return 0;

        if (root.left == null && root.right == null) return 1;

        if (root.left == null)
            return 1 + MinDepth(root.right);

        if (root.right == null)
            return 1 + MinDepth(root.left);

        return 1 + Math.Min(MinDepth(root.left), MinDepth(root.right));
    }

    public IList<IList<int>> Generate(int numRows)
    {
        IList<IList<int>> ints = new List<IList<int>>(numRows);

        for (int i = 0; i < numRows; i++)
        {
            List<int> row = new() { 1 };
            if (i >= 1)
            {
                for (int j = 0; j < i - 1; j++)
                {
                    row.Add(ints[i - 1][j] + ints[i - 1][j + 1]);
                }

                row.Add(1);
            }
            ints.Add(row);
        }
        return ints;
    }
    public TreeNode SortedArrayToBST(int[] nums)
    {
        return BuildNode(nums, 0, nums.Length - 1);
    }
    private TreeNode BuildNode(int[] ints, int left, int right)
    {
        if (left > right) return null;

        int mid = left + (right - left) / 2;

        TreeNode node = new TreeNode();
        node.val = ints[mid];

        node.left = BuildNode(ints, left, mid - 1);
        node.right = BuildNode(ints, mid + 1, right);
        return node;
    }
    public bool IsBalanced(TreeNode root)
    {
        return HightTree(root) != -1;
    }
    private int HightTree(TreeNode node)
    {
        if (node == null) return 0;

        int leftSubTree = HightTree(node.left);
        if (leftSubTree == -1) return -1;

        int rigthSubTree = HightTree(node.right);
        if (rigthSubTree == -1) return -1;

        return Math.Abs(leftSubTree - rigthSubTree) > 1 ? -1 : 1 + Math.Max(leftSubTree, rigthSubTree);
    }

    public int MaxDepth(TreeNode root)
    {
        if (root == null) return 0;
        return 1 + Math.Max(MaxDepth(root.left), MaxDepth(root.right));
    }
    public bool IsSymmetric(TreeNode root) => IsMirror(root.left, root.right);
    private bool IsMirror(TreeNode node1, TreeNode node2)
    {
        if (node1 == null && node2 == null) return true;
        if (node1 == null || node2 == null) return false;
        return node1.val == node2.val
               && IsMirror(node1.left, node2.right)
               && IsMirror(node1.right, node2.left);
    }
    public bool IsSameTree(TreeNode p, TreeNode q)
    {
        if (p == null && q == null) return true;
        if (p == null || q == null) return false;
        return p.val == q.val && IsSameTree(p.left, q.left) && IsSameTree(p.right, q.right);
    }
    public IList<int> InorderTraversal(TreeNode root)
    {
        Stack<TreeNode> nodes = [];
        IList<int> ints = [];
        var current = root;

        while (current != null || nodes.Count > 0)
        {
            if (current != null)
            {
                nodes.Push(current);
                current = current.left;
            }
            else
            {
                current = nodes.Pop();
                ints.Add(current.val);
                current = current.right;
            }

        }

        return ints;
    }

    public void Merge(int[] nums1, int m, int[] nums2, int n)
    {
        //Input: nums1 = [1,2,3,0,0,0], m = 3, nums2 = [2,5,6], n = 3
        int i = m - 1;
        int j = n - 1;
        int k = nums1.Length - 1;

        while (i >= 0 && j >= 0)
        {
            if (nums1[i] > nums2[j])
            {
                nums1[k] = nums1[i];
                i--;
                k--;
            }
            else
            {
                nums1[k] = nums2[j];
                j--;
                k--;
            }
        }

        while (j >= 0)
        {
            nums1[k] = nums2[j];
            j--;
            k--;
        }
    }
    public ListNode DeleteDuplicates(ListNode head)
    {
        var current = head;
        while (current != null && current.next != null)
        {
            if (current.value == current.next.value)
            {
                current.next = current.next.next;
            }
            else
            {
                current = current.next;
            }
        }
        return head;
    }
    public int ClimbStairs(int n)
    {
        if (n <= 2) return n;
        int a = 1, b = 2;

        for (int i = 3; i <= n; i++)
        {
            var temp = a + b;
            a = b;
            b = temp;
        }

        return b;
    }
    public int ClimbStairs1(int n)
    {
        if (n <= 2) return n;
        return ClimbStairs(n - 1) + ClimbStairs(n - 2);
    }

    public int MySqrt(int x)
    {
        if (x < 2) return x;

        int left = 1;
        int rigth = x / 2;

        while (left <= rigth)
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
