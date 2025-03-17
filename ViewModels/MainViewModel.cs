using System.IO;
using System.Text.Json;
using AutoClicker.Models;

namespace AutoClicker.ViewModels;

public class MainViewModel
{
  public ClickCountViewModel ClickCountModel { get; set; }
  public HotkeysViewModel HotkeysModel { get; set; }

  public MainViewModel()
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

    ClickCountModel = new ClickCountViewModel(settings);
    HotkeysModel = new HotkeysViewModel(settings);
  }
}