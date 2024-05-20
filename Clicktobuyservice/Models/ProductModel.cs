namespace Clicktobuyservice.Models
{
    public class ProductTbl
    {
        // Product.cs

        public long Id { get; set; }
        public string? ProductName { get; set; }
        public string? Price { get; set; }
        public string? Description { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string? ContentType { get; set; }
        public string? ProdctImage { get; set; }
    }

}
