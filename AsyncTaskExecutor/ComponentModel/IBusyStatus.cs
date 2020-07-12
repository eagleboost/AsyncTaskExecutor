// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com

namespace AsyncTaskExecutor.ComponentModel
{
  public interface IBusyStatus
  {
    bool IsBusy { get; }
    
    string BusyStatus { get; }
  }
}