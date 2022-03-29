﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Models
{
    public class ProductType
    {
        // phím tắt: nhấn prop + 2 lần tab
        [Key]
        public int Id { get; set; }     //Id hoặc Tên class + Id  (int)

        [DisplayName("Loại sản phẩm")]
        [Required(ErrorMessage = "{0} không được bỏ trống")]
        public string Name { get; set; }

        [DisplayName("Còn hiệu lực")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;

        // Collection reference property cho khóa ngoại từ Product
        public List<Product> Products { get; set; }
    }
}
