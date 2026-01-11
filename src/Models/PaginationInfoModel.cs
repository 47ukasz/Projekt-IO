namespace projekt_io.Models;

public class PaginationInfoModel {
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    
    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
    public int From => TotalCount == 0 ? 0 : ((Page - 1) * PageSize) + 1;
    public int To => Math.Min(Page * PageSize, TotalCount);
}