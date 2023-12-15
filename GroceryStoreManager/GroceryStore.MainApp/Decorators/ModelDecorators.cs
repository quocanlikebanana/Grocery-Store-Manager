using GroceryStore.Domain.Model;
using GroceryStore.MainApp.Helpers;

namespace GroceryStore.MainApp.Decorators;

// been used in the past
public class CustomerDecorator
{
    private readonly Customer _customer;

    public CustomerDecorator(Customer customer)
    {
        _customer = customer;
    }

    public Customer Get()
    {
        return _customer;
    }
    public override string ToString() => ToDisplayString(_customer);

    public static string ToSearchString(Customer customer)
    {
        return $"{customer.Name.ToLower()} {customer.Id}".TextNormalize();
    }

    public static string ToDisplayString(Customer customer)
    {
        return $"{customer.Name} ({customer.Id})";
    }

    public static bool Equal(Customer? customer1, Customer? customer2)
    {
        var res = false;
        if (customer1 != null && customer2 != null)
        {
            res = customer1.Id == customer2.Id;
        }
        return res;
    }
}


public class ProductDecorator
{
    private readonly Product _product;

    public ProductDecorator(Product product)
    {
        _product = product;
    }

    public Product Get()
    {
        return _product;
    }

    public override string ToString() => ToDisplayString(_product);

    public static string ToSearchString(Product product)
    {
        return $"{product.Name.ToLower()} {product.Type?.Name} {product.Id}".TextNormalize();
    }

    public static string ToDisplayString(Product product)
    {
        return $"{product.Name} ({product.Type?.Name} - {product.Id})";
    }
}


public class OrderDecorator
{
    private readonly Order _order;

    public OrderDecorator(Order order)
    {
        _order = order;
    }

    public Order Get()
    {
        return _order;
    }

    public static bool Equal(Order? order1, Order? order2)
    {
        var res = false;
        if (order1 != null && order2 != null)
        {
            res = order1.Id == order2.Id;
        }
        return res;
    }
}
