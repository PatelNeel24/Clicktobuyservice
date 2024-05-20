using System.ComponentModel.DataAnnotations;

namespace Clicktobuyservice.Models
{
    public class UserAddress
    {
        [Key]
        public int AddressID { get; set; }
        public long? UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public bool? IsDefaultAddress { get; set; }

        public bool? IsActive { get; set; }

        public string? Title { get; set; }


    }


}
