using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.Helper
{
    public static class CommonHelper
    {
        public static int calculateAvailableQuantity(short currentPosition,short end,short start)
        {
            return (currentPosition==0)?(end-start)+1:end - currentPosition;
        }
    }
}