using Microsoft.UI.Xaml.Controls;

using TemplateTest.ViewModels;

namespace TemplateTest.Views;

public sealed partial class ReportPage : Page
{
    public ReportViewModel ViewModel
    {
        get;
    }

    public ReportPage()
    {
        ViewModel = App.GetService<ReportViewModel>();
        InitializeComponent();
    }
}
