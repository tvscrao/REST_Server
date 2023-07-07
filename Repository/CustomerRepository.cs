using WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System;

namespace WebAPI.Repository
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomers();
        Task<HttpResponseMessage> InsertCustomers(List<Customer> customers);
    }
    public class CustomerRepository : ICustomerRepository
    {
        private List<Customer> customers;
        public CustomerRepository()
        {
            customers = new List<Customer>();
        }
        public async Task<List<Customer>> GetCustomers()
        {
            if (File.Exists("customers.json"))
            {
                var json = File.ReadAllText("customers.json");
                return JsonConvert.DeserializeObject<List<Customer>>(json);
            }
            return new List<Customer>();
        }
          
        public async Task<HttpResponseMessage> InsertCustomers(List<Customer> newCustomers)
        {
            var json = File.ReadAllText("customers.json");
            customers = JsonConvert.DeserializeObject<List<Customer>>(json);

            foreach (var customer in newCustomers)
            {
                if (ValidateCustomer(customer))
                {
                    InsertSortedCustomer(customer);
                }
                else
                {
                     throw new Exception("Id all ready exists.");
                }
            }
            SaveCustomers();
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }
        private void SaveCustomers()
        {
            var json = JsonConvert.SerializeObject(customers);
            File.WriteAllText("customers.json", json);
        }
        private bool ValidateCustomer(Customer customer)
        {
            if (customers == null) return true;

            if (customers.Exists(c => c.Id == customer.Id))
            {
                return false;
            }
            return true;
        }

        private void InsertSortedCustomer(Customer customer)
        {
            int index = 0;
            if (customers != null) 
            { 
                while (index < customers.Count &&
                       (string.Compare(customers[index].LastName, customer.LastName, StringComparison.OrdinalIgnoreCase) < 0 ||
                        (string.Compare(customers[index].LastName, customer.LastName, StringComparison.OrdinalIgnoreCase) == 0 &&
                         string.Compare(customers[index].FirstName, customer.FirstName, StringComparison.OrdinalIgnoreCase) < 0)))
                {
                    index++;
                }
            }
            else
            {
                customers = new List<Customer>();
            }
            customers.Insert(index, customer);
        }
    }
}