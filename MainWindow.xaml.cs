using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Gma.System.MouseKeyHook;

namespace AutoClicker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
  private readonly System.Windows.Controls.TextBox clicksTextBox;
  private readonly System.Windows.Controls.TextBox clicksForTextBox;

  [GeneratedRegex("^[0-9]+$")]
  private static partial Regex NumbersOnly();
  private static readonly Regex _regex = NumbersOnly();
  private readonly IKeyboardMouseEvents _hook;
  private readonly List<string> mods = [];
  private string _lastKey = "";
  private int _clicksInput = 100;
  private int _clickForInput = 10;
  private string _hotkey = "";
  private bool _isClickCountSelected;
  private bool _isClickForCountSelected;

  public bool IsClickCountSelected
  {
    get => _isClickCountSelected;
    set
    {
      _isClickCountSelected = value;
      if(value) IsClickForCountSelected = false;
      OnPropertyChanged(nameof(IsClickCountSelected));

      if(clicksForTextBox != null)
      {
        clicksForTextBox.IsReadOnly = true;
        clicksForTextBox.Background = new SolidColorBrush(Colors.LightGray);
      }

      if(clicksTextBox != null)
      {
        clicksTextBox.IsReadOnly = false;
        clicksTextBox.Background = new SolidColorBrush(Colors.White);
      }
    }
  }

  public bool IsClickForCountSelected
  {
    get => _isClickForCountSelected;
    set
    {
      _isClickForCountSelected = value;
      if(value) IsClickCountSelected = false;

      if(clicksTextBox != null)
      {
        clicksTextBox.IsReadOnly = true;
        clicksTextBox.Background = new SolidColorBrush(Colors.LightGray);
      }

      if(clicksTextBox != null)
      {
        clicksForTextBox.IsReadOnly = false;
        clicksForTextBox.Background = new SolidColorBrush(Colors.White);
      }

      OnPropertyChanged(nameof(IsClickForCountSelected));
    }
  }

  public int ClicksInput
  {
    get => _clicksInput;
    set
    {
      if(value > 0)
      {
        _clicksInput = value;
        OnPropertyChanged(nameof(ClicksInput));
      }
    }
  }

  public int ClickForInput
  {
    get => _clickForInput;
    set
    {
      if(value > 0)
      {
        _clickForInput = value;
        OnPropertyChanged(nameof(ClickForInput));
      }
    }
  }

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
    _hook.KeyUp += OnKeyUp;
    Closed += (s, e) => _hook.Dispose();
    clicksTextBox = (System.Windows.Controls.TextBox)this.FindName("clicksTextBoxName");
    clicksForTextBox = (System.Windows.Controls.TextBox)this.FindName("clicksForTextBoxName");
  }

  private void NumberOnly_PreviewTextInput(object sernder, TextCompositionEventArgs e)
  {
    e.Handled = !_regex.IsMatch(e.Text);
  }

  private void NumberOnly_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
  {
    if(e.Command == ApplicationCommands.Paste)
    {
      string clipboardText = System.Windows.Clipboard.GetText();
      if(!_regex.IsMatch(clipboardText))
      {
        e.Handled = true;
      }
    }
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