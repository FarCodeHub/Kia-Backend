﻿using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Unit.Command.Update
{
    public class UpdateUnitCommandValidator : AbstractValidator<UpdateUnitCommand>
    {
        public UpdateUnitCommandValidator()
        {
            RuleFor(x => x.Title)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x is { Length: < 50 }).WithMessage($"{ExceptionMessage.MaxLenght}:50");

            RuleFor(x => x.ParentId)
                .Must(x => (x == null) != (x != null && x > 0)).WithMessage($"{ExceptionMessage.Zero}");

            RuleFor(x => x.BranchId)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");
        }
    }
}