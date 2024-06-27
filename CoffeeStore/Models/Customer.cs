using System.ComponentModel.DataAnnotations;

namespace CoffeeStore.Models
{
    public class Customer
    {
        [Key]
        public string Phone { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public DateTime NgaySinh { get; set; }
        public string Email { get; set; } = String.Empty;
        public string GioiTinh { get; set; } = String.Empty;
    }
}
