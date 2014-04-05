using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using EasyHook;

namespace ScanSnapHelperInject
{
    public class Main : EasyHook.IEntryPoint
    {
        static string TempPath = Environment.GetEnvironmentVariable("TEMP");

        ScanSnapHelper.ScanSnapHelperInterface Interface;
        LocalHook CreateFileWHook;
        LocalHook CreateFileAHook;
        LocalHook DeleteFileWHook;
        LocalHook DeleteFileAHook;
        Stack<ScanSnapHelper.ApiCall> Queue = new Stack<ScanSnapHelper.ApiCall>();

        public Main(
            RemoteHooking.IContext InContext,
            String InChannelName)
        {
            // connect to host...
            Interface = RemoteHooking.IpcConnectClient<ScanSnapHelper.ScanSnapHelperInterface>(InChannelName);

            Interface.Ping();
        }

        public void Run(
            RemoteHooking.IContext InContext,
            String InChannelName)
        {
            // install hook...
            try
            {
                //CreateFileWHook = LocalHook.Create(
                //    LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"),
                //    new DCreateFileW(CreateFileW_Hooked),
                //    this);
                //CreateFileWHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                //CreateFileAHook = LocalHook.Create(
                //    LocalHook.GetProcAddress("kernel32.dll", "CreateFileA"),
                //    new DCreateFileA(CreateFileA_Hooked),
                //    this);
                //CreateFileAHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                DeleteFileWHook = LocalHook.Create(
                    LocalHook.GetProcAddress("kernel32.dll", "DeleteFileW"),
                    new DDeleteFileW(DeleteFileW_Hooked),
                    this);
                DeleteFileWHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                DeleteFileAHook = LocalHook.Create(
                    LocalHook.GetProcAddress("kernel32.dll", "DeleteFileA"),
                    new DDeleteFileA(DeleteFileA_Hooked),
                    this);
                DeleteFileAHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            }
            catch (Exception ExtInfo)
            {
                Interface.ReportException(ExtInfo);

                return;
            }

            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());

            RemoteHooking.WakeUpProcess();

            // wait for host process termination...
            try
            {
                while (true)
                {
                    Thread.Sleep(500);

                    // transmit newly monitored file accesses...
                    if (Queue.Count > 0)
                    {
                        ScanSnapHelper.ApiCall[] Package = null;

                        lock (Queue)
                        {
                            Package = Queue.ToArray();

                            Queue.Clear();
                        }

                        Interface.OnApiCall(RemoteHooking.GetCurrentProcessId(), Package);
                    }
                    else
                        Interface.Ping();
                }
            }
            catch
            {
                // Ping() will raise an exception if host is unreachable
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate IntPtr DCreateFileW(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Ansi,
            SetLastError = true)]
        delegate IntPtr DCreateFileA(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate bool DDeleteFileW(
            String InFileName);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Ansi,
            SetLastError = true)]
        delegate bool DDeleteFileA(
            String InFileName);

        // just use a P-Invoke implementation to get native API access from C# (this step is not necessary for C++.NET)
        [DllImport("kernel32.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true,
            CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr CreateFileW(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile);

        [DllImport("kernel32.dll",
            CharSet = CharSet.Ansi,
            SetLastError = true,
            CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr CreateFileA(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile);

        [DllImport("kernel32.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true,
            CallingConvention = CallingConvention.StdCall)]
        static extern bool DeleteFileW(
            String InFileName);

        [DllImport("kernel32.dll",
            CharSet = CharSet.Ansi,
            SetLastError = true,
            CallingConvention = CallingConvention.StdCall)]
        static extern bool DeleteFileA(
            String InFileName);

        // this is where we are intercepting all file accesses!
        static IntPtr CreateFileW_Hooked(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile)
        {

            try
            {
                Main This = (Main)HookRuntimeInfo.Callback;

                lock (This.Queue)
                {
                    This.Queue.Push(new ScanSnapHelper.ApiCall("CreateFileW", InFileName));
                }
            }
            catch
            {
            }

            // call original API...
            return CreateFileW(
                InFileName,
                InDesiredAccess,
                InShareMode,
                InSecurityAttributes,
                InCreationDisposition,
                InFlagsAndAttributes,
                InTemplateFile);
        }

        static IntPtr CreateFileA_Hooked(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile)
        {

            try
            {
                Main This = (Main)HookRuntimeInfo.Callback;

                lock (This.Queue)
                {
                    This.Queue.Push(new ScanSnapHelper.ApiCall("CreateFileA", InFileName));
                }
            }
            catch
            {
            }

            // call original API...
            return CreateFileA(
                InFileName,
                InDesiredAccess,
                InShareMode,
                InSecurityAttributes,
                InCreationDisposition,
                InFlagsAndAttributes,
                InTemplateFile);
        }

        static bool DeleteFileW_Hooked(
            String InFileName)
        {
            bool ShouldBeHooked = InFileName.StartsWith(TempPath);

            if (ShouldBeHooked)
            {
                try
                {
                    Main This = (Main)HookRuntimeInfo.Callback;

                    lock (This.Queue)
                    {
                        This.Queue.Push(new ScanSnapHelper.ApiCall("DeleteFileW", InFileName));
                    }
                }
                catch
                {
                }
                // don't call original API
                return true;
            }
            else
            {
                // call original API...
                return DeleteFileW(
                    InFileName);
            }
        }

        static bool DeleteFileA_Hooked(
            String InFileName)
        {
            bool ShouldBeHooked = InFileName.StartsWith(TempPath);

            if (ShouldBeHooked)
            {
                try
                {
                    Main This = (Main)HookRuntimeInfo.Callback;

                    lock (This.Queue)
                    {
                        This.Queue.Push(new ScanSnapHelper.ApiCall("DeleteFileA", InFileName));
                    }
                }
                catch
                {
                }
                // don't call original API
                return true;
            }
            else
            {
                // call original API...
                return DeleteFileA(
                    InFileName);
            }
        }
    }
}
