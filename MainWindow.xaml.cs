using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using Gma.System.MouseKeyHook;

using AutoClicker.ViewModels;
using System.Text.Json;
using System.IO;
using System.Diagnostics;
using AutoClicker.Models;

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

  private void BindStartAction_Click(object sender, RoutedEventArgs e)
  {
    _mainViewModel.HotkeysModel.BindStartAction();
  }

  private void BindStopAction_Click(object sender, RoutedEventArgs e)
  {
    _mainViewModel.HotkeysModel.BindStopAction();
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
}