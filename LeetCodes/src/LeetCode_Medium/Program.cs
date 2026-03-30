namespace LeetCode_Medium;

internal class Program
{
    static void Main(string[] args)
    {

    }

    static int SildingWindow(string s)
    {
        //s = "abcba"
        HashSet<char> let = [];
        int l = 0;
        int r = 0;
        int length = 0;

        while (r < s.Length)
        {
            if (!let.Contains(s[r]))
            { let.Add(s[r]); length = Math.Max((r++) - l + 1, length); }
            else
                let.Remove(s[l++]);

        }
        return length;
    }

    public int MinSubArrayLen(int target, int[] nums)
    {
        int l = 0, r = 0;
        int summ = 0;
        int minLength = int.MaxValue;

        while (r < nums.Length)
        {
            summ += nums[r];

            while (summ >= target)
            {
                minLength = Math.Min(r - l + 1, minLength);
                summ -= nums[l++];
            }
            r++;
        }

        return minLength == int.MaxValue ? 0 : minLength;
    }
    public int LongestOnes(int[] nums, int k)
    {
        //nums = [0,0,1,1,0,0,1,1,1,0,1,1,0,0,0,1,1,1,1], k = 3
        int l = 0, r = 0;
        int maxLength = 0;
        int zeroCount = 0;

        while (r < nums.Length)
        {
            if (nums[r] == 0) zeroCount++;

            while (zeroCount > k)
            {
                if (nums[l++] == 0) zeroCount--;
            }

            maxLength = Math.Max((r++) - l + 1, maxLength);
        }

        return maxLength;
    }
    public int MaxArea(int[] height)
    {
        int l = 0, r = height.Length - 1;
        int maxArea = 0;


        while (l < r)
        {
            int width = r - l;

            maxArea = Math.Max(maxArea, width * Math.Min(height[l], height[r]));

            if (height[l] < height[r]) l++;
            else r--;
        }

        return maxArea;
    }
    public IList<IList<int>> ThreeSum(int[] nums)
    {
        Array.Sort(nums);

        IList<IList<int>> res = [];

        for (int i = 0; i < nums.Length; i++)
        {
            if (i > 0 && nums[i] == nums[i - 1]) continue;

            int l = i + 1;
            int r = nums.Length - 1;

            while (l < r)
            {
                int summ = nums[i] + nums[l] + nums[r];

                if (summ == 0)
                {
                    res.Add([nums[i], nums[l], nums[r]]);
                    while (l < r && nums[l] == nums[l + 1]) l++;
                    while (l < r && nums[r] == nums[r - 1]) r--;
                    l++;
                    r--;
                }
                else if (summ < 0) l++;
                else r--;
            }
        }
        return res;
    }

    public void SortColors(int[] nums)
    {
        int l = 0;
        int m = 0;
        int r = nums.Length - 1;

        while (m <= r)
        {
            if (nums[m] == 0)
            {
                (nums[l], nums[m]) = (nums[m], nums[l]);
                l++;
                m++;
            }
            else if (nums[m] == 1) m++;
            else
            {
                (nums[m], nums[r]) = (nums[r], nums[m]);
                r--;
            }
        }
    }

    public int Search(int[] nums, int target)
    {
        int l = 0;
        int r = nums.Length - 1;

        while (l <= r)
        {
            int m = (l + r) / 2;
            if (nums[m] == target) return m;

            else if (nums[l] <= nums[m])
            {
                if (nums[l] <= target && target < nums[m]) r = m - 1;
                else l = m + 1;
            }
            else
            {
                if (nums[m] < target && target <= nums[r]) l = m + 1;
                else r = m - 1;
            }
        }
        return -1;
    }
    public int FindMin(int[] nums)
    {
        int l = 0;
        int r = nums.Length - 1;

        while (l < r)
        {
            int m = (l + r) / 2;

            if (nums[m] > nums[r]) l = m + 1;
            else r = m;

        }
        return nums[l];
    }
    public bool SearchMatrix(int[][] matrix, int target)
    {
        int m = matrix.Length;
        int n = matrix[0].Length;
        int l = 0;
        int r = m * n - 1;

        while (l <= r)
        {
            int med = l + (r - l) / 2;
            int row = med / n;
            int col = med % n;
            if (matrix[row][col] == target) return true;
            else if (matrix[row][col] < target) l = med + 1;
            else r = med - 1;
        }
        return false;
    }
    //public ListNode DetectCycle(ListNode head)
    //{
    //    var fast = head;
    //    var slow = head;

    //    while (fast != null && fast.next != null)
    //    {
    //        slow = slow.next;
    //        fast = fast.next.next;

    //        if (slow == fast)
    //        {
    //            slow = head;

    //        }
    //    }
    //}

    public ListNode OddEvenList(ListNode head)
    {
        ListNode even = new ListNode(0);
        ListNode odd = new ListNode(0);
        var tempEven = even;
        var tempOdd = odd;
        var cur = head;

        while (cur != null)
        {
            tempEven?.next = cur;
            tempEven = tempEven?.next;
            tempOdd?.next = cur?.next;
            tempOdd = tempOdd?.next;
            cur = cur?.next?.next;
        }
        tempEven?.next = odd.next;
        return even.next;
    }
    public ListNode MiddleNode(ListNode head)
    {
        var fast = head;
        var slow = head;

        while (fast.next != null)
        {
            fast = fast.next.next;
            slow = slow.next;
        }
        return slow;
    }
    public bool LeafSimilar(TreeNode root1, TreeNode root2)
    {
        List<int> nums1 = [];
        List<int> nums2 = [];
        Helper(root1, nums1);
        Helper(root2, nums2);
        return nums1.SequenceEqual(nums2);
    }
    private void Helper(TreeNode root, List<int> nums)
    {
        if (root == null) return;
        if (root.left == null && root.right == null)
        { 
            nums.Add(root.val);
            return;
        }
        Helper(root.left, nums);
        Helper(root.right, nums);
    }
}

public class Solution1
{
    public int res = 0;
    public int RangeSumBST(TreeNode root, int low, int high)
    {
        Helper(root, low, high);
        return res;
    }
    private void Helper(TreeNode root, int low, int high)
    {
        if (root == null) return;

        if (root.val > low) Helper(root.left, low, high);
        if (root.val < high) Helper(root.right, low, high);

        if (root.val >= low && root.val <= high) res += root.val;
    }
}
public class Solution2
{
    private int res = 0;
    public int MaxAncestorDiff(TreeNode root)
    {
        if (root == null) return res;
        Dfs(root, root.val, root.val);
        return res;
    }

    private void Dfs(TreeNode node, int high, int low)
    {
        if (node == null) return;
        res = Math.Max(res, Math.Abs(high - node.val));
        res = Math.Max(res, Math.Abs(low - node.val));
        var h = Math.Max(node.val, high);
        var l = Math.Min(node.val, low);
        Dfs(node.left, h, l);
        Dfs(node.right, h, l);
    }
}
public class Solution
{
    private long totalProduct = 0;
    private long totalSum = 0;
    private const int MOD = 1_000_000_007;

    public int MaxProduct(TreeNode root)
    {
        if (root == null) return 0;
        totalSum = GetTotalSum(root);
        Helper(root);
        return (int)(totalProduct % MOD);
    }

    private long Helper(TreeNode root)
    {
        if (root == null) return 0;

        var left = Helper(root.left);
        var right = Helper(root.right);

        long currentSubTreeSum = root.val + left + right;

        // Shu tugunning tepasidan kessak qanday ko'paytma hosil bo'lishini hisoblaymiz
        long currentProduct = currentSubTreeSum * (totalSum - currentSubTreeSum);

        totalProduct = Math.Max(totalProduct, currentProduct);

        return currentSubTreeSum;
    }

    private long GetTotalSum(TreeNode root)
    {
        if (root == null) return 0;
        return root.val + GetTotalSum(root.left) + GetTotalSum(root.right);
    }
}