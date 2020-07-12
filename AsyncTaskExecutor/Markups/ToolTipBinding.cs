// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Markups
{
  using System;
  using System.Globalization;
  using System.Windows;
  using System.Windows.Data;

  /// <summary>
  /// ToolTipBinding
  /// </summary>
  public sealed class ToolTipBinding : BindingDecoratorBase
  {
    public ToolTipBinding()
    {
    }
    
    public ToolTipBinding(string path)
    {
      Path = new PropertyPath(path);
    }
    
    public override object ProvideValue(IServiceProvider provider)
    {
      var converter = Binding.Converter;
      var newConverter = new ToolTipConverter {Converter = converter, Format = StringFormat};
      Binding.Converter = newConverter;
      
      return Binding.ProvideValue(provider);
    }
    
    private class ToolTipConverter : IValueConverter
    {
      public IValueConverter Converter { get; set; }
      
      public string Format { get; set; }

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        var v = value;
        var cvt = Converter;
        if (cvt != null)
        {
          v = cvt.Convert(value, targetType, parameter, culture);
        }
        
        var format = Format;
        if (format == null)
        {
          throw new ArgumentException("parameter is not a string", nameof(parameter));
        }
        
        return string.Format(format, v);
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }
    }
  }
}