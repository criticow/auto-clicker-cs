using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Threading;

namespace AutoClicker.ViewModels;

public class RunViewModel(ClickCountViewModel ClickCountModel, ClickOptionsViewModel ClickOptionsModel) : INotifyPropertyChanged
{
  private volatile bool _isRunning = false;
  private Thread? _thread;
  private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

  public bool _isStartButtonEnabled = true;
  public bool IsStartButtonEnabled
  {
    get => _isStartButtonEnabled;
    private set
    {
      _isStartButtonEnabled = value;
      OnPropertyChanged(nameof(IsStartButtonEnabled));
    }
  }

  public bool _isStopButtonEnabled = false;
  public bool IsStopButtonEnabled
  {
    get => _isStopButtonEnabled;
    private set
    {
      _isStopButtonEnabled = value;
      OnPropertyChanged(nameof(IsStopButtonEnabled));
    }
  }

  public void Start()
  {
    _isRunning = true;

    // Use dispatcher to safe UI update via main thread
    _dispatcher.Invoke(() => 
    {
      IsStartButtonEnabled = !_isRunning;
      IsStopButtonEnabled = _isRunning;
    });

    _thread = new Thread(Run)
    {
      IsBackground = true
    };

    _thread.Start();
  }

  public void Stop()
  {
    _isRunning = false;

    // Finishes the thread if it is running
    if(_thread != null && _thread.IsAlive)
    {
      _thread.Join();
    }

    // Use dispatcher to safe UI update via main thread
    _dispatcher.Invoke(() => 
    {
      IsStartButtonEnabled = !_isRunning;
      IsStopButtonEnabled = _isRunning;
    });
  }

  private void Run()
  {
    while(_isRunning)
    {
      Debug.WriteLine("running");
      Thread.Sleep(100);
    }
  }

  private readonly ClickCountViewModel _clickCountModel = ClickCountModel;
  private readonly ClickOptionsViewModel _clickOptionsModel = ClickOptionsModel;

  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged(string propertyName)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}