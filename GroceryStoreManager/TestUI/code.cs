using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace TestUI;

public class DiaSer
{
    private readonly XamlRoot _xamlRoot;
    private readonly UIElement _content;
    private ContentDialog _dialog;
    public DiaSer(XamlRoot xamlRoot, UIElement content)
    {
        _xamlRoot = xamlRoot;
        _content = content;
    }

    public async void Show()
    {
        _dialog = new ContentDialog()
        {
            Content = _content,
            XamlRoot = _xamlRoot,
        };
        await _dialog.ShowAsync();
        Console.WriteLine();
    }

    public void Close()
    {
        _dialog.Hide();
    }
}