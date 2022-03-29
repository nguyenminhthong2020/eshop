﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Models
{
    public class InvoiceDetail
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Invoices")]
        public int InvoiceId { get; set; }

        // Navigation reference property cho khóa ngoại đến Invoice
        [DisplayName("Hóa đơn")]
        public Invoice Invoice { get; set; }

        [ForeignKey("Products")]
        public int ProductId { get; set; }

        // Navigation reference property cho khóa ngoại đến Product
        [DisplayName("Sản phẩm")]
        public Product Product { get; set; }

        [DisplayName("Số lượng")]
        [DefaultValue(1)]
        public int Quantity { get; set; } = 1;

        [DisplayName("Đơn giá")]
        [DefaultValue(0)]
        public int UnitPrice { get; set; } = 0;
    }
}
