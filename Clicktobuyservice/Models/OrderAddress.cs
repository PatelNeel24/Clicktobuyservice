using System.ComponentModel.DataAnnotations;

namespace Clicktobuyservice.Models
{
    public class OrderAddress
    {
        [Key]
        public long AddressId { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? Country { get; set; }
        public int? City { get; set; }
        public string PostCode { get; set; }
        public int? IsDefaultAddress { get; set; }
    }

}
