using GroceryStore.Domain.Model;
using GroceryStore.MainApp.Helpers;

namespace GroceryStore.MainApp.Decorators;

// ToSearchString is for making string that easy to search, which can be found by ID, name, ... unlike unique string

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

public class ProductTypeDecorator
{
    private readonly ProductType _productType;

    public ProductTypeDecorator(ProductType productType)
    {
        _productType = productType;
    }

    public ProductType Get()
    {
        return _productType;
    }

    public override string ToString() => ToDisplayString(_productType);

    public static string ToSearchString(ProductType productType)
    {
        return $"{productType.Name.ToLower()} {productType.Id}".TextNormalize();
    }

    public static string ToDisplayString(ProductType productType)
    {
        return $"{productType.Name} - ({productType.Id})";
    }

    public static bool Equal(ProductType? proType1, ProductType? proType2)
    {
        var res = false;
        if (proType1 != null && proType2 != null)
        {
            res = proType1.Id == proType2.Id;
        }
        return res;
    }
}
