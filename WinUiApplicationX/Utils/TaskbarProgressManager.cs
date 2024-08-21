using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX.Pages.Behaviours;

namespace WinUiApplicationX.Utils
{
    public static class TaskbarProgressManager
    {
        private static readonly ITaskbarList3 taskbarInstance = (ITaskbarList3)new TaskbarInstance();

        public static void SetTaskbarState(Window window, TaskbarProgressData taskbarProgressData)
        {
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            taskbarInstance.SetProgressValue(hwnd, (ulong)taskbarProgressData.Value, 100UL);
            taskbarInstance.SetProgressState(hwnd, (int)taskbarProgressData.State);
        }

        [ComImport()]
        [Guid("56fdf344-fd6d-11d0-958a-006097c9a090")]
        [ClassInterface(ClassInterfaceType.None)]
        private class TaskbarInstance { }

        [ComImport()]
        [Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface ITaskbarList3
        {
            // ITaskbarList
            [PreserveSig]
            void HrInit();
            [PreserveSig]
            void AddTab(IntPtr hwnd);
            [PreserveSig]
            void DeleteTab(IntPtr hwnd);
            [PreserveSig]
            void ActivateTab(IntPtr hwnd);
            [PreserveSig]
            void SetActiveAlt(IntPtr hwnd);

            // ITaskbarList2
            [PreserveSig]
            void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

            // ITaskbarList3
            [PreserveSig]
            void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);
            [PreserveSig]
            void SetProgressState(IntPtr hwnd, int state);
        }  
    }
}
