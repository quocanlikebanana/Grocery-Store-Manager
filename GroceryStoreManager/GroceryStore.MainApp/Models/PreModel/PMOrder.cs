using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using System.Diagnostics;

namespace GroceryStore.MainApp.Models.PreModel;
public class PMOrder
{
    private IDataService<Order> _orderDataService;
    private IDataService<Customer> _customerDataService;
    private IDataService<Product> _productDataService;
    private IDataService<OrderDetail> _orderDetailDataService;

    public PMOrder(Order? order = null, int? couponUsed = null)
    {
        _orderDataService = App.GetService<IDataService<Order>>();
        _customerDataService = App.GetService<IDataService<Customer>>();
        _productDataService = App.GetService<IDataService<Product>>();
        _orderDetailDataService = App.GetService<IDataService<OrderDetail>>();

        // Edit case
        if (order != null)
        {
            Id = order.Id;
            Customer = order.Customer;
            Details = order.details ?? new();
            OrderDate = order.OrderDate;
            TotalPrice = order.TotalPrice;
            TotalDiscount = order.TotalDiscount;
        }
        CouponUsed = couponUsed;
        IsUsed = false;
        CompressDetails();
    }

    public int? Id { get; set; } = null;
    public Customer? Customer { get; set; } = null;
    public List<OrderDetail> Details { get; set; } = new();
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public double TotalDiscount { get; set; } = 0;
    public double TotalPrice { get; set; } = 0;

    // Extra:
    public int? CouponUsed { get; set; } = null;

    public Order GetInsertObject()
    {
        var details = Details.Select(x => new OrderDetail() { ProductId = x.ProductId, Quantity = x.Quantity }).ToList();
        var order = new Order()
        {
            CustomerID = Customer?.Id,
            details = details,
            OrderDate = OrderDate,
            TotalPrice = TotalPrice,
            TotalDiscount = TotalDiscount,
        };
        return order;
    }

    public Order GetUpdateObject()
    {
        var details = Details.Select(x => new OrderDetail()
        {
            ProductId = x.ProductId,
            OrderId = x.OrderId,
            Quantity = x.Quantity
        }).ToList();
        var order = new Order()
        {
            CustomerID = Customer?.Id,
            details = details,
            OrderDate = OrderDate,
            TotalPrice = TotalPrice,
            TotalDiscount = TotalDiscount,
        };
        return order;
    }

    public Order GetFullObject()
    {
        var order = new Order()
        {
            Id = Id,
            Customer = Customer,
            CustomerID = Customer?.Id,
            details = Details,
            OrderDate = OrderDate,
            TotalDiscount = TotalDiscount,
            TotalPrice = TotalPrice,
        };
        return order;
    }

    public bool IsUsed { get; private set; }
    public async Task<bool> Insert()
    {
        try
        {
            var obj = GetInsertObject();
            var result = await _orderDataService.Create(obj) ?? throw new Exception();
            Id = result.Id;
            await UpdateCustomer();
            await UpdateProduct();
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
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
            var result = await _orderDataService.Update((int)Id!, obj) ?? throw new Exception();
            Id = result.Id;
            //await UpdateCustomer();   // We dont update customer coupon because coupon price may vary (and we out of time)
            await UpdateProduct();
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            return false;
        }
        IsUsed = true;
        return true;
    }

    // The trigger has not been handled because this wasnt sent by the PopupVM
    public async Task<bool> RawDelete()
    {
        try
        {
            // update every product first, then push it down to the database
            // then delete all related order detail (foreign key constraint)
            foreach (var detail in Details)
            {
                detail.Product!.Quantity += detail.Quantity;
                var orderDetail = await _orderDetailDataService.Delete((int)Id!, detail.ProductId);
                if (orderDetail == false)
                {
                    throw new Exception();
                }
            }
            await UpdateProduct();
            var result = await _orderDataService.Delete((int)Id!);
            if (result == false)
            {
                throw new Exception();
            }
            //await UpdateCustomer();   // We dont update customer coupon because coupon price may vary (and we out of time)
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            return false;
        }
        IsUsed = true;
        return true;
    }

    // === HELPER ===

    private void CompressDetails()
    {
        int length = Details.Count;
        for (int i = 0; i < length; i++)
        {
            for (int j = i + 1; j < length; j++)
            {
                if (Details[i].ProductId == Details[j].ProductId)
                {
                    Details[i].Quantity += Details[j].Quantity;
                    Details.RemoveAt(j);
                    j--;
                    length--;
                }
            }
        }
    }

    // === UPDATE ===

    private async Task UpdateProduct()
    {
        foreach (var detail in Details)
        {
            var pId = detail?.Product?.Id;
            var productUpdate = await _productDataService.Update(pId ?? -1, detail!.Product!) ?? throw new Exception();
        }
    }

    private async Task UpdateCustomer()
    {
        var cId = Customer?.Id;
        var customerUpdate = await _customerDataService.Update(cId ?? -1, Customer!) ?? throw new Exception(); ;
    }
}