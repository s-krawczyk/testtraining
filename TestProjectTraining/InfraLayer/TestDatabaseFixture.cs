namespace TestProjectTraining.InfraLayer
{
  using InfrastructureLayer;
  using Microsoft.EntityFrameworkCore;

  public class TestDatabaseFixture
  {
    public const string ConnectionString =
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
}
