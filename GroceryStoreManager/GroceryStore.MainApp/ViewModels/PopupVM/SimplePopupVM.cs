using GroceryStore.MainApp.Contracts.Services;

namespace GroceryStore.MainApp.ViewModels.PopupVM;

public partial class SimplePopupVM : PopupVMBase
{
    public SimplePopupVM(IPopupService dialogService, object? message) : base(dialogService, null)
    {
        Message = message?.ToString() ?? "";
    }

    public string Message { get; }

    public override object? GetFormData()
    {
        return true;
    }
}
