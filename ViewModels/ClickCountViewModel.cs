using System.ComponentModel;

namespace AutoClicker.ViewModels;

public class ClickCountViewModel : INotifyPropertyChanged
{
  private int _clicksInput = 100;
  public int ClicksInput
  {
    get => _clicksInput;
    set
    {
      if(value > 0)
      {
        _clicksInput = value;
        OnPropertyChanged(nameof(ClicksInput));
      }
    }
  }

  private int _clickForInput = 10;
  public int ClickForInput
  {
    get => _clickForInput;
    set
    {
      if(value > 0)
      {
        _clickForInput = value;
        OnPropertyChanged(nameof(ClickForInput));
      }
    }
  }

  private bool _isClickCountSelected;
  public bool IsClickCountSelected
  {
    get => _isClickCountSelected;
    set
    {
      _isClickCountSelected = value;
      if(value) IsClickForCountSelected = false;
      OnPropertyChanged(nameof(IsClickCountSelected));
    }
  }

  private bool _isClickForCountSelected;
  public bool IsClickForCountSelected
  {
    get => _isClickForCountSelected;
    set
    {
      _isClickForCountSelected = value;
      if(value) IsClickCountSelected = false;
      OnPropertyChanged(nameof(IsClickForCountSelected));
    }
  }

  private string _selectedClickCount = "minutes";
  public string SelectedClickCount
  {
    get => _selectedClickCount;
    set
    {
      _selectedClickCount = value;
      OnPropertyChanged(nameof(SelectedClickCount));
    }
  }

  public event PropertyChangedEventHandler? PropertyChanged;
  public virtual void OnPropertyChanged(string propertyName)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}