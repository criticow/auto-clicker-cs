using System.ComponentModel;
using AutoClicker.Models;
using AutoClicker.Services;

namespace AutoClicker.ViewModels;

public class ClickOptionsViewModel : INotifyPropertyChanged
{
  private readonly SettingsService _settingsService = SettingsService.Instance;

  private Settings Settings => _settingsService.Settings;

  private int _clickInterval;
  public int ClickInterval
  {
    get => _clickInterval;
    set
    {
      if(value != _clickInterval)
      {
        _clickInterval = value;
        OnPropertyChanged(nameof(ClickInterval));
      }
    }
  }

  private string _mouseButton = "";
  public string MouseButton
  {
    get => _mouseButton;
    set
    {
      _mouseButton = value;
      OnPropertyChanged(nameof(MouseButton));
    }
  }

  private string _clickAction = "";
  public string ClickAction
  {
    get => _clickAction;
    set
    {
      _clickAction = value;
      OnPropertyChanged(nameof(ClickAction));
    }
  }

  public ClickOptionsViewModel()
  {
    ClickInterval = _settingsService.Settings.ClickInterval;

    List<string> supportedButtons = ["Left Button", "Right Button", "Middle Button"];

    MouseButton = "Left Button";
    if(supportedButtons.Exists(button => button == Settings.MouseButton))
    {
      MouseButton = Settings.MouseButton;
    }

    List<string> supportedActions = ["Single Click", "Double Click", "Mouse Down", "Mouse Up"];

    ClickAction = "Single Click";
    if(supportedActions.Exists(action => action == Settings.ClickAction))
    {
      ClickAction = Settings.ClickAction;
    }
  }

  private void SaveSettings()
  {
    Settings.ClickInterval = ClickInterval;
    Settings.MouseButton = MouseButton;
    Settings.ClickAction = ClickAction;

    SettingsService.Save();
  }

  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged(string propertyName)
  {
    if(PropertyChanged != null)
    {
      PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
      SaveSettings();
    }
  }
}