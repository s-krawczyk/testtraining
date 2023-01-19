namespace TestProjectTraining.AppLayer.Validators
{
  using ApplicationLayer.Validators;
  using AutoFixture;
  using DomainLayer.Models;

  public class CustomerValidatorTest : IDisposable
  {
    private readonly CustomerValidator _sut;
    private readonly Fixture _fixture;

    public CustomerValidatorTest()
    {
      _sut = new CustomerValidator();
      _fixture = new Fixture();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData(null)]
    [InlineData("1")]
    [InlineData("12")]
    public void Validate_CompamyNameInvalid(string customerName)
    {
      // arrange
      var customer = _fixture.Create<Customer>();
      customer.CustomerName = customerName;

      // act
      var result = _sut.Validate(customer);

      // assert
      Assert.False(result.IsValid);
      Assert.Contains(result.Errors, p => p.PropertyName == nameof(Customer.CustomerName));
    }

    [Theory]
    [InlineData("01234567890123456789012345678912")]
    [InlineData("1234")]
    public void Validate_CompamyNameValid_True(string customerName)
    {
      // arrange
      var customer = _fixture.Create<Customer>();
      customer.CustomerName = customerName;

      // act
      var result = _sut.Validate(customer);

      // assert
      Assert.True(result.IsValid);
    }

    public void Dispose() { }
  }
}