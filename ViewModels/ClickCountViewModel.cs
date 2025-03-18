using System.ComponentModel;
using AutoClicker.Models;
using AutoClicker.Services;

namespace AutoClicker.ViewModels;

public class ClickCountViewModel : INotifyPropertyChanged
{
  public ClickCountViewModel(Settings settings)
  {
    ClicksInput = settings.ClickTimes;
    ClickForInput = settings.ClickFor;

    IsClickCountSelected = settings.ClickCountSelected == "times";
    IsClickForCountSelected = settings.ClickCountSelected != "times";

    List<string> supportedUnits = ["ms", "seconds", "minutes", "hours"];

    SelectedClickCount = "seconds";
    if(supportedUnits.Exists(value => value == settings.ClickForUnit))
    {
      SelectedClickCount = settings.ClickForUnit;
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
    var settings = SettingsService.Load();
    settings.ClickTimes = ClicksInput;
    settings.ClickFor = ClickForInput;

    settings.ClickCountSelected = IsClickCountSelected ? "times" : "for";
    settings.ClickForUnit = SelectedClickCount;

    SettingsService.Save(settings);
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