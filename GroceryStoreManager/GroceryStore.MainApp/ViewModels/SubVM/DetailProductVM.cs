using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Command;
using System.Windows.Input;

namespace GroceryStore.MainApp.ViewModels.SubVM;

public class DetailProductVM : ObservableRecipient
{
    private readonly Product _product;
    private readonly IDataService<Product> _productDS;
    public DetailProductVM(int id)
    {
        _productDS = App.GetService<IDataService<Product>>();
        // loading
        _product = Task.Run(async () => await _productDS.Get(id)).Result ?? throw new Exception();
        GoBackCommand = new DelegateCommand(GoBack);
    }

    public int? Id => _product.Id;
    public string Name => _product.Name;
    public ProductType? ProductType => _product.Type;
    public int Quantity => _product.Quantity;
    public double Price => _product.Price;
    public double BasePrice => _product.BasePrice;
    public double Profit => Price - BasePrice;

    public ICommand GoBackCommand { get; }
    public Action? GoBackHandle { get; set; }
    private void GoBack(object? param)
    {
        GoBackHandle?.Invoke();
    }
}
