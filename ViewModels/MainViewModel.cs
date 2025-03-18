namespace AutoClicker.ViewModels;

public class MainViewModel
{
  public ClickCountViewModel ClickCountModel { get; set; }
  public HotkeysViewModel HotkeysModel { get; set; }
  public ClickOptionsViewModel ClickOptionsModel { get; set; }
  public RunViewModel RunModel { get; set; }

  public MainViewModel()
  {
    ClickCountModel = new ClickCountViewModel();
    ClickOptionsModel = new ClickOptionsViewModel();
    RunModel = new RunViewModel();
    HotkeysModel = new HotkeysViewModel(RunModel);
  }
}