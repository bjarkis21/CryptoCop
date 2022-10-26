namespace Cryptocop.Software.API.Models.Dtos
{
    public class ShoppingCartItemDto
    {
        public string Id { get; set; }
        public string ProductIdentifier { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
    }
}