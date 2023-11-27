namespace FiorelloBackend.Helpers.Responses
{
    public class ProductCountResponse
    {
        public int Count { get; set; }
        public decimal BasketGrandTotal { get; set; }
        public decimal ProductGrandTotal{ get; set; }
    }
}
