
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace AutoClicker.ViewModels;

public class HotkeysViewModel : INotifyPropertyChanged
{
  private readonly IKeyboardMouseEvents _hook;
  private readonly List<string> _modsPressed = [];

  private readonly List<Keys> _modKeys = [
    Keys.LShiftKey, Keys.RShiftKey,     // Shift keys
    Keys.LMenu, Keys.RMenu,             // Alt keys
    Keys.LControlKey, Keys.RControlKey  // Control keys
  ];


  private string _lastKey = "";
  private readonly string _pressKeysMessage = "Press keys to bind";
  static private readonly string _noKeysMessage = "No key bound";

  private string _startActionKey = _noKeysMessage;
  public string StartActionKey
  {
    get => _startActionKey;
    set
    {
      _startActionKey = value;
      OnPropertyChanged(nameof(StartActionKey));
    }
  }
  
  private string _stopActionKey = _noKeysMessage;
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

    StartActionKey = _pressKeysMessage;

    BindStopButtonEnabled = false;
  }

  public void BindStopAction()
  {
    _bindingStopAction = true;
    _bindingStartAction = false;

    StopActionKey = _pressKeysMessage;

    BindStartButtonEnabled = false;
  }

  public void Dispose()
  {
    _hook?.Dispose();
  }

  private void OnKeyDown(object? sender, KeyEventArgs e)
  {
    if(Array.Exists([.._modKeys.Take(2)], key => key == e.KeyCode))
    {
      if(!_modsPressed.Exists(key => key == "Shift"))
      {
        _modsPressed.Add("Shift");
      }
    }
    else if(Array.Exists([.._modKeys.Skip(2).Take(2)], key => key == e.KeyCode))
    {
      if(!_modsPressed.Exists(key => key == "Alt"))
      {
        _modsPressed.Add("Alt");
      }
    }
    else if(Array.Exists([.._modKeys.Skip(4)], key => key == e.KeyCode))
    {
      if(!_modsPressed.Exists(key => key == "Ctrl"))
      {
        _modsPressed.Add("Ctrl");
      }
    }
    else
    {
      _lastKey = e.KeyCode.ToString();
    }

    if(e.KeyCode == Keys.Escape)
    {
      if(_bindingStartAction)
      {
        StartActionKey = _noKeysMessage;
      }
      else
      {
        StopActionKey = _noKeysMessage;
      }

      _bindingStartAction = false;
      _bindingStopAction = false;
      BindStartButtonEnabled = true;
      BindStopButtonEnabled = true;

      return;
    }

    List<string> keys = _lastKey != "" ? [.._modsPressed, _lastKey] : [.. _modsPressed];
    string hotkey = string.Join(" + ", keys);

    if(_bindingStartAction)
    {
      StartActionKey = hotkey;
    }

    if(_bindingStopAction)
    {
      StopActionKey = hotkey;
    }

    if(!_modKeys.Exists(key => key == e.KeyCode))
    {
      _bindingStartAction = false;
      _bindingStopAction = false;
      BindStartButtonEnabled = true;
      BindStopButtonEnabled = true;
    }
  }

  private void OnKeyUp(object? sender, KeyEventArgs e)
  {
    if(Array.Exists([.._modKeys.Take(2)], key => key == e.KeyCode))
    {
      if(_modsPressed.Exists(key => key == "Shift"))
      {
        _modsPressed.Remove("Shift");
      }
    }
    else if(Array.Exists([.._modKeys.Skip(2).Take(2)], key => key == e.KeyCode))
    {
      if(_modsPressed.Exists(key => key == "Alt"))
      {
        _modsPressed.Remove("Alt");
      }
    }
    else if(Array.Exists([.._modKeys.Skip(4)], key => key == e.KeyCode))
    {
      if(_modsPressed.Exists(key => key == "Ctrl"))
      {
        _modsPressed.Remove("Ctrl");
      }
    }

    if(e.KeyCode.ToString() == _lastKey)
    {
      _lastKey = "";
    }

    List<string> keys = _lastKey != "" ? [.._modsPressed, _lastKey] : [.. _modsPressed];
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