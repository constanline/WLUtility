using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WLUtility
{
    public enum DllInjectionResult
    {
        DllNotFound,
        GameProcessNotFound,
        InjectionFailed,
        UnInjectionFailed,
        Success
    }


    class DllInjector
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, uint dwStackSize, IntPtr lpStartAddress,
            IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int WaitForSingleObject(IntPtr hHandel, uint dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetExitCodeThread(IntPtr hThread, IntPtr lpExitCode);

        static DllInjector _instance;

        public static DllInjector GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DllInjector();
                }
                return _instance;
            }
        }

        public DllInjectionResult Inject(string sProcName, string sDllPath)
        {
            uint dwInjectePid = 0;

            Process[] _procs = Process.GetProcesses();
            for (int i = 0; i < _procs.Length; i++)
            {
                if (_procs[i].ProcessName == sProcName)
                {
                    dwInjectePid = (uint)_procs[i].Id;
                    break;
                }
            }

            return Inject(dwInjectePid, sDllPath);
        }

        public DllInjectionResult Inject(uint dwInjectePid, string sDllPath)
        {
            if (dwInjectePid == 0)
            {
                return DllInjectionResult.GameProcessNotFound;
            }

            if (!File.Exists(sDllPath))
            {
                return DllInjectionResult.DllNotFound;
            }

            if (!InjectOrUnInject(dwInjectePid, sDllPath))
            {
                return DllInjectionResult.InjectionFailed;
            }

            return DllInjectionResult.Success;
        }

        public DllInjectionResult UnInject(uint dwInjectePid, string sDllPath)
        {
            if (dwInjectePid == 0)
            {
                return DllInjectionResult.GameProcessNotFound;
            }

            if (!File.Exists(sDllPath))
            {
                return DllInjectionResult.DllNotFound;
            }

            if (!InjectOrUnInject(dwInjectePid, sDllPath, false))
            {
                return DllInjectionResult.InjectionFailed;
            }

            return DllInjectionResult.Success;
        }

        bool InjectOrUnInject(uint dwInjectePid, string sDllPath, bool isInject = true)
        {
            IntPtr hProcess = OpenProcess((0x2 | 0x8 | 0x10 | 0x20 | 0x400), 0, dwInjectePid);
            if (hProcess == IntPtr.Zero)
                return false;

            byte[] bytes = Encoding.ASCII.GetBytes(sDllPath);

            IntPtr lpAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)bytes.Length, 0x1000, 0X04);
            if (lpAddress == IntPtr.Zero)
                return false;

            if (WriteProcessMemory(hProcess, lpAddress, bytes, (uint)bytes.Length, 0) == 0)
                return false;
            if (isInject)
            {
                IntPtr lpLLAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                if (lpLLAddress == IntPtr.Zero)
                    return false;

                if (CreateRemoteThread(hProcess, IntPtr.Zero, 0, lpLLAddress, lpAddress, 0, IntPtr.Zero) == IntPtr.Zero)
                    return false;

                WaitForSingleObject(hProcess, 0xFFFFFFFF);
            }
            else
            {
                IntPtr lpLLAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "GetModuleHandleA");
                if (lpLLAddress == IntPtr.Zero)
                    return false;

                IntPtr hRemoteThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, lpLLAddress, lpAddress, 0, IntPtr.Zero);
                if (hRemoteThread == IntPtr.Zero)
                    return false;
                WaitForSingleObject(hProcess, 0xFFFFFFFF);

                //uint dwMoudle = 0;
                //unsafe
                //{
                //    hMoudle = (IntPtr)(&dwMoudle);
                //}

                IntPtr hMoudle = Marshal.AllocHGlobal(4);
                if (GetExitCodeThread(hRemoteThread, hMoudle) == IntPtr.Zero)
                    return false;
                uint dwMoudle = (uint)Marshal.PtrToStructure(hMoudle, typeof(uint));

                lpLLAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "FreeLibrary");
                if (lpLLAddress == IntPtr.Zero)
                    return false;

                CloseHandle(hRemoteThread);

                hRemoteThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, lpLLAddress, (IntPtr)dwMoudle, 0, IntPtr.Zero);
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
