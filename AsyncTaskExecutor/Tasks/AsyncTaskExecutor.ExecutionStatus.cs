// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Tasks
{
  /// <summary>
  /// AsyncTaskExecutionStatus
  /// </summary>
  public class AsyncTaskExecutionStatus
  {
    public static readonly AsyncTaskExecutionStatus WaitForActivation = new AsyncTaskExecutionStatus("WaitForActivation");
    public static readonly AsyncTaskExecutionStatus Started = new AsyncTaskExecutionStatus("Started");
    public static readonly AsyncTaskExecutionStatus Paused = new AsyncTaskExecutionStatus("Paused");
    public static readonly AsyncTaskExecutionStatus Resumed = new AsyncTaskExecutionStatus("Resumed");
    public static readonly AsyncTaskExecutionStatus Canceled = new AsyncTaskExecutionStatus("Canceled");
    public static readonly AsyncTaskExecutionStatus Faulted = new AsyncTaskExecutionStatus("Faulted");
    public static readonly AsyncTaskExecutionStatus Completed = new AsyncTaskExecutionStatus("Completed");

    public AsyncTaskExecutionStatus(string id)
    {
      Id = id;
    }

    public readonly string Id;

    public override string ToString()
    {
      return Id;
    }
  }
}