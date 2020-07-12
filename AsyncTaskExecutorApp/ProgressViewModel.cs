// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutorApp
{
  using System;
  using AsyncTaskExecutor.ComponentModel;

  /// <summary>
  /// ProgressViewModel
  /// </summary>
  public class ProgressViewModel : NotifyPropertyChangedBase, IProgress<double>, IProgress
  {
    private double _progress;

    object IProgress.Progress => Progress;
    
    public double Progress
    {
      get => _progress;
      private set => this.SetValue(ref _progress, value);
    }
    
    public void Report(double value)
    {
      Progress = value;
    }
  }
}