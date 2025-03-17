
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace AutoClicker.ViewModels;

public class HotkeysViewModel : INotifyPropertyChanged
{
  private IKeyboardMouseEvents? _hook;
  private readonly List<string> mods = [];

  private string _lastKey = "";
  private string _pressKeysMessage = "Press keys to bind";

  private string _startActionKeyOld = "";
  private string _startActionKey = "";
  public string StartActionKey
  {
    get => _startActionKey;
    set
    {
      _startActionKey = value;
      OnPropertyChanged(nameof(StartActionKey));
    }
  }
  
  private string _stopActionKeyOld = "";
  private string _stopActionKey = "";
  public string StopActionKey
  {
    get => _stopActionKey;
    set
    {
      _stopActionKey = value;
      OnPropertyChanged(nameof(StopActionKey));
    }
  }

  private bool _bindingStartAction;
  private bool _bindingStopAction;

  private bool _bindStartButtonEnabled = true;
  public bool BindStartButtonEnabled
  {
    get => _bindStartButtonEnabled;
    set
    {
      _bindStartButtonEnabled = value;
      OnPropertyChanged(nameof(BindStartButtonEnabled));
    }
  }

  private bool _bindStopButtonEnabled = true;
  public bool BindStopButtonEnabled
  {
    get => _bindStopButtonEnabled;
    set
    {
      _bindStopButtonEnabled = value;
      OnPropertyChanged(nameof(BindStopButtonEnabled));
    }
  }

  public HotkeysViewModel()
  {
    _hook = Hook.GlobalEvents();
    _hook.KeyDown += OnKeyDown;
    _hook.KeyUp += OnKeyUp;
  }

  public void BindStartAction()
  {
    _bindingStartAction = true;
    _bindingStopAction = false;
    _startActionKeyOld = _startActionKey;

    StartActionKey = _pressKeysMessage;

    BindStopButtonEnabled = false;
  }

  public void BindStopAction()
  {
    _bindingStopAction = true;
    _bindingStartAction = false;
    _stopActionKeyOld = _stopActionKey;

    StopActionKey = _pressKeysMessage;

    BindStartButtonEnabled = false;
  }

  public void Dispose()
  {
    _hook?.Dispose();
  }

  private void OnKeyDown(object? sender, KeyEventArgs e)
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
    string hotkey = string.Join(" + ", keys);

    if(_bindingStartAction)
    {
      StartActionKey = hotkey;
    }

    if(_bindingStopAction)
    {
      StopActionKey = hotkey;
    }
  }

  private void OnKeyUp(object? sender, KeyEventArgs e)
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
    string hotkey = string.Join(" + ", keys);

    if(_bindingStartAction)
    {
      StartActionKey = hotkey != "" ? hotkey : _pressKeysMessage;
    }

    if(_bindingStopAction)
    {
      StopActionKey = hotkey != "" ? hotkey : _pressKeysMessage;
    }
  }


  public event PropertyChangedEventHandler? PropertyChanged;
  protected virtual void OnPropertyChanged(string propertyName)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}