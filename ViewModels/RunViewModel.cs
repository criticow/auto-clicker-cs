using System.ComponentModel;

namespace AutoClicker.ViewModels;

public class RunViewModel(ClickCountViewModel ClickCountModel, ClickOptionsViewModel ClickOptionsModel) : INotifyPropertyChanged
{
  private bool _isRunning = false;

  public bool _isStartButtonEnabled = true;
  public bool IsStartButtonEnabled
  {
    get => _isStartButtonEnabled;
    set
    {
      _isStartButtonEnabled = value;
      OnPropertyChanged(nameof(IsStartButtonEnabled));
    }
  }

  public bool _isStopButtonEnabled = false;
  public bool IsStopButtonEnabled
  {
    get => _isStopButtonEnabled;
    set
    {
      _isStopButtonEnabled = value;
      OnPropertyChanged(nameof(IsStopButtonEnabled));
    }
  }

  public void Start()
  {
    _isRunning = true;

    IsStartButtonEnabled = !_isRunning;
    IsStopButtonEnabled = _isRunning;
  }

  public void Stop()
  {
    _isRunning = false;

    IsStartButtonEnabled = !_isRunning;
    IsStopButtonEnabled = _isRunning;
  }

  private readonly ClickCountViewModel _clickCountModel = ClickCountModel;
  private readonly ClickOptionsViewModel _clickOptionsModel = ClickOptionsModel;

  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged(string propertyName)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}