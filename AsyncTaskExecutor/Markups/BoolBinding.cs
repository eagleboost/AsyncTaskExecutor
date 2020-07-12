// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Markups
{
  using System;
  using System.Globalization;
  using System.Windows;
  using System.Windows.Data;

  /// <summary>
  /// BoolBinding - Compare binding result against Value property and produce boolean result
  /// </summary>
  public class BoolBinding : BindingDecoratorBase
  {
    public object Value { get; set; }
    
    public BoolBinding()
    {
    }
    
    public BoolBinding(string path)
    {
      Path = new PropertyPath(path);
    }
    
    public override object ProvideValue(IServiceProvider provider)
    {
      var converter = Binding.Converter;
      var newConverter = new ValueEqualityConverter {Converter = converter, Value = Value};
      Binding.Converter = newConverter;
      
      return Binding.ProvideValue(provider);
    }
    
    private class ValueEqualityConverter : IValueConverter
    {
      public IValueConverter Converter { get; set; }
      
      public object Value { get; set; }

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        var v = value;
        var cvt = Converter;
        if (cvt != null)
        {
          v = cvt.Convert(value, targetType, parameter, culture);
        }

        var result= Equals(v, Value);
        return result;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }
    }
  }
}