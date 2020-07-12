// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Tasks
{
  using System;
  using System.Threading.Tasks;

  /// <summary>
  /// AsyncTaskExecutor
  /// </summary>
  public class AsyncTaskExecutor : AsyncTaskExecutor<object> 
  {
    public AsyncTaskExecutor(string taskName, Func<TaskExecutionOption, Task> taskFunc)
      : base(taskName, (_, o) => taskFunc(o))
    {
    }
  }
}