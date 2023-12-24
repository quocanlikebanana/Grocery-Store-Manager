using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Mvvm;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Contracts.ViewModels;
using System.Windows.Input;

namespace GroceryStore.MainApp.ViewModels.SubVM;

// No binding is really needed
public class DetailOrderVM : ObservableRecipient
{
    private readonly Order _order;
    private readonly IDataService<Order> _orderDS;
    public DetailOrderVM(int id)
    {
        _orderDS = App.GetService<IDataService<Order>>();
        // loading
        _order = Task.Run(async () => await _orderDS.Get(id)).Result ?? throw new Exception();
        GoBackCommand = new DelegateCommand(GoBack);
    }

    public int? Id => _order.Id;
    public Customer? Customer => _order.Customer;
    public List<OrderDetail> Details => _order.details;
    public DateTime OrderDate => _order.OrderDate;
    public double TotalDiscount => _order.TotalDiscount;
    public double TotalPrice => _order.TotalPrice;

    public ICommand GoBackCommand { get; }
    public Action GoBackHandle { get; set; }
    private void GoBack()
    {
        GoBackHandle.Invoke();
    }
}
