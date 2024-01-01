using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GroceryStore.MainApp.Models.PreModel;

public class PMProduct
{
    private IDataService<Order> _orderDataService;
    private IDataService<Customer> _customerDataService;
    private IDataService<Product> _productDataService;
    private IDataService<OrderDetail> _orderDetailDataService;

    public PMProduct(Product? product = null)
    {
        _orderDataService = App.GetService<IDataService<Order>>();
        _customerDataService = App.GetService<IDataService<Customer>>();
        _productDataService = App.GetService<IDataService<Product>>();
        _orderDetailDataService = App.GetService<IDataService<OrderDetail>>();

        // Edit case
        if (product != null)
        {
            Id = product.Id;
            Name = product.Name;
            Type = product.Type;
            Price = product.Price;
            BasePrice = product.BasePrice;
            Quantity = product.Quantity;
        }
        IsUsed = false;
    }

    public int? Id { get; set; } = null;
    public string Name { get; set; } = string.Empty;
    public ProductType? Type { get; set; } = null;
    public double Price { get; set; } = 0;
    public double BasePrice { get; set; } = 0;
    public int Quantity { get; set; } = 0;

    public Product GetInsertObject()
    {
        var product = new Product()
        {
            Name = Name,
            TypeId = (Type!.Id ?? throw new Exception()),
            Price = Price,
            BasePrice = BasePrice,
            Quantity = Quantity,
        };
        return product;
    }

    public Product GetUpdateObject()
    {
        var product = new Product()
        {
            Id = Id,
            Name = Name,
            TypeId = (Type!.Id ?? throw new Exception()),
            Price = Price,
            BasePrice = BasePrice,
            Quantity = Quantity,
        };
        return product;
    }

    public Product GetFullObject()
    {
        var product = new Product()
        {
            Id = Id,
            Name = Name,
            TypeId = (Type!.Id ?? throw new Exception()),
            Type = Type,
            Price = Price,
            BasePrice = BasePrice,
            Quantity = Quantity,
        };
        return product;
    }

    public bool IsUsed { get; private set; }
    public async Task<bool> Insert()
    {
        try
        {
            var obj = GetInsertObject();
            var result = await _productDataService.Create(obj) ?? throw new Exception();
            Id = result.Id;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }
        IsUsed = true;
        return true;
    }

    public async Task<bool> Update()
    {
        try
        {
            var obj = GetUpdateObject();
            var result = await _productDataService.Update((int)Id!, obj) ?? throw new Exception();
            Id = result.Id;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }
        IsUsed = true;
        return true;
    }

    public async Task<bool> CheckCascade()
    {
        var orderList = await _orderDataService.GetAll();
        foreach (var order in orderList)
        {
            foreach (var detail in order.details)
            {
                if (detail.ProductId == Id)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public async Task<bool> RawDelete()
    {
        try
        {
            var result = await _productDataService.Delete((int)Id!);
            if (result == false)
            {
                throw new Exception();
            }
        }
        catch(DbUpdateException)
        {
            await DeleteCascadeOrder();
            // a little bit of scary here
            await RawDelete();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }
        IsUsed = true;
        return true;
    }

    // === UPDATE ===

    private async Task DeleteCascadeOrder()
    {
        var orderList = await _orderDataService.GetAll();
        foreach (var order in orderList)
        {
            foreach (var detail in order.details)
            {
                if (detail.ProductId == Id)
                {
                    var pmOrder = new PMOrder(order);
                    await pmOrder.RawDelete();
                }
            }
        }
    }
}
