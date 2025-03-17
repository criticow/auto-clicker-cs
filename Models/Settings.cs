namespace AutoClicker.Models;

public class Settings
{
  public string ClickCountSelected { get; set; } = "times";
  public int ClickTimes { get; set; } = 100;
  public int ClickFor { get; set; } = 10;
  public string ClickForUnit { get; set; } = "seconds";
  public List<string> StartActionKey { get; set; } = ["Shift", "F1"];
  public List<string> StopActionKey { get; set; } = ["Shift", "F2"];
}