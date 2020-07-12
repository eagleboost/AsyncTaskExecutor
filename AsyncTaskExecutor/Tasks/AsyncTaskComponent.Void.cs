// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Tasks
{
  using System;
  using System.Threading.Tasks;

  /// <summary>
  /// AsyncTaskComponent
  /// </summary>
  public class AsyncTaskComponent : AsyncTaskComponent<object>
  {
    public AsyncTaskComponent(IAsyncTaskExecutor executor, Func<object, bool> canExecute = null) : base(executor, canExecute)
    {
    }

    public AsyncTaskComponent(string taskName, Func<object, TaskExecutionOption, Task> taskFunc, Func<object, bool> canExecute = null)
      : base(taskName, taskFunc, canExecute)
    {
    }
  }
}