namespace AsyncTaskExecutor.ComponentModel
{
  using System;
  using System.Collections.Generic;
  using System.Threading;

  public sealed class DelegateDisposable : IDisposable
  {
    private List<Action> _actions = new List<Action>();

    public DelegateDisposable()
    {
    }
    
    public DelegateDisposable(Action action)
    {
      Add(action);
    }
    
    public void Add(Action action)
    {
      _actions.Add(action);
    }
    
    public void Dispose()
    {
      var actions = Interlocked.Exchange(ref _actions, null);
      if (actions != null)
      {
        for (var i = actions.Count - 1; i >= 0; i--)
        {
          var action = actions[i];
          action();
        }
      }
    }
  }
}