using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.MainApp.Models.PreModel
{
    public class PMProductType
    {
        private readonly ProductType _productType;
        private readonly IDataService<ProductType> _productTypeDS;
        private readonly IDataService<Product> _productDS;
        private readonly IDataService<Order> _orderDS;

        public PMProductType(ProductType productType)
        {
            _productType = productType;
            _productTypeDS = App.GetService<IDataService<ProductType>>();
            _productDS = App.GetService<IDataService<Product>>();
            _orderDS = App.GetService<IDataService<Order>>();
            IsUsed = false;
        }

        public ProductType GetInsertObject()
        {
            var customer = _productType;
            customer.Id = null;
            return customer;
        }

        public ProductType GetUpdateObject()
        {
            return _productType;
        }

        public ProductType GetFullObject()
        {
            return _productType;
        }

        public bool IsUsed { get; private set; }
        public async Task<bool> Insert()
        {
            try
            {
                var obj = GetInsertObject();
                var result = await _productTypeDS.Create(obj) ?? throw new Exception();
                _productType.Id = result.Id;
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
                var result = await _productTypeDS.Update((int)_productType.Id!, obj) ?? throw new Exception();
                _productType.Id = result.Id;
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
            var productList = await _productDS.GetAll();
            foreach (var product in productList)
            {
                if (product.Type!.Id == _productType.Id)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> RawDelete()
        {
            try
            {
                var result = await _productTypeDS.Delete((int)_productType.Id!);
                if (result == false)
                {
                    throw new Exception();
                }
            }
            // From the database
            catch (DbUpdateException)
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
            var productList = await _productDS.GetAll();
            foreach (var product in productList)
            {
                if (product.Type!.Id == _productType.Id)
                {
                    var pmProduct = new PMProduct(product);
                    await pmProduct.RawDelete();
                }
            }
        }
    }
}
