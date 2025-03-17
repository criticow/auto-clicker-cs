using System.IO;
using System.Text.Json;
using AutoClicker.Models;
using AutoClicker.Services;

namespace AutoClicker.ViewModels;

public class MainViewModel
{
  public ClickCountViewModel ClickCountModel { get; set; }
  public HotkeysViewModel HotkeysModel { get; set; }

  public MainViewModel()
  {
    var settings = SettingsService.Load();

    ClickCountModel = new ClickCountViewModel(settings);
    HotkeysModel = new HotkeysViewModel(settings);
  }
}