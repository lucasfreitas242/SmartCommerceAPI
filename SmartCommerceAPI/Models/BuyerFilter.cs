namespace SmartCommerceAPI.Models
{
    public class BuyerFilter
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PersonType { get; set; }
        public string? Document { get; set; }
        public string? StateRegistration { get; set; }
        public bool? Blocked { get; set; }
    }
}
