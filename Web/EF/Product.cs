namespace Web.EF;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsInStock { get; set; }
    public bool IsForSale { get; set; }
    public DateTime ExpireDate { get; set; }
}