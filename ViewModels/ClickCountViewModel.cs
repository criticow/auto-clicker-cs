using System.ComponentModel;
using AutoClicker.Models;
using AutoClicker.Services;

namespace AutoClicker.ViewModels;

public class ClickCountViewModel : INotifyPropertyChanged
{
  private readonly SettingsService _settingsService = SettingsService.Instance;
  private Settings Settings => _settingsService.Settings;

  public ClickCountViewModel()
  {
    ClicksInput = Settings.ClickTimes;
    ClickForInput = Settings.ClickFor;

    IsClickCountSelected = Settings.ClickCountSelected == "times";
    IsClickForCountSelected = Settings.ClickCountSelected != "times";

    List<string> supportedUnits = ["ms", "seconds", "minutes", "hours"];

    SelectedClickCount = "seconds";
    if(supportedUnits.Exists(value => value == Settings.ClickForUnit))
    {
      SelectedClickCount = Settings.ClickForUnit;
    }
  }

  private int _clicksInput = 100;
  public int ClicksInput
  {
    get => _clicksInput;
    set
    {
      if(value != _clicksInput)
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
      if(value != _clickForInput)
      {
        _clickForInput = value;
        OnPropertyChanged(nameof(ClickForInput));
      }
    }
  }

  private bool _isClickCountSelected = false;
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

  private bool _isClickForCountSelected = true;
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

  public void SaveSettings()
  {
    Settings.ClickTimes = ClicksInput;
    Settings.ClickFor = ClickForInput;

    Settings.ClickCountSelected = IsClickCountSelected ? "times" : "for";
    Settings.ClickForUnit = SelectedClickCount;

    SettingsService.Save();
  }

  public event PropertyChangedEventHandler? PropertyChanged;
  public virtual void OnPropertyChanged(string propertyName)
  {
    if(PropertyChanged != null)
    {
      PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
      SaveSettings();
    }
  }
}