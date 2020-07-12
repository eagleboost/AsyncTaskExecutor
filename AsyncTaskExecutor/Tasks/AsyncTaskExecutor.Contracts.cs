// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Tasks
{
  using System;
  using System.ComponentModel;
  using System.Threading.Tasks;
  using System.Windows.Input;
  using global::AsyncTaskExecutor.ComponentModel;

  /// <summary>
  /// IAsyncTaskExecutor
  /// </summary>
  public interface IAsyncTaskExecutor : IBusyStatus, IDisposable
  {
    #region Properties
    string TaskName { get; }
    
    AsyncTaskExecutionStatus Status { get; }
    #endregion Properties
    
    #region Methods
    Task ExecuteTask(object parameter);

    void CancelTask();

    void PauseTask();
    
    void ResumeTask();
    #endregion Methods

    #region Events
    event EventHandler Started;
    
    event EventHandler<ExecutionErrorEventArgs> Faulted;
    
    event EventHandler Executed;
    #endregion Events
  }
}