using DomainLayer.Models;
using FluentValidation;
using InfrastructureLayer.ExternalServices;
using InfrastructureLayer.RespositoryPattern;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.CustomerService
{
  public class CustomerService : ICustomerService
  {
    #region Property
    private IRepository<Customer> _repository;
    private readonly IValidatorFactory _validatorFactory;
    private readonly IEmailSender _emailSender;
    #endregion

    #region Constructor
    public CustomerService(IRepository<Customer> repository, IValidatorFactory validatorFactory, IEmailSender emailSender)
    {
      _repository = repository;
      _validatorFactory = validatorFactory;
      _emailSender = emailSender;
    }
    #endregion

    public IEnumerable<Customer> GetAllCustomers()
    {
      return _repository.GetAll();
      // mappowanie obiektu bazodanowego na DTO (Data Transfer Object)
    }

    public Customer GetCustomer(int id)
    {
      return _repository.Get(id);
    }

    public async Task InsertCustomer(Customer customer)
    {
      var validator = _validatorFactory.GetValidator<Customer>();

      validator.ValidateAndThrow(customer);

      _repository.Insert(customer);

      await _emailSender.SendEmail("thatMe@ey.com", $"Customer with Name = {customer.CustomerName}");
    }
    public void UpdateCustomer(Customer customer)
    {
      _repository.Update(customer);
    }

    public void DeleteCustomer(int id)
    {
      Customer customer = GetCustomer(id);
      _repository.Remove(customer);
      _repository.SaveChanges();
    }
  }
}
