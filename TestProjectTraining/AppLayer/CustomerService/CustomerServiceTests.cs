namespace TestProjectTraining.AppLayer.CustomerService
{
  using ApplicationLayer.CustomerService;
  using AutoFixture;
  using DomainLayer.Models;
  using FluentValidation;
  using InfrastructureLayer.ExternalServices;
  using InfrastructureLayer.RespositoryPattern;
  using NSubstitute;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class CustomerServiceTests : IDisposable
  {
    private readonly CustomerService _sut;
    private readonly Fixture _fixture;
    private readonly IRepository<Customer> _customerRepositoryMock;
    private readonly IEmailSender _emailSenderMock;
    private readonly IValidatorFactory _validatorFactoryMock;

    public CustomerServiceTests()
    {
      _customerRepositoryMock = Substitute.For<IRepository<Customer>>();
      _emailSenderMock = Substitute.For<IEmailSender>();
      _validatorFactoryMock = Substitute.For<IValidatorFactory>();

      _sut = new CustomerService(_customerRepositoryMock, _validatorFactoryMock, _emailSenderMock);
      _fixture = new Fixture();
    }

    [Fact]
    public void GetAllCustomers_10CustomersInDatabase_AllAreReturned()
    {
      // arrange
      _customerRepositoryMock.GetAll()
        .Returns(_fixture.CreateMany<Customer>(10));

      // act
      var result = _sut.GetAllCustomers();

      // assert
      Assert.Equal(10, result.Count());
    }

    [Fact]
    public void GetCustomer_ById_ReturnsQueriedCustomer()
    {
      // arrange
      var id = 3;
      Customer customerWithId = _fixture
                  .Build<Customer>()
                  .With(w => w.Id, id)
                  .Create();

      _customerRepositoryMock.Get(id)
        .Returns(
          customerWithId);

      // act
      var result = _sut.GetCustomer(id);

      // assert
      Assert.NotNull(result);
      Assert.Equal(customerWithId, result);
    }

    [Fact]
    public void GetCustomer_ById_ReturnsQueriedCustomer_v2()
    {
      // arrange
      var id = 3;
      Customer customerWithId = _fixture
                  .Build<Customer>()
                  .With(w => w.Id, id)
                  .Create();

      _customerRepositoryMock.Get(0).ReturnsForAnyArgs(customerWithId);

      // act
      var result = _sut.GetCustomer(id);

      // assert
      Assert.NotNull(result);
      Assert.Equal(customerWithId, result);
      _customerRepositoryMock.Received(1).Get(id);
    }

    [Fact]
    public void GetAllCustomers_SqlExceptionIsThrown_ExceptionIsPropagated()
    {
      // arrange
      _customerRepositoryMock.GetAll()
        .Returns(x => throw new AbandonedMutexException());

      // act, assert
      Assert.Throws<AbandonedMutexException>(() => _sut.GetAllCustomers());
    }

    public void Dispose()
    {
      // it shouldn't be empty
    }
  }
}
