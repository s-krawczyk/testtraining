namespace ApplicationLayer.Validators
{
  using DomainLayer.Models;
  using FluentValidation;
  using FluentValidation.Results;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Text.Json;
  using System.Text.Json.Serialization;

  public class CustomerValidator : AbstractValidator<Customer>
  {
    public CustomerValidator()
    {
      RuleFor(p => p.CustomerName)
        .MinimumLength(3)
        .MaximumLength(32)
        .NotEmpty()
        .NotNull();

      RuleFor(p => p.PurchasesProduct)
        .NotNull();
    }

    protected override void RaiseValidationException(ValidationContext<Customer> context, ValidationResult result)
    {
      throw new ValidationException(JsonSerializer.Serialize(result.Errors));
    }
  }
}
