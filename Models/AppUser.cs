using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebTN.Models
{
    public class AppUser:IdentityUser
    {
        [Column(TypeName ="nvarchar")]
        [StringLength(400)]
        public string? HomeAddress { get; set; }
    }
}