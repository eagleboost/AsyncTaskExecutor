// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Tasks
{
  using System;
  using System.Threading.Tasks;
  using System.Windows.Input;
  using global::AsyncTaskExecutor.Commands;
  using global::AsyncTaskExecutor.ComponentModel;

  /// <summary>
  /// AsyncTaskComponent
  /// </summary>
  public class AsyncTaskComponent<T> : NotifyPropertyChangedBase, IAsyncTaskComponent
  {
    #region Declarations
    private readonly Func<T, bool> _canExecute;
    private IProgress _progress;
    private ICommand _executeCommand;
    private ICommand _cancelCommand;
    private ICommand _pauseResumeCommand;
    #endregion Declarations

    #region ctor
    public AsyncTaskComponent(IAsyncTaskExecutor executor, Func<T, bool> canExecute = null)
    {
      Executor = executor;
      _canExecute = canExecute;
    }
    
    public AsyncTaskComponent(string taskName, Func<T, TaskExecutionOption, Task> taskFunc, Func<T, bool> canExecute = null)
    {
      Executor = new AsyncTaskExecutor<T>(taskName, taskFunc);
      _canExecute = canExecute;
    }
    #endregion ctor
    
    #region Public Properties
    public IAsyncTaskExecutor Executor { get; }

    public IProgress Progress
    {
      get => _progress;
      set => this.SetValue(ref _progress, value);
    }

    public ICommand ExecuteCommand => _executeCommand ??= CreateExecuteCommand();

    public ICommand CancelCommand => _cancelCommand ??= CreateCancelCommand();

    public ICommand PauseResumeCommand => _pauseResumeCommand ??= CreatePauseResumeCommand();
    #endregion Public Properties

    #region Private Methods

    private ICommand CreateExecuteCommand()
    {
      return new DelegateCommand<T>(o => Executor.ExecuteTask(o), _canExecute);
    }
    
    private ICommand CreateCancelCommand()
    {
      return new DelegateCommand(CancelTask, CanCancelTask);
    }

    private void CancelTask()
    {
      Executor.CancelTask();
    }

    private bool CanCancelTask()
    {
      return Executor.IsBusy;
    }
    
    private ICommand CreatePauseResumeCommand()
    {
      return new DelegateCommand(PauseResumeTask, CanPauseResumeTask);
    }

    private void PauseResumeTask()
    {
      if (Executor.Status == AsyncTaskExecutionStatus.Paused)
      {
        Executor.ResumeTask();
      }
      else
      {
        Executor.PauseTask();
      }
    }

    private bool CanPauseResumeTask()
    {
      return Executor.IsBusy;
    }
    #endregion Private Methods
  }
}