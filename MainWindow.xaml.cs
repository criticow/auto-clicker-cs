using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

using AutoClicker.ViewModels;

namespace AutoClicker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  private readonly MainViewModel _mainViewModel;

  [GeneratedRegex("^[0-9]+$")]
  private static partial Regex NumbersOnly();
  private static readonly Regex _regex = NumbersOnly();

  public MainWindow()
  {
    ((IComponentConnector)this).InitializeComponent();
    _mainViewModel = new MainViewModel();
    DataContext = _mainViewModel;
    Closed += (s, e) => _mainViewModel.HotkeysModel.Dispose();
  }

  private void LostFocus_SaveSettings(object sender, RoutedEventArgs e)
  {
    _mainViewModel.ClickCountModel.SaveSettings();
  }

  private void BindStartAction_Click(object sender, RoutedEventArgs e)
  {
    _mainViewModel.HotkeysModel.BindStartAction();
  }

  private void BindStopAction_Click(object sender, RoutedEventArgs e)
  {
    _mainViewModel.HotkeysModel.BindStopAction();
  }

  private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
  {
    TextBox? textBox = sender as TextBox;

    if(!int.TryParse(e.Text, out _))
    {
      e.Handled = true;
      return;
    }

    if(textBox?.Text == "0")
    {
      textBox.Text = e.Text; // Override the 0
      textBox.CaretIndex = textBox.Text.Length; // Set caret to the end of the text

      e.Handled = true;
    }
  }

  private void NumberOnly_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if(sender is not TextBox textBox)
    {
      return;
    }

    // Handle deleting when there is only one char
    if(
      (e.Key == Key.Back || e.Key == Key.Delete) && 
      (textBox.Text.Length == 1 || textBox.SelectedText.Length == textBox.Text.Length)
    )
    {
      textBox.Text = "0";
      textBox.CaretIndex = textBox.Text.Length; // Set caret to the end of the text
      e.Handled = true;
    }

    // Handle pasting
    if(Keyboard.Modifiers == ModifierKeys.Control)
    {
      if(e.Key == Key.V)
      {
        if(!int.TryParse(Clipboard.GetText(), out _))
        {
          e.Handled = true;
        }

        if(textBox.Text == "0")
        {
          textBox.Text = Clipboard.GetText();
          textBox.CaretIndex = textBox.Text.Length; // Set caret to the end of the text
          e.Handled = true;
        }
      }

      if(e.Key == Key.X && textBox.SelectedText.Length == textBox.Text.Length)
      {
        Clipboard.SetText(textBox.SelectedText);
        textBox.Text = "0";
        textBox.CaretIndex = textBox.Text.Length; // Set caret to the end of the text
        e.Handled = true;
      }
    }
  }
}
