namespace AutoClicker.ViewModels;

public class MainViewModel
{
  public ClickCountViewModel ClickCountModel { get; set; }
  public HotkeysViewModel HotkeysModel { get; set; }

  public MainViewModel()
  {
    ClickCountModel = new ClickCountViewModel();
    HotkeysModel = new HotkeysViewModel();
  }
}