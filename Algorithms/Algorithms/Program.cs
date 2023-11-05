using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Solution.Search(new int[]{ 5}, 5));
            Console.ReadLine();
        }
    }

    public static class Solution
    {
        public static int Search(int[] nums, int target)
        {
            int l = 0;
            int h = nums.Length - 1;
            int mid;

            while (l <= h)
            {
                mid = (l + h) / 2;
                if(nums[mid] == target)
                {
                    return mid;
                }
                if (nums[mid] < target)
                {
                    l = mid + 1;
                }
                else
                {
                    h = mid - 1;
                }
            }
            return -1;
    
    }
    }
}
