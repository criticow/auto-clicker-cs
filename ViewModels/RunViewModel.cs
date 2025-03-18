using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using AutoClicker.Models;
using AutoClicker.Services;

namespace AutoClicker.ViewModels;

public partial class RunViewModel : INotifyPropertyChanged
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

  [DllImport("user32.dll", SetLastError = true)]
  private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, IntPtr dwExtraInfo);

  private readonly Dictionary<string, (uint, uint)> events = new (){
    { "Left Button", (0x02, 0x04) },
    { "Right Button", (0x08, 0x10) },
    { "Middle Button", (0x20, 0x40) },
  };

  private void Click(string button, string action)
  {
    var mouseEvent = events[button];

    switch(action)
    {
      case "Single Click":
        mouse_event(mouseEvent.Item1, 0, 0, 0, IntPtr.Zero);
        Thread.Sleep(1);
        mouse_event(mouseEvent.Item2, 0, 0, 0, IntPtr.Zero);
      break;

      case "Double Click":
        mouse_event(mouseEvent.Item1, 0, 0, 0, IntPtr.Zero);
        Thread.Sleep(1);
        mouse_event(mouseEvent.Item2, 0, 0, 0, IntPtr.Zero);

        Thread.Sleep(1);

        mouse_event(mouseEvent.Item1, 0, 0, 0, IntPtr.Zero);
        Thread.Sleep(1);
        mouse_event(mouseEvent.Item2, 0, 0, 0, IntPtr.Zero);
      break;
    }
  }

  private void Run(Settings settings)
  {
    int clicksPerformed = 0;
    Stopwatch stopwatch = Stopwatch.StartNew();
    bool isCountTimes = settings.ClickCountSelected == "times";

    double targetMilliseconds =
      settings.ClickForUnit == "ms" ? settings.ClickFor : // Milliseconds
      settings.ClickForUnit == "seconds" ? settings.ClickFor * 1000.0f : // Seconds
      settings.ClickForUnit == "minutes" ? settings.ClickFor * 1000.0f * 60.0f : // Minutes
      settings.ClickFor * 1000.0f * 60.0f * 60.0f ; // Hours

    Debug.WriteLine("===========================Started===========================");

    Debug.WriteLine($"Click {(isCountTimes ? $"{settings.ClickTimes} times" : $"for {settings.ClickFor} {settings.ClickForUnit}")}");
    Debug.WriteLine($"Interval selected: {settings.ClickInterval}");
    Debug.WriteLine($"Button selected: {settings.MouseButton}");
    Debug.WriteLine($"Action selected: {settings.ClickAction}");

    while(_isRunning)
    {
      clicksPerformed++;

      Click(settings.MouseButton, settings.ClickAction);

      // Check if clicked the correct amount of times
      if(isCountTimes && settings.ClickTimes == clicksPerformed)
      {
        _isRunning = false;
        break;
      }

      // check if click the correct amount of time
      if(!isCountTimes && stopwatch.ElapsedMilliseconds >= targetMilliseconds)
      {
        _isRunning = false;
        break;
      }

      Thread.Sleep(settings.ClickInterval);
    }

    long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
    double secondsClicked = elapsedMilliseconds / 1000.0;
    double minutesClicked = elapsedMilliseconds / (1000.0 * 60.0);
    double hoursClicked = elapsedMilliseconds / (1000.0 * 60.0 * 60.0);

    Debug.WriteLine("---------------------------Report----------------------------");

    Debug.WriteLine($"Clicks performed: {clicksPerformed}");

    if(!isCountTimes)
    {
      Debug.WriteLine($"Target milliseconds: {targetMilliseconds}");
    }

    Debug.WriteLine($"Clicked for {elapsedMilliseconds} milliseconds");
    Debug.WriteLine($"Clicked for {secondsClicked} seconds");
    Debug.WriteLine($"Clicked for {minutesClicked} minutes");
    Debug.WriteLine($"Clicked for {hoursClicked} hours");

    Debug.WriteLine("===========================Stopped===========================");

    stopwatch.Stop();
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