// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutorApp
{
  using System.Threading.Tasks;
  using AsyncTaskExecutor.ComponentModel;
  using AsyncTaskExecutor.Tasks;

  /// <summary>
  /// ViewModel
  /// </summary>
  public class ViewModel : NotifyPropertyChangedBase
  {
    private readonly ProgressViewModel _loadProgress = new ProgressViewModel();

    public ViewModel()
    {
      AsyncLoad = new AsyncTaskComponent<string>("Loading items", ExecuteAsync) {Progress = _loadProgress};
    }
    
    public IAsyncTaskComponent AsyncLoad { get; }

    private async Task ExecuteAsync(string parameter, TaskExecutionOption option)
    {
      var progress = _loadProgress;
      progress.Report(0);
      
      for (var i = 0; i < 100; i++)
      {
        var pt = option.PauseToken; 
        await pt.WaitWhilePausedAsync();
        if (!option.CancellationToken.IsCancellationRequested)
        {
          await Task.Delay(1000, option.CancellationToken);
          progress.Report(i * 1);
        }
      }
    }
  }
}