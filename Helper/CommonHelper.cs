using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.Helper
{
    public static class CommonHelper
    {
        public static int calculateAvailableQuantity(short currentPosition, short end, short start)
        {
            return (currentPosition == 0) ? (end - start) + 1 : (end - currentPosition) + 1;
        }

        public static List<(int start, int end)> SplitRange(int start, int end, List<int> exclusions)
        {
            List<(int start, int end)> ranges = new List<(int start, int end)>();

            // Sort the exclusions to handle them in order
            exclusions.Sort();

            int currentStart = start;

            foreach (int exclusion in exclusions)
            {
                if (exclusion >= currentStart && exclusion <= end)
                {
                    if (exclusion > currentStart)
                    {
                        ranges.Add((currentStart, exclusion - 1));
                    }
                    currentStart = exclusion + 1;
                }
            }

            // Add the final range if needed
            if (currentStart <= end)
            {
                ranges.Add((currentStart, end));
            }

            return ranges;
        }
    }
}