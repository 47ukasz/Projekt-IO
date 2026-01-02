namespace projekt_io.Models;

public class NavViewModel {
    public bool IsAuthenticated { get; set; }
    public bool IsAdmin { get; set; }
    public List<(string, string)> NavItems { get; set; }
}