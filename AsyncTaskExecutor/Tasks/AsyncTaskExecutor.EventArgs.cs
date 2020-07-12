// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Tasks
{
  using System;

  /// <summary>
  /// ExecutionErrorEventArgs
  /// </summary>
  public class ExecutionErrorEventArgs : EventArgs
  {
    public ExecutionErrorEventArgs(Exception ex)
    {
      Exception = ex;
    }

    public readonly Exception Exception;
  }
}