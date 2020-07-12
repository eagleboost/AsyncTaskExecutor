// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Tasks
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using global::AsyncTaskExecutor.ComponentModel;

  /// <summary>
  /// AsyncTaskExecutorBase
  /// </summary>
  public class AsyncTaskExecutor<T> : NotifyPropertyChangedBase, IAsyncTaskExecutor
  {
    #region Declarations
    private readonly Func<T, TaskExecutionOption, Task> _taskFunc;
    private bool _isBusy;
    private AsyncTaskExecutionStatus _status = AsyncTaskExecutionStatus.WaitForActivation; 
    private string _busyStatus;
    private CancellationTokenSource _cts;
    private readonly PauseTokenSource _pts = new PauseTokenSource();
    #endregion Declarations

    #region ctors
    public AsyncTaskExecutor(string taskName, Func<T, TaskExecutionOption, Task> taskFunc) 
    {
      TaskName = taskName;
      _taskFunc = taskFunc;
    }
    #endregion ctors
    
    #region IAsyncTaskExecutor
    public string TaskName { get; }

    public AsyncTaskExecutionStatus Status
    {
      get => _status;
      private set => this.SetValue(ref _status, value);
    }

    public bool IsBusy
    {
      get => _isBusy;
      private set => this.SetValue(ref _isBusy, value);
    }

    public string BusyStatus
    {
      get => _busyStatus;
      private set => this.SetValue(ref _busyStatus, value);
    }

    public event EventHandler Started;

    private void RaiseStarted()
    {
      Started?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<ExecutionErrorEventArgs> Faulted;

    private void RaiseFaulted(Exception ex)
    {
      Faulted?.Invoke(this, new ExecutionErrorEventArgs(ex));
    }
    
    public event EventHandler Executed;
    
    private void RaiseExecuted()
    {
      Executed?.Invoke(this, EventArgs.Empty);
    }

    Task IAsyncTaskExecutor.ExecuteTask(object parameter)
    {
      return ExecuteAsync(parameter);
    }
    
    void IAsyncTaskExecutor.CancelTask()
    {
      CancelTask();
    }
    
    void IAsyncTaskExecutor.PauseTask()
    {
      PauseTask();
    }
    
    void IAsyncTaskExecutor.ResumeTask()
    {
      ResumeTask();
    }
    #endregion IAsyncTaskExecutor

    #region IDisposable
    public void Dispose()
    {
      CancelTask();
    }
    #endregion IDisposable

    #region Public Methods
    public Task ExecuteAsync(object parameter)
    {
      var ct = RefUtils.Reset(ref _cts);
      ct.Register(OnCanceled);
      
      SetStatus(AsyncTaskExecutionStatus.Started);
      var option = new TaskExecutionOption(TaskName, ct, _pts.Token);
      return ExecuteAsyncCore(option, CreateExecuteTask(parameter, option));
    }
    
    public void CancelTask()
    {
      RefUtils.Cancel(ref _cts);
    }
    
    public void PauseTask()
    {
      _pts.IsPaused = true;
      SetStatus(AsyncTaskExecutionStatus.Paused);
    }
    
    public void ResumeTask()
    {
      _pts.IsPaused = false;
      SetStatus(AsyncTaskExecutionStatus.Resumed);
    }
    #endregion Public Methods
    
    #region Private Methods
    private async Task ExecuteAsyncCore(TaskExecutionOption option, Task task)
    {
      try
      {
        RaiseStarted();
        SetBusy(option.TaskName + "...");

        await task.ConfigureAwait(false);

        SetBusy("Completed");

        await Task.Delay(800).ConfigureAwait(false);
        
        SetStatus(AsyncTaskExecutionStatus.Completed);
      }
      catch (Exception ex)
      {
        SetStatus(AsyncTaskExecutionStatus.Faulted);
        RaiseFaulted(ex);
      }
      finally
      {
        ClearBusy();
        RaiseExecuted();
      }
    }

    private Task CreateExecuteTask(object parameter, TaskExecutionOption option)
    {
      return _taskFunc((T) parameter, option);
    }
    
    private void SetStatus(AsyncTaskExecutionStatus status)
    {
      Status = status;
    }

    private void OnCanceled()
    {
      _pts.IsPaused = false;
      ClearBusy();
      SetStatus(AsyncTaskExecutionStatus.Canceled);
    }
    
    private void SetBusy(string status)
    {
      IsBusy = true;
      BusyStatus = status;
    }

    private void ClearBusy()
    {
      IsBusy = false;
      BusyStatus = null;
    }
    #endregion Private Methods

    #region Overrides
    public override string ToString()
    {
      return IsBusy ? BusyStatus : base.ToString();
    }
    #endregion Overrides
  }
}