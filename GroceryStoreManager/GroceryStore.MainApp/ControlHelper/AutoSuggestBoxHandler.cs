using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using GroceryStore.Domain.Model;
using GroceryStore.MainApp.Helpers;
using GroceryStore.MainApp.Models.PreModel;
using GroceryStore.MainApp.Strategies;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace GroceryStore.MainApp.ControlHelper;

enum LostFocusSource
{
    QuerySubmit,
    None,
}

public class AutoSuggestBoxHandler<T>
{
    private readonly ISearchStrategy<T> _searchStrategy;
    private readonly Action<T?> _onChoosen;
    private readonly Func<T, string> _toString;
    private readonly Action? _onNotValid;

    private LostFocusSource lostFocusSource = LostFocusSource.None;

    public AutoSuggestBoxHandler(ISearchStrategy<T> searchStrategy, Action<T?> onChoosen, Func<T, string> toString, Action? onNotValid)
    {
        _searchStrategy = searchStrategy;
        _onChoosen = onChoosen;
        _toString = toString;
        _onNotValid = onNotValid;
    }

    public void Assign(AutoSuggestBox target)
    {
        target.TextChanged += AutoSuggestBox_TextChanged;
        target.QuerySubmitted += AutoSuggestBox_QuerySubmitted;
        target.SuggestionChosen += AutoSuggestBox_SuggestionChosen;
        target.KeyUp += OnKeyUp;
        target.LostFocus += OnLostFocus;

        target.Unloaded += (s, re) =>
        {
            target.TextChanged -= AutoSuggestBox_TextChanged;
            target.QuerySubmitted -= AutoSuggestBox_QuerySubmitted;
            target.SuggestionChosen -= AutoSuggestBox_SuggestionChosen;
            target.KeyUp -= OnKeyUp;
            target.LostFocus -= OnLostFocus;
        };
    }

    // Route: TextChanged => SuggestionChosen => QuerySubmitted

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        switch (args.Reason)
        {
            case AutoSuggestionBoxTextChangeReason.UserInput:
                var suggestions = _searchStrategy.Search(sender.Text.TextNormalize());
                if (suggestions.Count > 0)
                {
                    sender.ItemsSource = suggestions;
                }
                else
                {
                    sender.ItemsSource = new string[] { "No results found" };
                }
                Debug.Write("UI in");
                break;
            case AutoSuggestionBoxTextChangeReason.SuggestionChosen:
                break;
            case AutoSuggestionBoxTextChangeReason.ProgrammaticChange:
                break;
        }
    }

    private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        //Don't autocomplete the TextBox when we are showing "no results" (type string)
        if (args.SelectedItem is T)
        {
            sender.Text = args.SelectedItem.ToString();
            // All of these works (when args.SelectedItem.ToString return the same string (so it knows it the same item))
            // Need to override ToString
            // We use Decorator design pattern here
            //sender.Text = args.SelectedItem.ToString();
            //sender.Text = ((Customer)args.SelectedItem).ToString();
            //var str = new StringBuilder();
            //str.Append(args.SelectedItem.ToString());
            //sender.Text = str.ToString();
            //var displayText = _toString.Invoke(item);
            //sender.Text = displayText;
        }
        else
        {
            //sender.Text = string.Empty;
        }
    }

    private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        lostFocusSource = LostFocusSource.QuerySubmit;
        if (args.ChosenSuggestion != null)
        {
            //User selected an 'correct' item, take an action
            if (args.ChosenSuggestion is T item)
            {
                _onChoosen.Invoke((T)args.ChosenSuggestion);
                var displayText = _toString.Invoke((T)args.ChosenSuggestion);
                sender.Text = displayText;
            }
            else
            {
                _onChoosen.Invoke(default);
                sender.Text = string.Empty;
            }
        }
        //User submitted text (could be empty) (and not sure if we can find matched record)
        else
        {
            EvalueateASBText(sender, args.QueryText);
        }
    }

    private void OnLostFocus(object sender, RoutedEventArgs e)
    {
        if (lostFocusSource == LostFocusSource.QuerySubmit)
        {
            lostFocusSource = LostFocusSource.None;
            return;
        }
        var asb = (sender as AutoSuggestBox);
        if (asb is not null)
        {
            EvalueateASBText(asb, asb.Text);
        }
    }

    private void OnKeyUp(object sender, KeyRoutedEventArgs e)
    {
        var asb = (AutoSuggestBox)sender;
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            MakeLoseFocus(sender);
        }
        else if (e.Key == Windows.System.VirtualKey.Escape)
        {
            asb.Text = string.Empty;
            MakeLoseFocus(sender);
        }
    }

    private void EvalueateASBText(AutoSuggestBox sender, string text)
    {
        string? displayText;
        if (!string.IsNullOrEmpty(text))
        {
            var suggestions = _searchStrategy.Search(text);
            if (suggestions.Count > 0)
            {
                _onChoosen.Invoke(suggestions.First());
                displayText = _toString.Invoke(suggestions.First());
            }
            else
            {
                _onNotValid?.Invoke();
                _onChoosen.Invoke(default);
                displayText = string.Empty;
            }
        }
        else
        {
            _onNotValid?.Invoke();
            _onChoosen.Invoke(default);
            displayText = string.Empty;
        }
        sender.Text = displayText;
    }

    /// <summary>
    /// Makes virtual keyboard disappear. It's ugly, but works
    /// ref: https://stackoverflow.com/questions/24251356/how-to-make-textbox-lose-its-focus
    /// </summary>
    /// <param name="sender"></param>
    private void MakeLoseFocus(object sender)
    {
        var control = sender as Control;
        if (control == null) { return; }
        var isTabStop = control.IsTabStop;
        control.IsTabStop = false;
        control.IsEnabled = false;
        control.IsEnabled = true;
        control.IsTabStop = isTabStop;
    }
}