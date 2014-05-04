using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using EasyHook;
using System.Diagnostics;

namespace ScanSnapHelperInject
{
    public class Main : EasyHook.IEntryPoint
    {
        ScanSnapHelper.ScanSnapHelperInterface Interface;
        LocalHook CreateFileWHook;
        LocalHook CreateFileAHook;
        Stack<ScanSnapHelper.ApiCall> Queue = new Stack<ScanSnapHelper.ApiCall>();
        static string FilePathPattern;
        static string HookCommand;

        public Main(
            RemoteHooking.IContext InContext,
            String InChannelName)
        {
            // connect to host...
            Interface = RemoteHooking.IpcConnectClient<ScanSnapHelper.ScanSnapHelperInterface>(InChannelName);

            Interface.Ping();

            FilePathPattern = Interface.GetFilePathPattern();
            HookCommand = Interface.GetHookCommand();
        }

        public void Run(
            RemoteHooking.IContext InContext,
            String InChannelName)
        {
            // install hook...
            try
            {
                CreateFileWHook = LocalHook.Create(
                    LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"),
                    new DCreateFileW(CreateFileW_Hooked),
                    this);
                CreateFileWHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                CreateFileAHook = LocalHook.Create(
                    LocalHook.GetProcAddress("kernel32.dll", "CreateFileA"),
                    new DCreateFileA(CreateFileA_Hooked),
                    this);
                CreateFileAHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
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
                // %TEMP%\SSRawData\ScanSnap*.raw の読み込み時に外部コマンドを起動する
                if (InDesiredAccess == 0x80000000 // GENERIC_READ
                    && InFileName.Contains(FilePathPattern)) {
                    ProcessStartInfo psInfo = new ProcessStartInfo();
                    psInfo.FileName = HookCommand;
                    psInfo.Arguments = InFileName;
                    psInfo.CreateNoWindow = true;   // コンソール・ウィンドウを開かない
                    psInfo.UseShellExecute = false; // シェル機能を使用しない
                    var p = Process.Start(psInfo);
                    p.WaitForExit();                // 終了まで待機
                }

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
    }
}
