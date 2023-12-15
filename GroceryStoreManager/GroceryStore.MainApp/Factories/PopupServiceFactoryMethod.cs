using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Services.PopupServices;
using GroceryStore.MainApp.ViewModels.SubWindowVM;
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
}

public class PopupServiceFactoryMethod
{
    public static IPopupService Get(PopupType type, PopupContent content)
    {
        var _orderDetailDataService = App.GetService<IDataService<OrderDetail>>();
        var _orderDataService = App.GetService<IDataService<Order>>();
        var _productDataService = App.GetService<IDataService<Product>>();
        var _customerDataService = App.GetService<IDataService<Customer>>();
        var _couponDataService = App.GetService<IDataService<Coupon>>();

        Type contentType;
        Func<IPopupService, object?, PopupVMBase> contentCreate;
        switch (content)
        {
            case PopupContent.Order:
                contentType = typeof(OrderPopup);
                contentCreate = (ps, obj) => new OrderPopupVM(ps, _orderDataService, _productDataService, _customerDataService, _couponDataService, obj as Order);
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
