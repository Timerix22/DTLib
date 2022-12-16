
// ReSharper disable UnusedMember.Local

using System.Diagnostics;

namespace DTLib.Extensions;

public static class ProcessExtensions
{
    private static IProcessSuspenderImpl processSuspender;


    static ProcessExtensions()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            processSuspender = new ProcessSuspenderImplWindows();
        else
            processSuspender = new ProcessSuspenderImplUnix();
    }

    public static void Suspend(this Process p) => processSuspender.Suspend(p);

    public static void Suspend(int pid)
    {
        if (pid <= 0)
            throw new Exception($"invalid pid: {pid}");
        processSuspender.Suspend(pid);
    }

    public static void Resume(this Process p) => processSuspender.Resume(p);

    public static void Resume(int pid)
    {
        if (pid <= 0)
            throw new Exception($"invalid pid: {pid}");
        processSuspender.Resume(pid);
    }
}