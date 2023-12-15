using GroceryStore.Domain.Model;
using GroceryStore.MainApp.Helpers;
using GroceryStore.MainApp.Strategies;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace GroceryStore.MainApp.ControlHelper;

enum ASBFlag
{
    None,
    SuggestionChosen,
    QuerySubmitted,
    TextChanged,
    LostFocus,
}

public class AutoSuggestBoxHandler<T>
{
    private readonly ISearchStrategy<T> _searchStrategy;
    private readonly Action<T?> _onChoosen;
    private readonly Func<T, string> _toDisplayString;
    private readonly Action? _onNotValid;

    public AutoSuggestBoxHandler(ISearchStrategy<T> searchStrategy, Action<T?> onChoosen, Func<T, string> toDisplayString, Action? onNotValid)
    {
        _searchStrategy = searchStrategy;
        _onChoosen = onChoosen;
        _toDisplayString = toDisplayString;
        _onNotValid = onNotValid;
    }

    public void Assign(AutoSuggestBox target, T? initValue)
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

        // Some properties need to have
        target.UpdateTextOnSelect = true;
        // Only get the initial text
        target.Loaded += (s, e) =>
        {
            if (s is AutoSuggestBox asb && initValue != null)
            {
                asb.Text = _toDisplayString.Invoke(initValue);
            }
        };
    }

    private void OnKeyUp(object sender, KeyRoutedEventArgs e)
    {
        var asb = (AutoSuggestBox)sender;
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            //MakeLoseFocus(sender);
        }
        else if (e.Key == Windows.System.VirtualKey.Escape)
        {
            asb.Text = string.Empty;
            MakeLoseFocus(sender);
        }
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

    // =================== PROCESS ===================

    // Thread controller
    private ASBFlag _oneTimeTextChanged = ASBFlag.None; // sender.Text = ...
    private ASBFlag _oneTimeLostFocus = ASBFlag.None;   // SC, QS will gonna trigger LostFocus

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            OnTextChanged(sender);
        }
        // Called when text changed during QuerySubmitted
        else if (args.Reason == AutoSuggestionBoxTextChangeReason.ProgrammaticChange)
        {
        }
        // Called when text changed during SuggestionChanged, and match the ToString() of the object
        else if (args.Reason == AutoSuggestionBoxTextChangeReason.SuggestionChosen)
        {
        }
    }

    private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        _oneTimeLostFocus = ASBFlag.SuggestionChosen;
        _oneTimeTextChanged = ASBFlag.SuggestionChosen;
        if (args.SelectedItem is T item)
        {
            var displayText = _toDisplayString.Invoke(item);
            sender.Text = displayText;
        }
        else
        {
            // The selected item is not T, most likely the "Not found" string
            sender.Text = string.Empty;
        }
    }

    private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        _oneTimeLostFocus = ASBFlag.QuerySubmitted;
        _oneTimeTextChanged = ASBFlag.QuerySubmitted;
        if (args.ChosenSuggestion != null)
        {
            HandleSuggestSubmit(sender, args.ChosenSuggestion);
        }
        //User submitted text (could be empty) (and not sure if we can find matched record)
        else
        {
            HandleTextSubmit(sender, args.QueryText);
        }
    }

    private void OnLostFocus(object sender, RoutedEventArgs e)
    {
        if (_oneTimeLostFocus != ASBFlag.None)
        {
            _oneTimeLostFocus = ASBFlag.None;
            return;
        }
        var asb = (AutoSuggestBox)sender;
        HandleTextSubmit(asb, asb.Text);
    }

    private void OnTextChanged(AutoSuggestBox sender)
    {
        // There're special cases when sender.Text is the same as text setter value (like sender.Text = "", when its already "") so it won't fire TextChanged event, we need to set flag manually
        // We not set for ProgrammaticChange manually, cause we don't know whether it gonna be fired somewhere else (so need to set flag manually where we think it gonna fire this event)
        if (_oneTimeTextChanged != ASBFlag.None)
        {
            _oneTimeTextChanged = ASBFlag.None;
            return;
        }
        var suggestions = _searchStrategy.Search(sender.Text.TextNormalize());
        if (suggestions.Count > 0)
        {
            sender.ItemsSource = suggestions;
        }
        else
        {
            sender.ItemsSource = new string[] { "No results found" };
        }
    }

    // ================= HELPER =================

    private void HandleSuggestSubmit(AutoSuggestBox sender, object suggestObject)
    {
        string? displayText;
        //User selected an 'correct' item, take an action
        if (suggestObject is T item)
        {
            _onChoosen.Invoke((T)suggestObject);
            displayText = _toDisplayString.Invoke((T)suggestObject);
            _oneTimeTextChanged = ASBFlag.None;
        }
        // The suggest is "Not found"
        else
        {
            // The SC has set sender.Text to "", when we set it again, it won't fire
            _onChoosen.Invoke(default); // Set the SelectedItem to null
            displayText = string.Empty;
            _oneTimeTextChanged = ASBFlag.None;
        }
        sender.Text = displayText;
    }

    private void HandleTextSubmit(AutoSuggestBox sender, string text)
    {
        string? displayText;
        if (!string.IsNullOrEmpty(text))
        {
            var suggestions = _searchStrategy.Search(text);
            // Record exists
            if (suggestions.Count > 0)
            {
                _onChoosen.Invoke(suggestions.First());
                displayText = _toDisplayString.Invoke(suggestions.First());
                _oneTimeTextChanged = ASBFlag.None;
            }
            else
            {
                // Record not exists
                _onNotValid?.Invoke();
                _onChoosen.Invoke(default);
                displayText = string.Empty;
                _oneTimeTextChanged = ASBFlag.None;
            }
        }
        // Empty string, Record not exists
        else
        {
            _onNotValid?.Invoke();
            _onChoosen.Invoke(default);
            displayText = string.Empty;
            _oneTimeTextChanged = ASBFlag.None;
        }
        sender.Text = displayText;
    }
}
