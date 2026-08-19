namespace LMS.Infrastructure.Options;

public class JwtOptions
{
    public const string SectionName = "JwtConfig";

    public string Secret { get; set; } = string.Empty;
}
