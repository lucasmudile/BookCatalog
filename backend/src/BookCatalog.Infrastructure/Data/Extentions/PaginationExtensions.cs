using BookCatalog.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Infrastructure.Data.Extentions;

public static class PaginationExtensions
{
    public static PagedResult<T> ToPagedResult<T>(
        this List<T> items,
        PagedParameters parameters,
        int totalCount)
    {
        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }

    public static PagedResult<T> ToPagedResult<T>(
        this IEnumerable<T> items,
        PagedParameters parameters,
        int totalCount)
    {
        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }

    public static PagedResult<T> ToPagedResult<T>(
        this IEnumerable<T> source,
        PagedParameters parameters)
    {
        var totalCount = source.Count();
        var items = source
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }

    public static PagedResult<T> ToPagedResult<T>(
        this List<T> source,
        PagedParameters parameters)
    {
        var totalCount = source.Count;
        var items = source
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }
}