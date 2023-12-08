using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDto
    {
    [Required]
    [StringLength(8, MinimumLength = 4)]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
    }
}