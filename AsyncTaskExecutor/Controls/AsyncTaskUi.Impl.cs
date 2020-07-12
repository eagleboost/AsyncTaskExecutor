// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Controls
{
  using System;
  using System.Windows;
  using System.Windows.Documents;
  using System.Windows.Threading;
  using AsyncTaskExecutor.ComponentModel;
  using AsyncTaskExecutor.Tasks;

  /// <summary>
  /// AdornerHandler
  /// </summary>
  public partial class AsyncTaskUi
  {
    private class AdornerHandler : IDisposable
    {
      private readonly IAsyncTaskExecutor _executor;
      private readonly FrameworkElement _element;
      private readonly AdornerLayer _layer;
      private readonly AdornerContentPresenter _adorner;
      private DelegateDisposable _cleanup = new DelegateDisposable();
      
      public AdornerHandler(IAsyncTaskExecutor executor, FrameworkElement element, AdornerLayer layer, AdornerContentPresenter adorner)
      {
        _executor = executor;
        _element = element;
        _layer = layer;
        _adorner = adorner;
        Start();
      }

      private void Start()
      {
        _executor.Started += HandleStarted;
        _executor.Executed += HandleExecuted;
        _cleanup.Add(() =>
        {
          _executor.Executed -= HandleExecuted;
          _executor.Started -= HandleStarted;
        });
      }
      
      private void HandleStarted(object sender, EventArgs e)
      {
        AddAdorner();
        _cleanup.Add(RemoveAdorner);
      }
      
      private void HandleExecuted(object sender, EventArgs e)
      {
        RemoveAdorner();
      }

      private void AddAdorner()
      {
        _element.Dispatcher.BeginInvoke(() =>
        {
          _element.IsEnabled = false;
          _layer.Add(_adorner);
        });
      }

      private void RemoveAdorner()
      {
        _element.Dispatcher.BeginInvoke(() =>
        {
          _layer.Remove(_adorner);
          _element.IsEnabled = true;
        });
      }
      
      public void Dispose()
      {
        RefUtils.Dispose(ref _cleanup);
      }
    }
  }
}