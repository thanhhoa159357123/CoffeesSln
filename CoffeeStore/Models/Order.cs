using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CoffeeStore.Models.Cart;

namespace CoffeeStore.Models
{
    public class Order
    {
        [BindNever]
        public string OrderID { get; set; }

        public ICollection<CartLine> OrderLines { get; set; } = new List<CartLine>();
        // Thông tin khách hàng
        [Required]
        public string Phone { get; set; }
        [ForeignKey("Phone")]
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "Please enter the address line")]
        public string Address { get; set; }

        // Dòng sản phẩm trong đơn hàng
        [NotMapped]
        public string CustomerName => $"{Customer?.FirstName} {Customer?.LastName}";

    }
}
