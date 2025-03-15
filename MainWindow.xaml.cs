using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using Gma.System.MouseKeyHook;

namespace AutoClicker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
  private readonly IKeyboardMouseEvents _hook;

  private List<string> mods = [];

  private string _lastKey = "";

  private string _userInput = "Type something here...";
  public string UserInput
  {
    get => _userInput;
    set
    {
      if(_userInput != value)
      {
        _userInput = value;
        OnPropertyChanged(nameof(UserInput));
      }
    }
  }

  private string _hotkey = "";
  public string Hotkey
  {
    get => _hotkey;
    set
    {
      _hotkey = value;
      OnPropertyChanged(nameof(Hotkey));
    }
  }

  public MainWindow()
  {
    ((IComponentConnector)this).InitializeComponent();
    DataContext = this;
    _hook = Hook.GlobalEvents();
    _hook.KeyDown += OnKeyDown;
    _hook.KeyPress += OnKeyPress;
    _hook.KeyUp += OnKeyUp;
    Closed += (s, e) => _hook.Dispose();
  }

    private void OnKeyPress(object? sender, System.Windows.Forms.KeyPressEventArgs e)
  {
    // Debug.WriteLine($"Key pressed: {e.KeyChar}");
  }

  private void OnKeyDown(object? sender, System.Windows.Forms.KeyEventArgs e)
  {
    if(Array.Exists([Keys.LShiftKey, Keys.RShiftKey], key => key == e.KeyCode))
    {
      if(!mods.Exists(key => key == "Shift"))
      {
        mods.Add("Shift");
      }
    }
    else if(Array.Exists([Keys.LMenu, Keys.RMenu], key => key == e.KeyCode))
    {
      if(!mods.Exists(key => key == "Alt"))
      {
        mods.Add("Alt");
      }
    }
    else if(Array.Exists([Keys.LControlKey, Keys.RControlKey], key => key == e.KeyCode))
    {
      if(!mods.Exists(key => key == "Ctrl"))
      {
        mods.Add("Ctrl");
      }
    }
    else
    {
      _lastKey = e.KeyCode.ToString();
    }

    List<string> keys = _lastKey != "" ? [..mods, _lastKey] : [.. mods];
    Hotkey = string.Join(" + ", keys);
  }

  private void OnKeyUp(object? sender, System.Windows.Forms.KeyEventArgs e)
  {
    if(Array.Exists([Keys.LShiftKey, Keys.RShiftKey], key => key == e.KeyCode))
    {
      if(mods.Exists(key => key == "Shift"))
      {
        mods.Remove("Shift");
      }
    }
    else if(Array.Exists([Keys.LMenu, Keys.RMenu], key => key == e.KeyCode))
    {
      if(mods.Exists(key => key == "Alt"))
      {
        mods.Remove("Alt");
      }
    }
    else if(Array.Exists([Keys.LControlKey, Keys.RControlKey], key => key == e.KeyCode))
    {
      if(mods.Exists(key => key == "Ctrl"))
      {
        mods.Remove("Ctrl");
      }
    }

    if(e.KeyCode.ToString() == _lastKey)
    {
      _lastKey = "";
    }

    List<string> keys = _lastKey != "" ? [..mods, _lastKey] : [.. mods];
    Hotkey = string.Join(" + ", keys);
  }

  public event PropertyChangedEventHandler? PropertyChanged;
  protected virtual void OnPropertyChanged(string propertyName)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}