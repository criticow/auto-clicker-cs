
using System.IO;
using System.Text.Json;
using AutoClicker.Models;

namespace AutoClicker.Services;

public class SettingsService
{
  static public Settings Load()
  {
    Settings settings = new();

    if(!File.Exists("settings.json"))
    {
      var json = JsonSerializer.Serialize(settings);
      File.WriteAllText("settings.json", json);
    }
    else
    {
      string json = File.ReadAllText("settings.json");

      var loaded = JsonSerializer.Deserialize<Settings>(json);

      if(loaded != null)
      {
        settings = loaded;
      }
    }

    return settings;
  }

  static public void Save(Settings settings)
  {
    var json = JsonSerializer.Serialize(settings);
    File.WriteAllText("settings.json", json);
  }
}