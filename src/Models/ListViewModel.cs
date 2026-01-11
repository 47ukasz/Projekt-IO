namespace projekt_io.Models;

public class ListViewModel<T> {
    public List<T> Items { get; set; }
    public PaginationInfoModel PaginationInfo { get; set; }
}