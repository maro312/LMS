using System.Text.Json.Serialization;

namespace LMS.Core.Results;

public class PagedResult<T>
{
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = new();

    [JsonPropertyName("page")]
    public int Page { get; set; } = 1;

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 20;

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 0;

    public PagedResult()
    {
    }

    public PagedResult(IEnumerable<T> items, int page, int pageSize, int totalItems)
    {
        Items = items.ToList();
        Page = page;
        PageSize = pageSize;
        TotalItems = totalItems;
    }
}
