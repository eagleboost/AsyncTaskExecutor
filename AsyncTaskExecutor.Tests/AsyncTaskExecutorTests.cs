// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Tests
{
  using System.Threading.Tasks;
  using AsyncTaskExecutor.Tasks;
  using NUnit.Framework;

  public class AsyncTaskExecutorTests
  {
    private const string TaskName = "TaskName";
    
    [Test]
    public void Test_01_Init()
    {
      var executor = new AsyncTaskExecutor(TaskName, ExecuteAsync);
      Assert.That(executor.TaskName == TaskName);
      Assert.That(executor.IsBusy == false);
      Assert.That(executor.BusyStatus == null);
      Assert.That(executor.Status == AsyncTaskExecutionStatus.WaitForActivation);
    }

    [Test]
    public async Task Test_02_Execute()
    {
      IAsyncTaskExecutor executor = new AsyncTaskExecutor(TaskName, ExecuteAsync);
      var task = executor.ExecuteTask(null);
      Assert.That(executor.IsBusy);
      Assert.That(executor.BusyStatus == TaskName+"...");
      await task;
      Assert.That(executor.IsBusy == false);
      Assert.That(executor.BusyStatus == null);
    }
    
    [Test]
    public async Task Test_02_Cancel()
    {
      IAsyncTaskExecutor executor = new AsyncTaskExecutor(TaskName, ExecuteTimeConsumingAsync);
      var task = executor.ExecuteTask(null);
      Assert.That(executor.IsBusy);
      Assert.That(executor.BusyStatus == TaskName+"...");
      
      await Task.Delay(10);
      executor.CancelTask();
      await task;
      Assert.That(executor.IsBusy == false);
      Assert.That(executor.BusyStatus == null);
    }
    
    [Test]
    public async Task Test_03_PauseResume()
    {
      IAsyncTaskExecutor executor = new AsyncTaskExecutor(TaskName, ExecuteTimeConsumingAsync);
      var task = executor.ExecuteTask(null);
      Assert.That(executor.IsBusy);
      Assert.That(executor.BusyStatus == TaskName+"...");
      
      await Task.Delay(10);
      executor.PauseTask();
      Assert.That(executor.Status == AsyncTaskExecutionStatus.Paused);
      Assert.That(executor.IsBusy);
      Assert.That(executor.BusyStatus == TaskName+"...");
      
      executor.ResumeTask();
      Assert.That(executor.Status == AsyncTaskExecutionStatus.Resumed);
      Assert.That(executor.IsBusy);
      Assert.That(executor.BusyStatus == TaskName+"...");
    }
    
    private Task ExecuteAsync(TaskExecutionOption option)
    {
      return Task.Delay(100);
    }
    
    private async Task ExecuteTimeConsumingAsync(TaskExecutionOption option)
    {
      var pt = option.PauseToken;
      var ct = option.CancellationToken;
      for (var i = 0; i < 1000; i++)
      {
        await pt.WaitWhilePausedAsync();
        if (!ct.IsCancellationRequested)
        {
          await Task.Delay(100);
        }
      }
    }
  }
}