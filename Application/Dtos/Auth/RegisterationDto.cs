using LMS.Domain.Constants;

namespace Application.Dtos.Auth;

public class RegisterationDto 
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

