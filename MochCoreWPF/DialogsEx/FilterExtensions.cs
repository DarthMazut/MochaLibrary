using MochaCore.DialogsEx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochCoreWPF.DialogsEx
{
    /// <summary>
    /// Provides extension methods for translating <see cref="ExtensionFilter"/> collections
    /// into <see langword="string"/> interpretable by WPF. 
    /// </summary>
    public static class FilterExtensions
    {
        /// <summary>
        /// Translates collection of <see cref="ExtensionFilter"/> objects
        /// into <see langword="string"/> interpretable by WPF as extension filters.
        /// </summary>
        /// <param name="filters">Filters to be translated for WPF.</param>
        public static string ToWpfFilterFormat(this IList<ExtensionFilter> filters)
        {
            return string.Join('|', filters.Select(f => $"{f.Name}|*.{f.Extensions}"));
        }
    }
}
