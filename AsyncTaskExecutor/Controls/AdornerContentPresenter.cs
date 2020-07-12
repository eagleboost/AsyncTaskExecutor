// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Controls
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Documents;
  using System.Windows.Media;

  public class AdornerContentPresenter : Adorner
  {
    private readonly ContentPresenter _contentPresenter;
    private readonly VisualCollection _visuals;

    public AdornerContentPresenter(UIElement adornedElement)
      : base(adornedElement)
    {
      _visuals = new VisualCollection(this);
      _contentPresenter = new ContentPresenter();
      _visuals.Add(_contentPresenter);
    }

    public AdornerContentPresenter(UIElement adornedElement, Visual content)
      : this(adornedElement)
    {
      Content = content;
    }

    protected override int VisualChildrenCount => _visuals.Count;

    public object Content
    {
      get => _contentPresenter.Content;
      set => _contentPresenter.Content = value;
    }
    
    public DataTemplate ContentTemplate
    {
      get => _contentPresenter.ContentTemplate;
      set => _contentPresenter.ContentTemplate = value;
    }
    
    protected override Size ArrangeOverride(Size finalSize)
    {
      _contentPresenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
      return _contentPresenter.RenderSize;
    }

    protected override Visual GetVisualChild(int index)
    {
      return _visuals[index];
    }
  }
}