using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

// https://github.com/microsoft/WindowsAppSDK/discussions/3680
namespace GroceryStore.MainApp.Helpers;

public static class IntPrHelper
{
    [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

    public const int GWL_HWNDPARENT = (-8);

    public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
    {
        if (IntPtr.Size == 4)
        {
            return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
        }
        return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
    }

    [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
    public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
    public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
}
