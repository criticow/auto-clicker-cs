
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using AutoClicker.Models;

namespace AutoClicker.Services;

public class SettingsService : INotifyPropertyChanged
{
  private static readonly Lazy<SettingsService> _instance = new(() => new SettingsService());
  public static SettingsService Instance => _instance.Value;

  private SettingsService()
  {
    LoadSettings();
  }

  private static Settings _settings = new();

  public Settings Settings
  {
    get => _settings;
    set
    {
      _settings = value;
      OnPropertyChanged(nameof(Settings));
    }
  }

  private static void LoadSettings()
  {
    if(!File.Exists("settings.json"))
    {
      var json = JsonSerializer.Serialize(_settings);
      File.WriteAllText("settings.json", json);
    }
    else
    {
      string json = File.ReadAllText("settings.json");

      var loaded = JsonSerializer.Deserialize<Settings>(json);

      if(loaded != null)
      {
        _settings = loaded;
      }
    }
  }

  public static void Save()
  {
    var json = JsonSerializer.Serialize(_settings);
    File.WriteAllText("settings.json", json);

    Debug.WriteLine("Saving settings...");
  }

  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged(string propertyName)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}