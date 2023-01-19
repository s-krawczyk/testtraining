namespace TestProjectTraining.InfraLayer
{
  using AutoFixture;
  using DomainLayer.Models;
  using InfrastructureLayer;
  using Microsoft.EntityFrameworkCore;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class TestDatabaseFixture
  {
    private const string ConnectionString =
      @"Server=(localdb)\mssqllocaldb;Integrated Security=true;Initial Catalog=TestTrainingIntegrationTestDb";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public TestDatabaseFixture()
    {
      lock (_lock)
      {
        if (!_databaseInitialized)
        {
          using (var context = CreateContext())
          {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.SaveChanges();
          }

          _databaseInitialized = true;
        }
      }
    }

    public ApplicationDbContext CreateContext()
    {
      DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseSqlServer(ConnectionString)
                      .Options;

      return new ApplicationDbContext(
                  options);
    }
  }

  public class ApplicationDbContextTests : IClassFixture<TestDatabaseFixture>
  {
    private readonly TestDatabaseFixture _testDatabaseFixture;
    private readonly ApplicationDbContext _sut;
    private readonly Fixture _fixture;

    public ApplicationDbContextTests(TestDatabaseFixture testDatabaseFixture)
    {
      _testDatabaseFixture = testDatabaseFixture;
      _fixture = new Fixture();
      _sut = _testDatabaseFixture.CreateContext();
    }

    [Fact]
    public void Check_infra_set_RENEAME_ME()
    {
      // arrange
      _sut.Add(_fixture.Build<Customer>().Without(w => w.Id).Create());
      _sut.SaveChanges();
    }
  }
}
