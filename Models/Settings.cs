namespace AutoClicker.Models;

public class Settings
{
  // Click count
  public string ClickCountSelected { get; set; } = "times";
  public int ClickTimes { get; set; } = 100;
  public int ClickFor { get; set; } = 10;
  public string ClickForUnit { get; set; } = "seconds";
  // Hotkeys
  public List<string> StartActionKey { get; set; } = ["Shift", "F1"];
  public List<string> StopActionKey { get; set; } = ["Shift", "F2"];
  // Click options
  public int ClickInterval { get; set; } = 50;
  public string MouseButton { get; set; } = "Left Button";
  public string ClickAction { get; set; } = "Single Click";
}