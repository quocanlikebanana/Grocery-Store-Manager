using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Services.PopupServices;
using GroceryStore.MainApp.ViewModels.PopupVM;
using GroceryStore.MainApp.Views.PopupView;

namespace GroceryStore.MainApp.Factories;

public enum PopupType
{
    //Window,
    ContentDialog,
}

public enum PopupContent
{
    Order,
    Product,
    Customer,
    Warning,
    Login,
}

public class PopupServiceFactoryMethod
{
    public static IPopupService Get(PopupType type, PopupContent content)
    {
        Type contentType;
        Func<IPopupService, object?, PopupVMBase> contentCreate;
        switch (content)
        {
            case PopupContent.Order:
                contentType = typeof(OrderPopup);
                contentCreate = (ps, obj) => new OrderPopupVM(ps, obj as Order);
                break;
            case PopupContent.Product:
                contentType = typeof(ProductPopup);
                contentCreate = (ps, obj) => new ProductPopupVM(ps, obj as Product);
                break;
            case PopupContent.Customer:
                contentType = typeof(CustomerPopup);
                contentCreate = (ps, obj) => new CustomerPopupVM(ps, obj as Customer );
                break;
            case PopupContent.Warning:
                contentType = typeof(WarningPopup);
                contentCreate = (ps, message) => new SimplePopupVM(ps, message);
                break;
            case PopupContent.Login:
                contentType = typeof(LoginPopup);
                contentCreate = (ps, message) => new LoginPopupVM(ps, message);
                break;

            default:
                throw new Exception();
        }
        switch (type)
        {
            //case PopupType.Window:
            //    return new WindowPopupService(contentType, contentCreate);
            case PopupType.ContentDialog:
                return new ContentDialogPopupService(contentType, contentCreate);
        }
        throw new Exception();
    }
}
