using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Defines mutually exclusive window states.
    /// </summary>
    public enum ModuleWindowState
    {
        /// <summary>
        /// Window is currenly not opened.
        /// </summary>
        Closed,

        /// <summary>
        /// Default window state.
        /// </summary>
        Normal,

        /// <summary>
        /// Minimized window state.
        /// </summary>
        Minimized,

        /// <summary>
        /// Maximized window state.
        /// </summary>
        Maximized,

        /// <summary>
        /// Fullscreen window state.
        /// </summary>
        FullScreen,

        /// <summary>
        /// Floating window state.
        /// </summary>
        Floating,

        /// <summary>
        /// Docked window state.
        /// </summary>
        Docked,

        /// <summary>
        /// Hidden window state.
        /// </summary>
        Hidden,

        /// <summary>
        /// Modal window state.
        /// </summary>
        Modal
    }
}
