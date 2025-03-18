using System.ComponentModel;

namespace AutoClicker.ViewModels;

public class RunViewModel : INotifyPropertyChanged
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

  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged(string propertyName)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}