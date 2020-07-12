// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Tasks
{
  using System.Windows.Input;
  using global::AsyncTaskExecutor.ComponentModel;

  /// <summary>
  /// AsyncTaskComponent
  /// </summary>
  public interface IAsyncTaskComponent
  {
    IAsyncTaskExecutor Executor { get; }

    IProgress Progress { get; }

    ICommand ExecuteCommand { get; }
    
    ICommand CancelCommand { get; }

    ICommand PauseResumeCommand { get; }
  }
}