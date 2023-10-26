using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WebTN.Models
{   
    public class Article
    {
        [Key]
        public int ID { get; set; }
        [StringLength(255,MinimumLength = 5,ErrorMessage="{0} phải từ 5 đến 255 kí tự")]
        [Required(ErrorMessage = "{0} phải nhập")]
        [Column(TypeName = "nvarchar")]
        [DisplayName("Tiêu Đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} phải nhập")]
        [DataType(DataType.Date)]
        [DisplayName("Ngày Tạo")]
        public DateTime Created { get; set; }
        
        [DisplayName("Nội dung")]
        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "{0} phải nhập")]
        public string Content { get; set; }
    }
}