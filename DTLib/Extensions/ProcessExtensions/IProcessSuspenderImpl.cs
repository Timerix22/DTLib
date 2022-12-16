using System.Diagnostics;

namespace DTLib.Extensions;

internal interface IProcessSuspenderImpl
{
    void Suspend(Process p);
    void Suspend(int pid);
    void Resume(Process p);
    void Resume(int pid);
}