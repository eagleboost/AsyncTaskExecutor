// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.ComponentModel
{
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Runtime.CompilerServices;

  /// <summary>
  /// NotifyPropertyChangedBase
  /// </summary>
  public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected internal virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    protected internal virtual void OnPropertyChanged(PropertyChangedEventArgs args)
    {
      PropertyChanged?.Invoke(this, args);
    }
  }

  public static class NotifyPropertyChangedBaseExt
  {
    public static bool SetValue<T>(this NotifyPropertyChangedBase obj, ref T field, T value, [CallerMemberName] string propertyName = null)
    {
      if (EqualityComparer<T>.Default.Equals(field, value))
      {
        return false;
      }

      field = value;
      obj.OnPropertyChanged(propertyName);
      return true;
    }
  }
}