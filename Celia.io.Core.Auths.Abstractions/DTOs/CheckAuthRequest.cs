using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Celia.io.Core.Auths.Abstractions.DTOs
{
    public class CheckAuthRequest
    {
        [Required]
        [MaxLength(100)]
        public string AppId { get; set; }

        public string Path { get; set; }

        [Required]
        public long Timestamp { get; set; }

        [Required]
        public string Sign { get; set; }
    }
}
