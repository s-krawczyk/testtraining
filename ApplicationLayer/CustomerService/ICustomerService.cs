using DomainLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationLayer.CustomerService
{
  public interface ICustomerService
  {
    IEnumerable<Customer> GetAllCustomers();
    Customer GetCustomer(int id);
    Task InsertCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
    void DeleteCustomer(int id);
  }
}
