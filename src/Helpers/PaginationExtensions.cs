using Microsoft.EntityFrameworkCore;

namespace projekt_io.Helpers;

public static class PaginationExtensions {
    public static async Task<(List<T> Items, int TotalCount)> ToPagedAsync<T>(this IQueryable<T> query, int page, int pageSize) {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var total = await query.CountAsync();

        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, total);
    }
}