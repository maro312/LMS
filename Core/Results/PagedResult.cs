using System.Text.Json.Serialization;

namespace LMS.Core.Results;

public class PagedResult<T>
{
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = new();

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; } = 1;

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 20;

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

    public PagedResult()
    {
    }

    public PagedResult(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items.ToList();
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}
