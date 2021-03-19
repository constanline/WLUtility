using System;
using System.Runtime.InteropServices;

namespace WLUtility
{
    class DllHelper
    {
        [DllImport("WLHook.dll")]
        public static extern IntPtr SetTargetPid(uint dwPid);
    }
}
