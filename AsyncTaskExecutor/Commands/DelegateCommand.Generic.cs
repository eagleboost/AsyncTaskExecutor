// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Commands
{
  using System;
  using System.Windows.Input;

  /// <summary>
  /// DelegateCommand
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class DelegateCommand<T> : ICommand
  {
    private readonly Action<T> _execute;
    private readonly Func<T, bool> _canExecute;
    
    public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
    {
      _execute = execute;
      _canExecute = canExecute;
    }
    
    public bool CanExecute(object parameter)
    {
      return _canExecute?.Invoke((T) parameter) ?? true;
    }

    public void Execute(object parameter)
    {
      _execute((T)parameter);
    }

    public event EventHandler CanExecuteChanged;

    protected virtual void OnCanExecuteChanged()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}