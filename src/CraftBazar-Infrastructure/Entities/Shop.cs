public class Shop : BaseEntity
{
    public int SellerId { get; set; }

    public string ShopName { get; set; }
        = string.Empty;

    public string Description { get; set; }
        = string.Empty;

    public bool IsApproved { get; set; }

    public User Seller { get; set; } = null;
}