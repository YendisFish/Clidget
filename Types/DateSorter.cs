using System;
using System.Collections.Generic;
using System.Linq;

namespace Clidget.Core.Types
{
    class Date
    {
        public static List<string> Sort(List<string> Input)
        {
            List<string> initiallist = Input;

            var orderedList = initiallist.OrderByDescending(x => DateTime.Parse(x)).ToList();
    
            List<string> ret = orderedList;
            
            return orderedList;
        }
    }
}