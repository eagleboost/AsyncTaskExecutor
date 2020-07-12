// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Commands
{
  using System;
  using System.Windows.Input;

  /// <summary>
  /// DelegateCommand
  /// </summary>
  public class DelegateCommand : ICommand
  {
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;
    
    public DelegateCommand(Action execute, Func<bool> canExecute)
    {
      _execute = execute;
      _canExecute = canExecute;
    }
    
    public bool CanExecute(object parameter)
    {
      return _canExecute?.Invoke() ?? true;
    }

    public void Execute(object parameter)
    {
      _execute();
    }

    public event EventHandler CanExecuteChanged;

    protected virtual void OnCanExecuteChanged()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}