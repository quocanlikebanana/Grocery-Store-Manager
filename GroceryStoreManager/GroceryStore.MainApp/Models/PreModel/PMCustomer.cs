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
    public class PMCustomer
    {
        private readonly Customer _customer;
        private readonly IDataService<Customer> _customerDS;
        private readonly IDataService<Order> _orderDS;

        public PMCustomer(Customer customer)
        {
            _customer = customer;
            _customerDS = App.GetService<IDataService<Customer>>();
            _orderDS = App.GetService<IDataService<Order>>();
            IsUsed = false;
        }

        public Customer GetInsertObject()
        {
            var customer = _customer;
            customer.Id = null;
            return customer;
        }

        public Customer GetUpdateObject()
        {
            return _customer;
        }

        public Customer GetFullObject()
        {
            return _customer;
        }

        public bool IsUsed { get; private set; }
        public async Task<bool> Insert()
        {
            try
            {
                var obj = GetInsertObject();
                var result = await _customerDS.Create(obj) ?? throw new Exception();
                _customer.Id = result.Id;
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
                var result = await _customerDS.Update((int)_customer.Id!, obj) ?? throw new Exception();
                _customer.Id = result.Id;
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
            var orderList = await _orderDS.GetAll();
            foreach (var order in orderList)
            {
                if (order.Customer!.Id == _customer.Id)
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
                var result = await _customerDS.Delete((int)_customer.Id!);
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
            var orderList = await _orderDS.GetAll();
            foreach (var order in orderList)
            {
                if (order.Customer!.Id == _customer.Id)
                {
                    var pmOrder = new PMOrder(order);
                    await pmOrder.RawDelete();
                }
            }
        }
    }
}
