using Application.DTOs;
using FluentValidation;
using Application.Requests.Events;

namespace Application.Validators.Events;

public class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.StartDateTime)
            .GreaterThan(DateTime.UtcNow).WithMessage("Event must start in the future");

        RuleFor(x => x.EndDateTime)
            .GreaterThan(x => x.StartDateTime).WithMessage("End date must be after start date");

        RuleFor(x => x.RegistrationStart)
            .LessThanOrEqualTo(x => x.RegistrationEnd).WithMessage("Registration start cannot be after registration end");

        RuleFor(x => x.Capacity)
            .InclusiveBetween(1, 10000).WithMessage("Capacity must be between 1 and 10,000");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("Image URL length must be less than 500 characters");

        RuleFor(x => x.EventTypeId)
            .GreaterThan(0).WithMessage("Valid Event Type is required");
            
        RuleFor(x => x.Location).NotNull().WithMessage("Location details are required");
    }
}

public class UpdateEventRequestValidator : AbstractValidator<UpdateEventRequest>
{
    public UpdateEventRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Event ID is required");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Capacity)
            .InclusiveBetween(1, 10000).WithMessage("Capacity must be between 1 and 10,000");

        RuleFor(x => x.StartDateTime)
            .GreaterThan(DateTime.UtcNow).WithMessage("Updated start date must be in the future");

        RuleFor(x => x.EndDateTime)
            .GreaterThan(x => x.StartDateTime).WithMessage("End date must be after start date");
    }
}


public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MaximumLength(50).WithMessage("Username is too long");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?\d{10,16}$").WithMessage("Invalid phone number format");

        RuleFor(x => x.OneTimePassword)
            .NotEmpty().WithMessage("OTP is required")
            .Length(6).WithMessage("OTP must be exactly 6 characters");
            
        RuleFor(x => x.Department)
            .IsInEnum().WithMessage("Please select a valid department");
    }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Code).NotEmpty().WithMessage("Reset code is required");
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(6).WithMessage("New password must be at least 6 characters");
    }
}

public class CreateAgendaRequestValidator : AbstractValidator<CreateAgendaRequest>
{
    public CreateAgendaRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Duration)
            .Must(d => d.TotalMinutes > 0).WithMessage("Duration must be greater than 0");
            
        RuleForEach(x => x.AgendaTracks).SetValidator(new AgendaTrackDTOValidator());
    }
}

public class AgendaTrackDTOValidator : AbstractValidator<AgendaTrackDTO>
{
    public AgendaTrackDTOValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Speaker).MaximumLength(100);
        RuleFor(x => x.Room).MaximumLength(50);
    }
}

public class UpdateAgendaRequestValidator : AbstractValidator<UpdateAgendaRequest>
{
    public UpdateAgendaRequestValidator()
    {
        RuleFor(x => x.AgendaId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        
        RuleForEach(x => x.Tracks).SetValidator(new UpdateAgendaTrackDTOValidator());
    }
}

public class UpdateAgendaTrackDTOValidator : AbstractValidator<UpdateAgendaTrackDTO>
{
    public UpdateAgendaTrackDTOValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Speaker).MaximumLength(100);
    }
}