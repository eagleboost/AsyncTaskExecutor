// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Tasks
{
  using System.Threading;

  /// <summary>
  /// TaskExecutionOption
  /// </summary>
  public class TaskExecutionOption
  {
    public TaskExecutionOption(string taskName, CancellationToken ct, PauseToken pt)
    {
      TaskName = taskName;
      CancellationToken = ct;
      PauseToken = pt;
    }

    public readonly string TaskName;
    
    public readonly CancellationToken CancellationToken;

    public readonly PauseToken PauseToken;
  }
}