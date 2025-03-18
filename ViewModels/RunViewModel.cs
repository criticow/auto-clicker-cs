using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Threading;
using AutoClicker.Models;
using AutoClicker.Services;

namespace AutoClicker.ViewModels;

public class RunViewModel : INotifyPropertyChanged
{
  private volatile bool _isRunning = false;
  private Thread? _thread;
  private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

  private readonly SettingsService _settingsService = SettingsService.Instance;
  private Settings Settings => _settingsService.Settings;

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

    _thread = new Thread(() => Run(Settings))
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

  private void Run(Settings settings)
  {
    int clicksPerformed = 0;

    while(_isRunning)
    {
      clicksPerformed++;

      Debug.WriteLine("Clicked");

      if(settings.ClickCountSelected == "times" && settings.ClickTimes == clicksPerformed)
      {
        _isRunning = false;
        break;
      }

      Thread.Sleep(settings.ClickInterval);
    }

    _dispatcher.BeginInvoke(() => {
      IsStartButtonEnabled = true;
      IsStopButtonEnabled = false;
    });
  }

  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged(string propertyName)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}