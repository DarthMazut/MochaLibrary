using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWPF.Temp
{
    public class StandardDialogModuleEx : StandardDialogModule
    {
        public static int[] ArrayDiff(int[] a, int[] b)
        {
            List<int> result = new List<int>();

            foreach (int num1 in a)
            {
                if(!b.Contains(num1))
                {
                    result.Add(num1);
                }
            }

            return a.Where(n => !b.Contains(n)).ToArray();

            return result.ToArray();
        }
    }
}
