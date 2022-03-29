using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Accounts")]
        public int AccountId { get; set; }

        // Navigation reference property cho khóa ngoại đến Account
        [DisplayName("Khách hàng")]
        public Account Account { get; set; }

        [ForeignKey("Products")]
        public int ProductId { get; set; }

        // Navigation reference property cho khóa ngoại đến Product
        [DisplayName("Sản phẩm")]
        public Product Product { get; set; }

        [Required(ErrorMessage = "{0} không được bỏ trống")]
        [DefaultValue(1)]
        public int Quantity { get; set; } = 1;
    }
}
