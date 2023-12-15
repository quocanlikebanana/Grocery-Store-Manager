using GroceryStore.Domain.Model;
using GroceryStore.MainApp.Helpers;

namespace GroceryStore.MainApp.Decorators;

public class DecASBCustomer
{
    private readonly Customer _customer;

    public DecASBCustomer(Customer customer)
    {
        _customer = customer;
    }

    public Customer Get()
    {
        return _customer;
    }

    public static string ToUniqueString(DecASBCustomer customer)
    {
        return $"{customer.Get().Name.ToLower()} {customer.Get().Id}".TextNormalize();
    }
    public static string ToDisplayString(DecASBCustomer customer)
    {
        return $"{customer.Get().Name} ({customer.Get().Id})";
    }
    public override string ToString() => ToDisplayString(this);
}


public class DecASBProduct
{
    private readonly Product _product;

    public DecASBProduct(Product product)
    {
        _product = product;
    }
    public Product Get()
    {
        return _product;
    }
    public static string ToUniqueString(DecASBProduct product)
    {
        return $"{product.Get().Name.ToLower()} {product.Get().Type?.Name} {product.Get().Id}".TextNormalize();
    }
    public static string ToDisplayString(DecASBProduct product)
    {
        return $"{product.Get().Name} ({product.Get().Type?.Name} - {product.Get().Id})";
    }
    public override string ToString() => ToDisplayString(this);
}
