using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public class NavigationResultData
    {
        public NavigationResultData(NavigationResult result) : this(result, null) { }

        public NavigationResultData(NavigationResult result, object? data)
        {
            Data = data;
            Result = result;
        }

        public NavigationResult Result { get; }

        public object? Data { get; }
    }
}
