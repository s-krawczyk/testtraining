namespace TestProjectTraining.InfraLayer
{
  using AutoFixture;
  using DomainLayer.Models;
  using InfrastructureLayer;
  using InfrastructureLayer.RespositoryPattern;
  using Respawn;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class CustomerRepositoryTests : IClassFixture<TestDatabaseFixture>, IAsyncLifetime
  {
    private readonly TestDatabaseFixture _testDatabaseFixture;
    private readonly IRepository<Customer> _sut;
    private readonly Respawner _respawner;
    private readonly Fixture _fixture;

    public CustomerRepositoryTests(TestDatabaseFixture testDatabaseFixture)
    {
      _testDatabaseFixture = testDatabaseFixture;
      _fixture = new Fixture();

      _respawner = (Respawner.CreateAsync(TestDatabaseFixture.ConnectionString, new RespawnerOptions { })).Result;

      _sut = new Repository<Customer>(testDatabaseFixture.CreateContext());
    }

    public Task InitializeAsync() => _respawner.ResetAsync(TestDatabaseFixture.ConnectionString);

    [Fact]
    public void AAAAA_InsertAndGet_ValidCustomer_Pass()
    {
      // arrange
      Customer customer = _fixture.Build<Customer>().Without(w => w.Id).Create();

      //act
      _sut.Insert(customer);

      //assert
      var dataFromDb = _sut.Get(customer.Id);

      Assert.Equal(customer.CustomerName, dataFromDb.CustomerName);
      Assert.Equal(customer.PaymentType, dataFromDb.PaymentType);
      Assert.Equal(customer.IsActive, dataFromDb.IsActive);
    }

    [Fact]
    public void ZZZZZ_GetAll_MultipleCustomersInDatabase_Pass()
    {
      // arrange
      Customer customer = _fixture.Build<Customer>().Without(w => w.Id).Create();
      Customer customer2 = _fixture.Build<Customer>().Without(w => w.Id).Create();

      //act
      _sut.Insert(customer);
      _sut.Insert(customer2);

      //assert
      var dataFromDb = _sut.GetAll();

      Assert.Equal(2, dataFromDb.Count());
      Assert.Contains(dataFromDb, f => f.CustomerName == customer.CustomerName);
      Assert.Contains(dataFromDb, f => f.CustomerName == customer.CustomerName);
      Assert.Contains(dataFromDb, f => f.CustomerName == customer2.CustomerName);
    }

    public Task DisposeAsync() => Task.CompletedTask;
  }
}
