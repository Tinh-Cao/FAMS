using System.ComponentModel.DataAnnotations;

namespace FAMS_GROUP2.Repositories.ViewModels.AssignmentModels;

public class DateRangeValidateCustom : ValidationAttribute
{
    private readonly string _startDatePropertyName;

    public DateRangeValidateCustom(string startDatePropertyName)
    {
        _startDatePropertyName = startDatePropertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
        if (startDateProperty == null)
        {
            throw new ArgumentException("Invalid property name");
        }

        var endDateValue = (DateTime?)value;
        var startDateValue = (DateTime?)startDateProperty.GetValue(validationContext.ObjectInstance);

        if (endDateValue.HasValue && startDateValue.HasValue && endDateValue < startDateValue)
        {
            return new ValidationResult("End date must be greater than or equal to start date.");
        }

        return ValidationResult.Success;
    }
}