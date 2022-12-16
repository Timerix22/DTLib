using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace DTLib.Extensions;

// https://stackoverflow.com/questions/41041730/net-core-app-how-to-send-sigterm-to-child-processes
internal class ProcessSuspenderImplUnix : IProcessSuspenderImpl
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    enum Signum 
    {
        /// Hangup (POSIX).
        SIGHUP    =  1, 
        /// Interrupt (ANSI).
        SIGINT    =  2, 
        /// Quit (POSIX).
        SIGQUIT   =  3, 
        /// Illegal instruction (ANSI).
        SIGILL    =  4, 
        /// Trace trap (POSIX).
        SIGTRAP   =  5, 
        /// Abort (ANSI).
        SIGABRT   =  6, 
        /// IOT trap (4.2 BSD).
        SIGIOT    =  6, 
        /// BUS error (4.2 BSD).
        SIGBUS    =  7, 
        /// Floating-point exception (ANSI).
        SIGFPE    =  8, 
        /// Kill, unblockable (POSIX).
        SIGKILL   =  9, 
        /// User-defined signal 1 (POSIX).
        SIGUSR1   = 10, 
        /// Segmentation violation (ANSI).
        SIGSEGV   = 11, 
        /// User-defined signal 2 (POSIX).
        SIGUSR2   = 12, 
        /// Broken pipe (POSIX).
        SIGPIPE   = 13, 
        /// Alarm clock (POSIX).
        SIGALRM   = 14, 
        /// Termination (ANSI).
        SIGTERM   = 15, 
        /// Stack fault.
        SIGSTKFLT = 16, 
        /// Same as SIGCHLD (System V).
        SIGCLD    = SIGCHLD, 
        /// Child status has changed (POSIX).
        SIGCHLD   = 17, 
        /// Continue (POSIX).
        SIGCONT   = 18, 
        /// Stop, unblockable (POSIX).
        SIGSTOP   = 19, 
        /// Keyboard stop (POSIX).
        SIGTSTP   = 20, 
        /// Background read from tty (POSIX).
        SIGTTIN   = 21, 
        /// Background write to tty (POSIX).
        SIGTTOU   = 22, 
        /// Urgent condition on socket (4.2 BSD).
        SIGURG    = 23, 
        /// CPU limit exceeded (4.2 BSD).
        SIGXCPU   = 24, 
        /// File size limit exceeded (4.2 BSD).
        SIGXFSZ   = 25, 
        /// Virtual alarm clock (4.2 BSD).
        SIGVTALRM = 26, 
        /// Profiling alarm clock (4.2 BSD).
        SIGPROF   = 27, 
        /// Window size change (4.3 BSD, Sun).
        SIGWINCH  = 28, 
        /// Pollable event occurred (System V).
        SIGPOLL   = SIGIO, 
        /// I/O now possible (4.2 BSD).
        SIGIO     = 29, 
        /// Power failure restart (System V).
        SIGPWR    = 30, 
        /// Bad system call.
        SIGSYS    = 31, 
        SIGUNUSED = 31
    }
            
    [DllImport ("libc", SetLastError=true, EntryPoint="kill")]
    private static extern int sys_kill (int pid, int sig);
            
    public void Suspend(Process p) => Suspend(p.Id);
    public void Suspend(int pid) => sys_kill(pid, (int)Signum.SIGSTOP);

    public void Resume(Process p) => Resume(p.Id);
    public void Resume(int pid) => sys_kill(pid, (int)Signum.SIGCONT);
}