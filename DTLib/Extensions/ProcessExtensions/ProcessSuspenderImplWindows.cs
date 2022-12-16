using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace DTLib.Extensions;

// https://github.com/SarathR/ProcessUtil
internal class ProcessSuspenderImplWindows : IProcessSuspenderImpl
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    enum ThreadAccess
    {
        TERMINATE = 0x0001,
        SUSPEND_RESUME = 0x0002,
        GET_CONTEXT = 0x0008,
        SET_CONTEXT = 0x0010,
        SET_INFORMATION = 0x0020,
        QUERY_INFORMATION = 0x0040,
        SET_THREAD_TOKEN = 0x0080,
        IMPERSONATE = 0x0100,
        DIRECT_IMPERSONATION = 0x0200
    }
            
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern uint SuspendThread(IntPtr hThread);
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern int ResumeThread(IntPtr hThread);

            
    public void Suspend(Process process)
    {
        foreach (ProcessThread thread in process.Threads)
        {
            var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
            if (pOpenThread == IntPtr.Zero)
            {
                int errCode=Marshal.GetLastWin32Error();
                throw new Exception($"unmanaged function exited with error: {errCode}");
            }
            SuspendThread(pOpenThread);
        }
    }

    public void Suspend(int pid) => Suspend(Process.GetProcessById(pid));
            
    public void Resume(Process process)
    {
        foreach (ProcessThread thread in process.Threads)
        {
            var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
            if (pOpenThread == IntPtr.Zero)
            {
                int errCode=Marshal.GetLastWin32Error();
                throw new Exception($"unmanaged function exited with error: {errCode}");
            }
            ResumeThread(pOpenThread);
        }
    }

    public void Resume(int pid) => Resume(Process.GetProcessById(pid));
}