using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class VerificationCodeDto:IDto
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Code { get; set; }
    }
}
