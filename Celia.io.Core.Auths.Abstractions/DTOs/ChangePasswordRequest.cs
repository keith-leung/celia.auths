using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Celia.io.Core.Auths.Abstractions.DTOs
{
    public class ChangePasswordRequest
    {
        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }
        
        [MaxLength(100)]
        public string OldPassword { get; set; }
        [Required]
        [MaxLength(100)]
        public string NewPassword { get; set; }
    }
}
