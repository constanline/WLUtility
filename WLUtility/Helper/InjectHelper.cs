using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WLUtility.Helper
{
    public enum DllInjectionResult
    {
        DllNotFound,
        GameProcessNotFound,
        InjectionFailed,
        UnInjectionFailed,
        Success
    }


    internal class InjectHelper
    {
        private static InjectHelper _instance;

        public static InjectHelper GetInstance => _instance ?? (_instance = new InjectHelper());

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize,
            uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size,
            int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, uint dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int WaitForSingleObject(IntPtr hHandel, uint dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetExitCodeThread(IntPtr hThread, IntPtr lpExitCode);

        public DllInjectionResult Inject(string sProcName, string sDllPath)
        {
            uint dwInjectPid = 0;

            var processes = Process.GetProcesses();
            foreach (var t in processes)
                if (t.ProcessName == sProcName)
                {
                    dwInjectPid = (uint) t.Id;
                    break;
                }

            return Inject(dwInjectPid, sDllPath);
        }

        public DllInjectionResult Inject(uint dwInjectPid, string sDllPath)
        {
            if (dwInjectPid == 0) return DllInjectionResult.GameProcessNotFound;

            if (!File.Exists(sDllPath)) return DllInjectionResult.DllNotFound;

            if (!InjectOrUnInject(dwInjectPid, sDllPath)) return DllInjectionResult.InjectionFailed;

            return DllInjectionResult.Success;
        }

        public DllInjectionResult UnInject(uint dwInjectPid, string sDllPath)
        {
            if (dwInjectPid == 0) return DllInjectionResult.GameProcessNotFound;

            if (!File.Exists(sDllPath)) return DllInjectionResult.DllNotFound;

            if (!InjectOrUnInject(dwInjectPid, sDllPath, false)) return DllInjectionResult.UnInjectionFailed;

            return DllInjectionResult.Success;
        }

        private bool InjectOrUnInject(uint dwInjectPid, string sDllPath, bool isInject = true)
        {
            var hProcess = OpenProcess(0x2 | 0x8 | 0x10 | 0x20 | 0x400, 0, dwInjectPid);
            if (hProcess == IntPtr.Zero)
                return false;

            var bytes = Encoding.ASCII.GetBytes(sDllPath);

            var lpAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint) bytes.Length, 0x1000, 0X04);
            if (lpAddress == IntPtr.Zero)
                return false;

            if (WriteProcessMemory(hProcess, lpAddress, bytes, (uint) bytes.Length, 0) == 0)
                return false;
            if (isInject)
            {
                var procAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                if (procAddress == IntPtr.Zero)
                    return false;

                if (CreateRemoteThread(hProcess, IntPtr.Zero, 0, procAddress, lpAddress, 0, IntPtr.Zero) == IntPtr.Zero)
                    return false;

                WaitForSingleObject(hProcess, 0xFFFFFFFF);
            }
            else
            {
                var procAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "GetModuleHandleA");
                if (procAddress == IntPtr.Zero)
                    return false;

                var hRemoteThread =
                    CreateRemoteThread(hProcess, IntPtr.Zero, 0, procAddress, lpAddress, 0, IntPtr.Zero);
                if (hRemoteThread == IntPtr.Zero)
                    return false;
                WaitForSingleObject(hProcess, 0xFFFFFFFF);
                
                var hModule = Marshal.AllocHGlobal(4);
                if (GetExitCodeThread(hRemoteThread, hModule) == IntPtr.Zero)
                    return false;
                var dwModule = (uint) Marshal.PtrToStructure(hModule, typeof(uint));

                procAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "FreeLibrary");
                if (procAddress == IntPtr.Zero)
                    return false;

                CloseHandle(hRemoteThread);

                hRemoteThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, procAddress, (IntPtr) dwModule, 0,
                    IntPtr.Zero);
                if (hRemoteThread == IntPtr.Zero)
                    return false;
                WaitForSingleObject(hProcess, 0xFFFFFFFF);

                CloseHandle(hRemoteThread);
            }

            CloseHandle(hProcess);

            VirtualFreeEx(hProcess, lpAddress, 0, 0x00008000);

            return true;
        }
    }
}