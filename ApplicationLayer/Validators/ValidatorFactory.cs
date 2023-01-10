namespace ApplicationLayer.Validators
{
  using FluentValidation;
  using Microsoft.Extensions.DependencyInjection;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class ValidatorFactory : IValidatorFactory
  {
    private readonly IServiceProvider _serviceProvider;

    public ValidatorFactory(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    public IValidator<T> GetValidator<T>() => _serviceProvider.GetService<IValidator<T>>();
    public IValidator GetValidator(Type type) => (IValidator)_serviceProvider.GetService(type);
  }
}
