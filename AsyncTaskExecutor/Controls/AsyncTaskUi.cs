// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Controls
{
  using System;
  using System.Windows;
  using System.Windows.Documents;
  using AsyncTaskExecutor.ComponentModel;
  using AsyncTaskExecutor.Tasks;
  using Microsoft.Xaml.Behaviors;

  /// <summary>
  /// AsyncTaskUi
  /// </summary>
  public partial class AsyncTaskUi : Behavior<FrameworkElement>
  {
    private IDisposable _cleanup;
    
    public static readonly DependencyProperty TemplateProperty = DependencyProperty.Register(
      "Template", typeof(DataTemplate), typeof(AsyncTaskUi));

    public DataTemplate Template
    {
      get { return (DataTemplate) GetValue(TemplateProperty); }
      set { SetValue(TemplateProperty, value); }
    }
    
    public static readonly DependencyProperty AsyncTaskComponentProperty = DependencyProperty.Register(
      "AsyncTaskComponent", typeof(IAsyncTaskComponent), typeof(AsyncTaskUi), new PropertyMetadata(OnAsyncTaskExecutorChanged));

    public IAsyncTaskComponent AsyncTaskComponent
    {
      get { return (IAsyncTaskComponent) GetValue(AsyncTaskComponentProperty); }
      set { SetValue(AsyncTaskComponentProperty, value); }
    }

    private static void OnAsyncTaskExecutorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((AsyncTaskUi)obj).Setup();
    }
    
    protected override void OnAttached()
    {
      base.OnAttached();

      var element = AssociatedObject;
      element.Loaded += HandleLoaded;
    }

    protected override void OnDetaching()
    {
      Cleanup();
      
      base.OnDetaching();
    }

    private void HandleLoaded(object sender, RoutedEventArgs e)
    {
      var element = (FrameworkElement) sender;
      element.Loaded -= HandleLoaded;

      element.Unloaded += HandleUnloaded;
      
      Setup();
    }

    private void HandleUnloaded(object sender, RoutedEventArgs e)
    {
      var element = (FrameworkElement) sender;
      element.Unloaded -= HandleUnloaded;

      Cleanup();
    }
    
    private void Setup()
    {
      if (_cleanup != null)
      {
        return;
      }

      var taskComponent = AsyncTaskComponent;
      if (taskComponent == null)
      {
        return;
      }

      var element = AssociatedObject;
      var layer = AdornerLayer.GetAdornerLayer(element);
      if (layer == null)
      {
        return;
      }

      var adc = new AdornerContentPresenter(element)
      {
        Content = taskComponent,
        ContentTemplate = Template
      };

      _cleanup = new AdornerHandler(taskComponent.Executor, element, layer, adc);
    }

    private void Cleanup()
    {
      RefUtils.Dispose(ref _cleanup);
    }
  }
}