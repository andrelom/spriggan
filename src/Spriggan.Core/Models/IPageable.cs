namespace Spriggan.Core.Models;

public interface IPageable
{
    int PageNumber { get; set; }

    int PageSize { get; set; }
}
