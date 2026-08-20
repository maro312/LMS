namespace Application.Dtos.Lookups;

public class CreateUpdateCategoryDto 
{
    /// <summary>
    /// Display name of the lookup entry.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates whether the lookup entry is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
