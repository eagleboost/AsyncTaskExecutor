// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.Tasks
{
  using System.Threading;
  using System.Threading.Tasks;

  /// <summary>
  /// PauseTokenSource - https://blogs.msdn.microsoft.com/pfxteam/2013/01/13/cooperatively-pausing-async-methods/
  /// </summary>
  public class PauseTokenSource
  {
    #region Declarations
    private volatile TaskCompletionSource<bool> _pausedTcs;
    #endregion Declarations

    #region Public Properties
    public bool IsPaused
    {
      get { return _pausedTcs != null; }
      set
      {
        if (value)
        {
          Interlocked.CompareExchange(ref _pausedTcs, new TaskCompletionSource<bool>(), null);
        }
        else
        {
          while (true)
          {
            var tcs = _pausedTcs;
            if (tcs == null)
            {
              return;
            }

            if (Interlocked.CompareExchange(ref _pausedTcs, null, tcs) == tcs)
            {
              tcs.SetResult(true);
              break;
            }
          }
        }
      }
    }

    public PauseToken Token
    {
      get { return new PauseToken(this); }
    }
    #endregion Public Properties

    #region Internals
    internal static readonly Task CompletedTask = Task.FromResult(true);

    internal Task WaitWhilePausedAsync()
    {
      var cts = _pausedTcs;
      return cts != null ? cts.Task : CompletedTask;
    }
    #endregion Internals
  }

  /// <summary>
  /// PauseToken 
  /// </summary>
  public struct PauseToken
  {
    private readonly PauseTokenSource _pts;

    internal PauseToken(PauseTokenSource source)
    {
      _pts = source;
    }

    public bool IsPaused
    {
      get { return _pts != null && _pts.IsPaused; }
    }

    public Task WaitWhilePausedAsync()
    {
      return IsPaused ?
        _pts.WaitWhilePausedAsync() :
        PauseTokenSource.CompletedTask;
    }
  }
}