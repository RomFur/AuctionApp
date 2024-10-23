using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auctions;

public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DateTime? endDate = value as DateTime?;

        if (endDate.HasValue && endDate.Value <= DateTime.Now)
        {
            return new ValidationResult("End date must be in the future.");
        }

        return ValidationResult.Success;
    }
}