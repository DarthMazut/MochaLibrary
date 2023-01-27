using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs.Extensions
{
    /// <summary>
    /// Defines a filter for file extensions.
    /// </summary>
    public class ExtensionFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionFilter"/> class.
        /// </summary>
        /// <param name="name">Filter name displayed to the user.</param>
        /// <param name="extension">Extension handled by this <see cref="ExtensionFilter"/> without leading dot.</param>
        public ExtensionFilter(string name, string extension)
        {
            Name = name;
            Extensions = new List<string> { extension }; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionFilter"/> class.
        /// <code>
        /// // e.g.
        /// new ExtensionFilter("TIFF (*.tif; *.tiff)", new List&lt;string&gt; { "tif", "tiff" });
        /// </code>
        /// </summary>
        /// <param name="name">Filter name displayed to the user.</param>
        /// <param name="extensions">Extensions handled by this <see cref="ExtensionFilter"/> without leading dot.</param>
        public ExtensionFilter(string name, IList<string> extensions)
        {
            Name = name;
            Extensions = extensions;
        }

        /// <summary>
        /// Filter name displayed to the user. 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Extensions handled by this <see cref="ExtensionFilter"/> without leading dot.
        /// </summary>
        public ICollection<string> Extensions { get; }
    }

}
