using Digital.Lib.Net.Mvc.Controllers.Pagination;

namespace Digital.Pages.Api.Dto.Entities;

public class ViewQuery : Query
{
    public string? Title { get; set; }
    public bool? IsPublished { get; set; }
}