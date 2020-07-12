// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.ComponentModel
{
  using System;
  using System.Threading;

  /// <summary>
  /// RefUtils
  /// </summary>
  public static class RefUtils
  {
    public static void Dispose<T>(ref T disposable) where T : class, IDisposable
    {
      var d = Interlocked.Exchange(ref disposable, null);
      if (d != null)
      {
        d.Dispose();
      }
    }
    
    public static void Reset<T>(ref T disposable) where T : class, IDisposable, new()
    {
      var d = Interlocked.Exchange(ref disposable, new T());
      if (d != null)
      {
        d.Dispose();
      }
    }
    
    public static CancellationToken Reset(ref CancellationTokenSource cts)
    {
      var newCts = new CancellationTokenSource();
      var c = Interlocked.Exchange(ref cts, newCts);
      if (c != null)
      {
        c.Cancel();
        c.Dispose();
      }

      return newCts.Token;
    }
    
    public static void Cancel(ref CancellationTokenSource cts)
    {
      var c = Interlocked.Exchange(ref cts, null);
      if (c != null)
      {
        c.Cancel();
        c.Dispose();
      }
    }
  }
}