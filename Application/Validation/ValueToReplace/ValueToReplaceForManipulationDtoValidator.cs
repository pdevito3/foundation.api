namespace Application.Validation.ValueToReplace
{
    using Application.Dtos.ValueToReplace;
    using FluentValidation;
    using System;

    public class ValueToReplaceForManipulationDtoValidator<T> : AbstractValidator<T> where T : ValueToReplaceForManipulationDto
    {
        public ValueToReplaceForManipulationDtoValidator()
        {
            RuleFor(lambdaInitialsToReplace => lambdaInitialsToReplace.ValueToReplaceTextField1)
                .NotEmpty();
            RuleFor(lambdaInitialsToReplace => lambdaInitialsToReplace.ValueToReplaceIntField1)
                .GreaterThanOrEqualTo(0);
            RuleFor(lambdaInitialsToReplace => lambdaInitialsToReplace.ValueToReplaceDateField1)
                .LessThanOrEqualTo(DateTime.UtcNow);
        }
    }
}
